using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SanityEffect1 : MonoBehaviour
{
    [SerializeField] bool DestroyAnObject;
    [SerializeField] UnityEngine.UI.Image FlashingImage;
    [SerializeField] GameObject ObjectToDestroy;
    [SerializeField] float decreaseValue;
    [SerializeField] AudioClip sanitySounds1;
    [SerializeField] AudioSource source;
    [SerializeField] Color FlashingColor;
    [SerializeField] bool hasTriggerd;
    [SerializeField] bool WillSaySomething;
    [SerializeField] Conversation conversation;
    bool hastalked;
     
    // Start is called before the first frame update
    void Start()
    {

        FlashingColor = FlashingImage.color;
        conversation = GetComponent<Conversation>();
        conversation.source = source;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(hasTriggerd)
        {

           FlashingImage.color = FlashingColor; 
           
           FlashingColor.a = Mathf.Lerp(FlashingColor.a,0,decreaseValue * Time.deltaTime);

           

        }
        
    }

    public void TriggerFlash()
    {

      if(!hasTriggerd)
      {

       FlashingColor.a = 1;

       if(DestroyAnObject == true)
       Destroy(ObjectToDestroy);

       source.PlayOneShot(sanitySounds1);
       hasTriggerd = true;

      if(WillSaySomething) 
      StartCoroutine(StartTalk());

      }  


    }

    IEnumerator StartTalk()
    {

     if(hastalked) yield break;   
     
     yield return new WaitForSeconds(4);

     hastalked = true;

     conversation.ConversationOn = true; 
     

    }
}
