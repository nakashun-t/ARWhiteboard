using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasualMeeting
{
    public class DragManager : MonoBehaviourPunCallbacks
    {
        WhiteboardManager whiteboardManager = null;
        private bool isDragging = false;

        private int draggingNoteID = -1;
        private Vector3 noteLocalPosition = Vector3.zero;

        public bool IsDragging
        {
            get { return isDragging; }
        }

        private void Start()
        {
            whiteboardManager = GameObject.FindObjectOfType<WhiteboardManager>();
        }


        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider != null && hitInfo.collider.tag == "Note")
                    {
                        isDragging = true;
                        draggingNoteID = hitInfo.collider.GetComponent<Note>().NoteID;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                draggingNoteID = -1;
            }
            if (isDragging)
            {
                DragNote();
            }
        }

        private void DragNote()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider != null)
                {
                    Note[] notes = SpecifyWhiteboard().GetComponentsInChildren<Note>();
                    foreach(Note note in notes)
                    {
                        if(note.NoteID == draggingNoteID)
                        {
                            note.transform.position = hitInfo.point;
                            noteLocalPosition = note.transform.localPosition;
                            noteLocalPosition.y = 0.1f;
                            note.transform.localPosition = noteLocalPosition;
                            photonView.RPC(nameof(MoveNotePosition), RpcTarget.Others, SpecifyWhiteboard().WhiteboardID, note.NoteID, noteLocalPosition);
                        }
                    }

                }

            }


        }

        private Whiteboard SpecifyWhiteboard()
        {
            Whiteboard target = null;
            Whiteboard[] whiteboards = GameObject.FindObjectsOfType<Whiteboard>();
            foreach(Whiteboard whiteboard in whiteboards)
            {
                if(whiteboard.WhiteboardID == whiteboardManager.TargetWhiteboardID)
                {
                    target = whiteboard;
                    break;
                }
            }
            return target;
        }


        [PunRPC]
        private void MoveNotePosition(int whiteboardID, int noteID, Vector3 noteLocalPosition)
        {
            foreach(Transform child in SpecifyWhiteboard(whiteboardID).transform)
            {
                Note note = child.GetComponent<Note>();
                if(note.NoteID == noteID)
                {
                    note.transform.localPosition = noteLocalPosition;
                }
            }
            

        }

        private Whiteboard SpecifyWhiteboard(int whiteboardID)
        {
            Whiteboard target = null;
            Whiteboard[] whiteboards = GameObject.FindObjectsOfType<Whiteboard>();
            foreach (Whiteboard whiteboard in whiteboards)
            {
                if (whiteboard.WhiteboardID == whiteboardID)
                {
                    target = whiteboard;
                    break;
                }
            }
            return target;
        }
    }

}
