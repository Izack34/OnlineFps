using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Playermovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Vector3 thrusterforce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 85f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void move(Vector3 velocity_s){
        velocity = velocity_s;
    }

    public void rotate(Vector3 _rotation){
        rotation = _rotation;
    }

    public void rotate_camera(float rotation_cameraX){
        cameraRotationX = rotation_cameraX;
    }

    public void ApplyThruster(Vector3 thrusterforce_s){
        thrusterforce = thrusterforce_s;
    }

    void FixedUpdate(){
        Performmovement();
        PerformRotation();
    }

    void Performmovement(){
        if (velocity != Vector3.zero){
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        if (thrusterforce != Vector3.zero){
            rb.AddForce(thrusterforce*Time.deltaTime , ForceMode.Acceleration);
        }
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if(cam != null){
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX
                                                , -cameraRotationLimit 
                                                , cameraRotationLimit);

            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX,0f,0f);
        }
    }

}//end class
