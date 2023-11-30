using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    private float speed;
    private float playerSpeed = 20f;
    private Rigidbody playerRb;

    private GameObject focalPoint;
    private GameObject gun;
    private float shootDelay = 1f;
    public GameObject bullet;

    public Camera playerCamera;
    public Camera scopeCamera;

    private bool isScope = false;

    private bool canJump = true;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        gun = GameObject.Find("AssaultRifle");
    }

    // Update is called once per frame
    void Update()
    {

        var vertical = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * playerSpeed * vertical * Time.deltaTime, ForceMode.Impulse);


        playerRb.MoveRotation(Quaternion.LookRotation(focalPoint.transform.forward));

        if (Input.GetMouseButtonDown(1) && !isScope){
            playerCamera.enabled = false;
            scopeCamera.enabled = true;
            isScope = true;
        }
        if (Input.GetMouseButtonUp(1) && isScope){
            playerCamera.enabled = true;
            scopeCamera.enabled = false;
            isScope = false;
        }

       
        if (Input.GetKey(KeyCode.W)) 
        {
            //playerRb.AddForce(transform.forward * playerSpeed * vertical * Time.deltaTime, ForceMode.Force);
            speed = 1;
            playerAnim.SetTrigger("Run");
            playerAnim.SetFloat("speed", speed);
        }else if (Input.GetKey(KeyCode.S)){
            speed = -1;
            playerAnim.SetTrigger("Run");
            playerAnim.SetFloat("speed", speed);
        }
        if (speed > 0){
            speed -= 0.02f;
            
            playerAnim.SetFloat("speed", speed);

            if (speed <= 0) {
                speed = 0;
                playerAnim.SetFloat("speed", speed);
            }
        } else if (speed < 0){
            speed += 0.02f;
            playerAnim.SetFloat("speed", speed);
            if (speed >= 0) {
                speed = 0;
                playerAnim.SetFloat("speed", speed);
            }
        }

        //Fire
        
        if (Input.GetMouseButtonDown(0) && shootDelay == 0){
            Quaternion playerRotation = transform.rotation;
            Vector3 RotatedOffset = playerRotation * (new Vector3(0.2f,1.4f,0.74f));
            playerAnim.SetTrigger("Shoot");

            var s = Instantiate(bullet, transform.position + RotatedOffset,Quaternion.LookRotation(focalPoint.transform.forward) * Quaternion.Euler(90,0,0));
            
            Destroy(s,5);
            shootDelay = .25f;
        }
        if (shootDelay > 0) {
            shootDelay -= Time.deltaTime;
            if (shootDelay <= 0){
                shootDelay = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump){
            playerAnim.SetTrigger("jump");
            //playerRb.AddForce(Vector3.up * Time.deltaTime * 1000f, ForceMode.Impulse);
            canJump  = false;
        } 

        if (Input.GetKeyDown(KeyCode.LeftControl)){
            crouching();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl)){
            uncrouch();
        }

        
        

        if (transform.position.y <= 0.5f){
            //playerAnim.SetBool("jumping", canJump);
            canJump = true;
        }
    }

    void jump(){
        //playerAnim.SetTrigger("jump");
        playerRb.AddForce(Vector3.up * Time.deltaTime * 500f, ForceMode.Impulse);
        canJump  = false;
    }

    void crouching(){
        playerAnim.SetBool("crouch",true);
    }
    void uncrouch(){
        playerAnim.SetBool("crouch",false);
    }
}
