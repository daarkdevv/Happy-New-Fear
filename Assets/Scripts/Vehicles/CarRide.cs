using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRide : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float slowingDistance = 5f;
    [SerializeField] float stoppingDistance = 0.1f; // Distance at which the car stops moving
    [SerializeField] RaycastChecker raycastChecker;
    [SerializeField] LevelLoader levelLoader;
    [SerializeField] AudioClip carRideClip;
    [SerializeField] bool DestroyOnReach;

    public bool hasReached { get; private set; } = false;

    void Start()
    {
        raycastChecker = GetComponent<RaycastChecker>();
          target = GameObject.FindGameObjectWithTag("CarTarget").GetComponent<Transform>();
          levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        StartCoroutine(GoToNeededPos());
    }

    IEnumerator GoToNeededPos()
    {
        while (!hasReached)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > stoppingDistance)
            {
                // Calculate the speed based on the distance
                float speed = maxSpeed;

                if (distance < slowingDistance)
                {
                    speed = Mathf.Lerp(0, maxSpeed, distance / slowingDistance);
                }

                // Move the car towards the target
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

                raycastChecker.IsEnabled = false;
            }
            else
            {
                // If the car is within the stopping distance, set hasReached to true and stop moving
                hasReached = true;
                raycastChecker.IsEnabled = true;

                if(DestroyOnReach) Destroy(gameObject);
            }

            yield return null; // Wait until the next frame before continuing
        }
    }

    void Update()
    {
       
       OnRide();

    }

    void OnRide()
    {
         
         if(raycastChecker.isRaycasted && Input.GetKeyDown(KeyCode.E))
         {
        
          if(!hasReached) return;
           
          StartCoroutine(levelLoader.LoadLevel(1,true,2,"ForestLevel",true,carRideClip,1.8f));

         }


    }
}
