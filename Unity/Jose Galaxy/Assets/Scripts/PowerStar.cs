using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStar : MonoBehaviour
{
    // Start is called before the first frame update
    public int RotationSpeed = 50;

    // Update is called once per frame
    void Update()
    {
        rotate();

    }

    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerController>().Score += 10;
        Destroy(transform.gameObject);
    }

    void rotate()
    {
        transform.Rotate(new Vector3(0, RotationSpeed, 0) * Time.deltaTime);
    }

}
