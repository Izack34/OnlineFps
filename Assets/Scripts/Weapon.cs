using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon{
    
    public string name = "Rifle";

    public int damage = 10;
    public float range = 100f;

    public float fireRate = 0.4f;

    public int Clipsize = 20;

    public int bulletsInClip;
    public int all_bullets = 0;

    public bool is_owned = false;

    public float reloadTime = 1f;

    public GameObject graphics;

    public Weapon(){
        bulletsInClip = Clipsize;
    }
}

