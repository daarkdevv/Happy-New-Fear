using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PhoneManager : MonoBehaviour
{
    [SerializeField] AudioClip[] PhoneSfx;
    [SerializeField] AudioClip BeepSound;
    [SerializeField] Color SelectedTextColor, HighlightedColor;
    [SerializeField] AudioSource source;
    [SerializeField] bool isBeingCalled;
    [SerializeField] int currentChoice, CurrentChoiceSong;
    [SerializeField] int MaxChoicesCalls, MaxCurrentChoices;
    [SerializeField] float[] ImageYAmmount;
    [SerializeField] RectTransform[] image;
    [SerializeField] TMP_Text[] Texts;
    [SerializeField] public bool PhoneInInventory;
    [SerializeField] GameObject[] PhoneUiChoose;
    [SerializeField] int currentCalls;
    [SerializeField] int LastCaller;
    [SerializeField] Inventory inventory;
    [SerializeField] int currentPhonePage;
    [SerializeField] int maxPhonePage;
    [SerializeField] int phoneMusicMax;
    [SerializeField] int currentCalling;
    [SerializeField] int MaxCalling;
    [SerializeField] public bool[] activeToCall;
    [SerializeField] public bool isCallingSomeone;
    [SerializeField] bool wasCallingSomeone;
    [SerializeField] public bool SignalAvailable;
    [SerializeField] float RCallingTimeout;
    [SerializeField] float callingTimeout;
    [SerializeField] int CurrentContainer;
    [SerializeField] public bool PhoneConversationActive;
    [SerializeField] Conversation conversation;
    [SerializeField] RectTransform[] pages;
    [SerializeField] TMP_Text[] PagesText;
    [SerializeField] string[] CallersNames;
    [SerializeField] RectTransform[] containerText;
    [SerializeField] List<AudioClip> Songs;
    [SerializeField] List<Conversation> JeffCalled;
    [SerializeField] public bool isPlayingSong;
    [SerializeField] TMP_Text StoreCurrentSelectedSongText,storecurrentTextCalling;
    [SerializeField] int lastSongInt = -1;
    [SerializeField] int LastCallInt = -1;
    
    [SerializeField] public PhoneTextDisplayer phoneTextDisplayer;
    [SerializeField] bool canMoveChoice;
    [SerializeField] AudioClip phoneFailing;
    [SerializeField] bool MissedCall;
    [SerializeField] LeanTweenType easeType;
    [SerializeField] bool[] IncreaseMission;
    [SerializeField] AudioClip hangout;
    [SerializeField] TMP_Text CallerTextAbove;
    [SerializeField] int lastOflastSongInt;

    void Start()
    {
        InitializePhonePages();
        TextListTextCheck();
        MaxChoicesCheck();
        canMoveChoiceTrue();
        CheckTextPages();

        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        
    }

    void Update()
    {
        CheckPhoneEnabled();
        PhoneCall();

                if(MissedCall)
        {
           if(!isCallingSomeone)
           {
            
            RequestPhoneCall();        
            MissedCall = false;

           }
      
        }

        if(!PhoneInInventory && isPlayingSong)
        {
           
          CheckCurrentSongStatus();
          isPlayingSong = false;
          source.Stop();
          CheckTextPages();
          lastOflastSongInt = lastSongInt;
          lastSongInt = -1;

        }

        
       if(!PhoneInInventory) return;

        ChoosingPage();
        PlayASong();
        MaxChoicesCheck();
        CallSomeOneFunc();



        //CheckCallingTimeout();
    }

    public void PlayHangoutSound()
    {

       source.PlayOneShot(hangout);

    }

    void InitializePhonePages()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].gameObject.SetActive(i == currentPhonePage);
        }
    }

    void CallSomeOneFunc()
    {
        if (currentPhonePage != 2 || isCallingSomeone) return;

        isPlayingSong = false;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CallingFunc();
            
        }
    }

    IEnumerator CallingFuncEnd()
    {
        
       if(!SignalAvailable) 
       PhoneDisplayerCheck("No Signal", ":(", false,true,phoneFailing); 
       else if(!activeToCall[lastSongInt < 0 ? currentCalling : LastCallInt])
       PhoneDisplayerCheck("Not Available", ":(", false,true,phoneFailing); 
        
       PhoneConversationActive = true;

       yield return new WaitForSeconds(5.85f);
       
       ResetCalling();

       inventory.CanChangeSlot = true; 

       yield return null;

    }

    IEnumerator CallBro()
    {
           if(currentCalling == 0)
           {

             PhoneConversationActive = true;

             inventory.CanChangeSlot = false;
            
             yield return new WaitForSeconds(8);


             if(activeToCall[LastCallInt] && isCallingSomeone)
             {

               
             source.Stop();
             PhoneDisplayerCheck("Jeff Is On Phone",null, false,false,null);  
             JeffCalled[0].PlayerCalling = true;
             JeffCalled[0].ConversationOn = true;
             isCallingSomeone = true;
             PhoneConversationActive = true;

             }

             else
             {
              
              source.Stop();
              StartCoroutine(CallingFuncEnd());

             }

             inventory.CanChangeSlot = false;   

           }
           else
           {

             inventory.CanChangeSlot = false;
               
             PhoneConversationActive = true;
            
             yield return new WaitForSeconds(8);
             

              source.Stop();
              StartCoroutine(CallingFuncEnd());
    

           }
 

       yield return null;

    }

    public void ResetCalling()
    {
       Debug.Log("ResetCalled");

        source.Stop();
        isCallingSomeone = false;
        PhoneConversationActive = false;
        callingTimeout = 0;
        storecurrentTextCalling.color = SelectedTextColor;

       if(currentCalling == LastCallInt)
       {
        
       Debug.Log(lastSongInt + "LastInt");
       Debug.Log(currentCalling + "CurrentCall"); 

       storecurrentTextCalling.color = SelectedTextColor;
        
 
       }
       else
       {
        

       storecurrentTextCalling.color = Color.white;

       }


        PlayHangoutSound();

        CheckTextPages();

    }

    void CallingFunc()
    {
        if(isBeingCalled) return;

        if(!isCallingSomeone)
        {
            CheckCurrentSongStatus();
            isPlayingSong = false;
            source.Stop();
            
         if(!SignalAvailable)
         {
        
           
           isCallingSomeone = true;
           
           wasCallingSomeone = true;
           LastCallInt = currentCalling;

           var TextCalling_ = containerText[1].GetChild(currentCalling).GetComponent<TMP_Text>();
           TextCalling_.color = HighlightedColor;
           storecurrentTextCalling = TextCalling_;
   

          StartCoroutine(CallingFuncEnd());

         } 
          
         else
         {

            
        source.clip = Songs[Songs.Count - 2];
        source.Play();
        PhoneDisplayerCheck("Calling.", "Calling...", false,false,null);
        isCallingSomeone = true;
        wasCallingSomeone = true;
        LastCallInt = currentCalling;
        var TextCalling = containerText[1].GetChild(currentCalling).GetComponent<TMP_Text>();
        TextCalling.color = HighlightedColor;
        storecurrentTextCalling = TextCalling;
        callingTimeout = RCallingTimeout;
        
        StartCoroutine(CallBro());


         } 


        }

        else
        {

          if(PhoneConversationActive) return;  

          if(LastCallInt == currentCalling)
          {
             
             ResetCalling();
    
          }



        }
        
    }

    void CheckCallingTimeout()
    {
        if (callingTimeout < 0 && isCallingSomeone)
        {

            
        }
        else
        {
            if(isCallingSomeone)
            callingTimeout -= Time.deltaTime;

        }
        
    }

    void ChoosingPage()
    {
        bool ExceededLimitNegative = currentPhonePage == 0;
        bool ExceededLimitPositive = currentPhonePage == maxPhonePage;

        if (Input.GetKeyDown(KeyCode.RightArrow) && !PhoneConversationActive && !ExceededLimitPositive && isActiveAndEnabled)
        {
            currentPhonePage++;
            source.PlayOneShot(BeepSound);
            UpdatePhonePage();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !PhoneConversationActive && !ExceededLimitNegative)
        {
            source.PlayOneShot(BeepSound);
            currentPhonePage--;
            UpdatePhonePage();
        }
    }

    void UpdatePhonePage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].gameObject.SetActive(i == currentPhonePage);
        }
        CheckTextPages();
        MaxChoicesCheck();
    }

    void PhoneCall()
    {
        CallerTextAbove.text = (isBeingCalled ? CallerTextAbove.text = CallersNames[currentCalls - 1] : CallerTextAbove.text = "No One Is Calling.");

        if (!isBeingCalled) return;

        if (Input.GetKeyDown(KeyCode.Return) && currentPhonePage == 1)
        {
            HandleIncomingCall();
        }
    }

    void HandleIncomingCall()
    {
        if (Texts[currentChoice].name == "Accept")
        {
            PhoneDisplayerCheck(CallersNames[currentCalls - 1] + " Is Calling You!", null, false,false,null);
            source.Stop();
            source.PlayOneShot(PhoneSfx[0]);
            conversation.ConversationOn = true;
            isBeingCalled = false;
            PhoneConversationActive = true;
            conversation.increaseMissionOrder = IncreaseMission[currentCalls - 1];
            inventory.CanChangeSlot = false;
        }

        if (Texts[currentChoice].name == "Decline")
        {
            source.Stop();
            source.PlayOneShot(PhoneSfx[0]);
            isBeingCalled = false;
            CheckTextPages();
        }
    }

    void CheckCurrentSongStatus() ///////////
    {
        if (isBeingCalled || isCallingSomeone || !PhoneInInventory)
        {
  

            if(StoreCurrentSelectedSongText != null && lastSongInt == CurrentChoiceSong)
            StoreCurrentSelectedSongText.color = SelectedTextColor;
            else if(StoreCurrentSelectedSongText != null)
            StoreCurrentSelectedSongText.color = Color.white;
         
        }
    }

    void PlayASong()
    {
        if(PhoneConversationActive) return;

        if (Input.GetKeyDown(KeyCode.Return) && currentPhonePage == 0 && !isBeingCalled && !isCallingSomeone)
        {
            //CheckCurrentSongStatus();
            

            if (lastSongInt == CurrentChoiceSong)
            {
              
                isPlayingSong = !isPlayingSong;  
                source.Stop();
                StoreCurrentSelectedSongText.color = SelectedTextColor;
                CheckTextPages();
                lastSongInt = -1;
                
            }
            else
            { 
               
                if(isCallingSomeone || isBeingCalled )
                return;      
               
                if(StoreCurrentSelectedSongText != null)
                StoreCurrentSelectedSongText.color = StoreCurrentSelectedSongText.color == HighlightedColor ? StoreCurrentSelectedSongText.color = Color.white : StoreCurrentSelectedSongText.color = StoreCurrentSelectedSongText.color;  

                var textComponent = containerText[0].GetChild(CurrentChoiceSong).GetComponent<TMP_Text>();
                StoreCurrentSelectedSongText = textComponent;   
                textComponent.color = HighlightedColor;

                

                source.clip = Songs[CurrentChoiceSong];

                lastSongInt = CurrentChoiceSong;
                source.Play();
                isPlayingSong = true;
                PhoneDisplayerCheck("You are playing a Song!", "Press Enter Again To Stop", false,false,null);
            }
        }
    }

    void PhoneDisplayerCheck(string text0, string text1, bool CantSkipConditions,bool CanPlayAudio ,AudioClip phoneClip)
    {
        if (isPlayingSong || isBeingCalled || isCallingSomeone)
        {
            
            if (CantSkipConditions) return;
        }

        if (phoneTextDisplayer != null)
        {
            if(phoneTextDisplayer.IsTextDisabled)
            phoneTextDisplayer.IsTextDisabled = false;

            phoneTextDisplayer.DisableTime = phoneTextDisplayer.RDisableTime;

            phoneTextDisplayer.currentOrder = 0;

            if(CanPlayAudio)
            {
             
             phoneTextDisplayer.PhoneClip = phoneClip;
             phoneTextDisplayer.CanPlaySounds = true;

            }
            else
            {
              
              phoneTextDisplayer.CanPlaySounds = false;

            }  

            phoneTextDisplayer.CurrentPhoneTexts[0] = text0;
            phoneTextDisplayer.CurrentPhoneTexts[1] = text1;



        }
    }

    public void TextListTextCheck()
    {
        CurrentContainer = currentPhonePage == 0 ? 0 : currentPhonePage == 2 ? 1 : CurrentContainer;

        for (int i = 0; i < containerText[CurrentContainer].childCount; i++)
        {
            var textComponent = containerText[CurrentContainer].GetChild(i).GetComponent<TMP_Text>();
            if (i == CurrentChoiceSong && pages[0].gameObject.activeInHierarchy || i == currentCalling && pages[2].gameObject.activeInHierarchy)
            {
               textComponent.color = textComponent.color != HighlightedColor ? SelectedTextColor : textComponent.color;
                
            }
            else
            {
                if(textComponent.color != HighlightedColor)
                textComponent.color = Color.white;

            }
            

               

        }

          if(!wasCallingSomeone) return;
         
         for (int i = 0; i < containerText[0].childCount; i++)
         {

            var textComponent = containerText[0].GetChild(i).GetComponent<TMP_Text>();
        
            if(i == CurrentChoiceSong)
            textComponent.color = SelectedTextColor;
            else
            textComponent.color = Color.white;

            

         }

         wasCallingSomeone = false;
    }

    void CheckPhoneEnabled()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            RequestPhoneCall();
        }

        if (PhoneInInventory)
        {
            PhoneUiChoose[1].SetActive(isBeingCalled);
            PhoneUiChoose[0].SetActive(true);
        }
        else
        {
            foreach (var item in PhoneUiChoose)
            {
                if (item.activeInHierarchy)
                    item.SetActive(false);
            }
        }
    }

    public void RequestPhoneCall()
    {
        if(isCallingSomeone)
        {
         
          MissedCall = true;
          return;

        }

        isBeingCalled = true;
        CheckCallingPhoneText(false);
        CheckCurrentSongStatus();
        isPlayingSong = false;
        LastCaller = currentCalls;
        currentCalls++;
        source.clip = Songs[Songs.Count - 1];
        source.Play();
        CheckTextColor();
    }

    public void CheckCallingPhoneText(bool PickedUp)
    {
    
           
   

        PhoneDisplayerCheck(CallersNames[PickedUp ? LastCaller : currentCalls] + " Is Calling You!", CallersNames[PickedUp ? LastCaller : currentCalls] + " Is Calling You!", false,false,null);
         
       
             

    }

    void MaxChoicesCheck()
    {
        bool ExceededLimitNegative = currentChoice == 0;
        bool ExceededLimitPositive = currentChoice == MaxCurrentChoices;

        if(!canMoveChoice) return;

        if (currentPhonePage == 1)
        {
            MaxCurrentChoices = MaxChoicesCalls;
            ExceededLimitNegative = currentChoice == 0;
            ExceededLimitPositive = currentChoice == MaxCurrentChoices;
        }
        else if (currentPhonePage == 0)
        {
            MaxCurrentChoices = phoneMusicMax;
            ExceededLimitNegative = CurrentChoiceSong == 0;
            ExceededLimitPositive = CurrentChoiceSong == MaxCurrentChoices;
        }
        else if (currentPhonePage == 2)
        {
            ExceededLimitNegative = currentCalling == 0;
            ExceededLimitPositive = currentCalling == MaxCalling;
        }

        if (!image[currentPhonePage].gameObject.activeInHierarchy) return;

        HandleArrowInput(ExceededLimitNegative, ExceededLimitPositive);
    }

    void HandleArrowInput(bool ExceededLimitNegative, bool ExceededLimitPositive)
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) && !ExceededLimitNegative)
        {
            source.PlayOneShot(BeepSound);
            AdjustCurrentChoice(-1);
            MovingArrow(true);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !ExceededLimitPositive)
        {
            source.PlayOneShot(BeepSound);
            AdjustCurrentChoice(1);
            MovingArrow(false);
        }
    }

    void AdjustCurrentChoice(int delta)
    {
        if (currentPhonePage == 1)
        {
            currentChoice += delta;
            CheckTextColor();
        }
        else if (currentPhonePage == 0)
        {
            CurrentChoiceSong += delta;
            TextListTextCheck();
        }
        else if (currentPhonePage == 2)
        {
            currentCalling += delta;
            TextListTextCheck();
        }

        CurrentContainer = currentPhonePage == 0 ? 0 : currentPhonePage == 2 ? 1 : CurrentContainer;
    }

    public void CheckTextPages()
    {
        
        if (currentPhonePage == 0)
        {
            PhoneDisplayerCheck("Songs", null, true,false,null);
            UpdatePagesText("SongsUi");

            if(!wasCallingSomeone) return;
            TextListTextCheck();
        }
        else if (currentPhonePage == 1)
        {
            PhoneDisplayerCheck("Calls", null, true,false,null);
            UpdatePagesText("IncomingCallsUi"); 
        }
        else if (currentPhonePage == 2)
        {
            PhoneDisplayerCheck("Call Someone", null, true,false,null);
            UpdatePagesText("CallSomeOne");
        }
    }

    void UpdatePagesText(string tag)
    {
        foreach (var text in PagesText)
        {
            text.color = text.tag == tag ? SelectedTextColor : Color.white;
        }
    }

    void MovingArrow(bool positive)
    {
        float offset = positive ? ImageYAmmount[currentPhonePage] : -ImageYAmmount[currentPhonePage];
        canMoveChoice = false;
        LeanTween.moveLocalY(image[currentPhonePage].gameObject, image[currentPhonePage].localPosition.y + offset,0.28f).setEase(easeType).setOnComplete(canMoveChoiceTrue);
         

    }

    void canMoveChoiceTrue()
    {

      canMoveChoice = true;

    }

    void CheckTextColor()
    {
        for (int i = 0; i < Texts.Length; i++)
        {
            Texts[i].color = i == currentChoice ? SelectedTextColor : Color.white;
        }
    }
}
