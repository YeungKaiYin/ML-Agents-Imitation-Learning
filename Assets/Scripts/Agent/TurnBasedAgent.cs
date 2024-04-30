using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;

public class TurnBasedAgent : Agent
{
    [SerializeField]
    private int episodeCount = 0;
    //[SerializeField]
    //private int episodeLimit = 10000;
    public bool isPaused = true;
    public GameManager gm;
    public TurnManager tm;
    public LittleNightmare ln;
    int pAmount, eAmount;
    //MouseAgent ma;
    //bool cheeseGet = true;
    public AgentActiveContoller aac;
    public LineRenderer rewardLineRenderer, cheeseRemainLineRenderer, stepLineRenderer;
    public List<Vector3> r_curvePoints = new List<Vector3>();
    public List<Vector3> c_curvePoints = new List<Vector3>();
    public List<Vector3> s_curvePoints = new List<Vector3>();
    public float count = 0;
    public float xDistance = 0.5f;
    int step = 0;
    int cheeseCount = 3;
    //public Status sm;

    private void Awake()
    {
        //try
        //{
        //    if (GameObject.FindGameObjectWithTag("Mouse"))
        //        ma = GameObject.FindGameObjectWithTag("Mouse").GetComponent<MouseAgent>();
        //}
        //catch (Exception e)
        //{
        //    ma = null;
        //    UnityEngine.Debug.Log(e);
        //}

        if (!gm)
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        //gm.GameReset();
        if (!tm)
            tm = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();
        //if (sm == null)
        //    sm = GetComponent<Status>();

        pAmount = gm.PlayerAmount();
        eAmount = gm.EnemyAmount();
    }

    public override void OnEpisodeBegin()
    {
        // Reset the game state at the beginning of each episode
        ln.ModeSwitch("Stand");
        cheeseCount = 3;
        step = 0;
        if (aac == null)
            isPaused = false;
        else
            isPaused = true;

        UnityEngine.Debug.Log("OnEpisodeBegin");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Add game state information to the observation
        // For example, player health, enemy health, etc.

        for (int i = 0; i < pAmount; i++)
        {
            int c;
            float h, t;
            bool g, b, d,myturn;
            (c, h, t, g, b, d,myturn) = gm.PlayerState(i);
            sensor.AddObservation(c);
            sensor.AddObservation(h);
            sensor.AddObservation(t);
            sensor.AddObservation(g);
            sensor.AddObservation(b);
            sensor.AddObservation(d);
            sensor.AddObservation(myturn);
        }
        for (int i = 0; i < eAmount; i++)
        {
            float h, t;
            bool g, b, d;
            (h, t, g, b, d) = gm.EnemyState(i);
            sensor.AddObservation(h);
            sensor.AddObservation(t);
            sensor.AddObservation(g);
            sensor.AddObservation(b);
            sensor.AddObservation(d);
        }
    }

    public override void Initialize()
    {
        ActionSpec actionSpec = ActionSpec.MakeDiscrete(7);
    }

    public void IsPaused(bool tf)
    {
        this.isPaused = tf;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isPaused)
        {
            // Agent is paused, do not execute actions
            return;
        }

        if (ln.AgentTurnStart())
        {
            UnityEngine.Debug.Log("OnActionReceived??? " + actions);
            base.OnActionReceived(actions);
            // Convert the action values to discrete actions
            int action = actions.DiscreteActions[0];

            // Perform the selected action
            
            switch (action)
            {
                case 0:
                    // Attack
                    // Implement attack logic here
                    ln.ModeSwitch("Stand");
                    ln.AgentBladeAttack("Middle",0);
                    UnityEngine.Debug.Log("agent action: 0");
                    break;
                case 1:
                    ln.ModeSwitch("Stand");
                    ln.AgentBladeAttack("Lower", 0);
                    UnityEngine.Debug.Log("agent action: 1");
                    break;
                case 2:
                    ln.ModeSwitch("Stand");
                    ln.AgentBladeAttack("Upper", 0);
                    UnityEngine.Debug.Log("agent action: 2");
                    break;
                case 3:
                    ln.ModeSwitch("Cheese");
                    ln.AgentBladeAttack("Cheese_CatBall", 0);
                    UnityEngine.Debug.Log("agent action: 3");
                    cheeseCount--;
                    break;
                case 4:
                    ln.ModeSwitch("Cheese");
                    ln.AgentBladeAttack("Cheese_CatTeaserWand", 0);
                    UnityEngine.Debug.Log("agent action: 4");
                    cheeseCount--;
                    break;
                case 5:
                    ln.ModeSwitch("Cheese");
                    ln.AgentBladeAttack("Cheese_CatKibble", 0);
                    UnityEngine.Debug.Log("agent action: 5");
                    cheeseCount--;
                    break;
                    //case 6:
                    //    ln.AgentMagicAttack(3);
                    //    break;
                    //UnityEngine.Debug.Log("gmCheeseCount :" + gm.PlayerState(0).Item1);
                    //UnityEngine.Debug.Log("smCheeseCount :" + sm.CheeseCount());
            }
            step++;
            ln.AgentTurnEnd();
            //AddReward(TurnEndReward());
        }

        //if (IsGameOver())
        //{
        //    // Provide a reward based on the game state and the action taken
        //    float reward = CalculateReward();
        //    AddReward(reward);

        //    EndEpisode();
        //}
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;


