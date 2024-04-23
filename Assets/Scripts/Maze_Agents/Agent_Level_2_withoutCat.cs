using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using System;
using Unity.VisualScripting;
using Unity.Collections.LowLevel.Unsafe;

public class Agent_Level_2_withoutCat : Agent
{
    public static bool getCheese = false;

    [SerializeField] private Transform CheeseTransform;
    [SerializeField] private Transform GoalTransform;

   private Transform WallTransform;

    string fileName = "";


    private Rigidbody2D agentRb;

    public Sensor s2;
    int total_move;
    int count_episode;
    int count_up;
    int count_down;
    int count_right;
    int count_left;
    int count_getCheese;
    float getReward;
    int count_coll_cat;
    int count_not_move;
    Boolean hit_wall;
    int count_goalWithOutCheese;
    float distanceToCheese;
    float distanceToGoal;
    public override void Initialize()
    {

        agentRb = GetComponent<Rigidbody2D>();

        count_episode = 0;

        fileName = Application.dataPath + "/Level2_ModelTest.txt";

    }

    public override void OnEpisodeBegin()
    {
        agentRb.transform.position = new Vector2(-3.4748f, 3.515f);



        total_move = 0;
        count_up = 0;
        count_down = 0;
        count_right = 0;
        count_left = 0;
        count_episode += 1;
        count_not_move = 0;
        getCheese = false;
        hit_wall = false;
        count_goalWithOutCheese = 0;
        
        CheeseTransform.gameObject.SetActive(true);

        Application.logMessageReceived += Log;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentRb.position);

        if(getCheese == false)
        {
            
            sensor.AddObservation(CheeseTransform.transform.position);
            sensor.AddObservation(distanceToCheese);

        }else{
            sensor.AddObservation(getCheese == true);
            sensor.AddObservation(GoalTransform.transform.position);
            sensor.AddObservation(distanceToGoal);
        }
        
        if(hit_wall == true)
        {
            sensor.AddObservation(1f);
        }
        else
        {
            sensor.AddObservation(0f);
        }
        sensor.AddObservation(gameObject.transform.position);
        sensor.AddObservation(s2.GetContactPoint());
        
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

        distanceToCheese = Vector2.Distance(agentRb.transform.position, CheeseTransform.position);
        distanceToGoal = Vector2.Distance(agentRb.transform.position, GoalTransform.position);
        
        if(getCheese == false){

            if(distanceToCheese < 4f)
            {
                SetReward(1f);
            }
   
        }
        if(getCheese == true){
            

            if(distanceToGoal < 4f)
            {
                SetReward(1f);
            }
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
    Vector2 contactPoint;
    public void ReceiveContactPoint(Vector2 pos)
    {
        contactPoint = pos;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Cheese")
        {
            getCheese = true;
            other.gameObject.SetActive(false);
            SetReward(500);
            count_getCheese += 1;
        }




    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "cat1")
        {
            SetReward(-100f);
            getReward = GetCumulativeReward();
            count_coll_cat += 1;
            Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = " + count_down + " Move right = " + count_right + " Move left = " + count_left+ " Choose stay = "+ count_not_move + " Reward = " + getReward + " Get Cheese or not = " + getCheese + " Collide with cat = " + count_coll_cat + " Hit wall = " + hit_wall + " Goal without cheese = " + count_goalWithOutCheese);
            Application.logMessageReceived -= Log;
            EndEpisode();



        }
        if (other.gameObject.tag == "Goal" && getCheese == true)
        {   
            SetReward(100f);
            getReward = GetCumulativeReward();
            Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = " + count_down + " Move right = " + count_right + " Move left = " + count_left+ " Choose stay = "+ count_not_move + " Reward = " + getReward + " Get Cheese or not = " + getCheese + " Collide with cat = " + count_coll_cat + " Hit wall = " + hit_wall + " Goal without cheese = " + count_goalWithOutCheese);
            Application.logMessageReceived -= Log;
            EndEpisode();
        }
        else if (other.gameObject.tag == "Goal" && getCheese == false)
        {
            SetReward(0);
            count_goalWithOutCheese += 1;
        }



    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Wall")
        {   
            hit_wall = true;
            WallTransform = collision.gameObject.transform;
            AddReward(-0.05f);
            //Debug.Log(collision.contacts[0].point);
        }
    }

    public void Log(string msg, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(fileName, true);

        tw.WriteLine(msg);

        tw.Close();
    }
}
