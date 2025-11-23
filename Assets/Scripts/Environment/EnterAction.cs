using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterAction : MonoBehaviour
{
    [SerializeField] bool DestroyOnCollision;
    [SerializeField] bool IncreaseMission;
    [SerializeField] bool haveColliderToEnable;
    [SerializeField] Collider BarrierToDisable;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag != "Player") return;

        if(IncreaseMission)
        {

        QuestManager.QuestInstance.currentMission++;

        }

        if(haveColliderToEnable)
        BarrierToDisable.enabled = true;
        
        if(DestroyOnCollision)
        Destroy(gameObject);
    }
}
