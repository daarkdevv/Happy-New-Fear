using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couch : MonoBehaviour
{
    [SerializeField] public bool isSitting;

    [SerializeField] public bool CanGetUp;
    [SerializeField] Transform PlayerPos;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] PlayerMove playerMove;
    [SerializeField] Transform SitPos, StandPos;
    private CharacterController characterController;

    RaycastChecker raycastChecker;

    // Start is called before the first frame update
    void Start()
    {
        isSitting = false;

        raycastChecker = GetComponent<RaycastChecker>();
  
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();

        PlayerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        characterController = PlayerPos.GetComponent<CharacterController>();

        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
         
         if(playerMove.isCrouch) return;



        if (Input.GetKeyDown(KeyCode.LeftShift) && isSitting && CanGetUp)
        {
            isSitting = false;

            playerMove.canMove = true;

            boxCollider.enabled = true;

            // Disable CharacterController before setting the position
            characterController.enabled = false;
            PlayerPos.position = StandPos.position;
            characterController.enabled = true;

        playerMove.isSitting_ = isSitting;
        Debug.Log("PLayer Is Sitting");
            
           StartCoroutine(QuestManager.QuestInstance.DisplayMessage(0,true,false)); 

            
        }

        if(!raycastChecker.isRaycasted)
        return;

        

        if (Input.GetKeyDown(KeyCode.E) && !isSitting)
        {
            isSitting = true;

            boxCollider.enabled = false;

            playerMove.canMove = false;

            playerMove.movement = Vector3.zero;

            // Disable CharacterController before setting the position
            characterController.enabled = false;
            PlayerPos.position = SitPos.position;
            characterController.enabled = true;

        playerMove.isSitting_ = isSitting;


            Debug.Log("Yea"); 

           StartCoroutine(QuestManager.QuestInstance.DisplayMessage(0,false,true)); 
             
        }

    }
}
