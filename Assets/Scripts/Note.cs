using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasualMeeting
{
    public class Note : MonoBehaviour
    {
        private int noteID;
        public int NoteID
        {
            get { return noteID; }
        }

        private void Start()
        {
            noteID = GameObject.FindObjectOfType<NoteManager>().GeneratedNoteNumber;
        }
    }
}
