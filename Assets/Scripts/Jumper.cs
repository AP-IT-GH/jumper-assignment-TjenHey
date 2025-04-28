using System;
using Unity.MLAgents;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Random = UnityEngine.Random;

public class Jumper : Agent
{
    public Transform target;
    private bool _isReward = false;
    public Vector3 startPosition;
    public Vector3 targetStartPosition;
    public override void OnEpisodeBegin()
    {
        // _isReward = target.gameObject.GetComponent<Renderer>().material.name == "Green";
        this.transform.localPosition = startPosition;
        target.localPosition = targetStartPosition;
        print(_isReward);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        print("COLLECTOBSERVATION");
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(_isReward);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float movement = actionBuffers.ContinuousActions[0];
        print(this.transform.localPosition.y);
        print(movement);
        if (this.transform.localPosition.y - 0.01 <= 0.6)
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, movement * 10, 0), ForceMode.Force);
        if (this.transform.localPosition.y > 10)
            EndEpisode();
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            continuousActionsOut[0] = 1;
        }
    }
}
