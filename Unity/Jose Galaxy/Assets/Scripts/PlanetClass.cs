using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetClass : MonoBehaviour {

	public int planetClass;

	private void OnTriggerEnter(Collider other)
	{
		other.GetComponent<Jose>().planetClass = this.planetClass;
	}
}
