using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class NoteMovement : MonoBehaviour
{
    [SerializeField] GameObject note1;
    [SerializeField] GameObject noteButton1;
    [SerializeField] public int beatsPerMinute;
    Transform gridBackground;
    Vector3 note1Center;
    Vector3 noteButton1Center;
    float scrollSpeed;
    [SerializeField] float fract;

    void Start()
    {
        gridBackground = transform.Find("Grid Background");

        if (gridBackground != null)
        {
            string kappa = "kappa";
        }

        beatsPerMinute = gridBackground.GetComponent<NewNoteGrid>().beatsPerMinute;

        note1Center = note1.GetComponent<Renderer>().bounds.center;

        noteButton1Center = noteButton1.GetComponent<Renderer>().bounds.center;
    }

    void Update()
    {
        note1.transform.position = Vector3.Lerp(note1Center, noteButton1Center, fract);
    }

}