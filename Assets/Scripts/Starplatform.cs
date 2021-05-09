using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Starplatform : NetworkBehaviour
{
    public static Starplatform instance;

    [SerializeField]
    Animator Wallanim;

    public delegate void OnStart();
    public OnStart onStart;

    void Awake(){
        if(instance != null){
            Debug.LogError("More startplatfrom than one");
        }else{
            instance = this;
        }
        Starplatform.instance.onStart += Startcounttoopen;
    }

    
    void Startcounttoopen(){
        Invoke("dropWalls", 5.0f);
    }

    void dropWalls(){
        Wallanim.SetBool("Open", true);

        Invoke("Upwalls", 10.0f);
    }
    
    void Upwalls(){
        Wallanim.SetBool("Open", false);
    
    }
}
