using UnityEngine;

public class JumpTestScript : MonoBehaviour
{
    public float moveDir;
    public float moveSpd;
    public Vector2 JumpVar;
    public bool Grounded;
    public float JumpTimer;
    public Rigidbody2D JumpTestRB;
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    public Vector2 currentvelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpd = 3;
    }

    // Update is called once per frame
    void Update()
    {
        currentvelocity = new Vector2(JumpTestRB.linearVelocity.x, JumpTestRB.linearVelocity.y);
        Grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, GroundLayer);
        if (Grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpVar = new Vector2(currentvelocity.x, 3);
                JumpTimer = 20;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            JumpTimer = 0;
            //JumpTestRB.gravityScale = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 0;
        }
    }
    void FixedUpdate()
    {
            JumpTestRB.linearVelocity = new Vector2(moveDir*moveSpd,currentvelocity.y);
            moveDir = 0;
        if (JumpTimer > 0)
        {
            //JumpTestRB.gravityScale = 0;
            JumpTestRB.linearVelocity = JumpVar;
            Debug.Log("Jumped");
            JumpTimer -= 1;
        }
        else
        {



        }


    }
}

