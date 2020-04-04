using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void SetActive(string name, bool b)
    {
        Canvas canvas = this.GetComponent<Canvas>();
        foreach(Transform child in this.GetComponent<Canvas>().transform)
        {
            if(child.name == name)
            {
                child.gameObject.SetActive(b);
                return;
            }
        }
        Debug.LogWarning("not found object name " + name);
    }

    public void ClickedNoteButton()
    {
        SetActive("OrangeNoteButton", false);
        SetActive("PinkNoteButton", false);
        SetActive("GreenNoteButton", false);
        SetActive("AlignNoteButton", true);
    }
    public void ClickedAlignButton()
    {
        SetActive("OrangeNoteButton", true);
        SetActive("PinkNoteButton", true);
        SetActive("GreenNoteButton", true);
        SetActive("AlignNoteButton", false);
    }
}
