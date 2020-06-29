using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoseController : MonoBehaviour
{
    // Start is called before the first frame update

    public float JumpForce = 1.2f;

    public float Speed = 4.0f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //MOVEMENT

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * Speed;

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

        //Jump

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * 40000 * JumpForce * Time.deltaTime);

        }
    }
}
