using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour
{
    public static bool pauseisOn = false;

    private NetworkManager networkManager;

    void Start()
    {   
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoom(){
        MatchInfo matchinfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchinfo.networkId,
                                         matchinfo.nodeId ,0 , networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
