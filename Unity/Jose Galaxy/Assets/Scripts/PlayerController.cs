using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float jumpForce = 1.2f;
    public PlanetType planetType;
    public float rotationSpeed = 10f;

    private GameObject planet;
    private Animator animator;
    private float gravity = 1000f;
    private bool OnGround = false;
    private float distanceToGround;
    private Vector3 groundNormalVector;

    //movement variables
    private Quaternion targetRotation;
    private Vector2 input;
    private float angle;
    private Transform cam;

    private Rigidbody rb;

    // Start is called before the first frame update
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        planetType = PlanetType.Platform;
        animator = gameObject.GetComponentInChildren<Animator>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    public void Update()
    {
        Move2();

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

        rb.AddForce(gravityDirection * -gravity);
        // if (!IsGrounded())
        // {

        // }
    }

    //CHANGE PLANET
    private void OnTriggerEnter(Collider collision)
    {
        planet = collision.transform.gameObject;

        if (collision.transform != planet?.transform)
        {
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

    private void Move()
    {
        //get inputs
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(input.x) < 1 && Mathf.Abs(input.y) < 1) return;

        //calculate direction
        angle = Mathf.Atan2(input.x, input.y);
        angle = Mathf.Rad2Deg * angle;
        angle += cam.eulerAngles.y;

        //rotate
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (input != Vector2.zero)
        {
            animator.SetInteger("AnimPlayer", 1);
        }
        else
        {
            animator.SetInteger("AnimPlayer", 0);
        }

        //transform.Translate(x, 0, z);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void Move2()
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

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, groundNormalVector) * transform.rotation;
        transform.rotation = toRotation;
    }
}