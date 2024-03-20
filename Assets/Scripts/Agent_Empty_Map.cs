using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Unity.MLAgents;

public class Agent_Empty_Map : Agent
{
    public static bool getCheese = false;

    [SerializeField] private Transform CheeseTransform;

    private Rigidbody2D agentRb;

    public override void Initialize()
    {

        agentRb = GetComponent<Rigidbody2D>();

        CheeseTransform = GameObject.Find("cheese").transform;

    }

    public override void OnEpisodeBegin()
    {
        agentRb.transform.position = new Vector2(Random.Range(-4f, 4f), Random.Range(-3f, 3f));
        CheeseTransform.transform.position = new Vector2(Random.Range(-4f, 4f), Random.Range(-3f, 3f));
        CheeseTransform.gameObject.SetActive(true);


        Debug.Log("Episode Begin");

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(agentRb.transform.position);

        sensor.AddObservation(CheeseTransform.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int movement = actions.DiscreteActions[0];

        float speed = 0.6f;

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

        Debug.Log(movement);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");

    }
    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Cheese")
        {

            SetReward(10f);
            getCheese = true;
            other.gameObject.SetActive(false);
            Debug.Log("Get Cheese");

            EndEpisode();
        }

    }

    public void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.GetComponent<EdgeCollider2D>())
        {

            SetReward(-10f);
            Debug.Log("Hit Wall");
            EndEpisode();
        }

    }
}
