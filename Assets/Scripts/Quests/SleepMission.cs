using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SleepMission : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerMove move;
    [SerializeField] LevelLoader levelLoader;
    [SerializeField] RaycastChecker raycastChecker;
    [SerializeField] TentDoor tentDoor;
    [SerializeField] CharacterController characterController;
    [SerializeField] PlayerController playerController;
    [SerializeField] Inventory inventory;
    [SerializeField] ViewBobbing bobbing;

    [Header("Positions")]
    [SerializeField] Transform SleepPosition;
    [SerializeField] Transform StandPosition;
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        raycastChecker = GetComponent<RaycastChecker>();
        raycastChecker.isRaycasted = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        CheckMissionRequirement();
        
    }

    void CheckMissionRequirement()
    {
       
       if(QuestManager.QuestInstance.currentMission == 3)
       {

          raycastChecker.IsEnabled = true;
           
           if(Input.GetKeyDown(KeyCode.E) && raycastChecker.isRaycasted)
           {
             
             move.movement = Vector3.zero;
             move.canMove = false;
             characterController.enabled = false;
             move.gameObject.transform.position = SleepPosition.position;
             characterController.enabled = true;
             inventory.CurrentInventorySlot = 0;
             inventory.ChangeInventory(0,false);
             playerController.enabled = false;
             inventory.CanChangeSlot = false;
             bobbing.enabled = false;
             playerController.gameObject.transform.rotation = Quaternion.Euler(0,176,0);

             StartCoroutine(levelLoader.LoadLevel(1,false,3,null,false,null,1));
             StartCoroutine(AfterSleep()); 
             tentDoor.CloseTent();
             

           }
         

       }
 

    }

    IEnumerator AfterSleep()
    {
     yield return new WaitForSeconds(3);   
     
     bool ZEnd;
     int ZzzTime = 0;
     int ZzzZRepetion = 2;
     
     while(ZzzTime < 10)
     {
        yield return new WaitForSeconds(0.7f); 

        int RandomZ = Random.Range(0,2);  

        ZzzTime++;
        text.text += RandomZ == 1 ? "Z" : "z";

        if(ZzzTime == 10)
        {
         
          ZEnd = ZzzZRepetion <= 0;
          
          if(ZEnd)
          {
             Destroy(text);
             yield break;

          }

          ZzzTime = ZzzZRepetion >= 0 ? 1 : 0;
          text.text = "z";
          ZzzZRepetion--;
          

        }

        yield return null;

     }

 
    }
}
