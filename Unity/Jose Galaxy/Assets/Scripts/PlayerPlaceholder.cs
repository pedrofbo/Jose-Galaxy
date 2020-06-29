using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerPlaceholder : MonoBehaviour
{
 
    public GameObject Player;
    public GameObject Planet;

	public PlanetType planetType;

    float speed;
 
    // Update is called once per frame
    void Update()
    {
        //SMOOTH
		
        //POSITION
        transform.position = Vector3.Lerp(transform.position, Player.transform.position, 0.1f);
        //transform.position = Player.transform.position;

        /*speed = Player.GetComponent<TutorialPlayer>().speed;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
 
        transform.Translate(x, 0, z);
 
        //Local Rotation
 
        if (Input.GetKey(KeyCode.E)) {
 
            transform.Rotate(0, 150 * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
 
            transform.Rotate(0, -150 * Time.deltaTime, 0);
        }*/
 
        Vector3 gravDirection;
		if (planetType == PlanetType.Spherical)
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
 
 
    public void NewPlanet(GameObject newPlanet, PlanetType newPlanetType) {
 
        planetType = newPlanetType;
        Planet = newPlanet;
    }
 
}