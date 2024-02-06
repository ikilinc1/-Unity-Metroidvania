using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;
    public float jumpForce;
    public Transform groundPoint;
    public LayerMask whatIsGround;
    public Animator anim;
    public Animator ballAnim;
    public BulletController shotToFire;
    public Transform shotPoint;
    public float dashSpeed;
    public float dashTime;
    public SpriteRenderer theSR;
    public SpriteRenderer afterImage;
    public float afterImageLifetime;
    public float timeBetweenAfterImages;
    public Color afterImageColor;
    public float waitAfterDashing;
    public GameObject standing, ball;
    public float waitToBall;
    public Transform bombPoint;
    public GameObject bomb;
    
    private bool isOnGround;
    private bool canDoubleJump;
    private float dashCounter;
    private float afterImageCounter;
    private float dashRechargeCounter;
    private float ballCounter;
    private PlayerAbilityTracker abilities;
    
    // Start is called before the first frame update
    void Start()
    {
        abilities = GetComponent<PlayerAbilityTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dashRechargeCounter > 0)
        {
            dashRechargeCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetButtonDown("Fire2") && standing.activeSelf && abilities.canDash)
            {
                dashCounter = dashTime;
                ShowAfterImage();
            }
        }
        
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            theRB.velocity = new Vector2(dashSpeed * transform.localScale.x, theRB.velocity.y);
            afterImageCounter -= Time.deltaTime;
            if (afterImageCounter <= 0)
            {
                ShowAfterImage();
            }

            dashRechargeCounter = waitAfterDashing;
        }
        else
        {
            // Move sideways
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);
        
            // Handle Direction Change
            if (theRB.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if(theRB.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
        }
        
        
        // Ground Check
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);
        if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && abilities.canDoubleJump)))
        {
            if (isOnGround)
            {
                canDoubleJump = true;
            }
            else
            {
                canDoubleJump = false;
                anim.SetTrigger("doubleJump");
            }
            // Jump
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }

        // Shooting
        if (Input.GetButtonDown("Fire1"))
        {
            if (standing.activeSelf)
            {
                Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir =
                                new Vector2(transform.localScale.x, 0f);
                            anim.SetTrigger("shotFired");
            }
            else if (ball.activeSelf && abilities.canDropBomp)
            {
                Instantiate(bomb, bombPoint.position, bombPoint.rotation);
            }
        }

        // Ball Mode
        if (!ball.activeSelf)
        {
            if (Input.GetAxisRaw("Vertical") < -0.9f && abilities.canBecomeBall)
            {
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    ball.SetActive(true);
                    standing.SetActive(false);
                }
            }
            else
            {
                ballCounter = waitToBall;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") > 0.9f)
            {
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    ball.SetActive(false);
                    standing.SetActive(true);
                }
            }
            else
            {
                ballCounter = waitToBall;
            }
        }




        // Animation Triggers
        if (standing.activeSelf)
        {
            anim.SetBool("isOnGround", isOnGround);
            anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
        }

        if (ball.activeSelf)
        {
            ballAnim.SetFloat("speed",Mathf.Abs(theRB.velocity.x));
        }
        
    }

    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position, transform.rotation);
        image.sprite = theSR.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;
        Destroy(image.gameObject, afterImageLifetime);

        afterImageCounter = timeBetweenAfterImages;
    }
}
