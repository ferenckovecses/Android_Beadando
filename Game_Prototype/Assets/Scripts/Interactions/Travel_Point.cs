using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travel_Point : MonoBehaviour
{
	public Gates gate;
    Travel_Controller travelController;

    // Start is called before the first frame update
    void Start()
    {
        travelController = GameObject.Find("Travel").GetComponent<Travel_Controller>();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
    	if(col.gameObject.name == "Player")
        {
            travelController.TravelThrough(this.gate);
        }
    }
}
