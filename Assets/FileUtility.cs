using System.IO;
using System;

namespace FileUtility
{
    enum VERSION : short //V[Major][Minor]
    {
        V001001,
        V002001 //BREAKING CHANGE: File length now before Version Info and the 16-bit floating point number is now 32-bit
    }

    public class Note
    {
        public float StartTime;
        public byte Sound;
        public float Length;

        public Note(float StartTime, byte Sound, float Length)
        {
            this.StartTime = StartTime;
            this.Sound = Sound;
            this.Length = Length;
        }

        public Note()
        {

        }

        public override string ToString()
        {
            return "NOTE: " + StartTime + " | " + Sound + " | " + Length;
        }
    }

    public static class Write
    {
        /// <summary>
        /// Writes information about a level to a local file
        /// </summary>
        /// <param name="path">The path of the file to write to</param>
        /// <returns>true if successful, false otherwise.</returns>
        public static bool ToFile(string path, string Username, string Levelname, float speed, Note[] Notes) //I'm naming it this way to leave space for possible later "ToNetwork" Methods
        {
            int FileLength = 4 + 2 + 2 + Username.Length + 2 + Levelname.Length + 4 + 3 + (Notes.Length * (4 + 1 + 4));
            byte[] bytes = new byte[FileLength];
            int counter = 0;
            bytes[counter++] = (byte) ((FileLength & 0xFF000000) >> 24);
            bytes[counter++] = (byte) ((FileLength & 0x00FF0000) >> 16);
            bytes[counter++] = (byte) ((FileLength & 0x0000FF00) >> 8);
            bytes[counter++] = (byte) (FileLength & 0x000000FF);

            bytes[counter++] = ((short) VERSION.V002001 & 0xFF00) >> 8;
            bytes[counter++] = (short) VERSION.V002001 & 0x00FF;

            bytes[counter++] = (byte) ((Username.Length & 0xFF00) >> 8);
            bytes[counter++] = (byte) (Username.Length & 0x00FF);

            foreach (char c in Username)
            {
                bytes[counter++] = (byte) c;
            }

            bytes[counter++] = (byte) ((Levelname.Length & 0xFF00) >> 8);
            bytes[counter++] = (byte) (Levelname.Length & 0x00FF);

            foreach (char c in Levelname)
            {
                bytes[counter++] = (byte) c;
            }

            Buffer.BlockCopy(new[] { speed }, 0, bytes, counter, 4); //I miss pointers D:
            counter += 4;

            bytes[counter++] = (byte) ((Notes.Length & 0xFF0000) >> 16);
            bytes[counter++] = (byte) ((Notes.Length & 0x00FF00) >> 8);
            bytes[counter++] = (byte) (Notes.Length & 0x0000FF);

            foreach (Note n in Notes)
            {
                Buffer.BlockCopy(new[] { n.StartTime }, 0, bytes, counter, 4);
                counter += 4;
                bytes[counter++] = n.Sound;
                Buffer.BlockCopy(new[] { n.Length }, 0, bytes, counter, 4);
                counter += 4;
            }

            try
            {
                File.WriteAllBytes(path, bytes);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }

    public static class Read
    {
        public static bool FromFile(string path, out string Username, out string Levelname, out float speed, out Note[] Notes)
        {
            //No implementation for V001001 because there's never been functions to save it.

            byte[] bytes;
            try
            {
                bytes = File.ReadAllBytes(path);
            }
            catch
            {
                Username = null;
                Levelname = null;
                speed = -1;
                Notes = null;
                return false;
            }

            short version = (short) ((bytes[4] << 8) | bytes[5]);
            if (bytes[4] == 0 && bytes[5] == 1) //first 4 bytes not currently used, but may be useful for other implementations
            {
                int counter = 6;
                ushort UNLength = (ushort) (bytes[counter++] << 8);
                UNLength |= bytes[counter++];
                StringBuilder UserNameBuilder = new StringBuilder();
                for (ushort i = 0; i < UNLength; i++)
                    UserNameBuilder.Append((char) bytes[counter++]);
                Username = UserNameBuilder.ToString();


                ushort LNLength = (ushort) (bytes[counter++] << 8);
                LNLength |= bytes[counter++];
                StringBuilder LevelNameBuilder = new StringBuilder();
                for (ushort i = 0; i < LNLength; i++)
                    LevelNameBuilder.Append((char) bytes[counter++]);
                Levelname = LevelNameBuilder.ToString();


                speed = -1; //needed so it doesnt complain about speed being unassigned in the next line
                float[] speedTmpArray = new float[1];
                Buffer.BlockCopy(bytes, counter, speedTmpArray, 0, 4);
                speed = speedTmpArray[0];
                counter += 4;


                int noteNum = (bytes[counter] << 16) | (bytes[counter + 1] << 8) | bytes[counter + 2];
                counter += 3;
                Notes = new Note[noteNum];
                for (int i = 0; i < Notes.Length; i++)
                    Notes[i] = new Note();
                for (int i = 0; i < noteNum; i++)
                {
                    var StartTimeTmpArray = new float[1];
                    Buffer.BlockCopy(bytes, counter, StartTimeTmpArray, 0, 4);
                    Notes[i].StartTime = StartTimeTmpArray[0];
                    counter += 4;
                    Notes[i].Sound = bytes[counter++];
                    var LengthTmpArray = new float[1];
                    Buffer.BlockCopy(bytes, counter, LengthTmpArray, 0, 4);
                    Notes[i].Length = LengthTmpArray[0];
                    counter += 4;
                }

                return true;
            }
            Username = null;
            Levelname = null;
            speed = -1;
            Notes = null;
            return false;
        }
    }
}