using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    // Ground
    [Header("State")]
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool wasGrounded;
    [HideInInspector] public bool justGOTGrounded;
    [HideInInspector] public bool justNOTGrounded;
    [HideInInspector] public bool isFalling;
    public int numHitsGround;

    // Ceiling
    [HideInInspector] public bool isTouchingCeiling;
    [HideInInspector] public bool wasTouchingCeiling;
    [HideInInspector] public bool justTouchedCeiling;
    [HideInInspector] public bool justDidntTouchCeiling;

    // Sides / walls
    [HideInInspector] public bool isTouchingWall;
    [HideInInspector] public bool wasTouchingWall;

    public Transform meshTransform;
    public bool facingRight;

    // Physics -----------------------
    private Rigidbody2D rb;
    public float axis;
    public float speed;
    public float SpeedRun;
    public float SpeedWalk;

    public bool jump;
    public bool doubleJump;
    public float jumpForce;


    //--------------------------------

    [Header("Permissions")]
    public bool checkGround;
    public bool checkCeiling;

    //--------------------------------

    [Header("Filter Properties")]
    public ContactFilter2D groundFilter; // usamos este mismo filter para ceiling, ground y walls
    public int maxHits; // cuántos colliders quiero inicializar máximo

    //--------------------------------

    [Header("Bottom box")]
    public Vector2 groundBoxPos; // para wall tendré que crear estos pero análogos en wall
    public Vector2 groundBoxSize;

    [Header("Upper box")]
    public Vector2 ceilingBoxPos;
    public Vector2 ceilingBoxSize;

    [Header("Side box")]
    public Vector2 wallBoxPos;
    public Vector2 wallBoxSize;

    //--------------------------------

    public Vector2 pos; // Player's position

    private void Start()
    {
        ResetState();
        pos = this.transform.position; // Player's position
        rb = GetComponent<Rigidbody2D>();
    }

    public void MyStart()
    {

    }

    void ResetState()
    {
        // Ground
        wasGrounded = isGrounded;
        isGrounded = false;
        justGOTGrounded = false;
        justNOTGrounded = false;
        isFalling = true;
        checkGround = true;

        // Ceiling
        wasTouchingCeiling = isTouchingCeiling;
        isTouchingCeiling = false;
        justTouchedCeiling = false;
        justDidntTouchCeiling = false;
        checkCeiling = true;

        // Sides
        isTouchingWall = false;
        wasTouchingWall = false;

        facingRight = true; // Será true siempre que mire a la derecha

        SpeedRun = speed * 2;
        speed = SpeedWalk;

        jump = false;
        doubleJump = false;

        numHitsGround = 0;
    }

    void Update()
    {
        // Movement
        axis = Input.GetAxis("Horizontal") * speed;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = SpeedRun;
        }
        else speed = SpeedWalk;

        if(axis > 0 && !facingRight) Flip();
        else if(axis < 0 && facingRight) Flip();

        // Jump 
        if(Input.GetButtonDown("Jump"))
        {
            if(isGrounded)
            {
                if(!jump) jump = true;
            }
            else
            {
                if(!doubleJump)
                {
                    doubleJump = true;
                    jump = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        ResetState();
        if(checkGround) GroundDetection();
        if(checkCeiling) CeilingDetection();

        if(isTouchingWall)
        {
            if(facingRight && axis > 0) axis = 0;
            else if(!facingRight && axis < 0) axis = 0;
        }

        if(jump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jump = false;
        }

        rb.velocity = new Vector2(axis, rb.velocity.y);
    }

    public void MyFixedUpdate()
    {

    }

    public void Flip()
    {
        Vector2 newScale = meshTransform.localScale;
        newScale.x *= -1;
        meshTransform.localScale = newScale;

        facingRight = !facingRight;
    }

    void GroundDetection()
    {
        Vector2 pos = this.transform.position;
        Collider2D[] results = new Collider2D[maxHits];
        //La declaro en la clase para que se pueda acceder: Vector2 pos = this.transform.position; // Player's position
        numHitsGround = Physics2D.OverlapBox(pos + groundBoxPos, groundBoxSize, 0, groundFilter, results);

        if(numHitsGround > 0)
        {
            isGrounded = true;
        }

        if(isGrounded) isFalling = false;
        if(isGrounded && !wasGrounded) justGOTGrounded = true;
        if(isGrounded && wasGrounded) justNOTGrounded = true;

        if(justGOTGrounded) Debug.Log("Just got grounded");
        if(justNOTGrounded) Debug.Log("Just not grounded");
    }

    void CeilingDetection()
    {
        Collider2D[] ceilingResults = new Collider2D[maxHits];
        int numHitsCeiling = Physics2D.OverlapBox(pos + ceilingBoxPos, ceilingBoxSize, 0, groundFilter, ceilingResults);

        if(numHitsCeiling > 0)
        {
            isTouchingCeiling = true;
        }

        if(isTouchingCeiling && !wasTouchingCeiling) justTouchedCeiling = true;
        if(isTouchingCeiling && wasTouchingCeiling) justDidntTouchCeiling = true;

        if(justTouchedCeiling) Debug.Log("Just touched the ceiling");
        if(justDidntTouchCeiling) Debug.Log("Just didn't touch the ceiling");
    }

    void WallDetection()
    {
        Collider2D[] wallResults = new Collider2D[maxHits];
        int numHitsWall = Physics2D.OverlapBox(pos + wallBoxPos, wallBoxSize, 0, groundFilter, wallResults);

        if(numHitsWall > 0)
        {
            isTouchingWall = true;
        }
    }

    void Flip(bool face)
    {
        //if(face) sideBoxPos.x = Mathf.Abs(sideBoxPos.ps);
        //else()
    }

    private void OnDrawGizmosSelected() //Aparecerá el gizmo solo cuando lo selecionamos
    {
        Vector2 pos = this.transform.position;

        Gizmos.DrawWireCube(pos + groundBoxPos, groundBoxSize); //el origen debe ser la posición del player, si no se crea en el 0, 0 del mundo
        Gizmos.DrawWireCube(pos + ceilingBoxPos, ceilingBoxSize);
        Gizmos.DrawWireCube(pos + wallBoxPos, wallBoxSize);
    }
}
