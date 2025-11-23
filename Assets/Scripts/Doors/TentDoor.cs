using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentDoor : MonoBehaviour
{
    [SerializeField] bool CanOpen;
    [SerializeField] bool IsOpen;
    [SerializeField] RaycastChecker raycastChecker;
    [SerializeField] BoxCollider meshCollider;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] BoxCollider CloseTentTriggerer;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] clip;
    
    // Start is called before the first frame update
    void Start()
    {
        
        source = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        raycastChecker = GetComponent<RaycastChecker>();
        meshCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        IsOpen = false;
        
    }



    // Update is called once per frame
    void Update()
    {
       
       if(!CanOpen)
       {
           
        raycastChecker.IsEnabled = false; 

       }
       else
       {

        raycastChecker.IsEnabled = true; 

       }

       if(Input.GetKeyDown(KeyCode.E) && raycastChecker.isRaycasted)
       {
          
          if(IsOpen)
          CloseTent();
          else
          OpenTent();
 

       }
            
    }


   public void CloseTent()
    {
       
       IsOpen = false;
       gameObject.layer = 16;
       meshRenderer.enabled = false;
       raycastChecker.DisplayText = "Open Tent";
       source.PlayOneShot(clip[1]);

    }

    public void OpenTent()
    {
     
       IsOpen = true;
       gameObject.layer = 10;
       meshRenderer.enabled = true;
       raycastChecker.DisplayText = "Close Tent";
       source.PlayOneShot(clip[0]);
      

    }
}
