
using TMPro;
using UnityEngine;


public class DoorManager : MonoBehaviour
{

 public enum OpenDirection{
  Forward ,
  BackWard,

 }

 
 [SerializeField] enum OpenableType{
  DoorLike,
  DrawerLike

 }


  [SerializeField] enum DoorOpenMethod_{
  DoorParent,
  DoorMain,

 }

   [SerializeField] enum MainAxis{
  X,
  Y,
  Z,

 }

 [SerializeField] OpenableType openableType;
 [SerializeField] OpenDirection openDirection;
 [SerializeField] DoorOpenMethod_ DoorOpenMethod;
 [SerializeField]  MainAxis mainAxis;


 [Header("General Properties")]
 [SerializeField] float LerpSpeed;
 [SerializeField] public bool IsOpen;
 [SerializeField] public bool IsRayCastOn;
 [SerializeField] TMP_Text MainText;
 [SerializeField] bool IsParentDependet;
 [SerializeField] public string TextName;
 [SerializeField] public string CurrentTextName;
 [SerializeField] float InteractTimer;
 [SerializeField] bool DebugMode;
 float MainTimer;
 Outline outline;
 
 [Header("Properties , DoorLike")]

 [SerializeField] public bool Locked;
 [SerializeField] float MainRotation;
 [SerializeField] public float YRotation;
 [SerializeField] public float XRotation;
 public Quaternion targetPos; 
 [SerializeField] float ZRotation;

 [SerializeField] AudioSource audioSource;
 [SerializeField] AudioClip[] clips;


 [Header("Properties , DrawerLike")]
 
 [SerializeField] Vector3 originalPos;
    [SerializeField] Vector3 targetDrawerPosition;
    [SerializeField] Vector3 mainForwardMovement; 
    [SerializeField] Vector3 forwardMovement;
    [SerializeField] float zPosition;
    [SerializeField] public float yPosition;
    [SerializeField] public float xPosition;
    [SerializeField] BoxCollider interactionBoxCollider; 

 private void Start() 
 {
    originalPos = transform.localPosition;

     switch (DoorOpenMethod)
    {
      case DoorOpenMethod_.DoorParent :
      IsParentDependet = true;
      break;
      
    }

     if(interactionBoxCollider != null)
     interactionBoxCollider.enabled = false;
       
    outline = GetComponent<Outline>();
  
    targetPos = Quaternion.Euler(XRotation,YRotation,ZRotation);

    targetDrawerPosition = originalPos;

    audioSource = GetComponent<AudioSource>();
    
   
    IsDoorLike();

    CurrentTextName = TextName;

    if(IsDoorLike() && Locked)
    {
   
      CurrentTextName = "Cannot Open This Door";
      

    }

    gameObject.layer = 14;

 }

 void CheckInteractTimer()
 {
    
    if(MainTimer > 0)
    {

      MainTimer -= Time.deltaTime;

    }


 }

 void SwitchToDirection()
 {

  switch (openDirection)
  {

     case OpenDirection.Forward :

     forwardMovement = -forwardMovement;

     MainAxisCheck(MainRotation); 
    
     break;
   
     case OpenDirection.BackWard :

     forwardMovement = -forwardMovement;

     MainAxisCheck(-MainRotation); 

     break;
 
  }
 

 }

 


 private void Update() 
 {

  CheckRayCast();

   if(Locked)
   return;

  CheckInteractTimer();

  if(DebugMode)
  targetPos = Quaternion.Euler(XRotation,YRotation,ZRotation);

  if(IsDoorLike() && IsParentDependet)
  {
    

    transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation,targetPos,LerpSpeed * Time.deltaTime);
   

  }

  else if(IsDoorLike() && !IsParentDependet)
  {
    
    transform.rotation = Quaternion.Slerp(transform.rotation,targetPos,LerpSpeed * Time.deltaTime);

  }

  if(!IsDoorLike())
  {
     

     transform.localPosition = Vector3.Lerp(transform.localPosition,targetDrawerPosition,LerpSpeed * Time.deltaTime);
    

  }

  


  
 }


 public void DrawerController()
 {

  
      if(Locked)
   {

     Debug.Log("Cannot Open This Door");
     return;
  
   }

   if(MainTimer > 0)
   return;
    

   if(IsOpen)
   {
     

     if(interactionBoxCollider != null)
     interactionBoxCollider.enabled = true;
   
     targetDrawerPosition = originalPos;
     IsOpen = false;
     audioSource.PlayOneShot(clips[1]);
     

   }

   else
   {
          
     if(interactionBoxCollider != null)
     interactionBoxCollider.enabled = false;

     targetDrawerPosition = new Vector3(xPosition,yPosition,zPosition);
     IsOpen = true;
     audioSource.PlayOneShot(clips[0]); 

   }

      
      
     MainTimer = InteractTimer;
 

 }

 public void Interacte(bool SkipTimer)
 {

  if(MainTimer > 0 && !SkipTimer)
  return;

  SwitchToDirection();
  IsDoorLike();



  
  Debug.Log("Interacted");

  if(IsDoorLike())
  {

   if(Locked)
   {

     Debug.Log("Cannot Open This Door");
     return;
  
   }
   CurrentTextName = TextName;
    

   if(IsOpen)
   {
   
     MainAxisCheck(0); 
     IsOpen = false;
     audioSource.PlayOneShot(clips[1]);

   }

   else
   {

     IsOpen = true;
     audioSource.PlayOneShot(clips[0]); 

   }

    targetPos = Quaternion.Euler(XRotation,YRotation,ZRotation);

      
    MainTimer = InteractTimer;


  }

  



 }

 void CheckRayCast()
 {

       if(IsRayCastOn)
       {
    
          outline.enabled = true;

       

       }

       else
       {

         outline.enabled = false;

       }
 



 }

 public bool IsDoorLike()
 {

   switch (openableType)
    {
        case OpenableType.DoorLike:
            return true;
        case OpenableType.DrawerLike:
            return false;
        default:
            // Add a default case to handle any other values of openableType
            return false; // Or return true, depending on your logic
    }

 }

 public void MainAxisCheck(float changeAxis)
 {
     if(mainAxis == MainAxis.X)
     {

       XRotation = changeAxis;

     }

     else if(mainAxis == MainAxis.Y)
     {

        YRotation = changeAxis;

     }

     else if(mainAxis == MainAxis.Z)
     {

       ZRotation = changeAxis;


     }  
       

 }
 
}
