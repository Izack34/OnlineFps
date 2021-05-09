using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomsize = 6;
    private NetworkManager networkManager;
    private string roomName;

    public void SetRoomName(string name){
        roomName = name;
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.None;
        
        networkManager = NetworkManager.singleton;

        /*networkManager.customConfig = true;
        networkManager.connectionConfig.NetworkDropThreshold = 45;
        networkManager.connectionConfig.OverflowDropThreshold = 45;
        networkManager.connectionConfig.AckDelay = 200;
        networkManager.connectionConfig.AcksType = ConnectionAcksType.Acks128;
        networkManager.connectionConfig.MaxSentMessageQueueSize = 300;*/

        if(networkManager.matchMaker == null){
            networkManager.StartMatchMaker();
        }
    }

    public void CrateRoom(){
        if(roomName != ""){
            networkManager.matchMaker.CreateMatch(roomName, 
                roomsize, true, "", "", "", 0, 0,networkManager.OnMatchCreate);
        }
    }

}

