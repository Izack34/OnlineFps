using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UserSettings : MonoBehaviour
{
    
    public static UserSettings Instance;

    public string User_name;

    public void SetUserName(string name){
        User_name = name;
    }

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }
      
    }
    
    
}
