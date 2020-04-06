using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CasualMeeting
{
    public class WhiteboardManager : MonoBehaviourPunCallbacks
    {
        private int targetWhiteboardID = 1;

        private int generatedWhiteboardNumber = 0;

        //parameter for whiteboard position
        private static Vector3 initialPositionNreal = new Vector3(0, 0, 50);
        private static Vector3 initialRotationNreal = new Vector3(-90f, 0, 0);
        private Vector3 offset = new Vector3(45f, 0, 0);

        private Vector3 switcherInitialPosition = new Vector3(-30, -135, 0);
        private Vector3 switcherOffset = new Vector3(0, -35f, 0);

        [SerializeField]
        private GameObject whiteboardPrefab;
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private GameObject cameraSwitcherPrefab;
        [SerializeField]
        private Canvas canvas;

        private SyncManager syncManager = null;

        public int GeneratedWhiteboardNumber
        {
            get { return generatedWhiteboardNumber; }
        }

        public int TargetWhiteboardID
        {
            get { return targetWhiteboardID; }
        }

        private void Start()
        {
            syncManager = GameObject.FindObjectOfType<SyncManager>();
            GenerateWhiteboard();
        }

        public void GenerateWhiteboard()
        {
            GenerateLocal();

            photonView.RPC(nameof(GenerateRemote), RpcTarget.Others, generatedWhiteboardNumber);
        }

        private void GenerateLocal()
        {
            generatedWhiteboardNumber++;
            Debug.Log("generatedNum = " + generatedWhiteboardNumber);
            GameObject board = Instantiate(whiteboardPrefab, Vector3.zero, Quaternion.identity);
            AlignWhiteboard(board);
            if(!syncManager.IsNreal)
            {
                mainCamera.GetComponent<CameraManager>().MoveCamera(board.transform.position);
            }
            targetWhiteboardID = generatedWhiteboardNumber;

            GenerateSwitcher();
        }

        [PunRPC]
        private void GenerateRemote(int num)
        {
            generatedWhiteboardNumber = num;
            GameObject board = Instantiate(whiteboardPrefab, Vector3.zero, Quaternion.identity);

            AlignWhiteboard(board);

            GenerateSwitcher();
        }

        private void GenerateSwitcher()
        {
            GameObject cameraSwitcher = Instantiate(cameraSwitcherPrefab) as GameObject;
            cameraSwitcher.GetComponent<Transform>().position = switcherInitialPosition + (generatedWhiteboardNumber - 1) * switcherOffset;
            cameraSwitcher.transform.SetParent(canvas.transform, false);

            cameraSwitcher.GetComponentInChildren<Text>().text = generatedWhiteboardNumber.ToString();

            Button button = cameraSwitcher.GetComponent<Button>();
            this.AddButtonEvent(button, generatedWhiteboardNumber);
        }

        private void AddButtonEvent(Button button, int num)
        {
            button.onClick.AddListener(() =>
            {
                this.SwitchCamera(num);
            });
        }

        private void SwitchCamera(int num)
        {
            Whiteboard[] boards = GameObject.FindObjectsOfType<Whiteboard>();
            foreach(Whiteboard board in boards)
            {
                if(board.WhiteboardID == num)
                {
                    targetWhiteboardID = num;
                    mainCamera.GetComponent<CameraManager>().MoveCamera(board.transform.position);
                }
            }
        }

        private void AlignWhiteboard(GameObject board)
        {
            Vector3 initialPosition = Vector3.zero;
            if(syncManager.IsNreal)
            {
                initialPosition = initialPositionNreal;
                board.transform.rotation = Quaternion.Euler(-90, 0, 0);
            }
            board.transform.position = initialPosition + (generatedWhiteboardNumber - 1) * offset;
            //board.transform.position = (generatedWhiteboardNumber - 1) * offset;
            
        }
    }
}
