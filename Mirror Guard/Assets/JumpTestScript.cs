using UnityEngine;

public class JumpTestScript : MonoBehaviour
{
    public Vector2 JumpVar;
    public bool Grounded;
    public float JumpTimer;
    public Rigidbody2D JumpTestRB;
    public LayerMask GroundLayer;
    public Transform GroundCheck;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, GroundLayer);
        if (Grounded)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                JumpVar = new Vector2(0, 3);
                JumpTimer = 20;
            }
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            JumpTimer = 0;
            //JumpTestRB.gravityScale = 1;
        }
    }
    void FixedUpdate()
    {
        if (JumpTimer > 0)
        {
            //JumpTestRB.gravityScale = 0;
            JumpTestRB.linearVelocity = JumpVar;
            JumpTimer -= 1;
        }
        else
        {



        }       
    }
}

