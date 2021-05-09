using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Rocketexplosion : NetworkBehaviour
{
    public GameObject explosion;
    
    private void OnCollisionEnter(Collision other) {
        GameObject hitexp = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(hitexp,2f);

        Collider [] colliders = Physics.OverlapSphere(transform.position, 20f);

        foreach(Collider nearbyObject in colliders)
        {     
            if (nearbyObject.tag == "Player"){

                GameObject enemy = nearbyObject.gameObject;
                Player player_s = Gamemanger.GetPlayer(enemy.name);

                if (enemy != null)
                {
                    float dist = Vector3.Distance(transform.position, nearbyObject.GetComponent<Transform>().position);
                    player_s.NormalTakeDamage(Mathf.RoundToInt(((20f-dist)/20)*160) , "Rocket");
                }
            }
        }
        Destroy(gameObject);
    }

}
