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
    private bool _grounded = true;
    public float jumpForce;
    public override void OnEpisodeBegin()
    {
        this.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        _isReward = target.gameObject.GetComponent<Target>().isReward;
        this.transform.localPosition = startPosition;
        target.localPosition = targetStartPosition;
        _targetPosition = target.localPosition;
        _grounded = true;
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
        if (this.transform.localPosition.y < -1 || this.transform.localPosition.y > 3.5)
        {
            EndEpisode();
        }
        int jump = actionBuffers.DiscreteActions[0];
        if (jump == 1 && _grounded)
        {
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            _grounded = false;
        }
        _targetPosition = target.localPosition;
        if (_targetPosition.x < this.transform.position.x - 2)
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
            Spawn();
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        if (Input.GetKey(KeyCode.Space))
        {
            discreteActionsOut[0] = 1;
        }
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
            Spawn();
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            _grounded = true;
        }
    }

    private void Spawn()
    {
        Destroy(target.gameObject);
        target = spawner.Spawn().transform;
        _targetPosition = target.localPosition;
        _isReward = target.gameObject.GetComponent<Target>().isReward;
    }
}
