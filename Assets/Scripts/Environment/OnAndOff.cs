using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OnAndOff : MonoBehaviour
{
    public bool isOn;
    public float Timer,ResetTimer;
    private UnityEngine.UI.Image Image1;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Start() {

        Image1 = GetComponent<UnityEngine.UI.Image>();
    }
    void Update()
    {

        if(Timer <= 0)
        {
           
           Timer = ResetTimer;
           CheckON(); 
           

        }

     
         Timer -= Time.deltaTime;

        
        
    }

    void CheckON()
    {

        if(isOn)
        {

          Image1.enabled = false;
          isOn = false;

        }
        
  
        else
        {
             
            Image1.enabled = true;
            isOn = true; 

        }
       
  

    }
    
}
