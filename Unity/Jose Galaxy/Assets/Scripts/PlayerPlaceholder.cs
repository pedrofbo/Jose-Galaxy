using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerPlaceholder : MonoBehaviour
{
 
    public GameObject Player;
    public GameObject Planet;

	public int planetClass;
 
    // Update is called once per frame
    void Update()
    {
        //SMOOTH
		
        //POSITION
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, 0.1f);
 
        Vector3 gravDirection;
		if (planetClass == 1)
		{
			gravDirection = (transform.position - Planet.transform.position).normalized;
			
		}
        else
		{
			gravDirection = (Planet.transform.up).normalized;
		}
 
        //ROTATION
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.1f);
 
    }
 
 
    public void NewPlanet(GameObject newPlanet) {
 
        Planet = newPlanet;
    }
 
}