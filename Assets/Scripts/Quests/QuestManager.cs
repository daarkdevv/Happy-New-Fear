using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager QuestInstance { get; private set; }
    [SerializeField] public bool QuestSystemOn;
    [SerializeField] DoorManager fridgeDoor;
    [SerializeField] string[] Missions;
    [SerializeField] public int currentMission;
    [SerializeField] TMP_Text ObjTxt;

    [Header("Message Manager")]
    [SerializeField] TMP_Text messageText;
    [SerializeField] float messageDisplayDuration;
    [SerializeField] float messageAnimationSpeed;
    [SerializeField] bool messageDisplayed;
    [SerializeField] string[] messages;
    [SerializeField] RectTransform downPos;
    [SerializeField] RectTransform upWardPos;
    [SerializeField] bool IsUp;
    
    // Start is called before the first frame update
    void Start()
    {
     
    if (QuestInstance != null && QuestInstance != this) 
    { 
        Destroy(this); 
    } 
    else 
    { 
        QuestInstance = this; 
    } 
      
        
    }

    // Update is called once per frame
    void Update()
    {
      CheckMission(); 
      checkQuestSys();
      DisplayMessageUI();

    }

    void DisplayMessageUI()
    {
      
          if(IsUp)
          {

            messageText.rectTransform.localPosition = Vector3.Lerp(messageText.rectTransform.localPosition,upWardPos.localPosition,messageAnimationSpeed);
                                                                       
          }
          else 
          {
                                                                       
            messageText.rectTransform.localPosition = Vector3.Lerp(messageText.rectTransform.localPosition,downPos.localPosition,messageAnimationSpeed);

          }

    }

    void checkQuestSys()
    {

      if(currentMission <= Missions.Length)
      ObjTxt.text = Missions[currentMission];

    }

    void CheckMission()
    {
        



    }

    void MissionComplete()
    {
      
      currentMission++;
      

    }

    void newMission()
    {



        
    }

    public IEnumerator DisplayMessage(int messageID , bool makeMessageGoDown,bool messageStatic)
    {
      
      messageText.text = messages[messageID];
      
      if(makeMessageGoDown)
      {
        Debug.Log("Time To Go!");
        IsUp = false;
        yield break;

      }
      else if(messageStatic)
      {
        
        Debug.Log("Your Messege Is on screen forever");
        IsUp = true;
        yield break;

      }

      IsUp = true;
 
      yield return new WaitForSeconds(messageDisplayDuration);
      
      IsUp = false;

    }

}
