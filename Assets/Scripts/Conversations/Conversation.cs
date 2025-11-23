using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Conversation : MonoBehaviour
{
    
    [Header("Properties")]
    [SerializeField] public AudioSource source;
    [SerializeField] public bool ConversationOn;
    [SerializeField] public bool increaseNpcConv;
    [SerializeField] public bool Resetable;
    [SerializeField] string[] DialogueToText;
    [SerializeField] AudioClip[] DialogueAudio;
    [SerializeField] float silenceMoment,RSilenceMoment;
    [SerializeField] bool hasTriggerdSilence,Continue;
    [SerializeField] public int CurrentAudio;
    [SerializeField] TMP_Text DialogueText;
    [SerializeField] PhoneManager phoneManager;
    [SerializeField] Inventory inventory;
    [SerializeField] bool isNonPhoneConv; //non phone conversation, its a normal talk
    [SerializeField] public bool PlayerCalling;
    [SerializeField] public bool hasFinishedConv;
    [SerializeField] public GameObject MissionText;
    [SerializeField] public bool increaseMissionOrder;
    [SerializeField] public bool IncreaseNpcPosition;
    [SerializeField] public bool DestroyOnFinish;
    [SerializeField] public AudioSource[] Talkers;
    [SerializeField] bool MoreThanTalker;
    [SerializeField] bool AudioSourceFromNew;
    [SerializeField] string AudioContainerName;
    [SerializeField] AudioSourceContainer audioContainer;

    [Header("Character States")]
    
    [SerializeField] bool isMaxwellTalking, isJeffTalking, isDaveTalking;
    
    // Start is called before the first frame update
    void Start()
    {
      

      CurrentAudio = -1;
      MissionText = GameObject.FindGameObjectWithTag("MissionText");

      DialogueText = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<TMP_Text>();

      if(MoreThanTalker)
      source = Talkers[0];

      inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
      phoneManager =  GameObject.FindGameObjectWithTag("PhoneManager").GetComponent<PhoneManager>();

      if(AudioSourceFromNew && MoreThanTalker)
      {

        audioContainer = GameObject.FindGameObjectWithTag(AudioContainerName).GetComponent<AudioSourceContainer>();
        
         for (int i = 0; i < audioContainer.sources.Length; i++)
         {

          
           
          Talkers[i] = (audioContainer.sources[i] == null ? Talkers[i] = source : Talkers[i] = audioContainer.sources[i]);
        
         } 

      }


      
        
    }

    // Update is called once per frame
    void Update()
    {

        if(ConversationOn && !isNonPhoneConv)
        {
       
         phoneManager.PhoneConversationActive = true;

        }
 
        
       if(ConversationOn) 
       MissionText.SetActive(DialogueText.text == null ? true : false);

        if(CurrentAudio == DialogueToText.Length)
        {

          
            
            ConversationOn = false;

            phoneManager.PhoneConversationActive = false;

            hasFinishedConv = true;
          
            if(increaseMissionOrder)
            {
               
               QuestManager.QuestInstance.currentMission++;
               increaseMissionOrder = false;

            } 
              
              
            
            if(DestroyOnFinish)
            {

             Destroy(GetComponent<Conversation>());

            }

            Debug.Log("Not Destroyed yet");

            if(isNonPhoneConv) return;

            hasFinishedConv = true;

            phoneManager.PhoneConversationActive = false;
           
            phoneManager.PlayHangoutSound();
             
            if(PlayerCalling)
            {

            PlayerCalling = false;
            phoneManager.isCallingSomeone = false;  
            phoneManager.ResetCalling();

            }
          
            inventory.CanChangeSlot = true;
 
            phoneManager.CheckTextPages();

            QuestManager.QuestInstance.QuestSystemOn = true; 

            CurrentAudio = -1;
            silenceMoment = 0;
            
            if(DestroyOnFinish)
            Destroy(GetComponent<Conversation>());

            Debug.Log("Not Destroyed yet");


        }

        


        a7abgd();
         
        if(!source.isPlaying && hasTriggerdSilence && ConversationOn && Continue)
        {
          
          if(CurrentAudio != DialogueToText.Length)
          {
          
          CurrentAudio++;

          if(MoreThanTalker && CurrentAudio != Talkers.Length)
          source = Talkers[CurrentAudio];

          }
          
          if(CurrentAudio != DialogueToText.Length)
          CheckSpeaker(DialogueToText[CurrentAudio]);
          
          if(CurrentAudio != DialogueToText.Length)
          DialogueText.text = DialogueToText[CurrentAudio];
          
          if(CurrentAudio != DialogueToText.Length)
          source.PlayOneShot(DialogueAudio[CurrentAudio]);
          

        } 
        
     
    }

    void a7abgd()
    {
      
      if(source.isPlaying == false && ConversationOn)
      {

        DialogueText.text = null;
        
      if(silenceMoment < 0)
      {

        Continue = true;
        hasTriggerdSilence = true;
        silenceMoment = RSilenceMoment;

      }

      else
      {

        
      
        hasTriggerdSilence = false; 

        Continue = false;

        silenceMoment -= Time.deltaTime;


      }
        

      }
      

  


    }




    void CheckSpeaker(string dialogue)
    {
        isMaxwellTalking = false;
        isJeffTalking = false;
        isDaveTalking = false;

        Regex maxwellRegex = new Regex(@"^\s*Maxwell\s*:\s*");
        Regex jeffRegex = new Regex(@"^\s*Jeff\s*:\s*");
        Regex daveRegex = new Regex(@"^\s*Dave\s*:\s*");

        if (maxwellRegex.IsMatch(dialogue))
        {
            isMaxwellTalking = true;
        }
        else if (jeffRegex.IsMatch(dialogue))
        {
            isJeffTalking = true;
        }
        else if (daveRegex.IsMatch(dialogue))
        {
            isDaveTalking = true;
        }
    }



    

    
}
