using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPizza : MonoBehaviour
{
    [SerializeField] GameObject[] PizzaSlicesEat;
    [SerializeField] int CurrentSlice;
    [SerializeField] AudioSource PlayerAudio;
    [SerializeField] AudioClip eatSound;
    [SerializeField] QuestManager questManager;
    [SerializeField] RaycastChecker raycastChecker;
    [SerializeField] float EatDelay;
    [SerializeField] float EatDelayValue;
    [SerializeField] TvManager TvManager;
    [SerializeField] PhoneManager phoneManager;
    [SerializeField] TvTurner tvTurner;
    [SerializeField] int TriggerEventSlice;
    [SerializeField] PlayerMove playerMove;
    [SerializeField] Couch couch;
    [SerializeField] bool easAlarmPlayed;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAudio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        questManager = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestManager>();
        tvTurner = GameObject.FindGameObjectWithTag("TV").GetComponent<TvTurner>();
        TvManager = GameObject.FindGameObjectWithTag("TvManager").GetComponent<TvManager>();
        phoneManager = GameObject.FindGameObjectWithTag("PhoneManager").GetComponent<PhoneManager>();
        couch =  GameObject.FindGameObjectWithTag("Couch").GetComponent<Couch>();
        raycastChecker = GetComponent<RaycastChecker>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(raycastChecker.isRaycasted && Input.GetKeyDown(KeyCode.E))
        {

            if(!couch.isSitting) return;
            
            if(EatDelay < 0)
            {

                Eat();
                EatDelay = EatDelayValue;

            }

             raycastChecker.IsEnabled = CurrentSlice < PizzaSlicesEat.Length;
             Destroy(CurrentSlice >= PizzaSlicesEat.Length ? raycastChecker.objectOutline : null);
            
            couch.CanGetUp = easAlarmPlayed;

            StartCoroutine(QuestManager.QuestInstance.DisplayMessage(0,true,false));
 
         
        }

        EatDelay -= Time.deltaTime;
        
    }

    void Eat()
    {
       if(CurrentSlice != PizzaSlicesEat.Length)
       PizzaSlicesEat[CurrentSlice].SetActive(false);


       if(CurrentSlice == TriggerEventSlice)
       StartCoroutine(TriggerEvent());

       PlayerAudio.PlayOneShot(eatSound);

       CurrentSlice++;

    }

    IEnumerator TriggerEvent()
    {
      Debug.Log("EventTriggerd");
      yield return new WaitForSeconds(Random.Range(2,4));
      CheckEasAlarmEvent();
      yield return new WaitForSeconds(Random.Range(66,70));
      StartCoroutine(QuestManager.QuestInstance.DisplayMessage(0,false,true));
      phoneManager.RequestPhoneCall();
      QuestManager.QuestInstance.currentMission++;
      tvTurner.canControl = true;
      easAlarmPlayed = true;
      couch.CanGetUp = true;

    }

   

    void CheckEasAlarmEvent()
    {

        if(tvTurner.isTvOff)
        { 
         
         tvTurner.TurnOnTv();
         tvTurner.canControl = false;

        }


        TvManager.playEasAlarmVideo();
        

    }
}
