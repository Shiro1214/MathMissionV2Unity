using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 200;
    public GameObject player;
    public Vector3 offset;

    private Vector3 rotate;
    private float y;
        public float sensitivity = -1f;
    //public GameObject playerHead;

    void Start(){
        //playerHead = GameObject.Find("PlayerHead");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");


        transform.Rotate(Vector3.up, horizontalInput *speed * Time.deltaTime);


 // Get the player's local rotation
        Quaternion playerRotation = player.transform.rotation;

        // Use player's local rotation to adjust the offset
        Vector3 rotatedOffset = playerRotation * offset;
        

//        Debug.Log(rotatedOffset);

        // Calculate the desired position based on player's position and rotated offset
        Vector3 desiredPosition = player.transform.position + rotatedOffset;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPosition, 0.125f);
        transform.position = smoothedPos;

        // Rotate the camera with mouse

    }

}

