using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent_Level2 : Agent
{
    public static bool getCheese = false;

    [SerializeField] private Transform CheeseTransform;
    [SerializeField] private Transform CatTransform;
    [SerializeField] private Transform GoalTransform;

    [SerializeField] private Transform[] waypoints;

    private int pointsIndex;

    private Rigidbody2D agentRb;

    int count_move;



    public override void Initialize()
    {

        agentRb = GetComponent<Rigidbody2D>();


        count_move = 0;



    }



    public override void OnEpisodeBegin()
    {

        agentRb.transform.position = new Vector2(-3.53f, 3.53f);


        CatTransform.position = new Vector2(3.51f, 2.4988f);

        pointsIndex = 0;


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

        if (movement == 0)
        {

            agentRb.velocity += new Vector2(0, 1 * speed);
            count_move += 1;
            Debug.Log("Move Up" + count_move);

        }
        if (movement == 1)
        {
            agentRb.velocity += new Vector2(0, -1 * speed);
            count_move += 1;
            Debug.Log("Move Down" + count_move);
        }
        if (movement == 2)
        {

            agentRb.velocity += new Vector2(-1 * speed, 0);
            count_move += 1;
            Debug.Log("Move Right" + count_move);
        }
        if (movement == 3)
        {

            agentRb.velocity += new Vector2(1 * speed, 0);
            count_move += 1;
            Debug.Log("Move Down" + count_move);
        }

        Debug.Log(movement);


    }

    public void Start()
    {
        CatTransform.position = waypoints[pointsIndex].position;
    }

    public void Update()
    {
        CatTransform.position = Vector2.MoveTowards(CatTransform.position, waypoints[pointsIndex].transform.position, 0.6f * Time.deltaTime);

        if (CatTransform.position == waypoints[pointsIndex].transform.position)
        {
            pointsIndex += 1;
        }

        if (pointsIndex == waypoints.Length)
        {
            pointsIndex = 0;
        }


    }



    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Cheese")
        {

            SetReward(10f);
            getCheese = true;
            other.gameObject.SetActive(false);
            Debug.Log("Get Cheese");


            Debug.Log(count_move);

        }






    }

    public void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.GetComponent<EdgeCollider2D>())
        {

            SetReward(-10f);
            Debug.Log("Hit Wall");

        }

        if (other.gameObject.tag == "Cat")
        {
            SetReward(-999f);
            Debug.Log("Collide with cat");
            EndEpisode();
            Debug.Log(count_move);
        }
        if (other.gameObject.tag == "Goal" && getCheese == true)
        {
            SetReward(999);
            Debug.Log("Finish");
            Debug.Log(count_move);
            EndEpisode();
        }
        else if (other.gameObject.tag == "Goal" && getCheese == false)
        {
            SetReward(0);
            EndEpisode();
        }




    }



}
