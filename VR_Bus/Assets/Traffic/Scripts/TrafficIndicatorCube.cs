using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrafficIndicatorCube : MonoBehaviour
{
    private NavMeshAgent parentCar;
    public IntersectionSimple intersectionScript;
    public bool northSouth = false;
    private string stateUsed;
    private bool go = true;
    private void Start()
    {
        parentCar = GetComponentInParent<NavMeshAgent>();
        if (northSouth) stateUsed = "NorthSouthRoute";
        else stateUsed = "EastWestRoute";
    }
    private void Update()
    {
        switch (stateUsed)
        {
            case "NorthSouthRoute":
                if (intersectionScript.northSouthState == IntersectionSimple.State.Go) go = true;
                else go = false;
                break;
            case "EastWestRoute":
                if (intersectionScript.eastWestState == IntersectionSimple.State.Go) go = true;
                else go = false;
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "car":
                parentCar.GetComponent<CarTrafficSimple>().canGo = false;
                parentCar.velocity = Vector3.zero;
                parentCar.isStopped = true;
                break;
            case "stopLine":
                if(!go)
                {
                    parentCar.GetComponent<CarTrafficSimple>().canGo = false;
                    parentCar.velocity = Vector3.zero;
                    parentCar.isStopped = true;
                }
                break;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "stopLine")
        {
            if (go)
            {
                parentCar.GetComponent<CarTrafficSimple>().canGo = true;
                parentCar.isStopped = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "car":
                parentCar.GetComponent<CarTrafficSimple>().canGo = true;
                parentCar.isStopped = false;
                break;
            case "stopLine":
                parentCar.GetComponent<CarTrafficSimple>().canGo = true;
                parentCar.isStopped = false;
                break;
        }
    }
}
