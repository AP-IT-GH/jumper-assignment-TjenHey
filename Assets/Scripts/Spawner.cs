using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject prefab;
    public Vector3 spawnLocation;
    public int spawnCount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Spawn()
    {
        spawnCount++;
        return Instantiate(prefab, spawnLocation, Quaternion.identity);
    }
}
