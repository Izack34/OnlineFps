using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private const string WEAPON_LAYER = "Weapon";

    [SerializeField]
    private Weapon Sword;

    [SerializeField]
    private Weapon pistol;

    [SerializeField]
    private Weapon Rifle;

    [SerializeField]
    private Weapon rocketLuncher;

    private Weapon currentWeapon;
    private WeaponGFX currentgfx;

    private PlayerSetup Setup;
    private PlayerUI GUI;


    [SyncVar]
    private int currentWeaponnumber = 1;

    public bool isReloading = false;

    void Start()
    {
        Setup = GetComponent<PlayerSetup>();
        EquipWeapon(currentWeaponnumber);
        if(isLocalPlayer)
        {
            GUI = Setup.getUI().GetComponent<PlayerUI>();
        }
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && isReloading == false){
            
            EquipWeapon(1);
            
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && isReloading == false){
            
            EquipWeapon(2);
            
        }
        if(Input.GetKeyDown(KeyCode.Alpha3) && isReloading == false){
            
            EquipWeapon(3);
            
        }
        if(Input.GetKeyDown(KeyCode.Alpha4) && isReloading == false){
            
            EquipWeapon(4);
            
        }
        //Debug.Log(" current :"  +currentWeaponnumber);
        if(isLocalPlayer){
            CmdOnSwitchweapon(currentWeaponnumber);
        }
    }

    public Weapon GetCurrentWeapon(){
        return currentWeapon;
    }

    public WeaponGFX GetCurrentGFX(){
        return currentgfx;
    }


    void EquipWeapon(int weaponnumber){

        
        if(weaponnumber == 1 ){
            
            if(isLocalPlayer){
                currentWeapon = Sword;
                currentWeaponnumber = 1;
                
                Sword.graphics.SetActive(true);
                pistol.graphics.SetActive(false);
                Rifle.graphics.SetActive(false);
                rocketLuncher.graphics.SetActive(false);
            
                currentgfx = Sword.graphics.GetComponent<WeaponGFX>();

                Sword.graphics.layer = LayerMask.NameToLayer(WEAPON_LAYER);

            }

        
        }else if(weaponnumber == 2 && pistol.is_owned){    

            
             if(isLocalPlayer){
                currentWeapon = pistol;
                currentWeaponnumber = 2; 
                Sword.graphics.SetActive(false);
                pistol.graphics.SetActive(true);
                Rifle.graphics.SetActive(false);
                rocketLuncher.graphics.SetActive(false);

                currentgfx = pistol.graphics.GetComponent<WeaponGFX>();
                
                pistol.graphics.layer = LayerMask.NameToLayer(WEAPON_LAYER);

                foreach(Transform child in pistol.graphics.transform){
                    child.gameObject.layer = LayerMask.NameToLayer(WEAPON_LAYER);
                }
            }
        }else if(weaponnumber == 3 && Rifle.is_owned){
            

            if(isLocalPlayer){
                currentWeapon = Rifle;
                currentWeaponnumber = 3;
                Sword.graphics.SetActive(false);
                pistol.graphics.SetActive(false);
                Rifle.graphics.SetActive(true);
                rocketLuncher.graphics.SetActive(false);
            
                currentgfx = Rifle.graphics.GetComponent<WeaponGFX>();

                Rifle.graphics.layer = LayerMask.NameToLayer(WEAPON_LAYER);

                foreach(Transform child in Rifle.graphics.transform){
                    child.gameObject.layer = LayerMask.NameToLayer(WEAPON_LAYER);
                }
            }
        }else if(weaponnumber == 4 && rocketLuncher.is_owned){

            
            if(isLocalPlayer){
                currentWeapon = rocketLuncher;
                currentWeaponnumber = 4;

                Sword.graphics.SetActive(false);
                pistol.graphics.SetActive(false);
                Rifle.graphics.SetActive(false);
                rocketLuncher.graphics.SetActive(true);

                currentgfx = rocketLuncher.graphics.GetComponent<WeaponGFX>();

                rocketLuncher.graphics.layer = LayerMask.NameToLayer(WEAPON_LAYER);

                foreach(Transform child in rocketLuncher.graphics.transform){
                    child.gameObject.layer = LayerMask.NameToLayer(WEAPON_LAYER);
                }
            }
        }else{
            return;
        }
    }


    public bool pickup_weapon(int Tag){
        if (Tag == 1 && !pistol.is_owned){
                Cmdpickup_weapon(Tag);
                return true;
        }else if (Tag == 2 && !Rifle.is_owned ){
                Cmdpickup_weapon(Tag);
                return true;
        }else if (Tag == 3 && !rocketLuncher.is_owned){
                Cmdpickup_weapon(Tag);
                return true;
        }else{
            return false;
        }
    }

    [Command]
    void Cmdpickup_weapon(int Tag){
        Rpcpickup_weapon(Tag);
    }

    [ClientRpc]
    void Rpcpickup_weapon(int Tag){
        if (Tag == 1){         
                pistol.is_owned = true;        
        }else if (Tag == 2){
                Rifle.is_owned = true;              
        }else if (Tag == 3){
                rocketLuncher.is_owned = true;
        }else{
            return;
        }
    }

    public void add_ammo(int Tag){
        if (Tag == 1){
                pistol.all_bullets += 14;
        }else if (Tag == 2){
                Rifle.all_bullets += 30;
        }else if (Tag == 3){
                rocketLuncher.all_bullets += 5;
        }else{
            return;
        }
    }

    public void thrustA(){
        if(isLocalPlayer){
            CmdthrustA();
        }
    }

    [Command]
    void CmdthrustA(){
        RpcthrustA();
    }

    [ClientRpc]
    void RpcthrustA(){
        Animator anim = currentgfx.GetComponent<Animator>();

        if(anim != null){
            
            anim.SetTrigger("Shoot");
        }
    }

    public void reload(){
        if(isReloading){
            return;
        }

        if(currentWeapon.all_bullets <= 0){
            return;
        }
        isReloading = true;
        
        StartCoroutine(ReloadCorutine());

    }

    private IEnumerator ReloadCorutine(){
        isReloading = true;

        if(isLocalPlayer){
            GUI.Startreloadanim(currentWeapon.reloadTime);
            CmdOnreload();
        }
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        
        currentWeapon.all_bullets += currentWeapon.bulletsInClip;
        if(currentWeapon.all_bullets < currentWeapon.Clipsize){
            currentWeapon.bulletsInClip = currentWeapon.all_bullets;
            currentWeapon.all_bullets = 0;          
        }else{
            currentWeapon.all_bullets -= currentWeapon.Clipsize;
            currentWeapon.bulletsInClip = currentWeapon.Clipsize;
        }
        isReloading= false;
    }

    [Command]
    void CmdOnreload(){
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload(){
        Animator anim = currentgfx.GetComponent<Animator>();

        if(anim != null){
            
            anim.SetTrigger("Reload");
        }
    }

    [Command]
    void CmdOnSwitchweapon(int weaponNumber){
        RpcOnSwitchweapon(weaponNumber);
    }

    [ClientRpc]
    void RpcOnSwitchweapon(int weaponnumber){
        if(weaponnumber == 1){
            if(!isLocalPlayer){
                
                Sword.graphics.SetActive(true);
                pistol.graphics.SetActive(false);
                Rifle.graphics.SetActive(false);
                rocketLuncher.graphics.SetActive(false);
                currentgfx = Sword.graphics.GetComponent<WeaponGFX>();
            }
        }else if(weaponnumber == 2){
            if(!isLocalPlayer){

                Sword.graphics.SetActive(false);
                pistol.graphics.SetActive(true);
                Rifle.graphics.SetActive(false);
                rocketLuncher.graphics.SetActive(false);
                currentgfx = pistol.graphics.GetComponent<WeaponGFX>();
            }
        }else if(weaponnumber == 3){
            if(!isLocalPlayer){
                
                Sword.graphics.SetActive(false);
                pistol.graphics.SetActive(false);
                Rifle.graphics.SetActive(true);
                rocketLuncher.graphics.SetActive(false);
                currentgfx = Rifle.graphics.GetComponent<WeaponGFX>();
            }
        }else if(weaponnumber == 4){
            if(!isLocalPlayer){
                Sword.graphics.SetActive(false);
                pistol.graphics.SetActive(false);
                Rifle.graphics.SetActive(false);
                rocketLuncher.graphics.SetActive(true);
                currentgfx = rocketLuncher.graphics.GetComponent<WeaponGFX>();
            }
        }else{
            return;
        }
    }

    
}
