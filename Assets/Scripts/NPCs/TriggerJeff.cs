using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerJeff : MonoBehaviour
{
    [SerializeField] GameObject JeffSpawn;
    [SerializeField] Transform JeffPosition;
    [SerializeField] bool hasSpawned;
    [SerializeField] int neededMission;
    [SerializeField] bool Spawn;
    [SerializeField] bool ActivateBarriers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    private void OnTriggerEnter(Collider other) 
    {

       if(QuestManager.QuestInstance.currentMission == neededMission)
       {

          if(hasSpawned)
          Destroy(gameObject);

          Instantiate(JeffSpawn,JeffPosition.position,JeffSpawn.transform.rotation);  


       }
        
    }
  
}
