using System;
using Unity.MLAgents;
using UnityEngine;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

public class Jumper : Agent
{
    public Transform target;
    private bool _isReward = false;
    public Vector3 startPosition;
    public Vector3 targetStartPosition;
    public Spawner spawner;
    private Vector3 _targetPosition;
    public override void OnEpisodeBegin()
    {
        _isReward = target.gameObject.GetComponent<Target>().isReward;
        this.transform.localPosition = startPosition;
        target.localPosition = targetStartPosition;
        _targetPosition = target.localPosition;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_targetPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(_isReward);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (spawner.spawnCount > 200)
        {
            EndEpisode();
            spawner.spawnCount = 0;
        }
        if (this.transform.localPosition.y < -1 || this.transform.localPosition.y > 25)
        {
            EndEpisode();
        }
        float movement = actionBuffers.ContinuousActions[0];
        if (this.transform.localPosition.y - 0.01 <= 0.6)
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, movement * 20, 0), ForceMode.Impulse);
        }
        _targetPosition = target.localPosition;
        if (_targetPosition.x < this.transform.localPosition.x - 2)
        {
            if (_isReward)
            {
                SetReward(-1f);
                EndEpisode();
            }
            else
            {
                SetReward(1f);
            }
            Transform newTarget = spawner.Spawn().transform;
            Destroy(target.gameObject);
            target = newTarget;
            _isReward = target.gameObject.GetComponent<Target>().isReward;
            _targetPosition = target.localPosition;
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Vertical");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Target")){
            if (_isReward)
            {
                SetReward(1f);
            }
            else
            {
                SetReward(-1f);
                EndEpisode();
            }
            Destroy(other.gameObject);
            target = spawner.Spawn().transform;
            _targetPosition = target.localPosition;
        }

    }
}
