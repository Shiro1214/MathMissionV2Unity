using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private Rigidbody bulletRb;
    private GameObject focalPoint;
    // Start is called before the first frame update
    private Vector3 flyDir;
    // Start is called before the first frame update
    void Start()
    {
        bulletRb= GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        flyDir = focalPoint.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        bulletRb.AddForce(flyDir * 1000f * Time.deltaTime);
    }
}
