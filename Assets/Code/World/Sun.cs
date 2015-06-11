using UnityEngine;

public class Sun : MonoBehaviour
{

    public float DayLength = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float rotationSpeed = Time.deltaTime / DayLength;

        transform.Rotate(0, rotationSpeed, 0);

	    float y = transform.rotation.eulerAngles.y;

	    if (y > 90 && y < 270)
	    {
	        GetComponent<Light>().enabled = false;
	    }
	    else
        {
            GetComponent<Light>().enabled = true;
	    }
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(1, 0, 0) * Time.deltaTime);
	}
}
