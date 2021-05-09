using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;


public class Joingame : MonoBehaviour
{
    List<GameObject> roomList = new List<GameObject>();
    
    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomlistItem;

    [SerializeField]
    private Transform roomlistParent;

    private NetworkManager networkManager;

    void Start()
    {
        
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null){
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();

    }//start

    public void RefreshRoomList(){
            networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
            status.text ="Loading...";
    }//refresh

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches){

        status.text ="";

        if(matches == null){
            status.text = "Cant't find any rooms";
            return;
        }

        ClearRoomList();
        foreach (MatchInfoSnapshot match in matches)
        {
            GameObject _roomListItemGo = Instantiate(roomlistItem);
            _roomListItemGo.transform.SetParent(roomlistParent, false);

            roomListitem _roomListItem = _roomListItemGo.GetComponent<roomListitem>();

            if(_roomListItem != null){
                _roomListItem.Setup(match,JoinRoom);
            }

            roomList.Add(_roomListItemGo);
        }

        if(roomList.Count==0){
            status.text = "No rooms aviable";
            return;
        }
    }

    void ClearRoomList()
    {
        for (int i=0; i< roomList.Count; i++){

            Destroy(roomList[i]);
        }
        roomList.Clear();
    }//ClearRoomlist

    public void JoinRoom(MatchInfoSnapshot _match){
        //Debug.Log(_match.name);
        networkManager.matchMaker.JoinMatch(_match.networkId, "" , "", "", 0, 0, networkManager.OnMatchJoined);
        StartCoroutine(WaitForJoin());
    }

    IEnumerator WaitForJoin(){
        ClearRoomList();
        int countdown = 5;

        while(countdown > 0){
            status.text ="JOINING (" + countdown + ")";

            yield return new WaitForSeconds(1);

            countdown--;
        }

        //failed
        status.text="Failed to connect";
        yield return new WaitForSeconds(1);

        MatchInfo matchinfo = networkManager.matchInfo;
        if(matchinfo != null){
            networkManager.matchMaker.DropConnection(matchinfo.networkId, matchinfo.nodeId ,0 , networkManager.OnDropConnection);
            networkManager.StopHost();
        }
    }
    
}
