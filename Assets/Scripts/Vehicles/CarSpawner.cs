using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{

   [SerializeField] bool isSpawningEnabled;  
   [Range(1,10)] public float spawnInterval;
    [SerializeField] float spawnTimer;
    [SerializeField] GameObject[] cars;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] public bool[] canSpawnHere;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

      if(!isSpawningEnabled)
      return;  
      
      CheckTimer();
        
    }

    void SpawnCar()
    {

      int randomCarIndex = Random.Range(0, cars.Length);

      if (!canSpawnHere[randomCarIndex]) return;
      
      Instantiate(cars[randomCarIndex],new Vector3(spawnPositions[randomCarIndex].position.x,cars[randomCarIndex].transform.position.y,spawnPositions[randomCarIndex].position.z),cars[randomCarIndex].transform.rotation);
     
    }

    void CheckTimer()
    {

      if(spawnTimer < 0)
      {

        SpawnCar();

        spawnTimer = Random.Range(spawnInterval,5);

      }

      else
      {

         spawnTimer -= Time.deltaTime;

      }




    }
}
