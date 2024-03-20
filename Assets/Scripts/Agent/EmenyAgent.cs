using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class EmenyAgent : Agent
{
    [SerializeField] private Transform env;
    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

    private int actionsReceived;

    public override void OnEpisodeBegin()
    {
        //transform.localPosition = new Vector3(Random.Range(-3.5f, -1.5f), Random.Range(-3.5f, 3.5f));
        transform.localPosition = new Vector3(-0.05f, 3.02f);
        transform.rotation = Quaternion.identity;
        target.rotation = Quaternion.identity;

        actionsReceived = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)(transform.position - env.position));
        sensor.AddObservation((Vector2)(target.position - env.position));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        actionsReceived++;

        float moveX = actions.DiscreteActions[0] - 1;
        float moveY = actions.DiscreteActions[1] - 1;

        float movementSpeed = 1f;

        transform.position += new Vector3(moveX, moveY) * Time.deltaTime * movementSpeed;

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        discreteActions[0] = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal") + 1);
        discreteActions[1] = Mathf.RoundToInt(Input.GetAxisRaw("Vertical") + 1);

        discreteActions[2] = Input.GetButton("Jump") ? 1 : 0;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Decent target))
        {
            AddReward(15f);
            backgroundSpriteRenderer.color = Color.grey;

            int timeRewoard = actionsReceived < 1000 ? (1000 - actionsReceived) / 50 : 0;
            Debug.Log(timeRewoard);
            AddReward(timeRewoard);

            EndEpisode();
        }
        else if (collision.TryGetComponent(out Wall wall))
        {
            AddReward(-7f);
            backgroundSpriteRenderer.color = Color.red;
            EndEpisode();
        }
        

    }

}
