using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TvTurner : MonoBehaviour
{
    [SerializeField] public bool isTvOff;
    [SerializeField] public bool canControl;
    [SerializeField] AudioSource Tvsource;
    [SerializeField] AudioSource sourceTv;
    [SerializeField] Canvas canvas;
    [SerializeField] RaycastChecker raycastChecker;
    [SerializeField] AudioClip[] TvTurningStatus;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
       
       raycastChecker = GetComponent<RaycastChecker>();
       sourceTv = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!canControl) return;

        if(raycastChecker.isRaycasted && Input.GetKeyDown(KeyCode.E) && timer <= 0)
        {

           if(!isTvOff)
           {

             turnOffTv();


           }

           else
           {
             
            TurnOnTv();
      
           }  
         
          timer = 0.5f;

        }

        timer -= Time.deltaTime;
        
    }

    public void TurnOnTv()
    {
       
        Tvsource.enabled = true; 
            
        sourceTv.PlayOneShot(TvTurningStatus[1]);

        Tvsource.enabled = false;
        canvas.enabled = false;
        isTvOff = true;


    }

    public void turnOffTv()
    {
      
        Tvsource.enabled = true;
        sourceTv.PlayOneShot(TvTurningStatus[0]);
        canvas.enabled = true;
        isTvOff = false;

    }
}
