using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private Rigidbody bulletRb;
    private GameObject focalPoint;
    public PlayerController player;
    // Start is called before the first frame update
    public Vector3 flyDir;
    // Start is called before the first frame update
    void Start()
    {
        bulletRb= GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        var scopeCamera = GameObject.Find("scopeCam");
        if (player.isScope){
            flyDir = scopeCamera.transform.forward;
        } else {
            flyDir = focalPoint.transform.forward;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //flyDir
        bulletRb.AddForce(flyDir * 2000f * Time.deltaTime);
    }
}
