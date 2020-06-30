using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public PlanetType planetType;

    private void OnTriggerEnter(Collider collider)
    {
        collider.GetComponent<PlayerController>().planetType = planetType;

    }
}
