using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class Target : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isReward = false;
    public float moveSpeed = 10f;
    void Start()
    {
        isReward = Random.Range(0, 2) == 0;
        if (!isReward)
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        this.transform.localPosition += new Vector3(- moveSpeed * Time.deltaTime, 0, 0);
    }
}
