using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;

public class TurnBasedAgent : Agent
{
    public GameManager gm;
    public TurnManager tm;
    public LittleNightmare ln;
    int pAmount, eAmount;
    MouseAgent ma;
    bool cheeseGet = false;

    public override void OnEpisodeBegin()
    {
        // Reset the game state at the beginning of each episode
        if (PlayerPrefs.GetInt("Cheese") == 1)
            cheeseGet = true;
        else
            cheeseGet = false;

        if (GameObject.FindGameObjectWithTag("Mouse"))
            ma = GameObject.FindGameObjectWithTag("Mouse").GetComponent<MouseAgent>();
        Debug.Log("OnEpisodeBegin");
        if (!gm)
            gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.GameReset();
        if (!tm)
            tm = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();

        pAmount = gm.PlayerAmount();
        eAmount = gm.EnemyAmount();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Add game state information to the observation
        // For example, player health, enemy health, etc.

        for (int i = 0; i < pAmount; i++)
        {
            float h, t;
            bool g, b, d;
            (h, t, g, b, d) = gm.PlayerState(i);
            sensor.AddObservation(h);
            sensor.AddObservation(t);
            sensor.AddObservation(g);
            sensor.AddObservation(b);
            sensor.AddObservation(d);
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

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (ln.AgentTurnStart())
        {
            Debug.Log("OnActionReceived??? " + actions);
            base.OnActionReceived(actions);
            // Convert the action values to discrete actions
            int action = actions.DiscreteActions[0];

            // Perform the selected action
            
            switch (action)
            {
                case 0:
                    // Attack
                    // Implement attack logic here
                    ln.AgentBladeAttack("Middle",0);
                    break;
                case 1:
                    ln.AgentBladeAttack("Lower", 0);
                    break;
                case 2:
                    ln.AgentBladeAttack("Upper", 0);
                    break;
                case 3:
                    ln.AgentFistAttack("Middle", 0);
                    break;
                case 4:
                    ln.AgentFistAttack("Lower", 0);
                    break;
                case 5:
                    ln.AgentFistAttack("Upper", 0);
                    break;
                //case 6:
                //    ln.AgentMagicAttack(3);
                //    break;
            }

            ln.AgentTurnEnd();
        }

        if (IsGameOver())
        {
            // Provide a reward based on the game state and the action taken
            float reward = CalculateReward();
            AddReward(reward);

            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal") + 1);
        
    }

    private float TurnEndReward()
    {
        float score = 0;
        for (int i = 0; i < pAmount; i++)
        {
            float pHealth = gm.PlayerState(i).Item1;
            float pToughness = gm.PlayerState(i).Item2;
            if (pHealth <= 1)
            {
                score -= 1.5f*(1 - pHealth);
            }
            if (pToughness <= 1)
            {
                score -= 1 - pToughness;
            }
        }

        for (int i = 0; i < eAmount; i++)
        {
            float eHealth = gm.EnemyState(i).Item1;
            float eToughness = gm.EnemyState(i).Item1;
            bool eBreak = gm.EnemyState(i).Item4;
            if (eHealth <= 1)
            {
                score += 1.5f * (1 - eHealth);
            }
            if (eToughness <= 1)
            {
                score += 1 - eToughness;
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
            float pHealth = gm.PlayerState(i).Item1;
            if (pHealth <= 1)
            {
                score -= 1-pHealth;
            }
        }

        for (int i = 0; i < eAmount; i++)
        {
            float eHealth = gm.EnemyState(i).Item1;
            if (eHealth <= 0)
            {
                score += 14;
            }
        }
        return score;
    }

    private bool IsGameOver()
    {
        // Check if the game is over
        return false; // Replace with actual game over condition
    }

    public void AgentVictory()
    {
        float reward = 100 * (10 - tm.RoundNumber());
        if (reward < 0)
            reward = 0;
        AddReward(reward);

        // Provide a reward based on the game state and the action taken
        float reward2 = CalculateReward();
        AddReward(reward2);
        EndEpisode();
    }

    public void AgentDefeat()
    {
        float reward = 100*(0+tm.RoundNumber());
        AddReward(-reward);

        float reward2 = CalculateReward();
        AddReward(reward2);
        EndEpisode();
        ma.AgentDefeat();
        Debug.Log("EndEpisode");
    }

    public void UnLoadScene()
    {
        SceneManager.UnloadSceneAsync("Fight AgentTest");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out GameOver gameOver))
            Debug.Log("GameOver");
    }
}
