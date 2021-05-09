using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class itemspwan : MonoBehaviour
{
    public GameObject Healthpack;
    public GameObject Pistolammo;
    public GameObject Riffleammo;
    public GameObject Rockets;
    public GameObject Pistol;
    public GameObject Rifle;
    public GameObject RocketL;

    public GameObject spawned;


    public void Spawn(){
        if(spawned != null){
            return;
        }
        int num = Random.Range(0, 7); 

        switch (num)
                {
                case 0:
                    spawned = Instantiate(Healthpack, transform);
                    //spawned.transform.parent = gameObject.transform;
                    break;
                case 1:
                    spawned = Instantiate(Pistolammo, transform);
                    //spawned.transform.parent = gameObject.transform;
                    break;
                case 2:
                    spawned = Instantiate(Riffleammo, transform);
                    //spawned.transform.parent = gameObject.transform;
                    break;
                case 3:
                    spawned = Instantiate(Rockets, transform);
                    //spawned.transform.parent = gameObject.transform;
                    break;
                case 4:
                    spawned = Instantiate(Pistol, transform);
                    //spawned.transform.parent = gameObject.transform;                     
                    break;
                case 5:
                    spawned = Instantiate(Rifle, transform);
                    //spawned.transform.parent = gameObject.transform;                   
                    break;
                case 6:
                    spawned = Instantiate(RocketL, transform);
                    //spawned.transform.parent = gameObject.transform;         
                    break;
                }

        NetworkServer.Spawn(spawned);
    }
}