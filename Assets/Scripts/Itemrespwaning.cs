using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Itemrespwaning : NetworkBehaviour
{
    
    void Start()
    {   
       Starplatform.instance.onStart += Startspawning;
    
    }

    public void Startspawning(){
        if(!isServer){
            return;
        }
        foreach(Transform child in transform)
        {
            child.gameObject.GetComponent<itemspwan>().Spawn();
        }
    }
    
}
