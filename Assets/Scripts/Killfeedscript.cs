using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killfeedscript : MonoBehaviour
{
    [SerializeField]
    GameObject killfeedItemPrefab;

    void Start() {
        Gamemanger.instance.onPlayerKilledCallback += OnKill;    
    }

    public void OnKill(string player, string source){
        //Debug.Log(source +" killed "+ player);
        GameObject go = (GameObject)Instantiate(killfeedItemPrefab, this.transform);
        go.GetComponent<KillItem>().Setup(player, source);

        Destroy(go, 5f);
    }
}
