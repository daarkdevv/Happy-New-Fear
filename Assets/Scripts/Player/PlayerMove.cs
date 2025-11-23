using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float SprintSpeed;
    public Camera camera1;
    public Transform cameraY;
    public float gravity;
    public float WalkSpeed;
    public bool isSitting_;
    public float CurrentSpeed;
    public CharacterController character;
    public bool isSprinting;
    bool HittingCelling;
    [SerializeField] public bool canSprint;
    public LayerMask whatisGround;
    public Vector3 GroundDistance;
    public Transform GroundTrans;
    public bool isCrouch;
    public bool canCrouch;
    public float crouchSmoothness;
    public float crouchSpeed;
    public float CameraYCrouch, OgCameraYCrouch;

    [HideInInspector]
    public Vector3 movement;
    public bool isGrounded, canMove;
    public Vector3 cellingCheckPosition;

    public Vector3 velocity;
    public AudioClip[] PlayerSfx; // Normal step sounds
    public AudioClip[] GrassSfx; // Grass step sounds
    private AudioSource source;
    public TMP_Text timepassed;
    public float timepassed1;
    TimeSpan timeSpan;
    private float WalkSfxTimer, SfxRunningTimer;
    public float RWalkSfxTimer, RSfxRunningTimer;
    bool Usingarrows;
    [SerializeField] CapsuleCollider PlayerColliders;
    [SerializeField] Vector3[] ColliderCenters;
    [SerializeField] float[] CollidersSizes;
    Vector2 camerayOriginal;

    public float standHeight = 2.0f;  // Height when standing
    public LayerMask ceilingLayer;    // Layer for ceiling objects

    // New variables for raycasting ground
    public LayerMask groundLayerMask; // Layer mask for detecting ground
    public float rayLength = 1.1f; // Length of the raycast

    void Start()
    {
        Application.targetFrameRate = 144;

        source = GetComponent<AudioSource>();
        character = GetComponent<CharacterController>();

        ColliderCenters[0] = PlayerColliders.center;
        ColliderCenters[1] = character.center;

        CollidersSizes[0] = PlayerColliders.height;
        CollidersSizes[1] = character.height;
    }

      void Update()
    {
         

         timepassed1 += Time.deltaTime;
         DisplayTime(timepassed1);
         

            
           if(canMove)
           {

                      float Xm = Input.GetAxisRaw("Horizontal");
          float Ym = Input.GetAxisRaw("Vertical");

            movement = transform.right * Xm + transform.forward * Ym;

           } 
                            
       
           character.Move(movement.normalized * CurrentSpeed * Time.deltaTime);

         
         velocity.y -= gravity * Time.deltaTime;

         character.Move(velocity * Time.deltaTime);

         if(isGrounded)
         {

          velocity.y = 0f;

         }


         if(movement.sqrMagnitude > 0)
         {
            

            

           if(!source.isPlaying && !isCrouch)
           {

             if(WalkSfxTimer <= 0 && !isSprinting)
             {

               WalkSfxTimer = RWalkSfxTimer;
               PlayStepSound();

             }

             else
             {

               WalkSfxTimer -= Time.deltaTime;

             }

             if(isSprinting && SfxRunningTimer <= 0)
             {

                                PlayStepSound();

                 SfxRunningTimer = RSfxRunningTimer;

             }

             else
             {

               SfxRunningTimer -= Time.deltaTime;

             }



           }
       

         }

        
  
         if(canMove)
         CheckSprint();
         
         CheckGround();
         CheckCellingCrouch();
         Crouch();

    }



   void PlaySfx(AudioClip[] clipArray)
    {
        source.PlayOneShot(clipArray[UnityEngine.Random.Range(0, clipArray.Length)]);
        if (isSprinting)
            source.pitch = UnityEngine.Random.Range(1.1f, 1.2f);
        else
            source.pitch = UnityEngine.Random.Range(0.8f, 1.0f);
    }

    void PlayStepSound()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, -transform.up, out hit, rayLength))
        {
          
                // Check the ground tag or material to determine the surface type
                if (hit.collider.CompareTag("Grass"))
                {
                    PlaySfx(GrassSfx);
                     // Play grass step sound
                }
                else
                {
                    PlaySfx(PlayerSfx);
                     // Play grass step sound
                     // Play normal step sound
                }
            
        }
    }

    void CheckCellingCrouch()
{
    RaycastHit hit;
    
    // Half extents of the box (width, height, depth) - adjust these according to your character's dimensions
Vector3 halfExtents = new Vector3(character.radius, 0.3f, character.radius);
    // Perform the BoxCast upwards from the player's position to check for a ceiling
    if (Physics.BoxCast(transform.position + cellingCheckPosition, halfExtents, Vector3.up, out hit, Quaternion.identity, rayLength))
    {
        // If the BoxCast hits something above
        HittingCelling = true;

        if(isSitting_) return;

        if (isCrouch)
        {
            canCrouch = false;  // Player can't stand up if crouched
        }
        else
        {
            canCrouch = true;  // Player can't crouch either
        }
    }
    else
    {
        // No ceiling detected, player can crouch/stand up
        HittingCelling = false;

        if (!isSitting_)
        {
              canCrouch = true;
        }
        else
        {

              canCrouch = false;


        }
    }
}

    void Crouch()
    {

      if(!canCrouch) return;
         
      if(Input.GetKeyDown(KeyCode.LeftControl) && !isCrouch)
      isCrouch = true;
      else if(Input.GetKeyDown(KeyCode.LeftControl) && isCrouch)
      isCrouch = false;
             

      crouchLerpSize(isCrouch);
               

    }

    void crouchLerpSize(bool isCrouchs)
    {

     Vector2 cameracrouch = new Vector2(cameraY.localPosition.x,CameraYCrouch);

     camerayOriginal = new Vector2(cameraY.localPosition.x,OgCameraYCrouch); 


     if(isCrouchs)
     {

      CurrentSpeed = crouchSpeed;

        PlayerColliders.center = Vector3.Lerp(PlayerColliders.center,ColliderCenters[2],crouchSpeed);
        character.center = Vector3.Lerp(character.center,ColliderCenters[2],crouchSpeed);

      PlayerColliders.height = Mathf.Lerp(PlayerColliders.radius,CollidersSizes[2],crouchSmoothness);
      character.height = Mathf.Lerp(character.height,CollidersSizes[2],crouchSmoothness);

          cameraY.transform.localPosition = Vector2.Lerp(cameraY.transform.localPosition,cameracrouch,crouchSmoothness); 


     }

     else
     {


        PlayerColliders.center = Vector3.Lerp(PlayerColliders.center,ColliderCenters[0],crouchSpeed);
        character.center = Vector3.Lerp(character.center,ColliderCenters[1],crouchSpeed);

        PlayerColliders.height = Mathf.Lerp(PlayerColliders.height,CollidersSizes[0],crouchSmoothness);
        character.height = Mathf.Lerp(PlayerColliders.height,CollidersSizes[1],crouchSmoothness);
 
            cameraY.transform.localPosition = Vector2.Lerp(cameraY.transform.localPosition,camerayOriginal,crouchSmoothness); 

 
     }


    }


    void CheckSprint()
    {
        if (!isCrouch && !isSprinting)
            CurrentSpeed = WalkSpeed;

        if (!canSprint)
        {
            CurrentSpeed = WalkSpeed;
            isSprinting = false;
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift) && movement.sqrMagnitude > 0 && !isCrouch)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (isSprinting)
        {
            CurrentSpeed = SprintSpeed;
            camera1.fieldOfView = Mathf.Lerp(camera1.fieldOfView, 74, 0.3f);
        }
        else
        {
            camera1.fieldOfView = Mathf.Lerp(camera1.fieldOfView, 65, 0.3f);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float hours = Mathf.FloorToInt(timeToDisplay / 3600); // Calculate hours
        float minutes = Mathf.FloorToInt((timeToDisplay % 3600) / 60); // Calculate minutes
        float seconds = Mathf.FloorToInt(timeToDisplay % 60); // Calculate seconds

        timepassed.text = string.Format("{0:00} : {1:00} : {2:00}", hours, minutes, seconds);
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckBox(GroundTrans.position, GroundDistance, Quaternion.identity, whatisGround);
    }

void OnDrawGizmos()
{
    // Define the half-extents of the box, matching the dimensions used in the BoxCast
    Vector3 halfExtents = new Vector3(character.radius, 0.3f, character.radius);

    // Calculate the center of the box based on the player's position and ceiling check position
    Vector3 boxCenter = transform.position + cellingCheckPosition + Vector3.up * rayLength / 2;

    // Draw the box gizmo in the Scene view (use the same rotation as in the BoxCast if needed)
    Gizmos.color = Color.red;  // You can change the color to distinguish it from other gizmos
    Gizmos.DrawWireCube(boxCenter, halfExtents * 2);  // Multiply by 2 to get the full size of the box
}


}
