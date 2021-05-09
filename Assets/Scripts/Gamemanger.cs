using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gamemanger : MonoBehaviour
{
    public static Gamemanger instance;

    public delegate void OnPlayerKilledCallback(string player , string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;
   
    void Awake(){
        if(instance != null){
            Debug.LogError("too much Gamemanager");
        }else{
            instance = this;
        }
    }

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player){
        string playerID = "Player " + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
        
    }

    public static void UnRegisterPlayer(string playerID){
        players.Remove(playerID);
    }
    
    public static Player GetPlayer(string playerID){
        return players[playerID];
    }

    public static Player[] Getallplayers(){
        return players.Values.ToArray();
    }


    /*void OnGUI() {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

        foreach(string playerID in players.Keys){
            GUILayout.Label(playerID + " - "+ players[playerID].transform.name);
        }

        GUILayout.EndVertical();
       GUILayout.EndArea();   
    }*/ 
}
