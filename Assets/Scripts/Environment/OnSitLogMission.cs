using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSitLogMission : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]  bool HasSit;
    
    [Header("Components")]
    [SerializeField] Couch couch;
    [SerializeField] NpcInteract DaveConversation;
    
    // Start is called before the first frame update
    void Start()
    {

     couch = GetComponent<Couch>();
        
    }

    // Update is called once per frame
    void Update()
    {

        CheckForQuest();
        
    }

    void CheckForQuest()
    {
          
          if(QuestManager.QuestInstance.currentMission == 1)
          {
              
              if(couch.isSitting)
              {

               couch.CanGetUp = false;
               QuestManager.QuestInstance.DisplayMessage(0,true,false);
               DaveConversation.conversations[1].ConversationOn = true;
               QuestManager.QuestInstance.currentMission++;

              }


          }

          if(QuestManager.QuestInstance.currentMission == 3)
          {
            
               couch.CanGetUp = true;
               QuestManager.QuestInstance.DisplayMessage(0,false,true);


          }


    }
}
