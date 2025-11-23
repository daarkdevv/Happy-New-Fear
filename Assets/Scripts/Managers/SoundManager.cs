using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] public AudioClip[] AmbienceSounds;
    [SerializeField] public AudioSource sourceManager;
    // Start is called before the first frame update
    void Start()
    {

        sourceManager.PlayOneShot(AmbienceSounds[0]);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
