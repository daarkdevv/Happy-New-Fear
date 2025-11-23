
using UnityEngine;


public class FlashBattery : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private AudioSource mainAudioSource; // Audio component of object
    [SerializeField] private AudioSource flickerAudioSource;
    [SerializeField] private AudioClip flickeringClip;
    [SerializeField] private AudioClip flashIntensityClip;
    [SerializeField] private FlashLightManager flashLightManager; // Flashlight manager
    [SerializeField] private Light LightFlash; // The light of the flashlight
    [SerializeField] private IntObject intObject;

    [Header("Properties")]
    [Range(0, 100)] [SerializeField] private float currentBattery = 100f; // Current Battery Charge
    [Range(0, 2)] [SerializeField] private float intensityAmount = 0.1f;
    [Range(0, 100)] [SerializeField] private float batteryDrainAmount = 1f;
    [Range(0,5)] [SerializeField] private float batteryDrainPenalty;
    [SerializeField] float BatteryIsLowThershold;

    [Header("Bools")]
    [SerializeField] private bool batteryOn;  
    [SerializeField] private bool isOutOfBattery; // Is out of battery
    [SerializeField] private bool isAtBrightest; // Brightest Level
    [SerializeField] private bool batteryIsLow; // Low Battery
    [SerializeField] private float BatteryPercentage;

    [Header("Flickering")]   
    [SerializeField] private float flickerTimerFrequency = 0.1f;
    [SerializeField] private float resetFlickerTimerFrequency = 0.1f;
    [SerializeField] private bool flickerOn;

    [Header("MaxValues")]
    [Range(0, 100)] [SerializeField] private float maxBattery = 100f; // Maximum Battery Possible
    [Range(0,2)] [SerializeField] float MaxPitchValue;
    [Range(0,5)] [SerializeField] float MaxIntensityValue;

    [Header("MinValues")]
    [Range(0,1)] [SerializeField] float MinPitchValue;
    [Range(0,2)] [SerializeField] float MinIntensityValue;

    [Header("StoredValues")]
    [SerializeField] float LastFlashLightIntensity;
    [SerializeField] float StoredIntensityPlus;

    [Header("UI")]
    [SerializeField] GameObject UiElements;
    [SerializeField] UnityEngine.UI.Image BatteryBar;
    [SerializeField] Color[] Gradient;
    [SerializeField] float colorLerpSpeed;
    [SerializeField] int CurrentColorStage;
    [SerializeField] RectTransform ContainerPowerL;
    [SerializeField] int currentPowerLevel;



    // Start is called before the first frame update
    void Start()
    {

        currentBattery = maxBattery;

        intObject = GetComponent<IntObject>();

//        UiElements = GameObject.FindGameObjectWithTag("BatteryUI").gameObject;

        UiElements.SetActive(true);

       // BatteryBar = GameObject.FindGameObjectWithTag("BatteryImg").GetComponent<UnityEngine.UI.Image>();

       // ContainerPowerL = GameObject.FindGameObjectWithTag("ContainerPower").GetComponent<RectTransform>();

        LastFlashLightIntensity = LightFlash.intensity;

        StoredIntensityPlus = LightFlash.intensity;

    }

    // Update is called once per frame
    void Update()
    {
        checkFlashUI();
        if(intObject.isObjGrounded) return;

        CheckBatteryLife();
        CheckIntensityInput();
        clampRange();
        CheckColorCondition();
        CheckUI();

        if(!LightFlash.gameObject.activeInHierarchy || !batteryOn || intObject.isObjGrounded) return;

        DrainBattery();
        BatteryPercentage_(currentBattery,maxBattery);
    }

    void clampRange()
    {

        LightFlash.range = Mathf.Clamp(LightFlash.range,73.5f,84.5f);
       

    }

    void UpdateContainerSize(bool Active)
    {

      if(Active)
      {
        for (int i = 0; i <= currentPowerLevel; i++)
        {

                ContainerPowerL.GetChild(i).gameObject.SetActive(Active);   
        }

      }
      else
      {

        ContainerPowerL.GetChild(currentPowerLevel + 1).gameObject.SetActive(Active); 


      }   

    
      

      UpdateContainerColor();

    }
    
    void UpdateContainerColor()
    {
        

       
        for (int i = 0; i < ContainerPowerL.childCount; i++)
        {

        ContainerPowerL.GetChild(i).GetComponent<UnityEngine.UI.Image>().color = Gradient[currentPowerLevel];
            
        }
          
    


    }
    


    void CheckColorCondition()
    {

        BatteryPercentage = BatteryPercentage_(currentBattery,maxBattery);
      
      if(IsInRange(BatteryPercentage,75f,100f))
       {

         CurrentColorStage = 0;


       }

       else if(IsInRange(BatteryPercentage,45,69))
       {


          CurrentColorStage = 1;


       }

       else if(IsInRange(BatteryPercentage,25,44))
       {

         
         CurrentColorStage = 2;


       }

       else if(IsInRange(BatteryPercentage,0,24))
       {

        
        CurrentColorStage = 3;


       }


    }

       float BatteryPercentage_(float CurrentBattery, float maxBattery)
    {

        return (CurrentBattery / maxBattery) * 100;


    }

    void checkFlashUI()
    {
           
        if(UiElements != null && !intObject.isObjGrounded)
        UiElements.SetActive(true);
        else if(UiElements != null && gameObject.activeInHierarchy && intObject.isObjGrounded)
        UiElements.SetActive(false);
        else if(UiElements != null && !gameObject.activeInHierarchy && !intObject.isObjGrounded)
        UiElements.SetActive(false);

  



    }

    void CheckUI()
    {
       
       BatteryBar.fillAmount = BatteryPercentage_(currentBattery,maxBattery) / 100;
       BatteryBar.color = Color.Lerp(BatteryBar.color,Gradient[CurrentColorStage],colorLerpSpeed);

    }



    void CheckBatteryLife()
    {
        isOutOfBattery = currentBattery <= 0; // Less or equal than zero battery is out
        batteryIsLow = currentBattery <= BatteryIsLowThershold; // Low Battery Detected

        if (isOutOfBattery)
        {
            
            flashLightManager.CanOpenFlashlight = false;

            if(!flashLightManager.IsFalse)
            flashLightManager.CloseFlashLight();

        }
        
        else
        {
        
         flashLightManager.CanOpenFlashlight = true;


        }

    
    }

    void DrainBattery()
    {
        if (isOutOfBattery) return;

        currentBattery -= batteryDrainAmount * Time.deltaTime;
    }

    void CheckIntensityInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ControlIntensity(intensityAmount,0.05f,batteryDrainPenalty);
            
            if(currentPowerLevel < 4)
            currentPowerLevel++;
            UpdateContainerSize(true);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ControlIntensity(-intensityAmount,-0.05f,-batteryDrainPenalty);
            
            if(currentPowerLevel > 0)
            currentPowerLevel--;
            UpdateContainerSize(false);
        }
    }

    void ControlIntensity(float amount , float pitchamout ,float DrainAmount)
    {
        if (isOutOfBattery) return;
        
        bool LightExceededValuesP = (LightFlash.intensity >= MaxIntensityValue && amount > 0);
        bool LightExceededValuesN = (LightFlash.intensity <= MinIntensityValue && amount < 0);      
        bool PitchExceededValuesP = ( mainAudioSource.pitch >= MaxPitchValue && amount > 0);
        bool PitchExceededValuesN = ( mainAudioSource.pitch <= MinPitchValue && amount < 0);

        
        if(!LightExceededValuesP && !LightExceededValuesN)
        {

        LightFlash.intensity += amount;

        LightFlash.range += amount * 3;
         
        LastFlashLightIntensity = LightFlash.intensity;

        batteryDrainAmount += DrainAmount;
        
        mainAudioSource.PlayOneShot(flashIntensityClip);

        flickerTimerFrequency = 0.2f;

        }

        if(!PitchExceededValuesP && !PitchExceededValuesN)
        mainAudioSource.pitch += pitchamout;

        

    }

      bool IsInRange(float currentValue, float minValue, float MaxValue)
    {

      return currentValue >= minValue && currentValue <= MaxValue;


    }

    private void OnDisable() {
      if(UiElements != null)
      UiElements.SetActive(false);
    }


}
