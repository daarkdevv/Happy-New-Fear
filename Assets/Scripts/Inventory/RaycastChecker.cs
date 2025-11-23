using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastChecker : MonoBehaviour
{
    [SerializeField] public bool IsEnabled;
    public bool isRaycasted;
    public string DisplayText;
    public Outline objectOutline;
    [SerializeField] bool DontControlOutline;
    [SerializeField] MeshRenderer renderer_;
    
    // Start is called before the first frame update
    void Start()
    {
        renderer_ = GetComponent<MeshRenderer>();

        if(gameObject.GetComponent<Outline>() != null)
        objectOutline = GetComponent<Outline>();
        
        IsEnabled  = true;

    }

    // Update is called once per frame
    void Update()
    {


        if(isRaycasted && IsEnabled)
        {

           if(objectOutline != null && !DontControlOutline)
           objectOutline.enabled = true; 
            
        }
        
        else
        {

          if(objectOutline != null && !DontControlOutline)
          objectOutline.enabled = false;

        }
        

        
    }
}
