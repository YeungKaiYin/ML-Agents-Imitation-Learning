using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Policies;

public class AgentActiveContoller : MonoBehaviour
{
    public BehaviorParameters mazeBp,battleBp;
    public TurnBasedAgent tba;
    public Agent_Level2 al2;
    public Agent_Level2 al3;

    public void PauseMazeAgent()
    {
        if (al2 != null)
            al2.IsPaused(true);
        Debug.Log("PauseMazeAgent");
    }

    public void ResumeMazeAgent()
    {
        if (al2 != null)
            al2.IsPaused(false);
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
