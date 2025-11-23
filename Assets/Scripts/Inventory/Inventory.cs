using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    public float CollectRaduis;
    public LayerMask Collectables;
    Collider[] CollectableIn;
    public int CurrentInventorySlot;
    public int MaxInventorySlots;
    [SerializeField] SprintDetector sprintDetector;
    
    [SerializeField] private Transform handPostion;
    RaycastChecker raycastChecker;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip inventorySound;

    public float mouseWheelLimit;
    public bool mouseWheelPostive;

    public int MaxSlots;
    public Transform camera_;
    public Ray ray;
    public RaycastHit hit;
    public float RayDistance;
    [SerializeField] LayerMask InteractableLayer;
    [SerializeField] LayerMask EntityLayerNblocker;
    [SerializeField] LayerMask DoorLayer;
    public List<GameObject> inventoryItems;
    public List<Transform> inventoryIcons;
    public TMP_Text raycastText;
    IntObject currentInventoryObject, inventoryObject;
    public float inventoryChangeTimer, inventoryChangeResetTimer;
    [SerializeField] PhoneManager phone;
    DoorManager doorManager;
    private GameObject currentTarget;
    public IntObject inttobj;
    private GameObject SpawnedObj;

    [SerializeField] GameObject ImageSlot;
    [SerializeField] RectTransform ImageSlotParent;
    [SerializeField] LeanTweenType EaseType;
    [SerializeField] Vector3 IconFocuesSize;
    [SerializeField] Vector3 IconNonFocuesSize;
    [SerializeField] Color FoucesdColor;
    [SerializeField] float EaseSpeed;
    [SerializeField] public bool CanChangeSlot;
    int iCounterIcon;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < handPostion.childCount; i++)
        {
            inventoryItems.Add(handPostion.GetChild(i).gameObject);
        }
        camera_ = transform.GetChild(0).transform.GetChild(0);

        if(inventoryItems[CurrentInventorySlot].GetComponent<IntObject>()!= null)
        {
                              
        sprintDetector.intttobj =  inventoryItems[CurrentInventorySlot].gameObject.GetComponent<IntObject>();


        }

        for (int i = 1; i < inventoryItems.Count; i++)
        {
        
          iCounterIcon = i;

          spawnImage(true );
            
        }

        audioSource = GetComponent<AudioSource>();


        IconSizeFunc();

        CanChangeSlot = true;
        


    }

    // Update is called once per frame
    void Update()
    {
        inventoryChangeTimer -= Time.deltaTime;

        if(inventoryChangeTimer < 0 && CanChangeSlot)
        CheckMouseWheel();

        CollectArea();
        DoorDetect();
        IsEntityType();
        DropItem();
    }

    void CollectArea()
    {
        Ray ray = new Ray(camera_.position, camera_.transform.forward);
 
        
        if (Physics.Raycast(ray, out hit, RayDistance,InteractableLayer))
        {
            if (currentInventoryObject == null)
            {
                currentInventoryObject = hit.collider.GetComponent<IntObject>();
            }

             if (hit.collider.gameObject.layer == 11 || hit.collider.gameObject.layer == 14)
             {
                currentInventoryObject = null;
                raycastText.text = ".";
                return;
             } 
           
            if(currentInventoryObject.isObjGrounded && currentInventoryObject.canBePickedUp)
            raycastText.text = currentInventoryObject.RayText;
            

            if (Input.GetKeyDown(KeyCode.E))
            {

                if(!currentInventoryObject.isObjGrounded || !CanChangeSlot || !currentInventoryObject.canBePickedUp)
                {
                 
                 return;

                }
                
                if(currentInventoryObject.SpawnItself)
                SpawnedObj = Instantiate(hit.collider.gameObject, handPostion.position, Quaternion.identity);
                else
                SpawnedObj = Instantiate(currentInventoryObject.gameObjectSpawn, handPostion.position, Quaternion.identity);


                    SpawnedObj.SetActive(false);
              
                    if(SpawnedObj.tag == "FlashLight")
                    {
                   
                      SpawnedObj.GetComponent<FlashLightManager>().DisableCounter++;

                    }
                    
                
                SpawnedObj.transform.parent = handPostion;
                Destroy(SpawnedObj.GetComponent<Rigidbody>());
                

                if(SpawnedObj.GetComponent<IntObject>() == null)
                {
                
                Destroy(hit.collider.transform.parent.gameObject);


                SpawnedObj.transform.GetChild(0).GetComponent<IntObject>().isObjGrounded = false;
                SpawnedObj.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                if(SpawnedObj.transform.GetChild(0).GetComponent<IntObject>().IncreaseMission) QuestManager.QuestInstance.currentMission++; 
                inventoryItems.Add(SpawnedObj.transform.GetChild(0).gameObject);
                SpawnedObj.transform.localScale = Vector3.one;
                SpawnedObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
                }

                else
                {
                 
                  Destroy(hit.collider.gameObject);
                 SpawnedObj.GetComponent<IntObject>().isObjGrounded = false;

                                 if(SpawnedObj.GetComponent<IntObject>().IncreaseMission) QuestManager.QuestInstance.currentMission++; 

                 SpawnedObj.GetComponent<Outline>().enabled = false;
                 inventoryItems.Add(SpawnedObj);
                 

                }


                StartCoroutine(WaitForItemToGoDown(1,false,0, true));
                
            
            }
        }
        else
        {
            raycastText.text = ".";
            currentInventoryObject = null;
        }
    }

    void DoorDetect()
    {
        Ray ray = new Ray(camera_.position, camera_.transform.forward);

        if (Physics.Raycast(ray, out hit, RayDistance,DoorLayer))
        {
            if (hit.collider.tag == "Openable")
            {
                GameObject hitObject = hit.transform.gameObject;

                // Check if the detected game object is different from the previous one
                if (currentTarget != hitObject)
                {
                    if (currentTarget != null)
                    {
                        // Disable the outline of the previous target
                        DoorManager previousDoorManager = currentTarget.GetComponent<DoorManager>();
                        if (previousDoorManager != null)
                        {
                            previousDoorManager.IsRayCastOn = false;
                        }
                    }

                    currentTarget = hitObject;
                    doorManager = currentTarget.GetComponent<DoorManager>();
                }

                // Ensure the doorManager is valid
                if (doorManager != null)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (doorManager.IsDoorLike())
                        {
                            doorManager.Interacte(false);
                        }
                        else
                        {
                            doorManager.DrawerController();
                        }
                    }

                    raycastText.text = doorManager.CurrentTextName;
                    doorManager.IsRayCastOn = true;
                }
            }
            else
            {
                ClearDoorManager();
            }
        }
        else
        {
            ClearDoorManager();
        }
    }

    void ClearDoorManager()
    {
        if (doorManager != null)
        {
            doorManager.IsRayCastOn = false;
        }

        currentTarget = null;
        doorManager = null;
    }

    void CheckMouseWheel()
    {
    
        bool IsSZero = (CurrentInventorySlot != 0);
        bool IsSmallerMax = (CurrentInventorySlot != MaxInventorySlots);

    
        
        if(!CanChangeSlot) return;


        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
           
           if(0 <= MaxInventorySlots && CurrentInventorySlot != 0)
           {

             StartCoroutine(WaitForItemToGoDown(1,true,0,false));
              return; 
               

           }
           

        }
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
           
           if(1 <= MaxInventorySlots && CurrentInventorySlot != 1)
           {

             StartCoroutine(WaitForItemToGoDown(1,true,1,false));
              return; 
             

           }
           

        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
           
           if(2 <= MaxInventorySlots && CurrentInventorySlot != 2)
           {

            StartCoroutine(WaitForItemToGoDown(1,true,2,false));
              return; 

           }
           

        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
           
           if(3 <= MaxInventorySlots && CurrentInventorySlot != 3)
           {

             StartCoroutine(WaitForItemToGoDown(1,true,3,false));
              return; 
                

           }
           

        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
           
           if(4 <= MaxInventorySlots && CurrentInventorySlot != 4)
           {

             StartCoroutine(WaitForItemToGoDown(1,true,4,false));
              return; 
               

           }
           

        }


        if (Input.mouseScrollDelta.y > mouseWheelLimit && IsSmallerMax)
        {
            mouseWheelPostive = true;
            StartCoroutine(WaitForItemToGoDown(1,false,0,false));
            return;
        }
        else if (Input.mouseScrollDelta.y < -mouseWheelLimit && IsSZero)
        {
            mouseWheelPostive = false;

            StartCoroutine(WaitForItemToGoDown(-1,false,0,false));

            return;
        }
         
        if(Input.mouseScrollDelta.y < -mouseWheelLimit && !IsSZero)
        {

            mouseWheelPostive = false;

            StartCoroutine(WaitForItemToGoDown(1,true,MaxInventorySlots,false));

            return;

        }


        if (Input.mouseScrollDelta.y > mouseWheelLimit && !IsSmallerMax)
        {
            mouseWheelPostive = true;

            StartCoroutine(WaitForItemToGoDown(1,true,0,false));

            return;
        } 

    }

    IEnumerator WaitForItemToGoDown(int delta , bool NonDelta , int NonDeltaNum,bool PickedUp)
    {

        if(inventoryChangeTimer > 0 && !PickedUp)
        yield break;
 
        inventoryChangeTimer = inventoryChangeResetTimer;
         
        if(inventoryItems[CurrentInventorySlot].GetComponent<IntObject>() != null)
        {
        
        var CurrentInventoyItem = inventoryItems[CurrentInventorySlot].GetComponent<IntObject>();

        CurrentInventoyItem.GoingDown = true;

        yield return new WaitForSeconds(0.12f);

        CurrentInventoyItem.GoingDown = false;

        }
        
        if(NonDelta)
        CurrentInventorySlot = NonDeltaNum;
        else
        CurrentInventorySlot += delta;
        

     

        audioSource.PlayOneShot(inventorySound);

        if(PickedUp)
        {


                ChangeInventory(1,true);

                spawnImage(false);
                Debug.Log(CurrentInventorySlot + "a7a");

                IconSizeFunc();
           
                SpawnedObj.SetActive(true); 
         
                inventoryItems[CurrentInventorySlot].gameObject.GetComponent<IntObject>().SetYPenalty();
                inventoryItems[CurrentInventorySlot].gameObject.GetComponent<IntObject>().RemoveNullAndOutlineMaterials();
                Debug.Log("Done 1 To inventory");

                if (SpawnedObj.tag == "Phone")
                {
                    phone.PhoneInInventory = true;
                    phone.phoneTextDisplayer = inventoryItems[CurrentInventorySlot].GetComponent<PhoneTextDisplayer>();
                    phone.phoneTextDisplayer.enableFeature = true;
                    phone.CheckCallingPhoneText(true);
                    phone.CheckTextPages();
                    //phone.TextListTextCheck();
                }
              
            
        }


        else
        {

        ChangeInventory(0,false);

        }
       
        yield return null;
    }

    void IsEntityType()
    {
        Ray ray = new Ray(camera_.position, camera_.transform.forward);

        if (Physics.Raycast(ray, out hit, RayDistance,EntityLayerNblocker))
        {
           
            if(hit.transform.gameObject.layer == 11 || hit.transform.gameObject.layer == 14)
            {
                if(raycastChecker == null) return; 
            
                raycastChecker.isRaycasted = false;
                raycastChecker = null;
                return;
            }
            


            if (raycastChecker == null )
            {
                raycastChecker = hit.collider.GetComponent<RaycastChecker>();
            }

            if(raycastChecker.IsEnabled == false)
            {
                return;
            }  

            raycastChecker.isRaycasted = true;
            raycastText.text = raycastChecker.DisplayText;
        }
        else
        {
            if (raycastChecker != null)
            {
                raycastChecker.isRaycasted = false;
                raycastChecker = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(camera_.position, camera_.transform.forward * RayDistance);
    }

    public void ChangeInventory(int MaxIncrease  , bool Pickup)
    {
        
        MaxInventorySlots += MaxIncrease;
        
        if(Pickup)
        CurrentInventorySlot = MaxInventorySlots;
        else
        CurrentInventorySlot += MaxIncrease;


        for (int i = 0; i < MaxInventorySlots + 1; i++)
        {
            

            if (i == CurrentInventorySlot)
            {
                inventoryItems[i].SetActive(true);

                if(inventoryItems[i].GetComponent<IntObject>()!= null)
                {
                 

                 inventoryItems[i].gameObject.GetComponent<IntObject>().SetYPenalty();
                 
                 sprintDetector.intttobj =  inventoryItems[i].gameObject.GetComponent<IntObject>();


                }
      

            }
            else if (i != CurrentInventorySlot)
            {

                if(inventoryItems[i].GetComponent<IntObject>()!= null)
                {
                   
                   inventoryItems[i].gameObject.GetComponent<IntObject>().setPosition();

                }
              
                inventoryItems[i].SetActive(false);
            }


            CheckPhone(i);
        }
            
            
            if(!Pickup)
            IconSizeFunc(); 


    }

    public void DropItem()
    {

          if(Input.GetKeyDown(KeyCode.G))
          {

         inttobj =  inventoryItems[CurrentInventorySlot].GetComponent<IntObject>();
          inttobj.isObjGrounded = true;
        inttobj.GetComponent<Outline>().enabled = true;

        

          if(inttobj.IsDropable)
          {

            if(inttobj.DropThisOnDestroy != null)
            {

                GameObject newObject = Instantiate(inttobj.DropThisOnDestroy, transform.position, Quaternion.identity);


            }
        

          }

          else
          {

          return;

          }
          

          RemoveItemFromInventory();



          }


    

    }

    void spawnImage(bool icounter )
    {

       
         GameObject ImageSlott = Instantiate(ImageSlot,ImageSlotParent.transform.position,Quaternion.Euler(0,0,0));
         ImageSlott.transform.localScale = IconNonFocuesSize;
         ImageSlott.transform.position = new Vector3(transform.position.x,transform.position.y,0);
         ImageSlott.transform.SetParent(ImageSlotParent, false);
         
         UnityEngine.UI.Image image = ImageSlott.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
         
         if(icounter)
         {
         IntObject intObject = inventoryItems[iCounterIcon].gameObject.GetComponent<IntObject>();
         image.sprite = intObject.InventoryIconSprite;
         image.transform.localScale = intObject.imageScale;
         image.transform.localPosition = intObject.imagePosition;
         
         }
         else
         {

         IntObject intObject = inventoryItems[CurrentInventorySlot].gameObject.GetComponent<IntObject>();
         image.sprite = intObject.InventoryIconSprite;
         image.transform.localScale = intObject.imageScale;
         image.transform.localPosition = intObject.imagePosition;

         }


         inventoryIcons.Add(ImageSlott.transform);


       //  image.rectTransform.position = intObject.imagePosition;


    }

    void IconSizeFunc()
    {
       
       for (int i = 0; i < inventoryIcons.Count; i++)
       {

         if(i == CurrentInventorySlot)
         {
           
            LeanTween.scale(inventoryIcons[i].gameObject,IconFocuesSize,EaseSpeed).setEase(EaseType);
         
            UnityEngine.UI.Image imageColor = inventoryIcons[i].GetComponent<UnityEngine.UI.Image>(); 

            imageColor.color = Color.Lerp(imageColor.color,FoucesdColor,1.5f);


         }

         else
         {
           
           LeanTween.scale(inventoryIcons[i].gameObject,IconNonFocuesSize,EaseSpeed).setEase(EaseType);

           UnityEngine.UI.Image imageColorLast = inventoryIcons[i].GetComponent<UnityEngine.UI.Image>();           imageColorLast.color = Color.Lerp(imageColorLast.color,Color.white,1.5f); 


         }
        
       }


    }

    public void RemoveItemFromInventory()
    {

        Destroy(inventoryItems[CurrentInventorySlot].gameObject);
        inventoryItems.RemoveAt(CurrentInventorySlot);   
        Destroy(inventoryIcons[CurrentInventorySlot].gameObject);
        inventoryIcons.RemoveAt(CurrentInventorySlot);   

        ChangeInventory(-1,false);


        if(inventoryItems[CurrentInventorySlot].GetComponent<IntObject>() != null)
        inventoryItems[CurrentInventorySlot].GetComponent<IntObject>().SetYPenalty();

        audioSource.PlayOneShot(inventorySound);
         
       
    }

    void CheckPhone(int i_)
    {
        if (inventoryItems[i_].tag != "Phone") return;

        if (inventoryItems[i_].activeInHierarchy) phone.PhoneInInventory = true;
        else phone.PhoneInInventory = false;
    }
}
