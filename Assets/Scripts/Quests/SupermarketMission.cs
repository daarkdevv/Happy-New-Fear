using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SupermarketMission : MonoBehaviour
{
    [SerializeField] public List<GameObject> SupermarketObjects;
    [SerializeField] int currentTakenObjects;
    [SerializeField] bool actionDone;
    [SerializeField] bool actionDone2;
    [SerializeField] bool action3Done;
    [SerializeField] int neededMission;
    [SerializeField] NpcInteract npcInteract;
    [SerializeField] int NpcConversationNum;
    [SerializeField] Conversation conversation3;
    [SerializeField] GameObject bagOfSupplements;
    [SerializeField] Transform bagPosition;
    [SerializeField] Collider BarrierToDisable;
    // Start is called before the first frame update
    void Start()
    {

        foreach(GameObject marketObj in SupermarketObjects)
        {
           
           marketObj.GetComponent<Outline>().enabled = false;

        }

        
    }

    // Update is called once per frame
    void Update()
    {

      if(conversation3.CurrentAudio == 4 && actionDone)
      {
         if(!action3Done)
         {
           
          Instantiate(bagOfSupplements,bagPosition.position,bagPosition.transform.rotation);

          npcInteract.canInterAct = false;

          BarrierToDisable.enabled = false;
          
          action3Done = true;

         }
 

      }
     
      if(QuestManager.QuestInstance.currentMission >= neededMission)
      {

              if(!actionDone2)
      {
        
        foreach(GameObject marketObj in SupermarketObjects)
        {
           
           marketObj.GetComponent<Outline>().enabled = true;

        }
        
        actionDone2 = true;

      } 
       
      if(SupermarketObjects.Count == 0)
      {

         
         if(!actionDone)
         {

            npcInteract.currentCoversation = NpcConversationNum;
            QuestManager.QuestInstance.currentMission++;
            actionDone = true;

         }


      }

         
      } 
       



        
    }
}
