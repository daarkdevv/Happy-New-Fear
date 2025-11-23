using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class callingChecker : MonoBehaviour
{
    [SerializeField] PhoneManager phoneManager;
    [SerializeField] int[] missionToEnableCall;
    [SerializeField] int OrderValue;
    [SerializeField] bool canCall;
    // Start is called before the first frame update
    void Start()
    {

        OrderValue = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

       if(QuestManager.QuestInstance.currentMission == missionToEnableCall[OrderValue])
       {
            
          phoneManager.SignalAvailable = true;
          phoneManager.activeToCall[0] = true;
          canCall = true;

       }
       else if(QuestManager.QuestInstance.currentMission > missionToEnableCall[OrderValue])
       {

          if(OrderValue + 1 <= missionToEnableCall[OrderValue]) OrderValue++;

       }
       else
       {
         phoneManager.SignalAvailable = (OrderValue == missionToEnableCall.Length ? phoneManager.SignalAvailable = false : phoneManager.SignalAvailable = true);
         phoneManager.activeToCall[0] = false;
         canCall = false;

       }
        
    }
}
