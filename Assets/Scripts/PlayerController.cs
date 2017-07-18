using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D myRigidBody;
    private Animator myAnim;

    public float moveSpeed;
    public float jumpSpeed;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public bool isGrounded;

    public Vector3 respawnPosition;

    public LevelManager theLevelManager;

    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        respawnPosition = transform.position;
        theLevelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (Input.GetAxisRaw("Horizontal") > 0f) //move to the right
        {
            myRigidBody.velocity = new Vector3(moveSpeed, myRigidBody.velocity.y, 0f);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f) //move to the left
        {
            myRigidBody.velocity = new Vector3(-moveSpeed, myRigidBody.velocity.y, 0f);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else //standing still (player releases movement keys
        {
            myRigidBody.velocity = new Vector3(0f, myRigidBody.velocity.y, 0f);
        }

        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            myRigidBody.velocity = new Vector3(myRigidBody.velocity.x, jumpSpeed, 0f);
        }

        myAnim.SetFloat("Speed", Mathf.Abs(myRigidBody.velocity.x));
        myAnim.SetBool("Grounded", isGrounded);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "KillPlane")
        {
            //gameObject.SetActive(false);

            //transform.position = respawnPosition;
            theLevelManager.Respawn();
        }

        if(other.tag == "Checkpoint")
        {
            respawnPosition = other.transform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = other.transform;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "MovingPlatform")
        {
            transform.parent = null;
        }
    }

}
