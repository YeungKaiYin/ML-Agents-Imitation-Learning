using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
public class Agent_Level2 : Agent
{
    bool isPaused=false;
    public GameObject fightScene;
    public AgentActiveContoller aac;
    public GameManager gm;
    public CheeseStatus cs;
    public static bool getCheese = false;
    public Sensor s1;

    [SerializeField] private Transform CheeseTransform;
    [SerializeField] private Transform CatTransform1;
    [SerializeField] private Transform CatTransform2;
    [SerializeField] private Transform CatTransform3;
    [SerializeField] private Transform CatTransform4;
    [SerializeField] private Transform GoalTransform;

    string fileName = "";


    private Rigidbody2D agentRb;

    int total_move;
    int count_episode;
    int count_up;
    int count_down;
    int count_right;
    int count_left;
    int count_getCheese;
    float getReward;
    int count_coll_cat;



    public override void Initialize()
    {

        agentRb = GetComponent<Rigidbody2D>();


        count_episode = 0;


        fileName = Application.dataPath + "/Logfile.txt";

    }
    public void Log(string msg, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(fileName, true);

        tw.WriteLine(msg);

        tw.Close();
    }


    public override void OnEpisodeBegin()
    {
        aac.PauseBattleAgent();
        agentRb.transform.position = new Vector2(-3.53f, 3.53f);

        total_move = 0;
        count_up = 0;
        count_down = 0;
        count_right = 0;
        count_left = 0;
        count_episode += 1;

        getCheese = false;
        CheeseTransform.gameObject.SetActive(true);


        Application.logMessageReceived += Log;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentRb.transform.position);

        sensor.AddObservation(CheeseTransform.transform.position);

        sensor.AddObservation(CatTransform1.transform.position);

        sensor.AddObservation(CatTransform2.transform.position);
        
        sensor.AddObservation(CatTransform3.transform.position);
        
        sensor.AddObservation(CatTransform4.transform.position);
        
        sensor.AddObservation(GoalTransform.transform.position);

        sensor.AddObservation(getCheese);

        sensor.AddObservation(gameObject.transform.position);
        sensor.AddObservation(s1.GetContactPoint());
    }

    public void IsPaused(bool tf)
    {
        isPaused = tf;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isPaused)
        {
            // Agent is paused, do not execute actions
            return;
        }

        int movement = actions.DiscreteActions[0];

        float speed = 0.6f;

        if (movement == 0)
        {

            agentRb.velocity += new Vector2(0, 1 * speed);
            count_up += 1;
            total_move += 1;
        }
        if (movement == 1)
        {
            agentRb.velocity += new Vector2(0, -1 * speed);

            count_down += 1;
            total_move += 1;
        }
        if (movement == 2)
        {

            agentRb.velocity += new Vector2(-1 * speed, 0);
            count_left += 1;
            total_move += 1;
        }
        if (movement == 3)
        {

            agentRb.velocity += new Vector2(1 * speed, 0);
            count_right += 1;
            total_move += 1;
        }
        if (movement == 4)
        {

            agentRb.velocity = new Vector2(0, 0);

        }
    }



    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W))
        {
            discreteActions[0] = 0;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActions[0] = 2;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            discreteActions[0] = 3;
        }
        else
        {
            discreteActions[0] = 4;
        }
    }

    void CatStun(bool tf)
    {
        if (tf)
        {
            StartCoroutine(Stunning());
            foreach (Collider2D c in GetComponents<Collider2D>())
            {
                c.enabled = false;
            }
        }

    }

    IEnumerator Stunning()
    {
        yield return new WaitForSeconds(3);
        CatStun(false);
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = true;
        }
    }

    Vector2 contactPoint;
    public void ReceiveContactPoint(Vector2 pos)
    {
        contactPoint = pos;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Cheese")
        {
            AddReward(50f);
            getCheese = true;
            other.gameObject.SetActive(false);
            count_getCheese += 1;
            cs.CheeseGet();
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.GetComponent<EdgeCollider2D>())
        {

            AddReward(-1f);

        }

        if (other.gameObject.tag == "Cat")
        {
            if(!getCheese)
            {
                SetReward(-999f);
                getReward = GetCumulativeReward();
                count_coll_cat += 1;
                Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = " + count_down + " Move right = " + count_right + " Move left = " + count_left + " Reward = " + getReward + " Get Cheese or not = " + getCheese + " Collide with cat = " + count_coll_cat);
                Application.logMessageReceived -= Log;
                EndEpisode();
            }
            else
            {
                aac.ResumeBattleAgent();
                aac.PauseMazeAgent();
            }

        }
        if (other.gameObject.tag == "Goal" && getCheese == true)
        {
            SetReward(999f);
            getReward = GetCumulativeReward();
            Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = " + count_down + " Move right = " + count_right + " Move left = " + count_left + " Reward = " + getReward + " Get Cheese or not = " + getCheese + " Collide with cat = " + count_coll_cat);
            Application.logMessageReceived -= Log;
            EndEpisode();
        }
        else if (other.gameObject.tag == "Goal" && getCheese == false)
        {
            AddReward(50f);
            getReward = GetCumulativeReward();
            Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = " + count_down + " Move right = " + count_right + " Move left = " + count_left + " Reward = " + getReward + " Get Cheese or not = " + getCheese + " Collide with cat = " + count_coll_cat);
            Application.logMessageReceived -= Log;
            EndEpisode();
        }
    }

}
