using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{

    [SerializeField]
    int currentFlicksDone ,MaxFlicks;
      

    Light MainLight;

    [SerializeField]
    bool FlickerDone;

    [SerializeField] [Range(0,10)] [Header("FlickProperties")]
    float minLightFlickPower, maxLightFlickPower;
    [SerializeField] [Range(0,30)]
    float minFlickTimer,MaxFlickTimer;

    public float FlickerTimer , RFlickerTimer;
    public float FlickerStartTimer;
    
    [SerializeField]
    int minFlickTimes, maxFlickTimes;
    
    // Start is called before the first frame update
    void Start()
    {

        MainLight = GetComponent<Light>();

        
    }

    // Update is called once per frame
    void Update()
    {

        CheckTimer();
        StartFlicker();
        
    }
    

    void CheckTimer()
    {
         
      if(FlickerStartTimer < 0 && FlickerDone)
      {

       FlickerStartTimer = Random.Range(minFlickTimer,MaxFlickTimer);

       currentFlicksDone = 0; 

       MaxFlicks = Random.Range(minFlickTimes,maxFlickTimes);   
 

      }

      else
      {

        FlickerStartTimer -= Time.deltaTime;

      }



    }


    void StartFlicker()
    {
       
          if(currentFlicksDone < MaxFlicks)
          {
 
             FlickerDone = false;

             if(FlickerTimer < 0)
             {
                float RandIntesity = Random.Range(minLightFlickPower,maxLightFlickPower);
               
               MainLight.intensity = RandIntesity;

               
               FlickerTimer = Random.Range(0.2f,RFlickerTimer);

               currentFlicksDone++;

             }

             else
             {

              FlickerTimer -= Time.deltaTime;

             }

            
          }

          else
          {
           
           FlickerDone = true;

          }


    }
}
