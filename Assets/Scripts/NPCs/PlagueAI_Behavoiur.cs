using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System.Threading;

public class PlagueAI_Behavoiur : MonoBehaviour
{
    public float DetectRaduis;
    public Transform Player;
    private NavMeshAgent agent;
    public bool isMoving;

    public Transform NextPos;

    public bool HasDetectedPlayer;

    public Animator animator;

    public Bounds box;
    public float Timer;
    public LayerMask enviroment,player;

    public bool HasReachedPlayer;

    public bool HasChoosedpos;

    public bool PlayerIsTooFar;
    public float HowFarTrigger;

    public float PlayerLostTimer;

    public bool PlayerRay;

    public bool hasLostPlayer;

    public float chaseSpeed,WalkSpeed;
    public float CheckRadius,TooFarRaduis;

    public bool isBehindSomething;
    private CapsuleCollider capsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
      capsuleCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
      agent = GetComponent<NavMeshAgent>();
      Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        
         CheckStatesStatements();
         
         if(HasDetectedPlayer && !HasReachedPlayer)
         {

   
            PlayerChaseState();


         }

         else
         {
        
       
         ChooseNextPose(); 
        

         }

       


    }

    void CheckStatesStatements()
    {
     
        HasReachedPlayer = (Vector3.Distance(agent.transform.position,Player.position) <= agent.stoppingDistance);
       
        HasDetectedPlayer = Physics.CheckSphere(transform.position,CheckRadius,player);

        PlayerIsTooFar = Physics.CheckSphere(transform.position,CheckRadius,player);

        PlayerRay = Physics.Linecast(transform.position,Player.position,out RaycastHit hit);

// Check if the raycast hit a collider other than the player collider
       if (hit.collider == capsuleCollider)
       {
          isBehindSomething = false;
       }
       else
       {

        isBehindSomething = true;

       }

        Debug.DrawLine(transform.position,Player.position);

        hasLostPlayer = (PlayerLostTimer <= 0);

       animator.SetFloat("Move",agent.velocity.sqrMagnitude);
       animator.SetBool("Reached",HasReachedPlayer);
       animator.SetBool("PlayerFound",HasDetectedPlayer);

    }

    void PlayerChaseState()
    { 

         

          if(PlayerRay)
          {


            PlayerLostTimer -= Time.deltaTime;


          }

          else
          {

             PlayerLostTimer = 3f;

          }  

        
          agent.SetDestination(Player.position);

          agent.speed = chaseSpeed;

          Timer = 0;


    }



    void ChooseNextPose()
    {

      if(HasDetectedPlayer)
      {
       
        return;

      }

      agent.speed = WalkSpeed;

      if(!HasChoosedpos)
      {

         NextPos.position = new Vector3(Random.Range(transform.position.x + -box.extents.x,transform.position.x + box.extents.x),transform.position.y,Random.Range(transform.position.z + -box.extents.z,  transform.position.z + box.extents.z));

         agent.SetDestination(NextPos.position);

         HasChoosedpos = true;

        
      }

      if(Timer <= 0)
      {

        HasChoosedpos = false;
 
        Timer = Random.Range(2f,9);

      }
      
   
       Timer -= Time.deltaTime;

    }


    private void OnDrawGizmosSelected() 
    {

      Gizmos.color = Color.blue;   

      Gizmos.DrawWireCube(transform.position,box.extents);

      Gizmos.color = Color.magenta;

      Gizmos.DrawLine(transform.position,Player.position);

      Gizmos.color = Color.green;

      Gizmos.DrawWireSphere(transform.position,CheckRadius);

      Gizmos.color = Color.red;

      Gizmos.DrawWireSphere(transform.position,TooFarRaduis);

    }
}