        if (Input.GetKey(KeyCode.Q))
        {
            discreteActions[0] = 0;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            discreteActions[0] = 2;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActions[0] = 3;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActions[0] = 4;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            discreteActions[0] = 5;
        }
    }

    public void AgentTurnEnd(bool agentTurn)
    {
        AddReward(TurnEndReward(agentTurn));
    }

    private float TurnEndReward(bool agentTurn)
    {
        float score = 0;
        if(!agentTurn)
        for (int i = 0; i < pAmount; i++)
        {
            float pHealth = gm.PlayerState(i).Item2;
            float pToughness = gm.PlayerState(i).Item3;
            if (pHealth <= 1)
            {
                score -= 1.5f*(1 - pHealth);
            }
            if (pToughness <= 1)
            {
                score -= 0.5f*(1 - pToughness);
            }
        }

        if(agentTurn)
        for (int i = 0; i < eAmount; i++)
        {
            float eHealth = gm.EnemyState(i).Item1;
            float eToughness = gm.EnemyState(i).Item1;
            bool eBreak = gm.EnemyState(i).Item4;
            if (eHealth <= 1)
            {
                score += 3f * (1 - eHealth);
            }
            if (eToughness <= 1)
            {
                score += 3f * (1 - eToughness);
            }
            if (eBreak)
                score += 3;
        }
        return score;
    }

    private float CalculateReward()
    {
        // Calculate the reward based on the game state and the action taken
        float score = 0;
        for (int i = 0; i < pAmount; i++)
        {
            int cheeseCount = gm.PlayerState(i).Item1;
            score += (7 * cheeseCount);
            float pHealth = gm.PlayerState(i).Item2;
            if (pHealth <= 1)
            {
                score -= 14*(1-pHealth);
            }
        }

        for (int i = 0; i < eAmount; i++)
        {
            float eHealth = gm.EnemyState(i).Item1;
            score += 14 * (1 - eHealth);      
        }
        return score;
    }

    private bool IsGameOver()
    {
        // Check if the game is over
        return false; // Replace with actual game over condition
    }

    public int winCount = 0;
    public int loseCount=0;
    public void AgentVictory()
    {
        winCount++;
        //UnityEngine.Debug.Log("Win :"+winCount+"\n"+ "Lose :" + loseCount+"\nWin Rate :"+(winCount/loseCount));
        float reward = (float)(0.5 * (10 - (float)tm.RoundNumber()));
        if (reward < 0)
            reward = 0;
        AddReward(reward);

        // Provide a reward based on the game state and the action taken
        float reward2 = CalculateReward();
        AddReward(reward2);

        float rewardcheck = GetCumulativeReward();
        if (rewardcheck < 65)
            SetReward(60);
        UnityEngine.Debug.Log("Current reward: " + rewardcheck);
        r_curvePoints.Add(new Vector3(count, rewardcheck, 10.0f));
        c_curvePoints.Add(new Vector3(count, cheeseCount, 10.0f));
        s_curvePoints.Add(new Vector3(count, step, 10.0f));
        count += xDistance;
        rewardLineRenderer.positionCount = r_curvePoints.Count;
        rewardLineRenderer.SetPositions(r_curvePoints.ToArray());
        cheeseRemainLineRenderer.positionCount = c_curvePoints.Count;
        cheeseRemainLineRenderer.SetPositions(c_curvePoints.ToArray());
        stepLineRenderer.positionCount = s_curvePoints.Count;
        stepLineRenderer.SetPositions(s_curvePoints.ToArray());

        EndEpisode();
        episodeCount++;

        if(aac)
        {
            aac.BattleRewardToMaze(7);
            aac.ResumeMazeAgent();
            aac.PauseBattleAgent();
        }
    }

    public void AgentDefeat()
    {
        loseCount++;
        UnityEngine.Debug.Log("Win :" + winCount + "\n" + "Lose :" + loseCount + "\nWin Rate :" + (winCount / loseCount));
        float reward = (float)(0.2 * (10 - (float)tm.RoundNumber()));
        AddReward(reward);

        float reward2 = CalculateReward();
        AddReward(reward2);

        float rewardcheck = GetCumulativeReward();
        if (rewardcheck > 60)
            SetReward(60);
        UnityEngine.Debug.Log("Current reward: " + rewardcheck);
        r_curvePoints.Add(new Vector3(count, rewardcheck, 10.0f));
        c_curvePoints.Add(new Vector3(count, cheeseCount, 10.0f));
        s_curvePoints.Add(new Vector3(count, step, 10.0f));
        count += xDistance;
        rewardLineRenderer.positionCount = r_curvePoints.Count;
        rewardLineRenderer.SetPositions(r_curvePoints.ToArray());
        cheeseRemainLineRenderer.positionCount = c_curvePoints.Count;
        cheeseRemainLineRenderer.SetPositions(c_curvePoints.ToArray());
        stepLineRenderer.positionCount = s_curvePoints.Count;
        stepLineRenderer.SetPositions(s_curvePoints.ToArray());

        EndEpisode();
        episodeCount++;
        //ma.AgentDefeat();
        //Debug.Log("EndEpisode");
        //if (ma != null)
        //    UnLoadScene();
        aac.BattleRewardToMaze(-7);
        aac.ResumeMazeAgent();
        aac.PauseBattleAgent();
    }

    public void UnLoadScene()
    {
        SceneManager.UnloadSceneAsync("Fight AgentTest");
    }

    
}
