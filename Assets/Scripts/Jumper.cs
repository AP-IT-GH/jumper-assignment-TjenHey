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
    public Spawner spawner;
    private Vector3 _targetPosition;
    private bool _grounded = true;
    public float jumpForce;
    private Vector3 _spawnLocation;
    private float _targetSpeed;
    private void Start()
    {
        _spawnLocation = transform.position;
    }

    public override void OnEpisodeBegin()
    {
        this.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        _isReward = target.gameObject.GetComponent<Target>().isReward;
        _grounded = true;
        Spawn();
        this.transform.position = _spawnLocation;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_targetPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(_isReward);
        sensor.AddObservation(_targetSpeed);
    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        UpdateTarget();
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
                EndEpisode();
            }
            Spawn();
        }
    }

    private void Spawn()
    {
        spawner.Spawn();
        spawner.spawnCount++;
    }
    private void UpdateTarget()
    {
        _isReward = target.gameObject.GetComponent<Target>().isReward;
        _targetPosition = target.localPosition;
        _targetSpeed = target.gameObject.GetComponent<Target>().moveSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _grounded = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target")){
            if (_isReward)
            {
                SetReward(1f);
                EndEpisode();
            }
            else
            {
                SetReward(-1f);
                EndEpisode();
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
}
