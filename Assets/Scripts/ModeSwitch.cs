using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public UnityEngine.UI.Button upper,middle, lower;
    ColorBlock theColor;
    public LittleNightmare ln;
    void Start()
    {
        tmpText.text = "Upper";
        ln = GameObject.FindGameObjectWithTag("Player").GetComponent<LittleNightmare>();
        //theColor = upper.colors;
        Middle();
    }

    public void TextChange()
    {
        
    }

    public void Upper()
    {
        upper.interactable = false;
        middle.interactable = true;
        lower.interactable = true;
        
        ln.ModeSwitch("Upper");
    }

    public void Middle()
    {
        upper.interactable = true;
        middle.interactable = false;
        lower.interactable = true;

        ln.ModeSwitch("Middle");
    }

    public void Lower()
    {
        upper.interactable = true;
        middle.interactable = true;
        lower.interactable = false;

        ln.ModeSwitch("Lower");
    }
}
