using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Playermovement))]
public class Playercontrol : MonoBehaviour
{
    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private float looksensitivity = 3f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float JetpackBurn = 1f;
    [SerializeField]
    private float JetpackRegen = 0.3f;
    private float JetpackAmount = 1f;

    public float GetJetpackFuelamount(){
        return JetpackAmount;
    }

    [Header("Spring settings:")]

    [SerializeField]
    private float jointSpring;
    [SerializeField]
    private float jointMaxForce = 40f;

    private Player player;
    private Playermovement movementScript;
    private ConfigurableJoint joint;
    private WeaponManager weaponManager;

    void Start()
    {
        player = GetComponent<Player>();
        movementScript = GetComponent<Playermovement>();
        joint = GetComponent<ConfigurableJoint>();
        weaponManager = GetComponent<WeaponManager>();
    }


    void Update()
    {
        if(PauseMenu.pauseisOn){
            if(Cursor.lockState != CursorLockMode.None){
                Cursor.lockState = CursorLockMode.None;
            }

            movementScript.move(Vector3.zero);
            movementScript.rotate(Vector3.zero);
           
            return;
        }

        if(Cursor.lockState != CursorLockMode.Locked){
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        float Xmov = Input.GetAxisRaw("Horizontal");
        float Zmov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * Xmov;
        Vector3 moveVertical = transform.forward * Zmov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        movementScript.move(velocity);

        float YRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0f, YRot , 0f) * looksensitivity;

        movementScript.rotate(rotation);

        float XRot = Input.GetAxisRaw("Mouse Y");

        float rotation_cameraX = XRot * looksensitivity;

        movementScript.rotate_camera(rotation_cameraX);

        Vector3 thrusterForce_s = Vector3.zero;

        if(Input.GetButton("Jump") && JetpackAmount > 0f ){
            JetpackAmount -= JetpackBurn * Time.deltaTime;

            if(JetpackAmount >= 0.01f)
                thrusterForce_s = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }else{
                JetpackAmount += JetpackRegen * Time.deltaTime;
                SetJointSettings(jointSpring);
        }
        JetpackAmount = Mathf.Clamp(JetpackAmount, 0f, 1f);

        movementScript.ApplyThruster(thrusterForce_s);   
    }

    private void SetJointSettings(float jointSpring_v){
        joint.yDrive = new JointDrive{
            positionSpring = jointSpring_v,
            maximumForce =  jointMaxForce
        };
    }
}// end class
