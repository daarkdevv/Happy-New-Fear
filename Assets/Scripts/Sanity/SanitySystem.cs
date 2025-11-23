using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SanitySystem : MonoBehaviour
{
    [Header("Sanity Settings")]
    [SerializeField] bool Activate;
    [SerializeField] bool IncreaseMode;
    [SerializeField] float CurrentSanity;
    [SerializeField] float maxSanity;

    [Header("Sanity Rates")]
    [SerializeField] float sanityIncreaseRate;
    [SerializeField] float sanityDecreaseRate;

    [Header("Timers")]
    [SerializeField] float sanityTimer;
    [SerializeField] float resetTimer;
    [SerializeField] float MaxTimer;

    [Header("Audio")]
    [SerializeField] AudioSource sourcePlayer;
    [SerializeField] AudioClip[] MildSounds;
    [SerializeField] AudioClip[] ModerateSounds;
    [SerializeField] AudioClip[] SevereSounds;
    [SerializeField] ArrayList arrayList;

    [Header("SanityStages")]
    [SerializeField] int Stage0;
    [SerializeField] int Stage1;
    [SerializeField] int Stage2;
    [SerializeField] int Stage3;
    [SerializeField] int currentStage;

    [Header("UI ELEMENTS")]
    [SerializeField] Image bar;
    [SerializeField] Color[] Gradient;
    [SerializeField] float colorLerpSpeed;

    [Header("PhoneSongs")]
    [SerializeField] PhoneManager phoneManager;


    // Start is called before the first frame update
    void Start()
    {
        
        sanityTimer = resetTimer;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Activate)
        return;

        sanityTimer -= Time.deltaTime;

        bar.fillAmount = CurrentSanity / maxSanity;

        bar.color = Color.Lerp(bar.color,Gradient[currentStage],colorLerpSpeed);

      
            if (IncreaseMode)
            {
                SanityIncreaseSys();
            }
            else
            {
                SanityDecreaseSys();
            }


            if(phoneManager.isPlayingSong)
            {

                if(!IncreaseMode)
                IncreaseMode = true;
         

            }
            else
            {

             IncreaseMode = false;

            }

       TriggerSoundSanity();
   
    }

    void SanityIncreaseSys()
    {
        if (CurrentSanity < maxSanity)
        {
            CurrentSanity += sanityIncreaseRate * Time.deltaTime;
            // Ensure sanity doesn't exceed max
            if (CurrentSanity > maxSanity)
            {
                CurrentSanity = maxSanity;
            }
        }
    }

    void SanityDecreaseSys()
    {
        if (CurrentSanity > 0)
        {
            CurrentSanity -= sanityDecreaseRate * Time.deltaTime;
            // Ensure sanity doesn't go below 0
            if (CurrentSanity < 0)
            {
                CurrentSanity = 0;
            }
        }
    }


    void TriggerSoundSanity()
    {
        
        if(CurrentSanity >= Stage0)
        {

          currentStage = 0;
          return;

        }


        if(CurrentSanity >= Stage1 && CurrentSanity <= Stage0)
        {

            
 
           currentStage = 1;


        }

        else if(CurrentSanity >= Stage2 && CurrentSanity <= Stage1)
        {


          
           currentStage = 2;

        }
        
        else if(CurrentSanity >= Stage3 && CurrentSanity <= Stage2)
        {


           
           currentStage = 3;

        }
         
        if(sanityTimer < 0)
        {

          PlaySounds(currentStage);

        }

    }

    void PlaySounds(int soundLevel)
    {

      if(soundLevel == 0)
      return;  
      
      if(sanityTimer > 0)
      return;
      
      if(soundLevel == 1)
      sourcePlayer.PlayOneShot(SevereSounds[Random.Range(0,SevereSounds.Length)]);
      else if(soundLevel == 2)
      sourcePlayer.PlayOneShot(ModerateSounds[Random.Range(0,ModerateSounds.Length)]);
      else if(soundLevel == 3)
      sourcePlayer.PlayOneShot(MildSounds[Random.Range(0,MildSounds.Length)]);


      sanityTimer = Random.Range(resetTimer,MaxTimer);

    }

}
