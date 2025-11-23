using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCollider : MonoBehaviour
{
    [SerializeField] Collider ColliderToActive;
    [SerializeField] NpcInteract npcInteract;
    [SerializeField] bool notNeeded; 
    // Start is called before the first frame update
    void Start()
    {

        npcInteract = GetComponent<NpcInteract>();
        ColliderToActive = GameObject.FindGameObjectWithTag("JeffColliderBorder").GetComponent<Collider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(npcInteract.conversationIsGoing && notNeeded)
        ColliderToActive.enabled = true;
        
    }
}
