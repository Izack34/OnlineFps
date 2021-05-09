using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenechanger : MonoBehaviour
{
    public void Scene0() {  
        SceneManager.LoadScene(0);  
    }  
    public void Scene1() {  
        SceneManager.LoadScene(1);  
    }  
    public void Quit() {  
        Application.Quit(); 
    } 
}
