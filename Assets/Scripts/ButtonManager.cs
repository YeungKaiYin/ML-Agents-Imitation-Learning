using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public UnityEngine.UI.Button[] p_Button;
    public UnityEngine.UI.Button exit_Button;

    public void p_ButtonDisable()
    {
        foreach (UnityEngine.UI.Button b in p_Button)
            b.interactable = false;
    }

    public void p_ButtonActivate()
    {
        foreach (UnityEngine.UI.Button b in p_Button)
            b.interactable = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
