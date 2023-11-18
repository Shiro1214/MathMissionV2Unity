using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnim;
    private float speed;
    private float playerSpeed = 5000f;
    private Rigidbody playerRb;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        var vertical = Input.GetAxis("Vertical");
        playerRb.AddForce(transform.forward * playerSpeed * vertical * Time.deltaTime, ForceMode.Force);
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
        
    }
}
