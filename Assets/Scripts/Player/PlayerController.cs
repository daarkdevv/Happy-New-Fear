using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform PlayerBody;

    [Range(0,100)]
    public float mouseSens;
    public float xRotation;
    public bool isRotaionNotGood;
    
    // Start is called before the first frame update
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        
        
    }

    // Update is called once per frame
    void Update()
    {

        isRotaionNotGood = (xRotation !> 85 || xRotation !< -85);

        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-85,85);

        transform.localRotation = Quaternion.Euler(xRotation,0f,0f);
        PlayerBody.Rotate(Vector3.up * mouseX);
    }
}
