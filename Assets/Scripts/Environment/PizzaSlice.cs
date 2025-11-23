using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaSlice : MonoBehaviour
{
    [SerializeField] GameObject SlicedPizza;
    [SerializeField] Inventory inventory;
    [SerializeField] RaycastChecker raycastChecker;
    [SerializeField] AudioSource playerSound;
    [SerializeField] AudioClip SliceSound;
    [SerializeField] AudioClip KnifeSwing;
    QuestManager questManager;
    bool hasDone;
    
    private GameObject knifeObject;

    knifeCut knifeCut_;
    // Start is called before the first frame update
    void Start()
    {

      inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
      raycastChecker = GetComponent<RaycastChecker>();
      playerSound = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
      questManager = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestManager>();
      
                knifeCut_ = GameObject.FindGameObjectWithTag("Knife").GetComponent<knifeCut>();

          knifeCut_.pizzaSlice = GetComponent<PizzaSlice>();  
   
        
    }



    // Update is called once per frame
    void Update()
    {

      
        
       if(raycastChecker.isRaycasted && inventory.inventoryItems[inventory.CurrentInventorySlot].tag == "Knife")
       raycastChecker.DisplayText = " LMB To Slice";
       else if(raycastChecker.isRaycasted)
       raycastChecker.DisplayText = "Need A Knife";

        
    }

    public void Slice()
    {


      if(!raycastChecker.isRaycasted)
      return;

      questManager.currentMission++;

      GameObject slicedVersion = Instantiate(SlicedPizza,transform.position,Quaternion.identity);

      slicedVersion.transform.rotation = Quaternion.Euler(-90,transform.localRotation.y,transform.localRotation.z);

      playerSound.PlayOneShot(SliceSound);

      inventory.RemoveItemFromInventory();

      inventory.CanChangeSlot = true;

      Destroy(gameObject);

    }
    
}
