using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using System;
using Unity.VisualScripting;

public class Agent_Empty_Map : Agent
{
    private Rigidbody2D agentRb;
    public static bool getCheese = false;
    bool isPaused=false;
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
    string fileName = "";
    Vector2 contactPoint;
    float distanceToCheese;
    float distanceToGoal;
    public Sensor s3;

    [SerializeField] private Transform CheeseTransform;
    [SerializeField]private Transform CatTransform;
    [SerializeField] private Transform CatTransform1;
    [SerializeField] private Transform CatTransform2;
    [SerializeField] private Transform CatTransform3;
    [SerializeField] private Transform CatTransform4;
    [SerializeField] private Transform GoalTransform;
    
    private Transform wallTransform;


    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody2D>();

        count_episode = 0;

        fileName = Application.dataPath + "/Level2_WithCat_ModelTest01.txt";

    }
    public void Log(string msg, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(fileName, true);

        tw.WriteLine(msg);

        tw.Close();
    }
    public override void OnEpisodeBegin()
    {
        agentRb.transform.position = new Vector2(-3.475f, 2.627f);

        total_move = 0;
        count_up = 0;
        count_down = 0;
        count_right = 0;
        count_left = 0;
        count_not_move = 0;
        
        count_episode += 1;

        getCheese = false;
        CheeseTransform.gameObject.SetActive(true);


        Application.logMessageReceived += Log;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentRb.transform.position);
        
        sensor.AddObservation(getCheese);

        if(getCheese == false)
        {   
            sensor.AddObservation(CheeseTransform.transform.position);
            sensor.AddObservation(distanceToCheese);

        }else{
            sensor.AddObservation(GoalTransform.transform.position);
            sensor.AddObservation(distanceToGoal);
        }
        
        sensor.AddObservation(CatTransform.transform.position);

        sensor.AddObservation(CatTransform1.transform.position);

        sensor.AddObservation(CatTransform2.transform.position);
        
        sensor.AddObservation(CatTransform3.transform.position);
        
        sensor.AddObservation(CatTransform4.transform.position);
        
        sensor.AddObservation(gameObject.transform.position);
        
        sensor.AddObservation(s3.GetContactPoint());

        if(hit_wall == true)
        {
            sensor.AddObservation(1f);
        }
        else
        {
            sensor.AddObservation(0f);
        }
    }


    public override void OnActionReceived(ActionBuffers actions)
    {
        int movement = actions.DiscreteActions[0];
        

        float speed = 1f;

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

        
        AddReward(-0.05f);
    
        distanceToCheese = Vector2.Distance(agentRb.transform.position, CheeseTransform.position);
        distanceToGoal = Vector2.Distance(agentRb.transform.position, GoalTransform.position);
        
        if(getCheese == false){
            if(distanceToCheese < 4.5f)
            {
                AddReward(1f);
            }
        }
        else{
            if(distanceToGoal < 4.5f)
            {
                AddReward(1f);
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
        if(other.gameObject.tag == "cat1" || other.gameObject.tag == "cat2" || other.gameObject.tag == "cat3" || other.gameObject.tag == "cat4" || other.gameObject.tag == "cat5")
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
            SetReward(1000f);
            getReward = GetCumulativeReward();
            Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = " + count_down + " Move right = " + count_right + " Move left = " + count_left+ " Choose stay = "+ count_not_move + " Reward = " + getReward + " Get Cheese or not = " + getCheese + " Collide with cat = " + count_coll_cat + " Hit wall = " + hit_wall + " Goal without cheese = " + count_goalWithOutCheese);
            Application.logMessageReceived -= Log;
            EndEpisode();
        }
        if (other.gameObject.tag == "Goal" && getCheese == false)
        {
            SetReward(0f);
            count_goalWithOutCheese += 1;
            
        }

    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Wall")
        {   
            hit_wall = true;
            wallTransform = collision.gameObject.transform;
            AddReward(-0.05f);
            //Debug.Log(collision.contacts[0].point);
        }
    }

    
}
