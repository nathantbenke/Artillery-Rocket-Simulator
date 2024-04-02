using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{

    /// <summary>
    /// Helps visualize waypoints used to create circuit for Jet object.
    /// 
    /// /// This script was adapted from MetalStorm Games' Waypoint Path system tutorial:
    /// https://www.youtube.com/watch?v=EwHiMQ3jdHw 
    /// </summary>


    
    private void OnDrawGizmos()
    {
        //Marks each waypoint with sphere
        foreach(Transform t in transform)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(t.position, 1f);
        }

        Gizmos.color = Color.red;

        //Draws lines connecting each of the waypoints.
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position);
        }

        Gizmos.DrawLine(transform.GetChild(0).position, transform.GetChild(transform.childCount-1).position);
    }


    /*
     * Fetches the next waypoint for the object to travel to.
     */
    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }

        if (currentWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currentWaypoint.GetSiblingIndex() + 1);
        } else
        {
            return transform.GetChild(0);
        }
    }

}
