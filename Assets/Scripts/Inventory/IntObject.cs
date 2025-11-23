using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntObject : MonoBehaviour
{
    public GameObject DropThisOnDestroy;
    [SerializeField] public bool canBePickedUp;
    [SerializeField] public bool IsMissionDependet;
    [SerializeField] public int neededMissionOrder;
    [SerializeField] public bool IncreaseMission;
    [SerializeField] public bool SpawnItself;
    public bool isObjGrounded;
    public bool IsParented;
    public GameObject gameObjectSpawn;
    public bool isRayCastOn;
    public string RayText;
    public TMP_Text text;
    public float ClipRayLength;
    public bool IsDropable;
    [SerializeField] public bool isPlacable;
    public Ray ray;
    public RaycastHit hit;
    public Outline outline;
    public bool isNotInPositionY;
    public bool GoingDown;
    public float smoothness, YPenalty;

    public Vector3 rotation, downardPosition, mainpos, Scale;
    MeshRenderer renderer_;
    [SerializeField] SprintDetector sprint;
    public float DownPos; 

    [Header("Item Icon")]
    [SerializeField] public Vector3 imagePosition;
    [SerializeField] public Vector3 imageScale;
    [SerializeField] public Sprite InventoryIconSprite;
    [SerializeField] GameObject CurrentGameObject;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if(IsParented)
        {
          
          CurrentGameObject = gameObject.transform.parent.gameObject;

        }
        else
        {

          CurrentGameObject = this.gameObject;

        }

        outline = GetComponent<Outline>();
        renderer_ = GetComponent<MeshRenderer>();

        DownPos = mainpos.y - 2; 

        if (!isObjGrounded)
        {
           
                setPosition();
                
            
            sprint = GetComponent<SprintDetector>();
            SetYPenalty();
        }
        else
        {
            text = GameObject.FindGameObjectWithTag("IntText").GetComponent<TMP_Text>();
        }
        

    }

    // Update is called once per frame
    void Update()
    { 
        if (isObjGrounded)
        {
            if(IsMissionDependet)
            {
              
              CheckMissionPickup();

            }

            if(canBePickedUp)
            {

            CheckRay();

            }
            return;
        }

        if (Vector3.Distance(CurrentGameObject.transform.localPosition, mainpos) <= 0.1f)
        {
            isNotInPositionY = false;
        }

        if (Vector3.Distance(CurrentGameObject.transform.localPosition, new Vector3(mainpos.x,DownPos,mainpos.z)) <= 0.1f)
        {
            GoingDown = false;
        }
        
        
        LerpToMainPost(); 
    }

    public void LerpToMainPost()
    {
        if (isNotInPositionY && !GoingDown)
        {
            // Interpolate towards the main position
            CurrentGameObject.transform.localPosition = Vector3.Lerp(CurrentGameObject.transform.localPosition, mainpos, smoothness * Time.deltaTime);
        }
        else if(GoingDown)
        {

         CurrentGameObject.transform.localPosition = Vector3.Lerp(CurrentGameObject.transform.localPosition, new Vector3(mainpos.x,DownPos,mainpos.z), smoothness * Time.deltaTime);

        }
    }

    void CheckMissionPickup()
    {
        
        if(QuestManager.QuestInstance.currentMission == neededMissionOrder)
        {
            
            canBePickedUp = true;

        }
        else
        {
            
            canBePickedUp = false;

        }


    }

    void CheckRay()
    {
        if (text.text != RayText && isObjGrounded)
        {
            isRayCastOn = false;
            outline.enabled = false;
        }
        else if (text.text == RayText && isObjGrounded)
        {
            isRayCastOn = true;
            outline.enabled = true;
        }
    }

    public void setPosition()
    {
        if (isObjGrounded) return;

        if(IsParented)
        {
          
          CurrentGameObject = gameObject.transform.parent.gameObject;

        }
        else
        {

          CurrentGameObject = this.gameObject;

        }

        CurrentGameObject.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        CurrentGameObject.transform.localPosition = mainpos; 
        transform.localScale = Scale;

        if (outline != null)
        {
            RemoveNullAndOutlineMaterials();
        }
    }

    public void SetYPenalty()
    {
        if (isObjGrounded) return;
        

        CurrentGameObject.transform.localPosition -= new Vector3(0, YPenalty, 0);
        isNotInPositionY = true;
    }

    public void RemoveNullAndOutlineMaterials()
    {
        if (renderer_ != null)
        {
            List<Material> validMaterials = new List<Material>();

            foreach (var material in renderer_.materials)
            {
                if (material != null && !material.name.Contains("Outline") && !material.name.Contains("Default"))
                {
                    validMaterials.Add(material);
                }
            }

            renderer_.materials = validMaterials.ToArray();
        }
    }
}
