using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [SerializeField] public Transform Target;
    [SerializeField] public Transform player;
    [SerializeField] public bool CanInteract;
    [SerializeField] NpcInteract npcInteract;
    [SerializeField] NavMeshAgent ai;
    [SerializeField] Animator animator;

    [Header("Status")]
    [SerializeField] bool isRunning;
    [SerializeField] bool isFollowingPlayer;
    [SerializeField] bool isSitting;
    [SerializeField] bool PassiveNpc;
    [SerializeField] bool ShouldMove;
    [SerializeField] bool FollowPlayer;
    [SerializeField] bool NeedsAcontainer;
    [SerializeField] AudioSourceContainer container;
    [SerializeField] string ContainerTag;
    
    [Header("Mission Related")]
    [SerializeField] bool[] ShouldMoveMission;
    [SerializeField] public bool[] ShouldTalk;
    [SerializeField] string[] AnimationTrigger;
    [SerializeField] bool[] shouldSit;
    [SerializeField] int lookDirectionOrder;
    [SerializeField] bool[] IncreasePosition;
    [SerializeField] bool[] shouldStand;
    [SerializeField] bool[] ShouldCrouch;
    [SerializeField] bool[] shouldWaitForPlayer;
    [SerializeField] Transform[] LookingDirections;
    [SerializeField] Transform[] TargetPosition;
    [SerializeField] int[] currentMissionStop;
    [SerializeField] bool[] ReadOnlyHasTalked;
    [SerializeField] public int CurrentPosition;
    [SerializeField] public int LastPosition;
    [SerializeField] public int TalkOrder;
    [SerializeField] bool WaitingForPlayer;

    [Header("FOV")]
    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float detectionAngle = 45f;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstructionMask;
    [SerializeField] bool canSeeTarget;
    [SerializeField] bool obstacleDetected;
    [SerializeField] bool playerIsInDetectionRange;
    [SerializeField] bool hasLostPlayer;
    [SerializeField] float playerLossDelayTimer;
    [SerializeField] float playerLossDelay;

    [Header("Properties")]
    [SerializeField] float WanderRadius;
    [SerializeField] bool ChooseNewPos;
    [SerializeField] float PastSpeed;
    [SerializeField] float DistanceToWaitForPlayer;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (CanInteract)
        npcInteract = GetComponent<NpcInteract>();

        ai = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        StartCoroutine(FOVRoutine());

        PastSpeed = ai.speed;
        CurrentPosition = 0;
       
       if(!NeedsAcontainer) return;
       

       container  = GameObject.FindGameObjectWithTag(ContainerTag).GetComponent<AudioSourceContainer>();
         
       for (int i = 0; i < container.transforms.Length; i++)
       {

        TargetPosition[i] = container.transforms[i];
        
       }  

    }

    private void Update()
    {
        HandleAnimation();

        if(PassiveNpc)
        PassiveNpc_();

        FieldOfViewCheck();

        if(PassiveNpc) return;

        Wander();
        LoseSightOfPlayer();
    }

    private void HandleAnimation()
    {

        
        animator.SetFloat("Move", ai.velocity.sqrMagnitude);
        animator.SetBool("Run", isRunning);
       // animator.SetBool("isSitting",isSitting);
       

    }

    void PassiveNpc_()
    {
       if(ShouldMoveMission[CurrentPosition])
       {
         ai.enabled = true; // Replace ai.speed = PastSpeed with ai.enabled = true
         ai.destination = TargetPosition[CurrentPosition].position;

         if(shouldWaitForPlayer[CurrentPosition])
         {
            if(Vector3.Distance(transform.position,player.position) >= DistanceToWaitForPlayer)
            {
               ai.enabled = false; // Replace ai.speed = 0 with ai.enabled = false
               WaitingForPlayer = true;

                LookAtTarget(player.position);
            }
            else
            {
               ai.enabled = true; // Replace ai.speed = PastSpeed with ai.enabled = true
               WaitingForPlayer = false;
            }
         }

         if(ai.enabled == true && !ai.pathPending && (ai.remainingDistance <= ai.stoppingDistance) )
         {
            LastPosition = CurrentPosition;
            CurrentPosition += (IncreasePosition[CurrentPosition] == true ? 1 : 0);
            ai.enabled = true; // Replace ai.speed = PastSpeed with ai.enabled = true
         }
       }
      else if (!ShouldMoveMission[CurrentPosition] && !isSitting)
{
    // Stop the AI movement
    ai.velocity = Vector3.zero;
    ai.enabled = false;

    // Handle NPC interaction logic
    if (npcInteract.conversationIsGoing && !isSitting)
    {
        var currentConversation = npcInteract.conversations[npcInteract.currentCoversation];
        int currentAudioIndex = currentConversation.CurrentAudio;

        // Ensure the current audio index is within range
        if (currentAudioIndex >= 0 && currentAudioIndex < currentConversation.Talkers.Length)
        {
            var currentTalkerPosition = currentConversation.Talkers[currentAudioIndex].gameObject.transform.position;

            // Look at either the target position or the current talker position
            LookAtTarget(currentTalkerPosition == transform.position 
                ? TargetPosition[CurrentPosition].position 
                : currentTalkerPosition);
        }
        else
        {
            // Fallback to target position if the audio index is out of range
            LookAtTarget(TargetPosition[CurrentPosition].position);
        }
    }
    else if (WaitingForPlayer)
    {
        // Look at the player if waiting for them
        LookAtTarget(player.position);
    }
    else
    {
        // Default to looking at the target position
        LookAtTarget(TargetPosition[CurrentPosition].position);
    }

    // Re-enable AI after setting rotation
    ai.enabled = true;
}

      if(shouldSit[CurrentPosition])
      { 

          if(!isSitting)
          {

           animator.SetTrigger("Sit");

           isSitting = true;

          }
           
          LookAtTarget(LookingDirections[lookDirectionOrder].position); 
         
      }

      
    
       if(playerIsInDetectionRange)
       {
          if(ShouldTalk[TalkOrder]) 
          {
            ReadOnlyHasTalked[CurrentPosition] = true;
            TalkOrder++;
            npcInteract.TriggerEvent_();
          }
       }
    }

    public void SetSitToTrue()
    {

        isSitting = true;

    }

    void LookAtTarget(Vector3 targetPosition)
    {
        // Disable NavMeshAgent rotation control temporarily
        ai.updateRotation = false;

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Ignore vertical difference for looking
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Re-enable NavMeshAgent rotation control
        ai.updateRotation = true;
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < detectionAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                obstacleDetected = Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask);
            }

            playerIsInDetectionRange = true;
        }
        else
        {
            playerIsInDetectionRange = false;
        }
    }

    void LoseSightOfPlayer()
    {
        if(!playerIsInDetectionRange && !obstacleDetected)
        {
           playerLossDelay -= Time.deltaTime;
        }
        else
        {
           playerLossDelay = playerLossDelayTimer;
        }

        hasLostPlayer = playerLossDelay < 0;
        canSeeTarget = !hasLostPlayer;

        if(canSeeTarget)  
            ai.destination = player.position;
    }

    private void Wander()
    {
        if (!canSeeTarget) // Only wander if the NPC is not seeing the target
        {
            // Check if the NPC has reached its current destination or needs a new position
            if (!ai.pathPending && (ai.remainingDistance <= ai.stoppingDistance))
            {
                // Choose a new position to wander to
                Vector3 newPos = RandomNavSphere(player.position, WanderRadius, -1);
                ai.destination = (newPos);
            }

            isRunning = false; // Ensure the NPC is in a walking state, not running
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 finalPosition = origin;
        bool validPosition = false;

        while (!validPosition)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;
            randDirection += origin;
            NavMeshHit navHit;

            if (NavMesh.SamplePosition(randDirection, out navHit, dist, layermask))
            {
                if (!Physics.Raycast(origin, (navHit.position - origin).normalized, dist))
                {
                    finalPosition = navHit.position;
                    validPosition = true;
                }
            }
        }

        return finalPosition;
    }

    // Draw Gizmos to visualize the FOV
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(detectionAngle / 2, transform.up) * transform.forward * detectionRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-detectionAngle / 2, transform.up) * transform.forward * detectionRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + fovLine1);
        Gizmos.DrawLine(transform.position, transform.position + fovLine2);

        if (canSeeTarget)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
