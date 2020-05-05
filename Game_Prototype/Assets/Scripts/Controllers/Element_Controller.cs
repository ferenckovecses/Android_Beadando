using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element_Controller : MonoBehaviour
{
	public List<Elements> elementList;
	int[,] elementMatrix;

    // Start is called before the first frame update
    void Start()
    {
        elementMatrix = new int[6,6];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
