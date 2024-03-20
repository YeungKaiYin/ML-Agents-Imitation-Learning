using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Data.Common;
using System;
using UnityEngine.Experimental.Rendering;
using System.Security.Cryptography;

public class Model_Test : Agent
{   
    public static bool getCheese = false;

    [SerializeField] private Transform CheeseTransform;
    [SerializeField] private Transform CatTransform;
    [SerializeField] private Transform GoalTransform;

    [SerializeField] private Transform[] waypoints;

    private int pointsIndex;

    private Rigidbody2D agentRb;

    int total_move;
    int count_episode;
    int count_up;
    int count_down;
    int count_right;
    int count_left;
    int count_getCheese;



    public override void Initialize(){

        agentRb = GetComponent<Rigidbody2D>();


    }



    public override void OnEpisodeBegin()
    {
     
        agentRb.transform.position = new Vector2(-4.4349f,3.4949f);


        CatTransform.position = new Vector2(-3.5061f,-3.4939f);

        pointsIndex= 0;

        total_move = 0;
        count_up = 0;
        count_down = 0;
        count_right = 0;
        count_left = 0;
        count_episode= 0;
      

        CheeseTransform.gameObject.SetActive(true);


        Debug.Log("Episode Begin");

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentRb.transform.position);

        sensor.AddObservation(CheeseTransform.transform.position);

        sensor.AddObservation(CatTransform.transform.position);

        sensor.AddObservation(GoalTransform.transform.position);

        sensor.AddObservation(getCheese);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int movement = actions.DiscreteActions[0];

        float speed = 0.6f;

        if(movement == 0){

            agentRb.velocity += new Vector2(0, 1 * speed);
            count_up += 1;
            total_move += 1;
        }
        if(movement == 1){
            agentRb.velocity += new Vector2(0, -1 * speed);
            
            count_down += 1;
            total_move += 1;
        }
        if(movement == 2){

            agentRb.velocity += new Vector2(-1 * speed, 0);
            count_left += 1;
            total_move+= 1;
        }
        if(movement == 3){

            agentRb.velocity += new Vector2(1 * speed, 0);
            count_right += 1;
            total_move += 1;
        }

        


    }

    public void Start()
    {
        CatTransform.position = waypoints[pointsIndex].position;
    }

    public void Update()
    {
        CatTransform.position = Vector2.MoveTowards(CatTransform.position, waypoints[pointsIndex].transform.position, 0.6f * Time.deltaTime);
        
        if(CatTransform.position == waypoints[pointsIndex].transform.position)
        {
            pointsIndex += 1;
        }
        
        if(pointsIndex == waypoints.Length)
        {
            pointsIndex = 0;
        }
    
    
    }



    public void OnTriggerEnter2D(Collider2D other){

        if(other.gameObject.tag == "Cheese"){

            SetReward(10f);
            getCheese = true;
            other.gameObject.SetActive(false);
            count_getCheese += 1;
            Debug.Log("Get Cheese");


        }






    }

    public void OnCollisionEnter2D(Collision2D other){

        if(other.gameObject.GetComponent<EdgeCollider2D>()){

            SetReward(-10f);
            Debug.Log("Hit Wall");
            
        }

        if (other.gameObject.tag == "Cat")
        {
            SetReward(-999f);
            Debug.Log("Collide with cat");
            EndEpisode();
            Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = "+count_down + " Move right = " + count_right + " Move left = " +count_left);
 
        }
        if (other.gameObject.tag == "Goal" && getCheese == true)
        {
            SetReward(999);
            Debug.Log("Finish");
            EndEpisode();
            Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = " + count_down + " Move right = " + count_right + " Move left = " + count_left);
        }
        else if (other.gameObject.tag == "Goal" && getCheese == false)
        {
            SetReward(0);
            EndEpisode();
            Debug.Log("Episode = " + count_episode + " Total movement = " + total_move + " Move Up = " + count_up + " Move down = " + count_down + " Move right = " + count_right + " Move left = " + count_left);
        }




    }




}
