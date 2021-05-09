using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class DestroyObject : NetworkBehaviour
{
    [Command]
    public void CmdDestoryThis(){
        Rpcdestroy();
    }

    [ClientRpc]
    public void Rpcdestroy(){
        Debug.Log("destroyed");
        Destroy(gameObject);
    }

}

