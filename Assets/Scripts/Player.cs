using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private bool isdead = false;
    public bool isdead_pub
    {
        get { return isdead; }
        protected set { isdead_pub = value; }
    }

    private GameObject Startplatform;
    public PlayerSetup PlayerS;

    [SerializeField]
    private int maxHealth = 100;

    public float GetHealth(){
        return (float)currenthealth/maxHealth;
    }

    public GameObject plasmaexplosion;

    [SyncVar]
    private int currenthealth;

    [SyncVar]
    public string username;

    [SerializeField]
    private Behaviour[] disableonDeath;
    private bool[] wasEnable;

    [SerializeField]
    private GameObject[] disableonDeathgameobjects;


    public void Setup() {
        wasEnable = new bool[disableonDeath.Length];
        for (int i =0; i < wasEnable.Length; i++){
            wasEnable[i] = disableonDeath[i].enabled;
        }

        SetDefaults();    
    }


    [ClientRpc]
    public void RpcTakeDamage(int amount, string _sourceID){
        if (isdead)
            return;
        currenthealth -= amount;

        //Debug.Log(transform.name + " health "+ currenthealth);
        if (currenthealth <= 0)
        {
            Die(_sourceID);
        }
    }

    public void NormalTakeDamage(int amount, string _sourceID){
        if (isdead)
            return;
        currenthealth -= amount;

        //Debug.Log(transform.name + " health "+ currenthealth);
        if (currenthealth <= 0)
        {
            Die(_sourceID);
        }
    }

    private void Die(string sourceID){
        isdead = true;

        
        for (int i =0; i < disableonDeath.Length; i++){
            disableonDeath[i].enabled = false;
        }
    
        for (int i =0; i < disableonDeathgameobjects.Length; i++){
            disableonDeathgameobjects[i].SetActive(false);
        }

        Collider col = GetComponent<Collider>();
        if (col != null){
            col.enabled = false;
        }


        Gamemanger.instance.onPlayerKilledCallback.Invoke(username ,sourceID);
        GameObject deatheffect = Instantiate(plasmaexplosion, transform.position, Quaternion.identity);
        Destroy(deatheffect,2f);

        if(isLocalPlayer){
            PlayerS.sceneCamera.gameObject.SetActive(true);
        }
        
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn(){

        yield return new WaitForSeconds(5f);

        
        Transform startPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
        
        SetDefaults();
    
    }

    
    public bool Healthpack(){
        if(currenthealth >= 100){
            return false;
        }
        currenthealth += 50;
        if(currenthealth > 100){
            currenthealth = maxHealth;
        }
        return true;
    }

    public void SetDefaults(){
        isdead = false;

        currenthealth = maxHealth;

        for (int i =0; i < disableonDeath.Length; i++){
            disableonDeath[i].enabled = wasEnable[i];
        }

        for (int i =0; i < disableonDeathgameobjects.Length; i++){
            disableonDeathgameobjects[i].SetActive(true);
        }

        Collider col = GetComponent<Collider>();
        if (col != null){
            col.enabled = true;
        }
    } 
}
