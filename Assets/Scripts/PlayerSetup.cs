using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

    GameObject settings;

    [SerializeField]
    GameObject playerUIprefab;
    public GameObject PlayerUiInstance;

    [SerializeField]
    public GameObject playerdieUIprefab;

    public GameObject Platform;
    public Starplatform Starscript;
    public PlayerUI ui;

	public Camera sceneCamera;

    public GameObject Ballmodel;

	void Start ()
	{
        if(!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();

        }else{
            sceneCamera = Camera.main;
            Ballmodel.layer = LayerMask.NameToLayer("TransparentFX");


            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            PlayerUiInstance = Instantiate(playerUIprefab);
            PlayerUiInstance.name = playerUIprefab.name;

            PlayerUI ui = PlayerUiInstance.GetComponent<PlayerUI>();
            if (ui == null){
                Debug.LogError("no player UI");
            }
            ui.SetPlayer(GetComponent<Player>());

            if (Settingschanger.UserName != null){
                CmdSetUsername(transform.name , Settingschanger.UserName);
            }else{
                CmdSetUsername(transform.name , transform.name);
            }
            
        }
        GetComponent<Player>().Setup();

    }

    void Update() {
        if(isServer && isLocalPlayer){
           if (Input.GetKeyDown("h"))
            {
                CmdUIstart();
            }
        }
    }

    [Command]
    public void CmdUIstart(){
        RpcUIstart();
    }

    [ClientRpc]
    public void RpcUIstart(){
        Starplatform.instance.onStart.Invoke();
    }


    [Command]
	void CmdSetUsername (string playerID, string username)
	{
		Player player = Gamemanger.GetPlayer(playerID);
		if (player != null)
		{ 
			player.username = username;
            Debug.Log(player.username);
		}
	}

    public override void OnStartClient(){
        base.OnStartClient();

        string netID_p = GetComponent<NetworkIdentity>().netId.ToString();
        Player player_p = GetComponent<Player>();

        Gamemanger.RegisterPlayer(netID_p, player_p);
    }
   
    void AssignRemoteLayer(){
        gameObject.layer = LayerMask.NameToLayer("RemotePlayer");

    }

    void DisableComponents(){
        for(int i=0; i< componentsToDisable.Length; i++){
                componentsToDisable[i].enabled = false;
        }
    }

    public GameObject getUI(){
        return PlayerUiInstance;
    }
    
    void OnDisable(){

        Destroy(PlayerUiInstance);

        if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(true);
            }
        Gamemanger.UnRegisterPlayer(transform.name);
    }

}