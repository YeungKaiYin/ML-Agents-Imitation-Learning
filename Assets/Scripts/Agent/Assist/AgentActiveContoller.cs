using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Policies;
using UnityEngine.UI;
using TMPro;

public class AgentActiveContoller : MonoBehaviour
{
    public BehaviorParameters mazeBp,battleBp;
    public TurnBasedAgent tba;
    public Agent_Empty_Map aem;
    public Agent_Level1 al1;
    public Agent_Level2 al2;
    public Agent_Level3 al3;
    public GameObject cam1, cam2;
    public LittleNightmare ln;
    public TextMeshProUGUI tmpText_HumanAI;
    bool human = true;
    public GameObject es,al3Mouse,al3Cat;

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
        Debug.Log("HumanOrAI :" + (human ? "Human" : "AI"));
    }

    public void Maze_HumanOrAI()
    {
        if (human)
        {
            tmpText_HumanAI.SetText("AI");
            human = false;
            if (al1||al2||al3||aem)
                mazeBp.BehaviorType = BehaviorType.InferenceOnly;
        }
        else
        {
            tmpText_HumanAI.SetText("Human");
            human = true;
            if (al1 || al2 || al3 || aem)
                mazeBp.BehaviorType = BehaviorType.HeuristicOnly;
        }
        Debug.Log("HumanOrAI :" + (human ? "Human" : "AI"));
    }

    public void PauseMazeAgent()
    {
        if (aem != null)
        {
            aem.IsPaused(true);
        }
        else if (al2)
            al2.IsPaused(true);
        else if (al3)
        {
            al3.IsPaused(true);
            //al3Mouse.SetActive(false);
            al3Cat.SetActive(false);
        }
            
        if (al1 || al2 || al3 || aem)
            cam2.SetActive(true);
        Debug.Log("PauseMazeAgent");
    }

    public void ResumeMazeAgent()
    {
        if (aem != null)
        {
            aem.IsPaused(false);
        }
        else if (al2)
            al2.IsPaused(false);
        else if (al3)
        {
            al3.IsPaused(false);
            al3Cat.SetActive(true);
        }
            
        if (al1 || al2 || al3 || aem)
            cam2.SetActive(false);

        Debug.Log("ResumeMazeAgent");
    }

    public void PauseBattleAgent()
    {
        if (tba != null)
            tba.IsPaused(true);
        Debug.Log("PauseBattleAgent");
    }

    public void ResumeBattleAgent()
    {
        if (tba != null)
            tba.IsPaused(false);
        Debug.Log("ResumeBattleAgent");
    }

    public void BattleRewardToMaze(int reward)
    {
        if (al2 != null)
            al2.AddReward(reward);
        else if (al3 != null)
            al3.AddReward(reward);
    }
}
