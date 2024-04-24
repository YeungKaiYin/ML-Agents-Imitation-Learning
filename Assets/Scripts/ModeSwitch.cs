using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModeSwitch : MonoBehaviour
{
    public TextMeshProUGUI tmpText_HumanAI;
    public UnityEngine.UI.Button upper,middle, lower,stand,cheese;
    ColorBlock theColor;
    public LittleNightmare ln;
    bool human = true;
    public Image sr_Middle, sr_Lower, sr_Upper;
    public Sprite ms_Middle, ms_Lower, ms_Upper, t_Middle, t_Lower, t_Upper;
    public Agent_Level1 al1;
    public Agent_Level2 al2;
    public Agent_Level3 al3;
    void Start()
    {
        //if(ln==null)
        //ln = GameObject.FindGameObjectWithTag("Player").GetComponent<LittleNightmare>();
        if(ln != null)
        ButtonActive();
        //theColor = upper.colors;
        //Middle();
    }

    public void TextChange()
    {
        
    }

    public void HumanOrAI()
    {
        if (human)
        {
            tmpText_HumanAI.SetText("AI");
            human = false;
            ln.HumanOrAI(false);
            
        }
        else
        {
            tmpText_HumanAI.SetText("Human");
            human = true;
            ln.HumanOrAI(true);
            
        }
        Debug.Log("HumanOrAI :" + (human?"Human":"AI"));
    }

    public void Maze_HumanOrAI()
    {
        if (human)
        {
            tmpText_HumanAI.SetText("AI");
            human = false;
            if(al1)
                al1.HumanOrAI(false);
            else if(al2)
                al2.HumanOrAI(false);
            else if (al3)
                al3.HumanOrAI(false);

        }
        else
        {
            tmpText_HumanAI.SetText("Human");
            human = true;
            if(al1)
                al1.HumanOrAI(true);
            else if(al2)
                al2.HumanOrAI(true);
            else if (al3)
                al3.HumanOrAI(true);

        }
        Debug.Log("HumanOrAI :" + (human ? "Human" : "AI"));
    }

    public void ButtonActive()
    {
        upper.interactable = true;
        middle.interactable = true;
        lower.interactable = true;
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

    public void Stand()
    {
        cheese.interactable = true;
        stand.interactable = false;

        ln.ModeSwitch("Stand");
        sr_Lower.sprite = ms_Lower;
        sr_Middle.sprite = ms_Middle;
        sr_Upper.sprite = ms_Upper;
    }

    public void Cheese()
    {
        stand.interactable = true;
        cheese.interactable = false;

        ln.ModeSwitch("Cheese");
        sr_Lower.sprite = t_Lower;
        sr_Middle.sprite = t_Middle;
        sr_Upper.sprite = t_Upper;
    }
}
