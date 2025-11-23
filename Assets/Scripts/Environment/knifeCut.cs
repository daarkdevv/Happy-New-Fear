using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class knifeCut : MonoBehaviour
{  
    [SerializeField] IntObject intObject;
    [SerializeField] Animator animator;
    [SerializeField] bool grounded;
    [SerializeField] public PizzaSlice pizzaSlice;
    [SerializeField] AudioSource playerSound;
    [SerializeField] AudioClip knifeSwingingSfx;
    [SerializeField] bool canSwing;
    [SerializeField] float swingTimer;
    [SerializeField] Inventory inventory;
    // Start is called before the first frame update
    void Start()
      
    {

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        playerSound = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        intObject = GetComponent<IntObject>();
        animator = animator.GetComponent<Animator>();
        
        if(!grounded)
        pizzaSlice = GameObject.FindGameObjectWithTag("CookedPizza").GetComponent<PizzaSlice>();
    }

    // Update is called once per frame
    void Update()
    {

        if(intObject.GoingDown) return;

        if(swingTimer > 0)
        {
             
          swingTimer -= Time.deltaTime;   
 
        }
         
        if(!intObject.isObjGrounded)
        {
            
           
           if(Input.GetMouseButtonDown(0) && swingTimer < 0)
           {
              
     
             swingTimer = 0.5f;

             playerSound.PlayOneShot(knifeSwingingSfx);


             animator.SetTrigger("KnifeSwing");

           }

        }
        
    }

    

    public void CutThePizza()
    {

       
      if(pizzaSlice != null)   
      pizzaSlice.Slice();
      
      inventory.CanChangeSlot = true;

    }

    public void CanChangeSlotFalse()
    {
      
      if(inventory.CanChangeSlot == true)
      inventory.CanChangeSlot = false;
      else
      inventory.CanChangeSlot = true;
     

    }

}
