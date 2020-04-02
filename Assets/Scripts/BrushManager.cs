using UnityEngine;
using UnityEngine.UI;

namespace CasualMeeting
{
    public class BrushManager : MonoBehaviour
    {
        private Slider brushScaleSlider;
        private Image brushImage;
        private RectTransform brushImageRect;

        private Drawer drawer;

        void Start()
        {
            brushScaleSlider = GameObject.Find("BrushScaleSlider").GetComponent<Slider>();

            //set visualized brush
            brushImage = GameObject.Find("BrushImage").GetComponent<Image>();
            brushImage.color = Color.black;
            brushImageRect = brushImage.GetComponent<RectTransform>();
            brushImageRect.sizeDelta = new Vector2(14f, 14f);
        }

        public void BrushColorSwitcher(int buttonNumber)
        {
            drawer = GameObject.FindObjectOfType<Drawer>();
            drawer.EraserOff();
            switch(buttonNumber)
            {
                case 0:
                    drawer.BrushColor = Color.black;
                    brushImage.color = Color.black;
                    break;
                case 1:
                    drawer.BrushColor = Color.red;
                    brushImage.color = Color.red;
                    break;
                case 2:
                    drawer.BrushColor = Color.green;
                    brushImage.color = Color.green;
                    break;
                case 3:
                    drawer.BrushColor = Color.blue;
                    brushImage.color = Color.blue;
                    break;
                case 4:
                    drawer.EraserOn();
                    drawer.BrushColor = drawer.DrawTargetObject.ResetColor;
                    brushImage.color = drawer.DrawTargetObject.ResetColor;
                    break;
                default:
                    drawer.BrushColor = Color.black;
                    brushImage.color = Color.black;
                    break;
            }
        }

        public void BrushScaler()
        {
            drawer = GameObject.FindObjectOfType<Drawer>();
            float sliderValue = brushScaleSlider.value;

            drawer.BrushWidth = (int)sliderValue;
            float brushImageSize = sliderValue * 12f + 5f;
            brushImageRect.sizeDelta = new Vector2(brushImageSize, brushImageSize);
        }

    }
}
