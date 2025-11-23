using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VLB;

public class FlashLightManager : MonoBehaviour
{
    public bool IsFalse;
    private AudioSource source;
    public AudioClip[] FlashLightsSfx;
    [SerializeField] public bool CanOpenFlashlight;
    [SerializeField] IntObject intObject;
    [SerializeField] public int DisableCounter;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        intObject = GetComponent<IntObject>();
        CanOpenFlashlight = true;
    }

    // Update is called once per frame
    void Update()
    {

      if(!CanOpenFlashlight || intObject.isObjGrounded) return;  

      if(Input.GetKeyDown(KeyCode.F) && IsFalse)
      OpenFlashLight();

      else if(Input.GetKeyDown(KeyCode.F) && !IsFalse)
      CloseFlashLight();
        
    }
    private void OnDisable() {
        if(DisableCounter > 0)
        CloseFlashLight();
    }

    public void OpenFlashLight()
    {
          
        IsFalse = false;
        transform.GetChild(0).gameObject.SetActive(true);
        source.PlayOneShot(FlashLightsSfx[0]);
        
    }

    public void CloseFlashLight()
    {

        IsFalse = true;

        if(transform.GetChild(0).gameObject != null)
        transform.GetChild(0).gameObject.SetActive(false);

        source.PlayOneShot(FlashLightsSfx[1]);


    }
}
