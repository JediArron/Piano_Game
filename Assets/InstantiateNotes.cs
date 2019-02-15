using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateNotes : MonoBehaviour
{
    [SerializeField] GameObject note;

    void Update()
    {
        Instantiate(note, note.transform.position, Quaternion.identity);
    }
}
