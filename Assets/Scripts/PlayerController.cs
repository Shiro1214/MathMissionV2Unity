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

    public GameObject playerCamera;
    public GameObject scopeCamera;

    public bool isScope = false;

    private bool canJump = true;
    private AudioSource playerAudio;
    public AudioClip shootSound;
    public Vector2 turn;
    public float sensitivity = .5f;

    private Vector3 crosshairPos = new Vector3(110,38, 0);
    private Vector3 firstCrosshairPos;
    private GameObject crosshair;
    private Quaternion onScopepRotation;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        gun = GameObject.Find("AssaultRifle");
        playerAudio = GetComponent<AudioSource>();
        crosshair = GameObject.Find("Crosshair");
        firstCrosshairPos = crosshair.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isScope){
            playerCamera.SetActive(false);
            scopeCamera.SetActive(true);
            onScopepRotation = scopeCamera.transform.localRotation;
            crosshair.transform.position = crosshairPos;
            isScope = true;

        }
        if (Input.GetMouseButtonUp(1) && isScope){
            playerCamera.SetActive(true);
            scopeCamera.SetActive(false);
            isScope = false;
            crosshair.transform.position = firstCrosshairPos;
            scopeCamera.transform.localRotation = Quaternion.Euler(0,0,0);
            turn = Vector2.zero;
        }

        if (isScope){
            var cameraTransform = scopeCamera.transform;
            var rotation = cameraTransform.rotation;
            Debug.Log(rotation);
            turn.x += Input.GetAxis("Mouse X") * sensitivity;
            turn.y += Input.GetAxis("Mouse Y") * sensitivity;

            turn.x = Mathf.Clamp(turn.x, -60, 60);
            cameraTransform.localRotation = Quaternion.Euler(-turn.y,  turn.x, 0);
        }


        if (!isScope){

            var vertical = Input.GetAxis("Vertical");
            playerRb.AddForce(focalPoint.transform.forward * playerSpeed * vertical * Time.deltaTime, ForceMode.Impulse);
            Quaternion targetRotation = Quaternion.LookRotation(focalPoint.transform.forward);
            playerRb.MoveRotation(targetRotation);

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
        }
        //Fire
        
        if (Input.GetMouseButtonDown(0) && shootDelay == 0){
            shoot();            
        }
        if (shootDelay > 0) {
            shootDelay -= Time.deltaTime;
            if (shootDelay <= 0){
                shootDelay = 0;
            }
        }
        
        if (transform.position.y <= 0.5f){
            //playerAnim.SetBool("jumping", canJump);
            canJump = true;
        }
    }

    void shoot(){
        playerAudio.Play(0);
        Quaternion playerRotation = transform.rotation;
        Vector3 RotatedOffset = playerRotation * (new Vector3(0.2f,1.4f,0.74f));
        playerAnim.SetTrigger("Shoot");
        var s = Instantiate(bullet,transform.position + RotatedOffset, Quaternion.LookRotation( isScope ? scopeCamera.transform.forward : focalPoint.transform.forward) * Quaternion.Euler(90,0,0));
        Destroy(s,5);
        shootDelay = .25f;
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
