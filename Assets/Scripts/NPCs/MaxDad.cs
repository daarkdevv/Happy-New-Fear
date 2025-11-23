using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MaxDad : MonoBehaviour
{
   [SerializeField] PlayerMove move;

   [SerializeField] Conversation conversation;
   [SerializeField] NavMeshAgent Agent;
   [SerializeField] bool ActionDone;
   [SerializeField] PlagueAI_Behavoiur aI;

   [SerializeField] Transform player;
   [SerializeField] SanityEffect1 sanityEffect1;
   [SerializeField] bool action2Done;
    // Start is called before the first frame update
    void Start()
    {

        Agent = GetComponent<NavMeshAgent>();
        conversation = GetComponent<Conversation>();
        move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        aI = GetComponent<PlagueAI_Behavoiur>();
         player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {

       if(conversation.hasFinishedConv && !action2Done)
       {
         action2Done = true;
         sanityEffect1.TriggerFlash();
         move.canMove = true;

       }
     
      if(aI.HasReachedPlayer && aI.HasDetectedPlayer && !ActionDone)
      {
          
         conversation.ConversationOn = true;
         ActionDone = true;
         move.movement = Vector3.zero;
         move.canMove = false;

      }

      if(ActionDone)
      {
       Vector3 targetPosition = new Vector3(transform.position.x, player.position.y, transform.position.z);

      gameObject.transform.LookAt(player);
      }

       

    
    }
}
