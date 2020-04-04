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
        [SerializeField]
        private GameObject dragHandle;

        private int generatedNoteNumber = 0;

        private GameObject targetWhiteboard;
        private Vector3 whiteBoardPosition;
        private Vector3 whiteBoardRotation;

        //note(handle) position parameter
        private Vector3 noteGeneratingOffset = new Vector3(0, -2.6f, 0);
        private Vector3 localNoteInitialPosition = new Vector3(-4f, 0.1f, 4f);
        private Vector3 xoffset = new Vector3(1.3f, 0, 0);
        private Vector3 yoffset = new Vector3(0, 0, -1.2f);

        public void GenerateNote(int noteColor)
        {
            GenerateLocalNote(noteColor);
            photonView.RPC(nameof(GenerateRemoteNote), RpcTarget.Others, noteColor, generatedNoteNumber);
        }

        public void AligneNote()
        {

        }

        private void GenerateLocalNote(int noteColor)
        {
            generatedNoteNumber++;
            Vector3 generatePosition = Camera.main.transform.position + noteGeneratingOffset;
            Generate(noteColor, generatePosition);

        }

        [PunRPC]
        private void GenerateRemoteNote(int noteColor, int generatedNum)
        {
            generatedNoteNumber = generatedNum;
            Generate(noteColor, Vector3.zero);
        }

        private void Generate(int noteColor, Vector3 position)
        {
            GameObject note = Instantiate(CheckNote(noteColor), position, Quaternion.identity);
            note.tag = "DrawableObject";
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


    }
}