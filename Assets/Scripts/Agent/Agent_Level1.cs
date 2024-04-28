using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;



public class Agent_Level1 : Agent
{    private Rigidbody2D agentRb;
    bool getCheese = false;
    int count_episode;
    int count_getCheese;
    float getReward;
    int count_goalWithOutCheese;
    string fileName = "";
    Vector2 contactPoint;
    float distanceToCheese;
    float distanceToGoal;
    public Sensor s1;
    float stepPenalty;
    int count_achive_goal;
    float score;

    [SerializeField] private Transform CheeseTransform;
    [SerializeField] private Transform GoalTransform;
    [SerializeField] private Transform wallTransform;
    private Transform CatTransform;
    private Transform CatTransform1;
    private Transform CatTransform2;
    private Transform CatTransform3;
    private Transform CatTransform4;

    bool human = true;

    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody2D>();

        count_episode = 0;
        
        //fileName = Application.dataPath + "/Level1_Test_T02.txt";
        fileName = Application.dataPath + "/Level1_TrainedModel_Performance.txt";

    }

    public void Log(string msg, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(fileName, true);

        tw.WriteLine(msg);

        tw.Close();
    }

    public override void OnEpisodeBegin()
    {

        agentRb.transform.position = new Vector2(-3.439f, 3.52f);

        count_episode += 1;
        
        getCheese = false;
        CheeseTransform.gameObject.SetActive(true);


        Application.logMessageReceived += Log;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentRb.transform.position);
    

        if(getCheese == false)
        {
            
            sensor.AddObservation(CheeseTransform.transform.position);

            sensor.AddObservation(distanceToCheese);

        }else{
            
            sensor.AddObservation(GoalTransform.transform.position);

            sensor.AddObservation(distanceToGoal);
        }

        sensor.AddObservation(gameObject.transform.position);
        
        if (CatTransform != null)
        {
            sensor.AddObservation(CatTransform.transform.position);
        }
        else
        {
        sensor.AddObservation(Vector2.zero); 
        }
        if (CatTransform1 != null)
        {
            sensor.AddObservation(CatTransform1.transform.position);
        }
        else
        {
            sensor.AddObservation(Vector3.zero);
        }
        if (CatTransform2 != null)
        {
            sensor.AddObservation(CatTransform2.transform.position);
        }
        else
        {
            sensor.AddObservation(Vector3.zero);
        }
        if (CatTransform3 != null)
        {
            sensor.AddObservation(CatTransform3.transform.position);
        }
        else
        {
            sensor.AddObservation(Vector3.zero);
        }
        if (CatTransform4 != null)
        {
            sensor.AddObservation(CatTransform4.transform.position);
        }
        else
        {
            sensor.AddObservation(Vector3.zero);
        }

        sensor.AddObservation(gameObject.transform.position);
        
        sensor.AddObservation(s1.GetContactPoint());
        
        sensor.AddObservation(wallTransform);
    }

    public void HumanOrAI(bool tf)
    {
        human = tf;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {   

        
        int movement = actions.DiscreteActions[0];
        

        float speed = 1f;

        if (movement == 0)
        {

            agentRb.velocity += new Vector2(0, 1 * speed);

        }
        if (movement == 1)
        {
            agentRb.velocity += new Vector2(0, -1 * speed);


        }
        if (movement == 2)
        {

            agentRb.velocity += new Vector2(-1 * speed, 0);

        }
        if (movement == 3)
        {

            agentRb.velocity += new Vector2(1 * speed, 0);


        }

        distanceToCheese = Vector2.Distance(agentRb.transform.position, CheeseTransform.position);
        distanceToGoal = Vector2.Distance(agentRb.transform.position, GoalTransform.position);
        
        if(StepCount >= 500){

            int getStepCount = 500;
            AddReward(-100f);
            getReward = GetCumulativeReward();
            Debug.Log( "Episode = " + count_episode + " Number of steps = " + getStepCount + " Reward = " + getReward + " getCheese= " + count_getCheese + " Goal(!Cheese) = " + count_goalWithOutCheese +  " FinalGoal = " +count_achive_goal);
            Application.logMessageReceived -= Log;
            EndEpisode();
        }else{
            score = -0.5f;
            score += score * 0.1f;
            AddReward(score);
        }

        if(getCheese){
            
            if(distanceToGoal < 4f){
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
            AddReward(300);
            count_getCheese += 1;
        }


    }

    public void OnCollisionEnter2D(Collision2D other)
    {   
        if(other.gameObject.tag == "cat1" || other.gameObject.tag == "cat2" || other.gameObject.tag == "cat3" || other.gameObject.tag == "cat4" || other.gameObject.tag == "cat5")
        {
            AddReward(-100f);
            getReward = GetCumulativeReward();
            Debug.Log( "Episode = " + count_episode + " Number of steps = " + StepCount + " Reward = " + getReward + " getCheese= " + count_getCheese + " Goal(!Cheese) = " + count_goalWithOutCheese +  " FinalGoal = " +count_achive_goal);
            Application.logMessageReceived -= Log;
            EndEpisode();
        }

        if (other.gameObject.tag == "Goal" && getCheese == true)
        {   
            count_achive_goal += 1;
            AddReward(1000f);
            getReward = GetCumulativeReward();
            Debug.Log( "Episode = " + count_episode + " Number of steps = " + StepCount + " Reward = " + getReward + " getCheese= " + count_getCheese + " Goal(!Cheese) = " + count_goalWithOutCheese +  " FinalGoal = " +count_achive_goal);
            Application.logMessageReceived -= Log;
            EndEpisode();
            if(human)
                SceneManager.LoadScene("Level2_CMaze");
        }
        if (other.gameObject.tag == "Goal" && getCheese == false)
        {
            AddReward(10f);
            count_goalWithOutCheese += 1;
            
        }

    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Wall")
        {   
            wallTransform = collision.gameObject.transform;
            //Debug.Log(collision.contacts[0].point);
        }
    }
}

