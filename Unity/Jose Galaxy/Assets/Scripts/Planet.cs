using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetClass : MonoBehaviour
{

    public int planetClass;

    private void OnTriggerEnter(Collider collider)
    {
        collider.GetComponent<Player>().planetClass = this.planetClass;
    }
}
