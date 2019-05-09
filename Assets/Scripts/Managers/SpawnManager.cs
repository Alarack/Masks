using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    


    public GameObject spawn;
    public Transform spawnPoint;
    public int maxSpawns;
    public float spawnTime;
    public bool haltSpawning;

    public List<Entity> spawns = new List<Entity>();

    private Timer spawnTimer;


    private void Start()
    {
        spawnTimer = new Timer(spawnTime, Spawn, true);
    }

    private void Update()
    {
        if (haltSpawning == false && spawnTimer != null && spawns.Count < maxSpawns)
        {
            spawnTimer.UpdateClock();
        }
    }

    private void Spawn()
    {
        GameObject activeSpawn = Instantiate(spawn, spawnPoint.position, spawnPoint.rotation) as GameObject;
        activeSpawn.gameObject.name = "Zombie " + spawns.Count;
        spawns.AddUnique(activeSpawn.GetComponent<Entity>());

    }

    public void EnemyDied(Entity target)
    {
        spawns.RemoveIfContains(target);
    }

}
