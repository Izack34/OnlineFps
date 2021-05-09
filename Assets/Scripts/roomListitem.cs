using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
using TMPro;
using System;

public class roomListitem : MonoBehaviour
{
    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    public JoinRoomDelegate joinroomCallback;
    private MatchInfoSnapshot match;

    [SerializeField]
    private TMP_Text roomName;

    public void Setup(MatchInfoSnapshot matchInfo, JoinRoomDelegate joinroomfunction){
       match = matchInfo;
       joinroomCallback = joinroomfunction;

       roomName.text = match.name + "(" + match.currentSize + "/" + match.maxSize + ")" ;
    }

    public void JoinGame(){
        joinroomCallback.Invoke(match);
    }
}
