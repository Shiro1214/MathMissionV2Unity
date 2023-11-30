using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyScript : MonoBehaviour
{
    private GameObject entrance;
    private Vector3 entrancePos;
    private Rigidbody enemyRb;
    private GameManager gm;
    private bool setAnswer = false;
    private ParticleSystem particleObj;
    public GameObject answerCanvas;
    public GameObject mapCanvas;
    public TextMeshProUGUI textUi;
    public TextMeshProUGUI mapLabel;
    
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI enemyIn;
    public bool hasRightAnswer, isHit;
    public float speedForce, answer;

    //private int score;
    // Start is called before the first frame update
    void Start()
    {
        
       // audioSource = GetComponent<AudioSource>();
        //fixedRotation = transform.rotation;
        textUi  = answerCanvas.transform.Find("MathAnswer").gameObject.GetComponent<TextMeshProUGUI>();
        mapLabel  = mapCanvas.transform.Find("MathAnswerLabel").gameObject.GetComponent<TextMeshProUGUI>();
        scoreUI = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        enemyIn = GameObject.Find("EnemyIn").GetComponent<TextMeshProUGUI>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        particleObj = transform.Find("CFXR Impact Glowing HDR (Blue)").gameObject.GetComponent<ParticleSystem>();
        //entranceCollider = entrance.GetComponent<Collider>();
       // entrancePos = entrance.transform.position  + randomEntranceOffset();
       // speedForce = GameSettings.Instance.enemySpeed;
         entrance = GameObject.Find("Entrance");
         entrancePos = entrance.transform.position;
         enemyRb = GetComponent<Rigidbody>();
         speedForce = 20f;
         isHit = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        var direction = entrancePos - transform.position;
        direction = direction.normalized;
        enemyRb.AddForce(direction * speedForce * Time.deltaTime,ForceMode.Impulse);
        enemyRb.rotation = Quaternion.LookRotation(direction); //velocity direction rotation
        if(transform.position.y < -10){
            Destroy(gameObject);
        }
        if (setAnswer==false){
            textUi.text = answer.ToString();
            mapLabel.text = answer.ToString();
        }
    }

     private void OnCollisionEnter(Collision other)
    {
        if (gm.isGameActive){
            if (other.gameObject == entrance){
                if (hasRightAnswer) {
                    GameSettings.Instance.score -= 10;
                    //gm.score -= 10;
                    GameSettings.Instance.enemyIn += 1;
                    Debug.Log("In +1");
                
                    scoreUI.text = "Score: " + gm.score.ToString();//GameSettings.Instance.score.ToString();
                    enemyIn.text = "Enemy in: " + GameSettings.Instance.enemyIn.ToString();
                    gm.clearEnemies();
                }
                Destroy(gameObject);
            }

            else if (other.gameObject.CompareTag("Bullet") && !isHit)
            {
            //explosionParticle.Play();
            isHit = true;
            particleObj.Play();
            //Destroy(particleObj.gameObject, 2f);
            Destroy(other.gameObject);
            //particleObj= Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            if (hasRightAnswer){

                //audioSource.PlayOneShot(correctSound);
                GameSettings.Instance.score += 10;
                Debug.Log("+10");
                //gm.score += 10;
                if (GameSettings.Instance.frugality){
                   GameSettings.Instance.score += 1;
                }
                scoreUI.text = "Score: " + GameSettings.Instance.score.ToString();
                //Destroy(gameObject, correctSound.length + 0.1f);
                gm.clearEnemies();
            } else 
            {
                //audioSource.PlayOneShot(incorrectSound);
                GameSettings.Instance.score -= 10;
                //gm.score -= 10;                
                //Debug.Log("-10");
                scoreUI.text = "Score: " + GameSettings.Instance.score.ToString();
                //Destroy(gameObject, incorrectSound.length + 0.1f);
                
            }

            Destroy(gameObject,0.1f);
            }
        }
    }
}
