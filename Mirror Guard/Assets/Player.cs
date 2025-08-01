using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //movement variables: 
    public Rigidbody2D PlayerRB;
    public float MoveDir;
    public float MoveSpd;
    //Jumping
    public float JumpForce;
    public float MaxJumpTime = 0.3f;
    public float JumpHoldForce = 9f;
    private float JumpTimeCounter;
    private bool IsJumping;
    //ground and wall checking
    public bool IsGrounded;
    public Transform GroundCheck;
    public Transform LeftWallCheck;
    public Transform RightWallCheck;
    public LayerMask GroundLayer;
    public LayerMask WallLayer;
    public LayerMask GroundWallLayer;
    //Falling Faster Over Time
    public float FallAcceleration = 2f;
    private float FallTime;
    //Jump Buffering/Coyote Time
    public float CoyoteTime = 0.2f;
    private float CoyoteTimer;
    public float JumpBufferTime = 0.15f;
    private float JumpBufferTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveSpd = 5;
        JumpForce = 3;
        JumpHoldForce = 9;
    }

    // Update is called once per frame
    void Update()
    {
        // Set Horizontal Movement
        if (Input.GetKey(KeyCode.A) == true)
        {
            MoveDir = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            MoveDir = 1;
        }
        else
        {
            MoveDir = 0;
        }
    if (Input.GetKeyDown(KeyCode.Space))
    {
        JumpBufferTimer = JumpBufferTime;
    }

    //if (Input.GetKey(KeyCode.Space) && IsJumping)
    //{
       // if (JumpTimeCounter > 0)
       // {
       //     PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, JumpHoldForce);
       //     JumpTimeCounter -= Time.deltaTime;
       // }
      //  else
      //  {
       //     IsJumping = false;
      //  }
  //  }

    if (Input.GetKeyUp(KeyCode.Space))
    {
        IsJumping = false;
    }
    }
    void FixedUpdate()
    {
        //Are you touching ground?
        IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, GroundLayer);
        if (IsGrounded)
{
    CoyoteTimer = CoyoteTime;
}
else
{
    CoyoteTimer -= Time.fixedDeltaTime;
}
        // Apply Horizontal Movement
        PlayerRB.linearVelocity = new Vector2(MoveDir * MoveSpd, PlayerRB.linearVelocity.y);
        // Apply Vertical Movement
        if (IsJumping && JumpTimeCounter > 0 && Input.GetKey(KeyCode.Space))
{
    PlayerRB.linearVelocity += Vector2.up * JumpHoldForce * Time.fixedDeltaTime;
    JumpTimeCounter -= Time.fixedDeltaTime;
}
if (JumpBufferTimer > 0 && CoyoteTimer > 0)
{
    PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, JumpForce);
    IsJumping = true;
    JumpTimeCounter = MaxJumpTime;

    JumpBufferTimer = 0;
    CoyoteTimer = 0;
}
        //Falling Acceleration
        if (PlayerRB.linearVelocity.y < 0)
        {
            FallTime += Time.fixedDeltaTime; // increase fall duration
            FallTime = Mathf.Min(FallTime, 1.5f);//terminalvelocity
            PlayerRB.linearVelocity += Vector2.down * FallAcceleration * FallTime * Time.fixedDeltaTime;
        }
        else
        {
            FallTime = 0; // reset when not falling
        }
    }
}
