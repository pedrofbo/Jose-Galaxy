using UnityEngine;

public enum PlanetType
{
    Spherical = 1,
    Platform = 2,
}

public class Jose : MonoBehaviour
{

    public GameObject planet;
    public PlanetType planetType;

    public float speed = 4;
    public float jumpAmplitude = 1.2f;

    private Animator animator;


    float gravity = 100;
    bool OnGround = false;

    float distanceToGround;
    Vector3 groundNormal;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        planetType = PlanetType.Platform;
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //MOVEMENT
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        if (z != 0)
        {
            animator.SetInteger("AnimPlayer", 1);
        }
        else
        {
            animator.SetInteger("AnimPlayer", 0);
        }

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

        Jump();

        //GroundControl
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {

            distanceToGround = hit.distance;
            groundNormal = hit.normal;

            if (distanceToGround <= 0.05f)
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
            gravityDirection = (transform.position - planet.transform.position).normalized;
        }
        else
        {
            gravityDirection = (planet.transform.up).normalized;
        }

        if (OnGround == false)
        {
            rb.AddForce(gravityDirection * -gravity);
        }

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        transform.rotation = toRotation;
    }


    //CHANGE PLANET
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform != planet.transform && OnGround == false)
        {

            planet = collision.transform.gameObject;

            Vector3 gravityDirection;
            if (planetType == PlanetType.Spherical)
            {
                gravityDirection = (transform.position - planet.transform.position).normalized;
            }
            else
            {
                gravityDirection = (planet.transform.up).normalized;
            }

            Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;
            transform.rotation = toRotation;

            rb.velocity = Vector3.zero;
            rb.AddForce(gravityDirection * gravity);
        }
    }


    private void Jump()
    {
        int jumpScale = 40000;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpScale * jumpAmplitude * Time.deltaTime);
            //animator.SetInteger("AnimPlayer", 2);
        }
    }
}