using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasualMeeting
{
    public class AvatarManager : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject avatar;

        private Transform cameraTransform;

        void Start()
        {
            if (GameObject.FindObjectOfType<SyncManager>().IsNreal)
            {
                cameraTransform = Camera.main.GetComponent<Transform>();

                if (photonView.IsMine)
                {
                    avatar.transform.parent = cameraTransform;
                    avatar.transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }

}
