using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject prefab;
    public GameObject target;
    private Vector3 _spawnLocation;
    public int spawnCount = 0;
    void Start()
    {
        _spawnLocation = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject Spawn()
    {
        spawnCount++;
        return Instantiate(prefab, _spawnLocation, Quaternion.identity);
    }
}
