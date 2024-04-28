using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using System;
using UnityEngine.SceneManagement;


public class Agent_Level3 : Agent
{
    public static bool getCheese = false;

    public Sensor s2;
    [SerializeField] private Transform CheeseTransform;
    [SerializeField] private Transform CatTransform;
    [SerializeField] private Transform GoalTransform;
    [SerializeField] private Transform wallTransform;
    [SerializeField] private Transform[] waypoints;
    private static System.Random randomNumber;
    private int pointsIndex;

    private Rigidbody2D agentRb;
    bool isPaused=false;
    int count_episode;
    int count_getCheese;
    float getReward;
    int count_coll_cat;
    int count_goalWithOutCheese;
    string fileName = "";
    int countbBattleWin;
    int countBattleLose;
    float distanceToCheese;
    float distanceToGoal;
    private Transform CatTransform1;
    private Transform CatTransform2;
    private Transform CatTransform3;
    private Transform CatTransform4;
    int count_achive_goal;
    bool catStun = false;
    float getStepCount;
    float score;

    bool human = true;
    public AgentActiveContoller aac;

    public override void Initialize()
    {

        agentRb = GetComponent<Rigidbody2D>();

        count_episode = 0;
        count_achive_goal = 0;
        fileName = Application.dataPath + "/Level3_Test_T02.txt";
        //fileName = Application.dataPath + "/Level3_Model_Test.txt";

    }






    public void Log(string msg, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(fileName, true);

        tw.WriteLine(msg);

        tw.Close();
    }


    public override void OnEpisodeBegin()
    {

        agentRb.transform.position = new Vector2(-3.494f, 3.481f);

        isPaused = false;

        CatTransform.position = new Vector2(3.51f, 2.4988f);

        pointsIndex = 0;
        
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
        
        sensor.AddObservation(CatTransform.transform.position);

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
        
        sensor.AddObservation(s2.GetContactPoint());
        
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


                getStepCount= 500;
                AddReward(-100f);
                getReward = GetCumulativeReward();
                Debug.Log( "Episode = " + count_episode + " Number of steps = " +  getStepCount + " Reward = " + getReward + " getCheese= " + count_getCheese  + " Collide with cat = " + count_coll_cat  + " Goal(!Cheese) = " + count_goalWithOutCheese + " Battle Win = " + countbBattleWin + " Battle Lose = " + countBattleLose + " FinalGoal = " +count_achive_goal);
                Application.logMessageReceived -= Log;
                EndEpisode();
                


        }else{
            score = -0.5f;
            score += score * 0.1f;
            AddReward(score);
        }


        if(getCheese){

            if(distanceToGoal < 5f){
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
    void Start()
    {
        CatTransform.position = waypoints[pointsIndex].transform.position;


    }

    void FixedUpdate()
    {   
        
        if(!catStun)
        if (pointsIndex <= waypoints.Length - 1)
        {

            CatTransform.transform.position = Vector2.MoveTowards(CatTransform.transform.position, waypoints[pointsIndex].transform.position, 1f * Time.deltaTime);


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

    void CatStun(bool tf)
    {
        catStun = tf;
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
            
            }else{
                aac.ResumeBattleAgent();
                aac.PauseMazeAgent();
                randomNumber = new System.Random();
                double margin = 89.0/100.0;
                if(randomNumber.NextDouble() < margin)
                {   
                
                    AddReward(5f);
                    countbBattleWin += 1;


                }else{
     
                AddReward(-100f);
                countBattleLose += 1;
                getReward = GetCumulativeReward();
                Debug.Log( "Episode = " + count_episode + " Number of steps = " + StepCount + " Reward = " + getReward + " getCheese= " + count_getCheese  + " Collide with cat = " + count_coll_cat  + " Goal(!Cheese) = " + count_goalWithOutCheese + " Battle Win = " + countbBattleWin + " Battle Lose = " + countBattleLose + " FinalGoal = " +count_achive_goal);
                Application.logMessageReceived -= Log;
                EndEpisode();

                }
            }
        }
      
        if (other.gameObject.tag == "Goal" && getCheese == true)
        {   
            AddReward(1000f);
            count_achive_goal += 1;
            getReward = GetCumulativeReward();
            Debug.Log( "Episode = " + count_episode + " Number of steps = " + StepCount + " Reward = " + getReward + " getCheese= " + count_getCheese  + " Collide with cat = " + count_coll_cat  + " Goal(!Cheese) = " + count_goalWithOutCheese + " Battle Win = " + countbBattleWin + " Battle Lose = " + countBattleLose + " FinalGoal = " +count_achive_goal);
            Application.logMessageReceived -= Log;
            EndEpisode();
            if(human)
                SceneManager.LoadScene("Opening");
        }
        else if (other.gameObject.tag == "Goal" && getCheese == false)
        {
            AddReward(10f);
            count_goalWithOutCheese += 1;
        }




    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Wall")
        {   
            
            //Debug.Log("collide with wall");
            wallTransform = collision.gameObject.transform;
            //Debug.Log(collision.contacts[0].point);
        }
    }




}
