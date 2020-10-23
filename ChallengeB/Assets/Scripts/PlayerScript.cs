using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScript : MonoBehaviour
{
    private bool isOnGround;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask allGround;

    private bool facingRight = true;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;


    public float speed;

    public Text scoreText;
    public Text livesText;
    public Text conditionalText;

    private int score;
    private int lives;

    private Rigidbody2D rd2d;

    Animator anim;

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }

    // Start is called before the first frame update
    void Start()
    {   
        anim= GetComponent<Animator>();

        musicSource.clip= musicClipOne;
        musicSource.Play();

        rd2d = GetComponent<Rigidbody2D>();

        score=0;
        lives=3;

        SetScoreText ();
        livesText.text="Lives: " +lives.ToString ();
        conditionalText.text="";
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey("escape"))
            {
            Application.Quit();
            }

        if (Input.GetKeyDown(KeyCode.W))
            {
            anim.SetInteger("State",2);
            } 
        if (Input.GetKeyUp(KeyCode.W))
            {
            anim.SetInteger("State",0);
            }
        if(Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.D))
            {
            anim.SetInteger("State", 1);
            }
        if(Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.D))
            {
            anim.SetInteger("State", 0);
            }

    }
    

    void FixedUpdate()
    {   
        isOnGround = Physics2D.OverlapCircle(groundCheck.position,1f,allGround);

        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

          if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        rd2d.AddForce(new Vector2(hozMovement*speed, verMovement*speed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag=="coin")
        {
            score= score+1;
            scoreText.text= score.ToString();
            Destroy(collision.collider.gameObject);
            SetScoreText ();
        }
        else if(collision.collider.tag=="Enemy")
        {
            lives= lives-1;
            livesText.text="Lives: " +lives.ToString ();
            Destroy(collision.collider.gameObject);
            if(lives==0)
            {
             Destroy(this);
             conditionalText.text="You Lose";
            }
        }

    }
  
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag=="Ground" && isOnGround==true)
        { 

            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0,3), ForceMode2D.Impulse);
            }
        }
    }

    void SetScoreText ()
        {
            scoreText.text="Score: " +score.ToString ();
            if (score==4)
            {
                lives=3;
               transform.position= new Vector2(15,3);
               livesText.text="Lives: " +lives.ToString ();
            }

            if (score==8)
            {
                musicSource.Stop();
                musicSource.clip= musicClipTwo;
                musicSource.Play();
                conditionalText.text="You win! Game created by JaeWoo Joe";
            }
        
         
        }

}
