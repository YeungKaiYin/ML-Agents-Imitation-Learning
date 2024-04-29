using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using System;
using Unity.VisualScripting;
using System.Security.Cryptography;
using System.IO.Abstractions;
using System.Linq;
using UnityEngine.SceneManagement;
public class Agent_Empty_Map : Agent
{
    public Rigidbody2D agentRb;
    public static bool getCheese = false;
    bool isPaused=false;
    int count_episode;
    int count_getCheese;
    float getReward;
    int count_coll_cat;
    int countbBattleWin;
    int countBattleLose;
    int count_goalWithOutCheese;
    string fileName = "";
    Vector2 contactPoint;
    float distanceToCheese;
    float distanceToGoal;
    int count_achive_goal;
    public Sensor s3;
    float stepPenalty;
    float score;

    float getStepCount;

    float getTotalStepCount;
    

    [SerializeField] private Transform CheeseTransform;
    [SerializeField]private Transform CatTransform;
    [SerializeField] private Transform CatTransform1;
    [SerializeField] private Transform CatTransform2;
    [SerializeField] private Transform CatTransform3;
    [SerializeField] private Transform CatTransform4;
    [SerializeField] private Transform GoalTransform;
    [SerializeField]private Transform wallTransform;
    private static System.Random randomNumber;
    public AgentActiveContoller aac;

    bool human = true;

    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody2D>();

        count_episode = 0;
        count_achive_goal = 0;
        //fileName = Application.dataPath + "/Level2_Test_T016.txt";
        fileName = Application.dataPath + "/Level2_Model_PerformanceC.txt";



        


    }
    public void Log(string msg, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(fileName, true);

        tw.WriteLine(msg);

        tw.Close();
    }
    public override void OnEpisodeBegin()
    {
        agentRb.transform.position = new Vector2(-3.465f, 2.632f);


        count_episode += 1;

        getCheese = false;
        isPaused = false;
        CheeseTransform.gameObject.SetActive(true);

        isPaused = false;


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
        
        sensor.AddObservation(CatTransform.transform.position);

        sensor.AddObservation(CatTransform1.transform.position);

        sensor.AddObservation(CatTransform2.transform.position);
        
        sensor.AddObservation(CatTransform3.transform.position);
        
        sensor.AddObservation(CatTransform4.transform.position);
        
        sensor.AddObservation(gameObject.transform.position);
        
        sensor.AddObservation(s3.GetContactPoint());
        
        sensor.AddObservation(wallTransform);

        


    }



    public void IsPaused(bool tf)
    {
        isPaused = tf;
    }

    public void HumanOrAI(bool tf)
    {
        human = tf;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isPaused)
        {
            // Agent is paused, do not execute actions
            return;
        }



        int movement = actions.DiscreteActions[0];


        float speed = 1f;

        if (movement == 0)
        {

            agentRb.velocity += new Vector2(0, 1 * speed);

 

        }else if (movement == 1)
        {
            agentRb.velocity += new Vector2(0, -1 * speed);
    

            
        }else if (movement == 2)
        {

            agentRb.velocity += new Vector2(-1 * speed, 0);
      

        }
        else if (movement == 3)
        {

            agentRb.velocity += new Vector2(1 * speed, 0);
      

        }

        distanceToGoal = Vector2.Distance(agentRb.position, GoalTransform.position);
        distanceToCheese = Vector2.Distance(agentRb.position, CheeseTransform.position);


        if(StepCount >= 500){


                getStepCount= 500;
                AddReward(-100f);
                getReward = GetCumulativeReward();
                Debug.Log( "Episode = " + count_episode + " Number of steps = " + getStepCount + " Reward = " + getReward + " getCheese= " + count_getCheese  + " Collide with cat = " + count_coll_cat  + " Goal(!Cheese) = " + count_goalWithOutCheese + " Battle Win = " + countbBattleWin + " Battle Lose = " + countBattleLose + " FinalGoal = " +count_achive_goal);
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
            
                if(getCheese == false)
                { 
                    count_coll_cat += 1;
                    AddReward(-100f);
                    getReward = GetCumulativeReward();
                    Debug.Log( "Episode = " + count_episode + " Number of steps = " + StepCount + " Reward = " + getReward + " getCheese= " + count_getCheese  + " Collide with cat = " + count_coll_cat  + " Goal(!Cheese) = " + count_goalWithOutCheese + " Battle Win = " + countbBattleWin + " Battle Lose = " + countBattleLose + " FinalGoal = " +count_achive_goal);
                    Application.logMessageReceived -= Log;
                    EndEpisode();
            
                }
                else
                {
                    aac.PauseMazeAgent();
                    aac.ResumeBattleAgent();
                    randomNumber = new System.Random();
                    double margin = 89.0/100.0;
                    if(randomNumber.NextDouble() < margin)
                    {   
                
                        AddReward(5f);
                        countbBattleWin += 1;
                    }
                    else
                    {
     
                        AddReward(-100f);
                        countBattleLose += 1;
                        getReward = GetCumulativeReward();
                        Debug.Log( "Episode = " + count_episode + " Number of steps = " + StepCount + " Reward = " + getReward + " getCheese= " + count_getCheese  + " Collide with cat = " + count_coll_cat  + " Goal(!Cheese) = " + count_goalWithOutCheese + " Battle Win = " + countbBattleWin + " Battle Lose = " + countBattleLose + " FinalGoal = " +count_achive_goal);
                        Application.logMessageReceived -= Log;
                        //EndEpisode();

                    }
                }
            }

        


        if (other.gameObject.tag == "Goal" && getCheese == true)
        {   
            count_achive_goal += 1;
            AddReward(1000f);
            getReward = GetCumulativeReward();
            Debug.Log( "Episode = " + count_episode + " Number of steps = " + StepCount + " Reward = " + getReward + " getCheese= " + count_getCheese  + " Collide with cat = " + count_coll_cat  + " Goal(!Cheese) = " + count_goalWithOutCheese + " Battle Win = " + countbBattleWin + " Battle Lose = " + countBattleLose + " FinalGoal = " +count_achive_goal);
            Application.logMessageReceived -= Log;
            EndEpisode();
            if (human)
                SceneManager.LoadScene("level3");

        }
        else if(other.gameObject.tag == "Goal" && getCheese == false){

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
