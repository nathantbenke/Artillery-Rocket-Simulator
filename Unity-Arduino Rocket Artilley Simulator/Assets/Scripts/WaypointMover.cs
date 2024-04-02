using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    /// <summary>
    /// Moves Jet objects through flight circuit
    /// 
    /// This script was adapted from MetalStorm Games' Waypoint Path system tutorial:
    /// https://www.youtube.com/watch?v=EwHiMQ3jdHw 
    /// </summary>

    public int startingWaypoint = 0;
    [SerializeField] private Waypoints circuit;

    [SerializeField] private float moveSpeed = 70f;
    [SerializeField] private float rotateSpeed = 100f;

    private Transform currentWaypoint;

    [SerializeField] private float distanceThreshold = 0.1f;

    private Quaternion rotationFinal;

    private Vector3 directionToWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        //Initial Point
        currentWaypoint = circuit.transform.GetChild(startingWaypoint);
        transform.position = currentWaypoint.position;

        currentWaypoint = circuit.GetNextWaypoint(currentWaypoint);
        transform.LookAt(currentWaypoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        //Moves object towards the next waypoint with the set movement speed.
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime) ;
        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold) 
        {
            currentWaypoint = circuit.GetNextWaypoint(currentWaypoint);
            transform.LookAt(currentWaypoint);

            SmoothRotation();

        }

    }

    /*
     * This function is used to slightly reduce the level of snapping when the object transitions between waypoints.
     */
    private void SmoothRotation()
    {
        directionToWaypoint = (currentWaypoint.position - transform.position).normalized;
        rotationFinal = Quaternion.LookRotation(directionToWaypoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationFinal, rotateSpeed * Time.deltaTime);
    }

    /*
     * Getter method to retrieve the starting waypoint for the object.
     */
    public int getStartWaypoint()
    {
        return startingWaypoint;
    }
}
