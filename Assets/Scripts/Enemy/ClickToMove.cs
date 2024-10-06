using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    private NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        //Create a ray from the camera to the mouse pos
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, NavMesh.AllAreas))
            {
                //Move the agent to the clicked position

                navAgent.SetDestination(hit.point);
            }
        }
    }
}
