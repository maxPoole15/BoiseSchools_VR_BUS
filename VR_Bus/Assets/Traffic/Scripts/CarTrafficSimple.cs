using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarTrafficSimple : MonoBehaviour
{
    [Header("Agent Attributes")]
    public NavMeshAgent agent;
    [Tooltip("The targets in the scene this car will travel to. Different from the start teleport.\nTheoretically you should only need two, but put as many as you want!\nMore targets = more complex road.")]
    public GameObject[] Checkpoints;
    [Tooltip("The location where the car will teleport back to once final destination has been reached.\nKeep out of sight of player!")]
    public Transform startTransform;
    [Tooltip("The first target you want this car to go after")]
    public int startingTargetFromArray;
    [Tooltip("Useful to see what target this car is after at the moment!")]
    public int currentDestination;

    [Header("Traffic Attributes")]
    public IntersectionSimple intersectionScript;

    [HideInInspector]
    public bool canGo = true;

    private float speedVariance;
    private float distanceToTarget;

    // Start is called before the first frame update
    void Start()
    {
        agent.destination = Checkpoints[startingTargetFromArray].transform.position;
        currentDestination = startingTargetFromArray;
        speedVariance = Random.Range(0.5f, 5f);
        agent.speed += speedVariance;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector3.Distance(agent.transform.position, agent.destination);
        if(canGo) ChangeTarget(distanceToTarget);
    }

    void ChangeTarget(float d)
    {
        GameObject curDest = Checkpoints[currentDestination];

        if (d <= 3f)
        {
            //checks to see if the current target is the last one in the array for this car
            //if so, warp to start and set target at index 0 as the new destination
            if (curDest == Checkpoints[Checkpoints.Length - 1])
            {
                agent.Warp(startTransform.position);
                agent.destination = Checkpoints[0].transform.position; 
                currentDestination = 0;
            }
            else
            {
                //otherwise just carry on goin through the array
                agent.destination = Checkpoints[currentDestination + 1].transform.position;
                currentDestination += 1;
            }
        }
        
    }
}
