using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CasualMeeting
{
    public class CameraManager : MonoBehaviour
    {
        private Vector3 cameraOffset = new Vector3(0, 26f, 0);

        public void MoveCamera(Vector3 targetWhiteboardPosition)
        {
            this.transform.position = targetWhiteboardPosition + cameraOffset;
        }
    }
}
