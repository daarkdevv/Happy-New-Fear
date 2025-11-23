using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    [SerializeField] float carSpeed;
    // Start is called before the first frame update
    void Start()
    {
        carSpeed = -0.07f;
        Destroy(gameObject,20f);
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += new Vector3(transform.position.x * carSpeed * Time.deltaTime,0,0);
        
    }
}
