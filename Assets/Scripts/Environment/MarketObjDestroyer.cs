using System;
using System.Collections;
using System.Collections.Generic;
using LlockhamIndustries.ExtensionMethods;
using UnityEngine;

public class MarketObjDestroyer : MonoBehaviour
{
    [SerializeField] RaycastChecker raycastChecker;
    [SerializeField] SupermarketMission supermarketMission;
    // Start is called before the first frame update
    void Start()
    {

        raycastChecker = GetComponent<RaycastChecker>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.E))
        {
        
           if(raycastChecker.isRaycasted)
           {

               for (int i = 0; i < supermarketMission.SupermarketObjects.Count; i++)
               {

                 if(supermarketMission.SupermarketObjects[i].name == gameObject.name && supermarketMission.SupermarketObjects[i])
                 {
                  
                  Destroy(gameObject);
                  supermarketMission.SupermarketObjects.RemoveAt(i);


                 }
                
               }

           }


        }
        
    }
}
