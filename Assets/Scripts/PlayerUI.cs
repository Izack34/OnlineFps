using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerUI : NetworkBehaviour
{
    [SerializeField]
    private Image Jetfuel;

    [SerializeField]
    private Image healthbarfill;

    [SerializeField]
    private Text ammoText;

    [SerializeField]
    private GameObject pickupInfo;

    [SerializeField]
    private GameObject Info;

    [SerializeField]
    private GameObject countText;

    [SerializeField]
    private GameObject pasuemenu;

    [SerializeField]
    private GameObject board;

    [SerializeField]
    private Image reloaddisplay;

    [SerializeField]
    private GameObject Topcross;

    [SerializeField]
    private GameObject botcross;

    [SerializeField]
    private GameObject leftcross;

    [SerializeField]
    private GameObject rightcross;

    private Player player;
    private Playercontrol controler;

    private WeaponManager weaponManager;

    private Vector3 toppos;
    private Vector3 botpos;
    private Vector3 leftpos;
    private Vector3 rightpos;


    public void SetPlayer(Player _player){
        player =_player;
        controler = player.GetComponent<Playercontrol>();
        weaponManager = player.GetComponent<WeaponManager>();
    }

    void Start() {
        PauseMenu.pauseisOn = false;   
        reloaddisplay.fillAmount=0f;
        StartCoroutine("Hostinfo");
        toppos = Topcross.transform.position;
        botpos = botcross.transform.position;
        leftpos = leftcross.transform.position;
        rightpos = rightcross.transform.position;
        Starplatform.instance.onStart += startcounting;
    }

    void Update() {
        SetFuelamount(controler.GetJetpackFuelamount());
        Sethealthamount(player.GetHealth());
        SetAmmoamount(weaponManager.GetCurrentWeapon().bulletsInClip, weaponManager.GetCurrentWeapon().all_bullets);

        if(Input.GetKeyDown(KeyCode.Escape)){
            TogglePauseMenu();
        }

        //if(Input.GetKeyDown(KeyCode.Tab)){
        //    board.SetActive(true);
        //}else if (Input.GetKeyUp(KeyCode.Tab)){
        //    board.SetActive(false);
        //}
    }

    void TogglePauseMenu(){
        pasuemenu.SetActive(!pasuemenu.activeSelf);

        PauseMenu.pauseisOn = pasuemenu.activeSelf;
    }

    void SetFuelamount(float amount){
        
        Jetfuel.fillAmount = amount;

    }

    void Sethealthamount(float amount){
        healthbarfill.fillAmount = amount;
    }

    void SetAmmoamount(int amount, int all_bulltes){
            ammoText.text = amount.ToString() + "/" + all_bulltes.ToString();
    }

    public void CrosshairMove(float recoil_){
        recoil_ *= 10f;
        float movvec = recoil_*70;
        Topcross.transform.position = Vector3.Lerp(Topcross.transform.position, 
                                                    toppos + Vector3.up*movvec , Time.time);
        botcross.transform.position = Vector3.Lerp(botcross.transform.position,
                                                 botpos + Vector3.up*(-1)*movvec , Time.time);
        leftcross.transform.position = Vector3.Lerp(leftcross.transform.position,
                                                 leftpos + Vector3.left*movvec , Time.time);
        rightcross.transform.position = Vector3.Lerp(rightcross.transform.position,
                                                 rightpos + Vector3.right*movvec , Time.time);

    }

    public void Startreloadanim(float time){
       StartCoroutine("CountDownAnimation",time);
       
    }

    IEnumerator CountDownAnimation(float time){
        float animationTime = time;
        while (animationTime > 0) {
            animationTime -= Time.deltaTime;
            reloaddisplay.fillAmount = animationTime/time;
            yield return null;
        }
        
    }

    IEnumerator Hostinfo(){
        int countdowninfo = 10;
        while(countdowninfo >0){
            yield return new WaitForSeconds(1f);
            countdowninfo--;
        }
        Info.gameObject.SetActive(false);
    }

    public void startcounting(){
        StartCoroutine("CountDown");
    }

    IEnumerator CountDown(){
        countText.gameObject.SetActive(true);
        int countdownStart = 5;
        while(countdownStart >0){
            countText.GetComponent<TMPro.TextMeshProUGUI>().text = "Game start in "+ countdownStart.ToString();
            yield return new WaitForSeconds(1f);
            countdownStart--;
        }
        countText.gameObject.SetActive(false);
    }

    public void Onpickuplook(bool looked){
        pickupInfo.gameObject.SetActive(looked);
    }

}
