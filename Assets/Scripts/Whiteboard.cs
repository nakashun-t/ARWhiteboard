using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasualMeeting
{
    public class Whiteboard : MonoBehaviour
    {
        private int whiteboardID;

        public int WhiteboardID
        {
            set { this.whiteboardID = value; }
            get { return whiteboardID; }
        }

        private void Start()
        {
            whiteboardID = GameObject.Find("WhiteboardManager").GetComponent<WhiteboardManager>().GeneratedWhiteboardNumber;
            Debug.Log("WhiteboardID = " + whiteboardID);
        }

        public int CountNoteInChild()
        {
            return GetComponentsInChildren<Note>().Length;
        }
    }

}
