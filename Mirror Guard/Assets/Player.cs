using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //movement variables: 
    public Rigidbody2D PlayerRB;
    public float MoveDir;
    public float MoveSpd;
    public float DashDuration;
    public float DashSpd = 10;
    public float Dashcooldown;
    public float LastDir;
    public bool LockMovement;
    public float WallClimbTimer;
    public float LockMovementTimer;
    public bool FreezeGravity;
    public bool AirDashUsed;
    //Jumping
    public float JumpForce;
    public float MaxJumpTime = 0.3f;
    public float JumpHoldForce = 9f;
    private float JumpTimeCounter;
    private bool IsJumping;
    //ground and wall checking
    public bool IsGrounded;
    public bool TouchingLeftWall;
    public bool TouchingRightWall;
    public Transform GroundCheck;
    public Transform LeftWallCheck;
    public Transform RightWallCheck;
    public LayerMask GroundLayer;
    public LayerMask WallLayer;
    public LayerMask GroundWallLayer;
    public bool firstwallframe;
    //Falling Faster Over Time
    public float FallAcceleration = 2f;
    public float FallTime;
    public Vector2 WallSlowing;
    public Vector2 fallVelocity;
    //Jump Buffering/Coyote Time
    public float CoyoteTime = 0.2f;
    private float CoyoteTimer;
    public float JumpBufferTime = 0.15f;
    private float JumpBufferTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveSpd = 5;
        JumpForce = 1;
        JumpHoldForce = 20;
        DashSpd = 15;
    }

    // Update is called once per frame
    void Update()
    {
        // Set Horizontal Movement
        if (LockMovement == false)
        {
            if (Input.GetKey(KeyCode.A) == true)
            {
                MoveDir = -1;
                LastDir = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                MoveDir = 1;
                LastDir = 1;
            }
            else
            {
                MoveDir = 0;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (Dashcooldown == 0)
                {
                    if (AirDashUsed==false)
                    {
                        if (IsGrounded == false)
                        {
                            AirDashUsed = true;
                        }
                        DashDuration = 10;
                    }
                }
            }
        }
        if (TouchingRightWall == false && TouchingLeftWall == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpBufferTimer = JumpBufferTime;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                IsJumping = false;
            }
        }
        else
        {
            if (LockMovement == false)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    LockMovement = true;
                    WallClimbTimer = 10;
                }
            }

        }
    }
    void FixedUpdate()
    {
        //Wall Jumping
        if (WallClimbTimer > 0)
        {
                if (WallClimbTimer > 5)
                {
                    if (TouchingLeftWall)
                    {
                        PlayerRB.linearVelocity = new Vector2(5, 5);
                    }
                    else if (TouchingRightWall)
                    {
                        PlayerRB.linearVelocity = new Vector2(-5, 5);
                    }
                    Debug.Log(PlayerRB.linearVelocity);
                }
                else
                {
                    PlayerRB.linearVelocity = new Vector2(0, 5);
                }
                WallClimbTimer -= 1;
                if (WallClimbTimer == 0)
                {
                LockMovementTimer = 4;
                }
        }
        if (LockMovementTimer > 0)
        {
            LockMovementTimer -= 1;
            if (LockMovementTimer == 0)
            {
                LockMovement = false;
            }
        }
        //Are you touching ground?
            IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, GroundLayer);
        if (IsGrounded == false)
        {
                     IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, GroundWallLayer);       
            }
        if (IsGrounded)
            {
            AirDashUsed = false;
                CoyoteTimer = CoyoteTime;
            }
            else
            {
                CoyoteTimer -= Time.fixedDeltaTime;
            }
        //are you touching a left wall?
        TouchingLeftWall = Physics2D.OverlapCircle(LeftWallCheck.position, 0.1f, WallLayer);
        if (TouchingLeftWall == false)
        {
            TouchingLeftWall = Physics2D.OverlapCircle(LeftWallCheck.position, 0.1f, GroundWallLayer);
        }
        if (TouchingLeftWall)
        {
            LastDir = 1;
        }
        else
        {

        }
        //are you touching a right wall
        TouchingRightWall = Physics2D.OverlapCircle(RightWallCheck.position, 0.1f, WallLayer);
        if (TouchingRightWall == false)
        {
            TouchingRightWall = Physics2D.OverlapCircle(RightWallCheck.position, 0.1f, GroundWallLayer);
        }
        if (TouchingRightWall)
        {
            LastDir = -1;
        }
        else
        {

        }
        if (TouchingLeftWall || TouchingRightWall)
        {
            AirDashUsed = false;
            //if dashing into wall resume usual movement
            if (DashDuration > 0)
            {
                if (firstwallframe == false)
                {
                    DashDuration = 0;
                    LockMovement = false;
                    FreezeGravity = false;
                }
            }
            //slide down wall variables
            WallSlowing = new Vector2(0, 10);
            FallAcceleration = 1;
            if (firstwallframe == false)
            {
                FallTime = 0;
                FallAcceleration = 0;
                fallVelocity = new Vector2(0,0);
            }
            firstwallframe = true;
        }
        else
        {
            firstwallframe = false;
            //no sliding variables
            WallSlowing = new Vector2(0, 0);
            FallAcceleration = 2;
        }

        // Apply Horizontal Movement
        if (LockMovement == false)
        {
            PlayerRB.linearVelocity = new Vector2(MoveDir * MoveSpd, PlayerRB.linearVelocity.y);
        }
        if (FreezeGravity == true)
        {
            PlayerRB.gravityScale = 0f;
            FallAcceleration = 0;
            fallVelocity = new Vector2(0, 0);
        }
        else
        {
            PlayerRB.gravityScale = 1f;
        }
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
        //Dashing
        if (DashDuration > 0)
        {
            FreezeGravity = true;
            LockMovement = true;
            PlayerRB.linearVelocity = new Vector2(LastDir * DashSpd, PlayerRB.linearVelocity.y);
            DashDuration -= 1;
            if (DashDuration == 0)
            {
                Dashcooldown = 20;
                FreezeGravity = false;
                LockMovement = false;
            }
        }
        if (Dashcooldown > 0)
        {
            Dashcooldown -= 1;
        }
        //Falling Acceleration
        if (FreezeGravity == false)
        {
            if (PlayerRB.linearVelocity.y < 0)
            {
                FallTime += Time.fixedDeltaTime; // increase fall duration
                FallTime = Mathf.Min(FallTime, 1.5f);//terminalvelocity
                fallVelocity = Vector2.down * FallAcceleration * FallTime;
                fallVelocity += WallSlowing;
                PlayerRB.linearVelocity += fallVelocity * Time.fixedDeltaTime;
            }
            else
            {
                FallTime = 0; // reset when not falling
            }
        }
        if (JumpBufferTimer > 0)
        {
            JumpBufferTimer -= Time.fixedDeltaTime;
        }

    }
}
