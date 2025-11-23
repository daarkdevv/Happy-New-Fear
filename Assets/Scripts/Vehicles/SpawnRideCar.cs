using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class SpawnRideCar : MonoBehaviour
{
 
     [SerializeField] bool hasSpawned;
     [SerializeField] GameObject CarToRide;
     [SerializeField] CarSpawner carSpawner;
    // Start is called before the first frame update
    void Start()
    {

      carSpawner = GameObject.FindGameObjectWithTag("CarSpawner").GetComponent<CarSpawner>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(QuestManager.QuestInstance.currentMission == 12)
        {

          
          StartCoroutine(CarSpawner());

          hasSpawned = true;

        }
        
    }

    IEnumerator CarSpawner()
    {

         
         if(hasSpawned)
         {
          
           yield break;
            
         }

        

         yield return new WaitForSeconds(7.5f);
     
         Instantiate(CarToRide,transform.position,CarToRide.transform.rotation);

         carSpawner.canSpawnHere[2] = false;

    }
}
