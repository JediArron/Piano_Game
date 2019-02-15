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

            Buffer.BlockCopy(new[] { speed }, 0, bytes, counter, sizeof(float)); //I miss pointers D:
            counter += sizeof(float);

            bytes[counter++] = (byte) ((Notes.Length & 0xFF0000) >> 16);
            bytes[counter++] = (byte) ((Notes.Length & 0x00FF00) >> 8);
            bytes[counter++] = (byte) (Notes.Length & 0x0000FF);

            foreach (Note n in Notes)
            {
                Buffer.BlockCopy(new[] { n.StartTime }, 0, bytes, counter, sizeof(float));
                counter += sizeof(float);
                bytes[counter++] = n.Sound;
                Buffer.BlockCopy(new[] { n.Length }, 0, bytes, counter, sizeof(float));
                counter += sizeof(float);
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

    /*public static class Read
    {
        public static bool FromFile()
        {
            //No implementation for V001001 because there's never been functions to save it.
            return true;
        }
    }*/
}