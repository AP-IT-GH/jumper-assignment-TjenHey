using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject target;
    private Vector3 _spawnLocation;
    public int spawnCount = 0;
    void Start()
    {
        _spawnLocation = target.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        target.transform.localPosition = _spawnLocation;
        target.GetComponent<Target>().UpdateState();
    }
}
