using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] PlayerMove Movement;
    [SerializeField] bool EnableStamina;

    [Header("Stamina Properties")]

    [SerializeField] float MaxStamina;
    [SerializeField] float HiddenStamina;
    [SerializeField] float staminaPercentage;
    [SerializeField] float MaxHiddenStamina;
    [SerializeField] float Stamina;
    [SerializeField] float StaminaDecreaseRate;
    [SerializeField] float StaminaRegenRate;
    [SerializeField] bool CanRegen;
    [SerializeField] bool RanOutOfHiddenStamina;
    //[SerializeField] TMP_Text StaminaText;
    [SerializeField] Image StaminaBar_;
    [SerializeField] int CurrentStamStage;


    [Header("Timers")]

    [SerializeField] float AfterSprintTimer;
    [SerializeField] float ResetTimer;
    [SerializeField] float MaxPlusMax;

     
    [Header("UI ELEMENTS")]
    [SerializeField] Color[] Gradient;
    [SerializeField] float colorLerpSpeed;

    
    
    // Start is called before the first frame update
    void Start()
    {

        Movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        MaxPlusMax = MaxHiddenStamina + MaxStamina;
        
    }

    // Update is called once per frame
    void Update()
    {

        

        if(!EnableStamina)
        return;

        if(Movement.isSprinting)
        {

             StaminaFunction();

        }

        else
        {

             StaminaRegen();

        }


       StaminaBarColor(); 
       
       // StaminaText.text = "Stamina" + "" + Stamina;

       if(HiddenStamina <= 0)
       {

         RanOutOfHiddenStamina = true;

       }

        if(RanOutOfHiddenStamina)
        {
          
          Movement.canSprint = false;

        } 

        else
        {

          Movement.canSprint = true;

        }

        StaminaBar_.fillAmount = Mathf.Clamp(Stamina + HiddenStamina , 0 ,Stamina + HiddenStamina) / 50f;
        
    }

    void StaminaBarColor()
    {

       staminaPercentage = staminaPercentageCalc(Stamina,MaxStamina);

       StaminaBar_.color = Color.Lerp(StaminaBar_.color,Gradient[CurrentStamStage],colorLerpSpeed);
       
       if(IsInRange(staminaPercentage,75f,100f))
       {

         CurrentStamStage = 0;


       }

       else if(IsInRange(staminaPercentage,45,69))
       {


          CurrentStamStage = 1;


       }

       else if(IsInRange(staminaPercentage,25,44))
       {

         
         CurrentStamStage = 2;


       }

       else if(IsInRange(staminaPercentage,0,24))
       {

        
        CurrentStamStage = 3;


       }
  
      

    }

    void StaminaFunction()
    {

       CanRegen = false; 
       
       if(Stamina > 0)
       Stamina -=  StaminaDecreaseRate * Time.deltaTime;
       else
       HiddenStamina -= StaminaDecreaseRate * Time.deltaTime;
       

       AfterSprintTimer = ResetTimer;

 

    }

    void StaminaRegen()
    {
       
      CheckAfterTimer();
  
      if(CanRegen && Stamina < MaxStamina && !RanOutOfHiddenStamina)
      Stamina += StaminaRegenRate * Time.deltaTime;

      if(Stamina > MaxStamina)
      Stamina = MaxStamina;

      if(HiddenStamina < MaxHiddenStamina && CanRegen)
      {
        
        HiddenStamina += StaminaRegenRate * Time.deltaTime;

      }

      else
      {

        RanOutOfHiddenStamina = false;
 

      }
       

    }

    void CheckAfterTimer()
    {

       if(AfterSprintTimer > 0)
       {
     
  
         AfterSprintTimer -= Time.deltaTime;

       }

       else
       {

          CanRegen = true;

       }


    }

    float staminaPercentageCalc(float currentstam, float mAxStam)
    {

        return (currentstam / mAxStam) * 100;


    }

    bool IsInRange(float currentValue, float minValue, float MaxValue)
    {

      return currentValue >= minValue && currentValue <= MaxValue;


    }

}
