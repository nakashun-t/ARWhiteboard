using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasualMeeting
{
    public class WhiteboardManager : MonoBehaviourPunCallbacks
    {
        private Camera camera;
        private Vector3 cameraOffset = new Vector3(0, 26f, 0);

        private int localGenerateNumber = 0;
        private int remoteGenerateNumber = 0;
        private int generatedWhiteboardNumber = 0;

        //whiteboard position variables
        private static Vector3 initialPosition = new Vector3(0, 0, 0);
        private Vector3 offset = new Vector3(45f, 0, 0);

        [SerializeField]
        private GameObject whiteboardPrefab;

        public int GeneratedWhiteboardNumber
        {
            get { return generatedWhiteboardNumber; }
        }

        private void Start()
        {
            camera = Camera.main;
            GenerateWhiteboard();
        }

        public void GenerateWhiteboard()
        {
            GenerateLocal();

            photonView.RPC(nameof(Generate), RpcTarget.Others, generatedWhiteboardNumber);
        }

        private void GenerateLocal()
        {
            generatedWhiteboardNumber++;
            Debug.Log("generatedNum = " + generatedWhiteboardNumber);
            GameObject board = Instantiate(whiteboardPrefab, Vector3.zero, Quaternion.identity);
            AlignWhiteboard(board);
            camera.transform.position = board.transform.position + cameraOffset;
        }

        [PunRPC]
        private void Generate(int num)
        {
            generatedWhiteboardNumber = num;
            GameObject board = Instantiate(whiteboardPrefab, Vector3.zero, Quaternion.identity);
            AlignWhiteboard(board);
        }

        private void AlignWhiteboard(GameObject board)
        {
            board.transform.position = (generatedWhiteboardNumber - 1) * offset;
        }
    }
}
