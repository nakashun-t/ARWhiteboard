using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasualMeeting
{
    public class WhiteboardManager : MonoBehaviourPunCallbacks
    {
        private int localGenerateNumber = 0;
        private int remoteGenerateNumber = 0;
        private int generatedWhiteboardNumber = 0;

        [SerializeField]
        private GameObject whiteboardPrefab;

        public int GeneratedWhiteboardNumber
        {
            get { return generatedWhiteboardNumber; }
        }

        private void Start()
        {
            GenerateWhiteboard();
        }

        public void GenerateWhiteboard()
        {
            GenerateLocal();

            photonView.RPC(nameof(Generate), RpcTarget.Others, localGenerateNumber + remoteGenerateNumber);
        }

        private void GenerateLocal()
        {
            localGenerateNumber++;
            generatedWhiteboardNumber = localGenerateNumber + remoteGenerateNumber;
            Debug.Log("generatedNum = " + generatedWhiteboardNumber);
            GameObject board = Instantiate(whiteboardPrefab, Vector3.zero, Quaternion.identity);
        }

        [PunRPC]
        private void Generate(int num)
        {
            remoteGenerateNumber++;
            generatedWhiteboardNumber = num;
            Instantiate(whiteboardPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
