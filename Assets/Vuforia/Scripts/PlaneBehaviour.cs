using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehaviour : MonoBehaviour
{
    private Vector3 touch;
    public GameObject moveTarget;
    public Vuforia.PlaneFinderBehaviour plane;

    private bool first=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0 && first)
        {

            touch = Input.GetTouch(0).position;

            //Debug - print position of latest touch
            Debug.Log(touch);
            first = false;
            
            Instantiate(moveTarget, transform.position, transform.rotation);

        }
    }
}
