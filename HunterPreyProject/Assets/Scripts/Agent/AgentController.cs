using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentController : Agent
{
    [Header("Movement Atts")]
    [SerializeField] private float movemetSpeed = 5.0f;

    [SerializeField] private Transform targetTransform;


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-8.0f,9.0f),0.5f,Random.Range(-8.0f,9.0f));

        targetTransform.localPosition = new Vector3(Random.Range(-8.0f, 9.0f), 0.5f, Random.Range(-8.0f, 9.0f));
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);

        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 movementDirection = new Vector3(moveX, 0.0f, moveZ);

        movementDirection = movementDirection.normalized;

        transform.position += movementDirection * movemetSpeed * Time.deltaTime;
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continiuousAction = actionsOut.ContinuousActions;
        continiuousAction[0] = Input.GetAxisRaw("Horizontal");
        continiuousAction[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Target")
        {
            AddReward(2.0f);

            EndEpisode();
        }

        if (other.gameObject.tag == "Wall")
        {
            AddReward(-2.0f);

            EndEpisode();
        }
    }

}
