using Photon.Pun;
using UnityEngine;

namespace CasualMeeting
{
    public class NoteManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject orangeNote;
        [SerializeField]
        private GameObject pinkNote;
        [SerializeField]
        private GameObject greenNote;

        private int generatedNoteNumber = 0;
        private int latestLocalGenratedNoteID = 0;

        //note position parameter
        private Vector3 noteGeneratingOffset = new Vector3(-2f, -2.6f, 1.5f);
        private Vector3 localNoteInitialPosition = new Vector3(-4f, 0.1f, 4f);
        private Vector3 noteInitialAlignPosition = new Vector3(-3.5f, 0.1f, 4f);
        private Vector3 xoffset = new Vector3(1.3f, 0, 0);
        private Vector3 yoffset = new Vector3(0, 0, -1.2f);

        public int GeneratedNoteNumber
        {
            get { return generatedNoteNumber; }
        }

        #region GenerateNote
        public void GenerateNote(int noteColor)
        {
            GenerateLocalNote(noteColor);
            photonView.RPC(nameof(GenerateRemoteNote), RpcTarget.Others, noteColor, generatedNoteNumber);
        }

        private void GenerateLocalNote(int noteColor)
        {
            generatedNoteNumber++;
            Vector3 generatePosition = Camera.main.transform.position + noteGeneratingOffset;
            GameObject note = Generate(noteColor, generatePosition);
            //latestLocalGenratedNoteID = note.GetComponent<Note>().NoteID;
            latestLocalGenratedNoteID = generatedNoteNumber;
        }

        [PunRPC]
        private void GenerateRemoteNote(int noteColor, int generatedNum)
        {
            generatedNoteNumber = generatedNum;
            GameObject note = Generate(noteColor, Vector3.zero);
        }

        private GameObject Generate(int noteColor, Vector3 position)
        {
            GameObject note = Instantiate(CheckNote(noteColor), position, Quaternion.identity);
            return note;
        }

        private GameObject CheckNote(int noteColorNumber)
        {
            GameObject note = orangeNote;
            switch(noteColorNumber)
            {
                case 0:
                    note = orangeNote;
                    break;
                case 1:
                    note = pinkNote;
                    break;
                case 2:
                    note = greenNote;
                    break;
                default:
                    note = orangeNote;
                    break;
            }
            return note;
        }
        #endregion

        public void AligneNote()
        {
            int targetWhiteboardID = GameObject.FindObjectOfType<WhiteboardManager>().TargetWhiteboardID;
            photonView.RPC(nameof(Align), RpcTarget.All, latestLocalGenratedNoteID, targetWhiteboardID);
        }

        [PunRPC]
        private void Align(int targetNoteID, int targetWhiteboardID)
        {
            int noteNumberInTarget = 0;
            Whiteboard targetWhiteboard = null;

            Whiteboard[] whiteboards = GameObject.FindObjectsOfType<Whiteboard>();
            foreach (Whiteboard whiteboard in whiteboards)
            {
                if (whiteboard.WhiteboardID == targetWhiteboardID)
                {
                    Debug.Log("target whiteboard is " + targetWhiteboardID);
                    noteNumberInTarget = whiteboard.CountNoteInChild();
                    targetWhiteboard = whiteboard;
                    break;
                }
            }

            Note[] notes = Note.FindObjectsOfType<Note>();
            foreach(Note note in notes)
            {
                if(note.NoteID==targetNoteID && targetWhiteboard != null)
                {
                    note.transform.rotation = targetWhiteboard.transform.rotation;
                    note.transform.parent = targetWhiteboard.transform;

                    int xindex = noteNumberInTarget % 5;
                    int yindex = noteNumberInTarget / 5;

                    note.transform.localPosition = noteInitialAlignPosition + xoffset * xindex + yoffset * yindex;
                }
            }
        }
    }
}