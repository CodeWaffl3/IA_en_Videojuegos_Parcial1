using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Pedestrian : MonoBehaviour
{
    //NACVMESH AGENT
    private NavMeshAgent _agent;
    public bool isChased = false;
    public GameObject chasedBy;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Seek(Vector3 targetLocation) 
    {
        _agent.SetDestination(targetLocation);
    }
    
    private Vector3 wanderTarget = Vector3.zero;
    [SerializeField] private float wanderRadius = 10;
    [SerializeField] private float wanderDistance = 20;
    [SerializeField] private float wanderJitter = 5;
    
    public void Wander(Vector3 wanderTarget)
    {
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }
    void Flee(Vector3 targetLocation)
    {
        Vector3 fleeVector = targetLocation - transform.position;
        _agent.SetDestination(transform.position - fleeVector);
    }

    private void Update()
    {
        if (isChased)
        {
            Flee(chasedBy.transform.position);
        }
        else if (!isChased)
        {
            Wander(wanderTarget);
        }
    }
}
