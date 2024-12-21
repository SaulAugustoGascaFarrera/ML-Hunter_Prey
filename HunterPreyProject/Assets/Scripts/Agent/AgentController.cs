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
        transform.localPosition = new Vector3(0.0f,0.5f,0.0f);

        int randomPos = Random.Range(0,2);

        if(randomPos == 0)
        {
            targetTransform.localPosition = new Vector3(-4,0.6f,0.0f);
        }
        else if (randomPos == 1)
        {
            targetTransform.localPosition = new Vector3(4, 0.6f, 0.0f);
        }
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);

        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = actions.ContinuousActions[0];

        

        transform.position += new Vector3(move, 0.0f, 0.0f) * movemetSpeed * Time.deltaTime;
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continiuousAction = actionsOut.ContinuousActions;
        continiuousAction[0] = Input.GetAxisRaw("Horizontal");
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
