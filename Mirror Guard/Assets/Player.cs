using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject trail;
    public float trailtimer;
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
    public float JumpRampTimer;
    public float JumpRampDuration = 0.06f;
    public float TargetJumpLaunch;
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
    public float newWalljX;
    public float newWalljY;
    public Vector2 currentvelocity;
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
        trailtimer = 10;
        MoveSpd = 5;
        JumpForce = 5;
        JumpHoldForce = 13;
        DashSpd = 15;
        TargetJumpLaunch = JumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        //trail for testing
        trailtimer -= 1;
        if (trailtimer == 0)
        {
            //Instantiate(trail, transform.position, transform.rotation);
            trailtimer = 10;
        } 
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
                        if (AirDashUsed == false)
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
        if (TouchingLeftWall==false&&TouchingRightWall==false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpBufferTimer = JumpBufferTime;
                JumpRampTimer = JumpRampDuration;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                IsJumping = false;
            }
        }
        else
        {
            if (IsGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    JumpBufferTimer = JumpBufferTime;
                    JumpRampTimer = JumpRampDuration;
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
                    //PlayerRB.linearVelocity = new Vector2(5, 5);
                    currentvelocity = PlayerRB.linearVelocity;
                    newWalljX = Mathf.Lerp(currentvelocity.x, 5, 0.6f);
                    newWalljY = Mathf.Lerp(currentvelocity.y, 5, 0.6f);
                    PlayerRB.linearVelocity = new Vector2(newWalljX, newWalljY);
                }
                else if (TouchingRightWall)
                {
                    currentvelocity = PlayerRB.linearVelocity;
                    newWalljX = Mathf.Lerp(currentvelocity.x, -5, 0.6f);
                    newWalljY = Mathf.Lerp(currentvelocity.y, 5, 0.6f);
                    PlayerRB.linearVelocity = new Vector2(newWalljX, newWalljY);
                }
                Debug.Log(PlayerRB.linearVelocity);
            }
            else
            {
                //PlayerRB.linearVelocity = new Vector2(0, 5);
                    PlayerRB.linearVelocity = new Vector2(0, newWalljY);
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
            if (JumpRampTimer == JumpRampDuration)
            {
                PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, 2);
            }
            if (JumpRampTimer > 0)
                {
                    // t from 0 -> 1 across the ramp (you can use other easing)
                    float t = 1f - (JumpRampTimer / JumpRampDuration);
                    // choose interpolation method: smoothstep or lerp
                    float interp = Mathf.SmoothStep(0f, 1f, t);
                    // current vertical velocity
                    float currentY = PlayerRB.linearVelocity.y;
                    // desired blended vertical velocity
                    float desiredY = Mathf.Lerp(currentY, TargetJumpLaunch, interp);
                    // apply only vertical change, preserve horizontal
                    PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, desiredY);

                    JumpRampTimer -= Time.fixedDeltaTime;
                }
            //PlayerRB.linearVelocity = new Vector2(PlayerRB.linearVelocity.x, 1);
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
