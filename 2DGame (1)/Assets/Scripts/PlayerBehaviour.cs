using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Physics")]
    private Rigidbody rb;
    public float jumpForce;
    public float speed;
    public float SpeedWalk;
    public float SpeedRun;
    public bool jump;
    public bool doubleJump;
    public bool run;
    public bool isDamaged;
    public float axis;

    // Death by explosion
    public static bool alive;
    public int framesCounter;

    [Header("GroundCheker")]
    public Transform groundCheker;
    public Vector3 groundSize;
    public bool isGrounded;
    public LayerMask groundMask;
    [Header("WallCheker")]
    public Transform wallCheker;
    public Vector3 wallSize;
    public bool isTouchingWall;
    public LayerMask wallMask;
    [Header("CeilingCheker")]
    public Transform ceilingCheker;
    public Vector3 ceilingSize;
    public bool isTouchingCeil;
    public LayerMask ceilingMask;
    [Header("Graphics")]
    public Transform meshTransform;
    public Animator anim;
    public Transform graphicsCheker;
    public bool facingRight; // Será true siempre que mire a la derecha 

    [Header("Interaction")]
    public bool gotTheKey;

    // Init
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        facingRight = true; // el código tiene que corresponderse con el feedback visual
        speed = SpeedWalk;
        isTouchingCeil = false;
        isTouchingWall = false;
        isGrounded = true;

        alive = true;
        framesCounter = 0;
        isDamaged = false;
        gotTheKey = false;
    }

    void Update()
    {
        if (alive)
        {
            // Movement
            axis = Input.GetAxis("Horizontal") * speed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = SpeedRun;
            }
            else speed = SpeedWalk;

            // Jump 
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    if (!jump) jump = true;
                }
                else
                {
                    if (!doubleJump)
                    {
                        doubleJump = true;
                        jump = true;
                        anim.SetTrigger("doubleJump");
                    }
                }
            }

            if (isDamaged) anim.SetTrigger("isDamaged");

            if (axis > 0 && !facingRight) Flip();
            else if (axis < 0 && facingRight) Flip();

            //Damage


            //Animation
            anim.SetBool("IsGroundedAnimator", isGrounded);
            anim.SetFloat("Speed", Mathf.Abs(axis));
            anim.SetBool("doubleJump", doubleJump);
            anim.SetBool("isDamaged", isDamaged);
        }
        else
        {
            anim.SetBool("isDamaged", isDamaged);
            Debug.Log("Ouch!");
            isDamaged = true;
            framesCounter++;
            if (framesCounter >= 120)
            {
                Application.LoadLevel(Application.loadedLevelName);
                Reload();
            }
        }
    }

    void FixedUpdate()
    {
        // Ground collision
        Collider[] hitColliders = Physics.OverlapBox(groundCheker.position, groundSize, Quaternion.identity, groundMask);
        if (hitColliders.Length != 0) isGrounded = true;
        else isGrounded = false;

        // Wall collision
        hitColliders = Physics.OverlapBox(wallCheker.position, wallSize, Quaternion.identity, wallMask);
        if (hitColliders.Length != 0) isTouchingWall = true;
        else isTouchingWall = false;

        // Ceiling collision
        hitColliders = Physics.OverlapBox(ceilingCheker.position, ceilingSize, Quaternion.identity, ceilingMask);
        if (hitColliders.Length != 0) isTouchingCeil = true;
        else isTouchingCeil = false;

        /* KEY // Key collision
        hitColliders = Physics.OverlapBox(keyCheker.position, keySize, Quaternion.identity, keyMask);
        if (hitColliders.Length != 0) gotTheKey = true;

        // Chest collision
        hitColliders = Physics.OverlapBox(chestCheker.position, chestSize, Quaternion.identity, chestMask);
        if (hitColliders.Length != 0 && gotTheKey == true);*/

        if (isGrounded)
        {
            if (doubleJump) doubleJump = false;
        }

        if (isTouchingWall)
        {
            if (facingRight && axis > 0) axis = 0;
            else if (!facingRight && axis < 0) axis = 0;
        }

        if (jump)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            jump = false;
        }

        rb.velocity = new Vector3(axis, rb.velocity.y, 0);
    }

    // Método para que giremos el personaje: escalar en x por -1
    void Flip()
    {
        Vector3 newScale = meshTransform.localScale; // Una variable local como esta, que no es global, sólo puede usarse en el método Flip, fuera no
        newScale.x *= -1;
        meshTransform.localScale = newScale;

        facingRight = !facingRight;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (groundCheker != null) Gizmos.DrawWireCube(groundCheker.position, groundSize * 2);
        Gizmos.color = Color.blue;
        if (wallCheker != null) Gizmos.DrawWireCube(wallCheker.position, wallSize * 2);
        Gizmos.color = Color.red;
        if (ceilingCheker != null) Gizmos.DrawWireCube(ceilingCheker.position, ceilingSize * 2);
    }

    public static void SetAlive(bool isAlive)
    {
        alive = isAlive;
    }

    // Start again
    void Reload()
    {
        alive = true;
        framesCounter = 0;
        isDamaged = false;
        gotTheKey = false;
    }
}
