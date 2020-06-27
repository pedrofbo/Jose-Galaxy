using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float jumpForce = 1.2f;
    public PlanetType planetType;

    private GameObject planet;
    private Animator animator;
    private float gravity = 100f;
    private bool OnGround = false;
    private float distanceToGround;
    private Vector3 groundNormalVector;

    private Rigidbody rb;

    // Start is called before the first frame update
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        planetType = PlanetType.Platform;
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    public void Update()
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

        if (!IsGrounded())
        {
            rb.AddForce(gravityDirection * -gravity);
        }

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, groundNormalVector) * transform.rotation;
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
        int jumpAmplitude = 5;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetInteger("AnimPlayer", 2);
            rb.AddForce(Vector3.up * jumpAmplitude * jumpForce, ForceMode.Impulse);

        }
    }

    private bool IsGrounded()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {

            var distanceToGround = hit.distance;
            var groundNormalVector = hit.normal;

            return distanceToGround <= 0.2f;
        }
        else
        {
            return false;
        }
    }
}