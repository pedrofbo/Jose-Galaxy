using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int Score = 0;

    public Text ScoreText;
    public GameObject Planet;
    public GameObject PlayerPlaceholder;
    public PlanetType planetType;

    public Vector3 gravDir;

    public float speed = 8f;
    public float runSpeed = 20f;
    float moveSpeed = 8f;
    public float JumpHeight = 5f;

    public float gravity = 1000f;
    public bool OnGround = false;

    public bool isGrounded;


    float distanceToGround;
    Vector3 Groundnormal;

    CapsuleCollider col;
    public LayerMask groundLayers;

    private Animator animator;

    private Quaternion targetRotation;
    private float angle;
    private Transform cam;
    public float rotationSpeed = 10f;

    public float floatJumpForce = 0.5f;


    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = gameObject.GetComponentInChildren<Animator>();

        col = GetComponent<CapsuleCollider>();

        cam = PlayerPlaceholder.transform;

        SetScoreText();
    }

    // Update is called once per frame
    void Update()
    {

        SimpleMove();

        Jump();

        SetScoreText();

        CheckRestart();

        //GroundControl

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {

            distanceToGround = hit.distance;
            Groundnormal = hit.normal;

            if (distanceToGround <= 1f)
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }


        }

        //GRAVITY and ROTATION
        Vector3 gravityDirection;
        if (planetType == PlanetType.Spherical)
        {
            gravityDirection = (transform.position - Planet.transform.position).normalized;
        }
        else
        {
            gravityDirection = (Planet.transform.up).normalized;
        }

        gravDir = gravityDirection.normalized;

        if (!Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(gravityDirection * -gravity);
        }

        //
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;



    }

    //CHANGE PLANET
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform != Planet.transform && collision.GetComponent<Planet>())
        {
            Planet = collision.transform.gameObject;

            Vector3 gravityDirection;
            if (planetType == PlanetType.Spherical)
            {
                gravityDirection = (transform.position - Planet.transform.position).normalized;
            }
            else
            {
                gravityDirection = (Planet.transform.up).normalized;
            }

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;
            transform.rotation = toRotation;

            rb.velocity = Vector3.zero;
            rb.AddForce(gravityDirection * gravity);


            PlayerPlaceholder.GetComponent<PlayerPlaceholder>().NewPlanet(Planet, planetType);

        }
    }

    private bool IsGrounded()
    {
        // return Physics.CheckCapsule(col.bounds.center,
        //     new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z),
        //     col.radius * .8f,
        //     groundLayers);
        return true;
    }


    #region Move

    private void ComplexMove()
    {
        Vector2 input;
        //get inputs
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (input.x != 0 || input.y != 0)
        {
            animator?.SetInteger("AnimPlayer", 1);
        }
        else
        {
            animator?.SetInteger("AnimPlayer", 0);
        }

        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1) return;

        //calculate direction
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.localEulerAngles.y;

        //rotate
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (input != Vector2.zero)
        {
            animator?.SetInteger("AnimPlayer", 1);
        }
        else
        {
            animator?.SetInteger("AnimPlayer", 0);
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
        //transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void SimpleMove()
    {
        //MOVEMENT

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        transform.Translate(x, 0, z);

        //Local Rotation

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, 150 * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {

            transform.Rotate(0, -150 * Time.deltaTime, 0);
        }


        if (x != 0 || z != 0)
        {
            animator?.SetInteger("AnimPlayer", 1);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator?.SetBool("is_running", true);
                moveSpeed = runSpeed;
            }
            else
            {
                animator?.SetBool("is_running", false);
                moveSpeed = speed;
            }
        }
        else
        {
            animator?.SetInteger("AnimPlayer", 0);
        }
    }
    #endregion

    #region Jump
    private void Jump()
    {
        FlyJump();
        JumpAnimation();
        SimpleJump();
    }

    private void FlyJump()
    {
        float addJumpForce = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            addJumpForce = floatJumpForce;
            animator?.SetBool("jump_init", true);
        }

        transform.Translate(0, addJumpForce, 0);
    }

    private void SimpleJump()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            animator?.SetBool("jump_init", true);
            rb.AddForce(transform.up * 1000 * JumpHeight, ForceMode.Impulse);
            animator?.SetBool("jump_init", true);
        }
    }

    private void JumpAnimation()
    {
        // animation
        if (OnGround)
        {
            animator?.SetBool("is_in_air", false);
        }
        else
        {
            animator?.SetBool("is_in_air", true);
            animator?.SetBool("jump_init", false);
        }
    }

    #endregion

    #region UI
    private void SetScoreText()
    {
        ScoreText.text = "Points: " + Score.ToString();
    }
    #endregion

    #region Game Life Cycle
    private void CheckRestart()
    {
        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}