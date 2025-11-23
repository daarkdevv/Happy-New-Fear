using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteract : MonoBehaviour
{
    [SerializeField] RaycastChecker raycastChecker;
    [SerializeField] PlayerMove move;
    [SerializeField] public Conversation[] conversations;
    [SerializeField] public bool shouldntRotateToPlayer;
    [SerializeField] public bool RayCastNot;
    [SerializeField] public int currentCoversation;
    [SerializeField] public bool conversationIsGoing;
    [SerializeField] bool rotateToPlayer;
    public Transform player; // Reference to the player's transform
    public float rotationSpeed = 5f; // Speed of rotation
    [SerializeField] bool hasCompletedRotation;
    [SerializeField] public bool canInterAct;
    
    private Quaternion initialRotation; // Store the initial rotation

    // Start is called before the first frame update
    void Start()
    {
        move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if(!RayCastNot)
        raycastChecker = GetComponent<RaycastChecker>();

        conversations = transform.GetChild(0).GetComponents<Conversation>();
        initialRotation = transform.rotation; // Store the initial rotation at start
    }

    // Update is called once per frame
    void Update()
    {


        if (conversationIsGoing)
        {

            if (conversations[currentCoversation].hasFinishedConv)
            {
                move.canMove = true;
                conversationIsGoing = false;

                if(!RayCastNot)
                raycastChecker.IsEnabled = true;

                StartCoroutine(RotateToInitial()); // Start rotating back to initial rotation

                rotateToPlayer = false;

                if(conversations[currentCoversation].Resetable)
                {

                conversations[currentCoversation].hasFinishedConv = false;
                conversations[currentCoversation].CurrentAudio = -1;

                }

                if(!canInterAct)
                {

                 disableRayCast();

                } 

                if(conversations[currentCoversation].increaseNpcConv)
                currentCoversation++;
            }
        }

        if(RayCastNot) return;

        if (Input.GetKeyDown(KeyCode.E) && raycastChecker.isRaycasted && !conversationIsGoing && hasCompletedRotation && canInterAct)
        {

            move.canMove = false;
            rotateToPlayer = true;
            TriggerEvent_();
        }

        TurnTowardsPlayer();
    }

    public void TriggerEvent_()
    {
         
        disableRayCast();
        conversations[currentCoversation].ConversationOn = true;
        conversationIsGoing = true;

    }

    public void disableRayCast()
    {

          if(RayCastNot) return;
 
        raycastChecker.IsEnabled = false;
        raycastChecker.objectOutline.enabled = false;

    }

    void TurnTowardsPlayer()
    {
        
        if (!rotateToPlayer || shouldntRotateToPlayer) return;

        // Calculate direction to player
        Vector3 direction = (player.position - transform.position).normalized;

        // Calculate target rotation
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    IEnumerator RotateToInitial()
    {
        if(shouldntRotateToPlayer) yield break;

        hasCompletedRotation = false;

        while (Quaternion.Angle(transform.rotation, initialRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }
        transform.rotation = initialRotation; // Ensure it ends at the exact initial rotation

        hasCompletedRotation = true;
    }
}
