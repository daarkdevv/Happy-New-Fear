using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaveControl : MonoBehaviour
{
    [SerializeField] NPC JeffNPC;
    [SerializeField] NPC DaveNpc;
    [SerializeField] NpcInteract daveInter;
    [SerializeField] bool ActionJeffDOne;
    // Start is called before the first frame update
    void Start()
    {

        DaveNpc = GetComponent<NPC>();
        daveInter = GetComponent<NpcInteract>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(JeffNPC.CurrentPosition == 1)
        {

          DaveNpc.ShouldTalk[0] = true;

        }

        if(daveInter.conversations[0].hasFinishedConv && !ActionJeffDOne)
        {
            JeffNPC.CurrentPosition++;
            DaveNpc.CurrentPosition++;
            ActionJeffDOne = true;
        }
        
    }
}
