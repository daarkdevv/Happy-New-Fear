using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] public bool Active;
    [SerializeField] bool IncreaseOnPlace;
    [SerializeField] QuestManager questManager;
    [SerializeField] bool DestroyOnPlace;
    [SerializeField] bool isInstant;
    [SerializeField] IntObject currentInventoryObject;
    [SerializeField] float CompleteValue , ValueSmoothness;
    [SerializeField] Image[] CompleteBars;
    [SerializeField] float NeededValue;
    [SerializeField] GameObject BarManager;
    [SerializeField] RaycastChecker raycastChecker;
    [SerializeField] string ogRaycastText;
    [SerializeField] Inventory inventory;
    [SerializeField] string NeededObject;
    [SerializeField] public Vector3 PlacePosition,Scale,Rotaion;
    [SerializeField] int questOrder;

    
    
    public GameObject spawned;
    // Start is called before the first frame update
    void Start()
    {
       
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        
        raycastChecker = GetComponent<RaycastChecker>();
        ogRaycastText = raycastChecker.DisplayText;
        
        questManager = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestManager>();

        
    }

    // Update is called once per frame
    void Update()
    { 
        if(questOrder != questManager.currentMission)
        {

          Active = false;
          raycastChecker.IsEnabled = false;

        }
        else
        {

          Active = true;
          raycastChecker.IsEnabled = true;

        }
        

        if(!Active)
        {

         
          return;

        }
   


        

        CheckRay_();
        
    }

    void CheckRay_()
    {

      if(raycastChecker.isRaycasted)
      {

        if(Input.GetKeyDown(KeyCode.E))
        {

          if(inventory.inventoryItems[inventory.CurrentInventorySlot].GetComponent<IntObject>() != null && inventory.inventoryItems[inventory.CurrentInventorySlot].GetComponent<IntObject>().RayText == NeededObject)
          currentInventoryObject = inventory.inventoryItems[inventory.CurrentInventorySlot].GetComponent<IntObject>();

        }

        if(Input.GetKey(KeyCode.E))
        {

         EnableCompleteValue();

        }

        else if(!Input.GetKey(KeyCode.E))
        {

         DisableCompleteValue();

        }


      }

      else
      {

        
        DisableCompleteValue();


      }




    }

    void DisableCompleteValue()
    {

          if(BarManager.activeInHierarchy && isInstant == true)
          BarManager.SetActive(false);  

          CompleteValue = 0;

          raycastChecker.DisplayText = ogRaycastText;


    }

    void EnableCompleteValue()
    {
          if(currentInventoryObject == null)
          return;

          if(!BarManager.activeInHierarchy && isInstant == false)
          BarManager.SetActive(true);  
          
          CompleteValue += Time.deltaTime * ValueSmoothness;

          raycastChecker.DisplayText = null;
          
          for (int i = 0; i < CompleteBars.Length; i++)
          {

            CompleteBars[i].fillAmount = CompleteValue;
            
          }

          if(CompleteValue >= NeededValue)
          {
            
            if(currentInventoryObject.RayText == NeededObject && currentInventoryObject.isPlacable)
            {

              if(currentInventoryObject.DropThisOnDestroy != null)
              spawned =  Instantiate(currentInventoryObject.DropThisOnDestroy.gameObject,transform.localPosition,Quaternion.identity);
              
              inventory.RemoveItemFromInventory();

              spawned.transform.rotation = Quaternion.Euler(Rotaion.x,Rotaion.y,Rotaion.z);

              spawned.transform.localScale = Scale;

              spawned.transform.position = PlacePosition;

              currentInventoryObject = null;

              if(IncreaseOnPlace) questManager.currentMission++;

              if(DestroyOnPlace) Destroy(gameObject);
              

            }
            

          }


    }

}
