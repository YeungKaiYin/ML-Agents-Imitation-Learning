using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.IO;
using System;



public class Agent_Level3 : Agent
{
    public static bool getCheese = false;

    public Sensor s2;
    [SerializeField] private Transform CheeseTransform;
    [SerializeField] private Transform CatTransform;
    [SerializeField] private Transform GoalTransform;
    private Transform wallTransform;
    [SerializeField] private Transform[] waypoints;

    
    private int pointsIndex;

    private Rigidbody2D agentRb;
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
    
    float distanceToCheese;
    float distanceToGoal;


    bool catStun = false;
    bool human = true;
    public OpeningManager om;

    public override void Initialize()
    {

        agentRb = GetComponent<Rigidbody2D>();

        count_episode = 0;

        fileName = Application.dataPath + "/Level3_ModelTest_.txt";

    }

    public void HumanOrAI(bool tf)
    {
        human = tf;
    }

    public void Log(string msg, string stackTrace, LogType type)
    {
        TextWriter tw = new StreamWriter(fileName, true);

        tw.WriteLine(msg);

        tw.Close();
    }


    public override void OnEpisodeBegin()
    {

        agentRb.transform.position = new Vector2(-3.4874f, 3.5172f);


        CatTransform.position = new Vector2(3.51f, 2.4988f);

        pointsIndex = 0;


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

        sensor.AddObservation(CheeseTransform.transform.position);

        sensor.AddObservation(GoalTransform.transform.position);

        if(getCheese == false)
        {
            
            sensor.AddObservation(distanceToCheese);

        }else{

            
            sensor.AddObservation(distanceToGoal);
        }

        sensor.AddObservation(CatTransform.transform.position);

        sensor.AddObservation(gameObject.transform.position);
        
        sensor.AddObservation(s2.GetContactPoint());

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

        if (!human)
        {
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
        

        SetReward(-0.05f);
        

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
            om.LoadOpenScene();
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
            wallTransform = collision.gameObject.transform;
            AddReward(-0.05f);
            //Debug.Log(collision.contacts[0].point);
        }
    }




}
