using UnityEngine;

public class Star : MonoBehaviour
{

    public StarType StarType;

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    void OnTriggerEnter(Collider other)
    {
        // we are dividing by two because this method is called two times before the star is destroyed
        other.GetComponent<PlayerController>().Score += GetPoints(StarType) / 2;
        Destroy(transform.gameObject);
    }

    void Rotate()
    {
        int rotationSpeed = 150;
        transform.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);
    }

    int GetPoints(StarType starType)
    {
        var point = 0;
        switch (starType)
        {
            case StarType.Blue:
                point = 10;
                break;
            case StarType.Green:
                point = 20;
                break;
            case StarType.Red:
                point = 30;
                break;
            case StarType.Power:
                point = 50;
                break;
            case StarType.Grand:
                point = 100;
                break;
        }
        return point;
    }

}
