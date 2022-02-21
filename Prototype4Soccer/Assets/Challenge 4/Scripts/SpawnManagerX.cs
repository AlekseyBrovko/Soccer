using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject powerupPrefab;

    [SerializeField] private int enemyCount;
    [SerializeField] private int waveCount = 1;
    
    public float enemySpeedIndex; //исправить!

    private float spawnRangeX = 10;
    private float spawnZMin = 15;
    private float spawnZMax = 25;

    private GameObject focalPoint;
    

    private void Start()
    {
        focalPoint = GameObject.Find("Focal Point");
    }
    
    private void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }
        enemySpeedIndex = waveCount;        
    }
    
    private Vector3 GenerateSpawnPosition ()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        Vector3 powerupSpawnOffset = new Vector3(0, 0, -15);
      
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }
        
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        waveCount++;
        ResetPlayerPosition();
    }
    
    private void ResetPlayerPosition ()
    {
        player.transform.position = new Vector3(0, 1, -7);       
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        focalPoint.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
