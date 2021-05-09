using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatearound : MonoBehaviour
{    
    void Update()
    {  
        transform.RotateAround(transform.position, 
                                Vector3.up, 20 * Time.deltaTime);
    }
}
