using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace CasualMeeting
{
    public class SyncManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private bool isNreal = false;

        private Vector3 userPos = Vector3.zero;

        public bool IsNreal
        {
            get { return isNreal; }
        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinOrCreateRoom("CasualMeetingRoom", new RoomOptions(), TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.Instantiate("Drawer", Vector3.zero, Quaternion.identity);

            if(isNreal)
            {
                int userCounter = PhotonNetwork.PlayerListOthers.Length;
                userPos.x = (float)userCounter * 8f;
                GameObject player = PhotonNetwork.Instantiate("Avatar", userPos, Quaternion.identity);
            }
        }
    }
}
