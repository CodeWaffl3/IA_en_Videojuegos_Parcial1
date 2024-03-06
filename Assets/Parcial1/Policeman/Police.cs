using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Police : MonoBehaviour
{
    //NACVMESH AGENT
    private NavMeshAgent _agent;
    //Detection
    [SerializeField] private GameObject _current_Chase_Robber;
    public bool isChasing = false;
    public string robberTag = "Robber";
    public float radio = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        _agent = this.GetComponent<NavMeshAgent>();
    }

    void Seek(Vector3 targetLocation) 
    {
        _agent.SetDestination(targetLocation);
    }

    Vector3 wanderTarget = Vector3.zero;
    [SerializeField] private float wanderRadius = 10;
    [SerializeField] private float wanderDistance = 20;
    [SerializeField] private float wanderJitter = 5;
    public void Wander(Vector3 wanderTarget)
    {
        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
            0,
            Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    void Update()
    {
        // Verificar si el ladrón  fue detectado
        if (isChasing)
        {
            Seek(_current_Chase_Robber.transform.position);
        }
        else if (!isChasing)
        {
            //Debug.Log("WANDERING");
            Wander(wanderTarget);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!isChasing)
        {
            if (other.CompareTag(robberTag))
            {
                //Debug.Log("CHASING!");
                _current_Chase_Robber = other.gameObject;
                isChasing = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Dibujar la esfera de detección en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
