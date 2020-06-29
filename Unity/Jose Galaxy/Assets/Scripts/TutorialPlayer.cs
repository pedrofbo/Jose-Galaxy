using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : MonoBehaviour
{

    public GameObject Planet;
    public GameObject PlayerPlaceholder;


    public float speed = 4;
    public float JumpHeight = 1.2f;

    float gravity = 100f;
    public bool OnGround = false;

    public bool isGrounded;


    float distanceToGround;
    Vector3 Groundnormal;

    CapsuleCollider col;
    public LayerMask groundLayers;

    private Animator animator;



    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        col = GetComponent<CapsuleCollider>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input;
        isGrounded = IsGrounded();

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

        //Jump

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // animator?.SetInteger("AnimPlayer", 2);
            rb.AddForce(transform.up * 40000 * JumpHeight * Time.deltaTime);

        }


        //GroundControl

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {

            distanceToGround = hit.distance;
            Groundnormal = hit.normal;

            if (distanceToGround <= 0.4f)
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }


        }


        //GRAVITY and ROTATION

        Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

        if (OnGround == false)
        {
            rb.AddForce(gravDirection * -gravity);

        }

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

            Vector3 gravDirection = (transform.position - Planet.transform.position).normalized;

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
            transform.rotation = toRotation;

            rb.velocity = Vector3.zero;
            rb.AddForce(gravDirection * gravity);


            // PlayerPlaceholder.GetComponent<TutorialPlayerPlaceholder>().NewPlanet(Planet);

        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center,
            new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z),
            col.radius * .8f,
            groundLayers);
    }

}