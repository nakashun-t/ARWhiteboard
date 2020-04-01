using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SyncManager : MonoBehaviourPunCallbacks
{
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
    }
}
