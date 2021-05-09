using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";   

    [SerializeField]
    private GameObject weaponGfx;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    [SerializeField]
    private LayerMask Pickupmask;

    [SerializeField]
    private GameObject rocketprefab;

    private PlayerSetup Setup;
    private PlayerUI GUI;
    private Player player;
    private Weapon currentWeapon;
    private WeaponManager weaponManager;
    private float recoil = 0.00f;
    private float meleeAttackCD = 1f;

    void Start()
    {
        if (cam == null){
            Debug.LogError("no camera");
            this.enabled = false;
        }    
        player = GetComponent<Player>();
        weaponManager = GetComponent<WeaponManager>();
        Setup = GetComponent<PlayerSetup>();
        if(isLocalPlayer)
        {
            GUI = Setup.getUI().GetComponent<PlayerUI>();
            
        }
    }
    
    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if(PauseMenu.pauseisOn ){
            return;
        }
        if(!isLocalPlayer)
        {
            return;
        }
        
        lookandpick();

        if(currentWeapon.bulletsInClip < currentWeapon.Clipsize){
            if(Input.GetKeyDown(KeyCode.R)){
                weaponManager.reload();
                CancelInvoke("Shoot");
                return;
            }
           
        }

        if(currentWeapon.name == "BlueSword"){
            meleeAttackCD += Time.deltaTime;
            meleeAttackCD = Mathf.Clamp(meleeAttackCD, 0f, 0.5f);
            if(Input.GetButtonDown("Fire1") && meleeAttackCD >= currentWeapon.fireRate){
                meleeAttackCD = 0;
                thrust();
            }
        }else if(currentWeapon.name == "Rocketluncher"){
            if(Input.GetButtonDown("Fire1")){
                Shootrocket();
            }
        }else if(currentWeapon.name == "Glock"){
            if(Input.GetButtonDown("Fire1")){
                Shoot();
            }
        }else{
            if(Input.GetButtonDown("Fire1")){
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            }else if(Input.GetButtonUp("Fire1")){
                CancelInvoke("Shoot");    
            }
        }
    
        
    }

    [Command]
    void CmdOnAttack(){
        RpcdoMuzzleFlashandAudio();
    }

    [ClientRpc]
    void RpcdoMuzzleFlashandAudio(){
        if(weaponManager.GetCurrentGFX().muzzleflash != null){
            weaponManager.GetCurrentGFX().muzzleflash.Play();
        }

        if(weaponManager.GetCurrentGFX().Audioeffect != null){
            weaponManager.GetCurrentGFX().Audioeffect.Play();
        }
    }

    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal, string TagSurface){
        RpcdoHiteffect(pos, normal, TagSurface);
    }

    [ClientRpc]
    void RpcdoHiteffect(Vector3 pos, Vector3 normal, string TagSurface){
        if(TagSurface == PLAYER_TAG){
            GameObject hitreff = Instantiate(weaponManager.GetCurrentGFX().hitEffectPlayerprefab, pos, Quaternion.LookRotation(normal));
            Destroy(hitreff, 2f);
        }else{
            GameObject hitreff = Instantiate(weaponManager.GetCurrentGFX().hitEffectprefab, pos, Quaternion.LookRotation(normal));
            Destroy(hitreff, 2f);
        }
    }

    [Client]
    void lookandpick(){

        Vector3 DirectionRay = cam.transform.TransformDirection(0,0,1);

        RaycastHit lookreturn;

        if(Physics.Raycast(cam.transform.position, DirectionRay, out lookreturn, 5, Pickupmask)){
            GUI.Onpickuplook(true);
            if(Input.GetKeyDown(KeyCode.E)){

                //Debug.Log(lookreturn.collider.name);
                switch (lookreturn.collider.tag)
                {
                case "Healthpack":
                    if(player.Healthpack()){
                        CmdDestory(lookreturn.collider.gameObject);
                    }
                    break;
                case "Pistolammo":
                    weaponManager.add_ammo(1);
                    
                    CmdDestory(lookreturn.collider.gameObject);
                    break;
                case "Riffleammo":
                    weaponManager.add_ammo(2);
                    
                    CmdDestory(lookreturn.collider.gameObject);
                    break;
                case "Rockets":
                    weaponManager.add_ammo(3);
                    CmdDestory(lookreturn.collider.gameObject);
                    break;
                case "Pistol":
                    if(weaponManager.pickup_weapon(1)){
                        
                        CmdDestory(lookreturn.collider.gameObject);
                    }                      
                    break;
                case "RIfle":
                    if(weaponManager.pickup_weapon(2)){
                        
                        CmdDestory(lookreturn.collider.gameObject);
                    }                    
                    break;
                case "RocketL":
                    if(weaponManager.pickup_weapon(3)){
                        CmdDestory(lookreturn.collider.gameObject);
                        
                    }           
                    break;
                }
            }
        }else
        {
            GUI.Onpickuplook(false);
        }   
        
    }

    [Command]
    public void CmdDestory(GameObject thing){
        NetworkServer.Destroy(thing);
    }

    

    [Client]
    void Shoot(){

        if(!isLocalPlayer){
            return;
        }
        if(weaponManager.isReloading){
            return;
        }
        if(currentWeapon.bulletsInClip <= 0){
            weaponManager.reload();
            return;
        }

        CancelInvoke("lowerRecoil");  
        StopCoroutine("lowering");   

        GUI.CrosshairMove(recoil);
        currentWeapon.bulletsInClip--;
        CmdOnAttack();
        
        RaycastHit hit;
        
        Vector3 DirectionRay = cam.transform.TransformDirection(Random.Range(recoil,-recoil),
                                                                 Random.Range(recoil,-recoil), 1);
        
        //Debug.DrawRay(cam.transform.position , cam.transform.forward , Color.blue , 10f);
        if(Physics.Raycast(cam.transform.position, DirectionRay, out hit, currentWeapon.range, mask)){
            
            if(hit.collider.tag == PLAYER_TAG){
                CmdPlayerDmg(hit.collider.name, currentWeapon.damage);
            }

            CmdOnHit(hit.point, hit.normal, hit.collider.tag);
        }
        recoil += 0.01f;
        recoil = Mathf.Clamp(recoil, 0f, 0.05f);
        
        InvokeRepeating("lowerRecoil", 0.2f, 0.001f);
        
    }

    void lowerRecoil(){
        recoil -= 0.001f;
        recoil = Mathf.Clamp(recoil, 0f, 0.05f);
        GUI.CrosshairMove(recoil);
        if(recoil == 0f){
            CancelInvoke("lowerRecoil");  
        }
    }



    [Client]
    void Shootrocket(){
        if(!isLocalPlayer){
            return;
        }
        if(weaponManager.isReloading){
            return;
        }
        if(currentWeapon.bulletsInClip <= 0){
            weaponManager.reload();
            return;
        }
        currentWeapon.bulletsInClip--;
        
        CmdOnShootrocket(weaponManager.GetCurrentGFX().rocketspawn.transform.position, 
                        weaponManager.GetCurrentGFX().rocketspawn.transform.rotation );
        
    }
    
    [Command]
    void CmdOnShootrocket(Vector3 pos, Quaternion rot){
        RpcdoShootrocket( pos, rot);
    }

    [ClientRpc]
    void RpcdoShootrocket( Vector3 pos, Quaternion rot){
        GameObject Rockettolunch = Instantiate(rocketprefab, pos , rot);
        
        Rockettolunch.GetComponent<Rigidbody>().velocity =
             Rockettolunch.transform.TransformDirection(Vector3.forward *100f);
    }
    


    [Client]
    void thrust(){
        if(!isLocalPlayer){
            return;
        }
        
        CmdOnAttackmelee();
        weaponManager.thrustA();   
        Vector3 DirectionRay = cam.transform.TransformDirection(0,0,1);
        RaycastHit meleehit;

        if(Physics.Raycast(cam.transform.position, DirectionRay,
                            out meleehit, currentWeapon.range, mask)){
                                
            if(meleehit.collider.tag == PLAYER_TAG){
                CmdPlayerDmg(meleehit.collider.name, currentWeapon.damage);
            }
        }     
    }

    [Command]
    void CmdOnAttackmelee(){
        RpcdoAudio();
    }

    [ClientRpc]
    void RpcdoAudio(){
        if( weaponManager.GetCurrentGFX().Audioeffect != null){
            weaponManager.GetCurrentGFX().Audioeffect.Play();
        }
    }


    [Command]
    void CmdPlayerDmg(string playerID, int dmg){

        //Debug.Log(playerID + " shot");
        Player player_s = Gamemanger.GetPlayer(playerID);
        player_s.RpcTakeDamage(dmg, player.username);

    }
}//end class
