using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FurnaceManager : MonoBehaviour
{
    [SerializeField] string neededObjectTag;
    [SerializeField] bool isInside;
    [SerializeField] float sphereRadius;
    [SerializeField] Vector3 spherePosition;
    [SerializeField] LayerMask furnaceMask;
    [SerializeField] DoorManager doorManager;
    [SerializeField] bool hasFinishedCooking;
    [SerializeField] float CookTime;
    [SerializeField] bool HasClosed;
    [SerializeField] ObjectManager objectManager;
    [SerializeField] GameObject cookedObject;
    [SerializeField] QuestManager questManager;

    int times;
    AudioSource source;
    
    void Start()
    {

        source = GetComponent<AudioSource>();
        questManager = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestManager>();
        
        
        
    }

    void Update()
    {
        
        objectManager.Active = (doorManager.transform.rotation.x >= 75);

        // Check if any colliders are within the sphere
        isInside = Physics.CheckSphere(transform.position + spherePosition, sphereRadius, furnaceMask);
        
        if(isInside && !doorManager.IsOpen && !hasFinishedCooking && CookTime > 0)
        {
            doorManager.Interacte(true);
            source.Play();
            StartCoroutine(LockDoor(true,0.666f));
        }

        if(CookTime <= 0 && !hasFinishedCooking)
        {
 
            hasFinishedCooking = true;
            source.Stop();
            doorManager.Locked = false;
            

        }

        if(doorManager.IsOpen && hasFinishedCooking && !HasClosed)
        {
           
           
           doorManager.Interacte(true);

           if(times <= 0)
           spawnCookedObject();

           times++;

        }

        if(!doorManager.IsOpen && hasFinishedCooking)
        HasClosed = true;

        if(isInside)
        {
          
          CookTime -= Time.deltaTime;

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + spherePosition, sphereRadius);
    }

    void spawnCookedObject()
    {

      Destroy(objectManager.spawned);    
      
      GameObject cook1 = Instantiate(cookedObject,transform.position,Quaternion.identity);

      cook1.transform.rotation = Quaternion.Euler(objectManager.Rotaion.x,objectManager.Rotaion.y,objectManager.Rotaion.z);

      cook1.transform.localScale = objectManager.Scale;

      cook1.transform.position = objectManager.PlacePosition;

      cook1.name = "Cooked Pizza";
      
      questManager.currentMission++;

    }


    IEnumerator LockDoor(bool choose,float Time)
    {
        yield return new WaitForSeconds(Time);
        doorManager.Locked = choose;
    }
}
