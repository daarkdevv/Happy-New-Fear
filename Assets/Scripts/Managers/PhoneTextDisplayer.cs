using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class PhoneTextDisplayer : MonoBehaviour
{
    [SerializeField] public bool enableFeature;

    [SerializeField] public bool canDisableTxt;
    [SerializeField] public List<string> CurrentPhoneTexts;
    [SerializeField] public bool IsTextDisabled;
    [SerializeField] public float DisableTime, RDisableTime;
    [SerializeField] bool Decrease;
    
    [SerializeField] TMP_Text PhoneScreenText;
    [SerializeField] public int currentOrder;
    [SerializeField] bool hasChangedNumber;
    [SerializeField] public AudioClip PhoneClip;
    [SerializeField] public AudioSource phoneManagerSource;
    [SerializeField] public bool CanPlaySounds;
    // Start is call ed before the first frame update
    void Start()
    {

        phoneManagerSource = GameObject.FindGameObjectWithTag("PhoneManager").GetComponent<AudioSource>();
        
        
    }

    // Update is called once per frame
    void Update()
    {

        
        PhoneTextDisplayerWork();
        

    }

    void PhoneTextDisplayerWork()
    {

        if(CurrentPhoneTexts[0] == null)
        {

          DisableTime = RDisableTime;  
    
          return; 

        }
        

        if(!enableFeature)
        {
         
 
          return; 

        }

        if(CurrentPhoneTexts[1] == null)
        {
          DisableTime = RDisableTime;
          PhoneScreenText.text = CurrentPhoneTexts[0];
          PhoneScreenText.enabled = true;
          return;

        }


        if (DisableTime <= 0)
        {
            if (IsTextDisabled)
            {
                // Enable text and update content
                PhoneScreenText.enabled = true;
                PhoneScreenText.text = CurrentPhoneTexts[currentOrder];

                // Update currentOrder
                if (!Decrease && currentOrder == CurrentPhoneTexts.Count - 1)
                {
                    Decrease = true;
                }
                else if (Decrease && currentOrder == 0)
                {
                    Decrease = false;
                }

                if (Decrease)
                {
                    currentOrder--;
                }
                else
                {
                    currentOrder++;
                }

                // Prepare to disable text next
                IsTextDisabled = false;
                DisableTime = RDisableTime;

                if(CanPlaySounds)
                {

                phoneManagerSource.PlayOneShot(PhoneClip);

                } 

            }
            else
            {
                // Disable text
                PhoneScreenText.enabled = false;
                IsTextDisabled = true;
                DisableTime = RDisableTime; // Disable text for 1 second
            }
        }

        else
        {
            // Decrease DisableTime
            DisableTime -= Time.deltaTime;
        }



    }
}
