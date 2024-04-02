using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;



public class Agent_Level3 : Agent
{
    public static bool getCheese = false;

    [SerializeField] private Transform CheeseTransform;
    [SerializeField] private Transform CatTransform;
    [SerializeField] private Transform GoalTransform;

    [SerializeField] private Transform[] waypoints;

    string fileName = "";


    
    private int pointsIndex;

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

        agentRb.transform.position = new Vector2(-3.5018f, 3.42f);


        CatTransform.position = new Vector2(3.407f, -1.509f);

        pointsIndex = 0;


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
        sensor.AddObservation(agentRb.position);

        sensor.AddObservation(CheeseTransform.position);

        sensor.AddObservation(CatTransform.position);

        sensor.AddObservation(GoalTransform.position);

        sensor.AddObservation(getCheese);


       
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
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


    }

    void Start()
    {
        CatTransform.position = waypoints[pointsIndex].transform.position;


    }

    void Update()
    {

        if (pointsIndex <= waypoints.Length - 1)
        {

            CatTransform.transform.position = Vector2.MoveTowards(CatTransform.transform.position, waypoints[pointsIndex].transform.position, 0.4f * Time.deltaTime);


            if (Vector2.Distance(CatTransform.transform.position, waypoints[pointsIndex].transform.position) < 0.001f)
            {
                pointsIndex += 1;

            }

            if (pointsIndex == waypoints.Length)
            {
                pointsIndex = 0;
  
            }
        }

    }



    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Cheese")
        {

            AddReward(50f);
            getCheese = true;
            other.gameObject.SetActive(false);
            count_getCheese += 1;


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
            SetReward(-999f);
            getReward = GetCumulativeReward();
            count_coll_cat += 1;

            Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = " + count_down + " Move right = " + count_right + " Move left = " + count_left + " Reward = " + getReward + " Get Cheese or not = " +getCheese + " Collide with cat = " + count_coll_cat);
            Application.logMessageReceived -= Log;
            EndEpisode();



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
            SetReward(50f);
            getReward = GetCumulativeReward();
            Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = " + count_down + " Move right = " + count_right + " Move left = " + count_left + " Reward = " + getReward + " Get Cheese or not = " + getCheese + " Collide with cat = " + count_coll_cat);
            Application.logMessageReceived -= Log;
            EndEpisode();
        }




    }
}
