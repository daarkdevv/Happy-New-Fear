using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMissionChecker : MonoBehaviour
{
   [SerializeField] int CurrentMissionToLock;
   [SerializeField] bool UnlockedDoor;
   [SerializeField] DoorManager doorManager;
    // Start is called before the first frame update
    void Start()
    {

        doorManager = GetComponent<DoorManager>();
        
    }

    // Update is called once per frame
    void Update()
    {

      UnlockedDoor = QuestManager.QuestInstance.currentMission >= CurrentMissionToLock;

      if(!UnlockedDoor)
      {

      doorManager.Locked = true;
      doorManager.CurrentTextName = "Locked";

      }
      else
      {
        doorManager.Locked = false;
        doorManager.CurrentTextName = doorManager.TextName;

      }
       
      
        
    }
}
