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
    public BulletController shotToFire;
    public Transform shotPoint;

    private bool isOnGround;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        
        // Ground Check
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, 0.2f, whatIsGround);
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            // Jump
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(shotToFire, shotPoint.position, shotPoint.rotation).moveDir =
                new Vector2(transform.localScale.x, 0f);
            anim.SetTrigger("shotFired");
        }








        anim.SetBool("isOnGround", isOnGround);
        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
    }
}
