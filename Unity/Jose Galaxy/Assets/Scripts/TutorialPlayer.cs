using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{

    public GameObject Planet;
    public GameObject PlayerPlaceholder;
    public PlanetType planetType;

    public Vector3 gravDir;

    public float speed = 4;
    public float JumpHeight = 1.2f;

    public float gravity = 100f;
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



    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        col = GetComponent<CapsuleCollider>();
        animator = gameObject.GetComponentInChildren<Animator>();
        //Planet = GameObject.Find("initialPlatform");
        cam = PlayerPlaceholder.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 input;
        isGrounded = IsGrounded();

        /*if (planetType == planetType.Spherical)
        {
            MoveSphere();
        }
        else
        {
            Move();
        }*/

        Move2();

        //Jump

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // animator?.SetInteger("AnimPlayer", 2);
            rb.AddForce(transform.up * 8000 * JumpHeight * Time.deltaTime);

        }


        //GroundControl

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {

            distanceToGround = hit.distance;
            Groundnormal = hit.normal;

            if (distanceToGround <= 0.5f)
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

        //if (OnGround == false)
        //{
            rb.AddForce(gravityDirection * -gravity);

        //}

        //

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, Groundnormal) * transform.rotation;
        transform.rotation = toRotation;



    }


    //CHANGE PLANET

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform != Planet.transform)
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
        return Physics.CheckCapsule(col.bounds.center,
            new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z),
            col.radius * .8f,
            groundLayers);
    }

    private void Move()
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

    private void Move2()
    {
        //MOVEMENT

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

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
        }
        else
        {
            animator?.SetInteger("AnimPlayer", 0);
        }
    }

}