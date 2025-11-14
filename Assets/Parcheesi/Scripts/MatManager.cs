using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatManager : MonoBehaviour
{
    public GameObject[] waypoints;
    public float moveSpeed = 5f;
    public int num = 0;
    public GameObject player;

    private int currentWaypointIndex = 0;

    private bool ismove = false;
    private void Update()
    {
        if (num != 0&&!ismove)
        {
            CheckMovePoint();
        }
        if (ismove)
        {
            MoveToNextWaypoint(currentWaypointIndex);
        }
    }


    private void MoveToNextWaypoint(int targetIndex)
    {
        
        if (Vector3.Distance(player.transform.position, waypoints[targetIndex].transform.position) >= 0.01f)
        {

            Vector3 targetPosition = waypoints[targetIndex].transform.position;
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            ismove = false;
            num--;
        }

    }
    private void CheckMovePoint()
    {
        if (currentWaypointIndex + 1 <= waypoints.Length - 1)
        {
            currentWaypointIndex++;           
        }

        else
        {
            currentWaypointIndex = 0;
        }

        ismove = true;
    }
}