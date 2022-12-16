using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPathing : MonoBehaviour
{
    public float currentSpeed;
    public Transform[] waypoints;
    public Transform currentTarget;
    public float stoppingDistance = 0.1f;
    // Start is called before the first frame update
    private int idx = 0;
    public float maxSpeed = 5f;
    void Start()
    {
        currentTarget = waypoints[0];
        currentSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        if (distanceToTarget <= stoppingDistance)
        {
            idx++;
            currentTarget = waypoints[idx % waypoints.Length];
            currentSpeed = 0;
        }
        
        //move toward the target.
        var direction = currentTarget.position - transform.position;
        direction = direction.normalized;
        if (currentSpeed < maxSpeed)
        {
            currentSpeed = maxSpeed;
        }
        
        transform.Translate(direction * Time.deltaTime * currentSpeed);
    }
}
