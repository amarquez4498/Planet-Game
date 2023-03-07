using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject enemy;
    public float timeBetweemSpawns = 50f;
    float cooldown;
    public List<GameObject> enemies;

    private void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        cooldown = timeBetweemSpawns;
    }

   
    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }

    GameObject GetEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].activeInHierarchy)
                return enemies[i];
        }

        GameObject en = Instantiate(enemy, transform.position, Quaternion.identity);
        enemies.Add(en);
        en.SetActive(false);
        return en;
    }

    void SpawnEnemy()
    {
        GameObject obj = GetEnemy();
        obj.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        obj.SetActive(true);
        //Instantiate(enemy, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
        cooldown = timeBetweemSpawns;
    }

}
