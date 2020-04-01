using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CasualMeeting
{
    public class Drawer : MonoBehaviourPunCallbacks
    {
        //brush status
        private Color brushColor = Color.black;
        private int brushWidth = 3;
        private bool eraserFlag = false;

        //
        private DrawableObject drawTargetObject;
        private int previousDrawTargetObjectID;

        private Vector2 defaultTexCoord = Vector2.zero;
        private Vector2 previousTexCoord;


        public Color BrushColor
        {
            set { this.brushColor = value; }
        }

        public int BrushWidth
        {
            set { this.brushWidth = value; }
        }

        public DrawableObject DrawTargetObject
        {
            get { return this.drawTargetObject; }
        }

        public void EraserOn()
        {
            eraserFlag = true;
            return;
        }
        public void EraserOff()
        {
            eraserFlag = false;
            return;
        }


        void Start()
        {
            drawTargetObject = GameObject.FindObjectOfType<DrawableObject>();
        }

        void Update()
        {
            bool mouseDown = Input.GetMouseButton(0);
            if (mouseDown)
            {
                bool manipulatingUI = false;
#if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    manipulatingUI = true;
                    return;
                }
#else
                if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    manipulatingUI = true;
                    return;
                }
#endif

                if (!manipulatingUI)
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        if (hitInfo.collider != null && hitInfo.collider.tag == "DrawableObject")
                        {
                            var collidedObject = hitInfo.transform.GetComponent<DrawableObject>();
                            int drawTargetObjectID = collidedObject.DrawableObjectID;
                            drawTargetObject = collidedObject;

                            Vector2 currentTexCoord = hitInfo.textureCoord;
                            if (eraserFlag)
                            {
                                brushColor = drawTargetObject.ResetColor;
                            }
                            if (drawTargetObjectID != previousDrawTargetObjectID)
                            {
                                previousTexCoord = defaultTexCoord;
                            }

                            photonView.RPC(nameof(ObjectDrawer), RpcTarget.All, currentTexCoord, previousTexCoord, brushWidth, brushColor, drawTargetObjectID);

                            previousTexCoord = currentTexCoord;
                            previousDrawTargetObjectID = drawTargetObjectID;
                        }
                        else
                        {
                            Debug.LogWarning("If you want to draw using a RaycastHit, need set MeshCollider for object.");
                        }
                    }
                    else
                    {
                        previousTexCoord = defaultTexCoord;
                    }
                }
            }
            else if(!mouseDown)
            {
                previousTexCoord = defaultTexCoord;
            }
        }

        [PunRPC]
        private void ObjectDrawer(Vector2 currentTexCoord, Vector2 previoiusTexCoord, int width, Color color, int id)
        {
            GameObject[] drawableObjects = GameObject.FindGameObjectsWithTag("DrawableObject");
            foreach(GameObject drawable in drawableObjects)
            {
                if(drawable.GetComponent<DrawableObject>().DrawableObjectID == id)
                {
                    drawTargetObject = drawable.GetComponent<DrawableObject>();
                    drawTargetObject.Draw(currentTexCoord, previoiusTexCoord, width, color);
                }

            }
        }
    }
}
