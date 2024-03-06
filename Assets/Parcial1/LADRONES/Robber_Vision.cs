
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Robber_Vision : MonoBehaviour
{
    //UI
    [SerializeField] private MainUI UIReference;
    
    //RAYCAST
    public float m_MaxDistance;
    public float m_Speed;
    bool m_HitDetect;
    Collider m_Collider;
    RaycastHit m_Hit;

    //STEERING BEHAVIOR
    private NavMeshAgent _agent;
    public GameObject chasedPedestrian;
    [SerializeField] private GameObject[] Policemans;
    [SerializeField] private GameObject ClosestPoliceman;

    private bool isEvading = true;

    void Start()
    {
        m_MaxDistance = 12.0f;
        m_Collider = GetComponent<Collider>();
        _agent = GetComponent<NavMeshAgent>();
    }

    //SEEK
    void Seek(Vector3 targetLocation)
    {
        _agent.SetDestination(targetLocation);
    }
    //FLEE
    void Flee(Vector3 targetLocation)
    {
        Vector3 fleeVector = targetLocation - transform.position;
        _agent.SetDestination(this.transform.position - fleeVector);
    }
    void Evade(Vector3 targetLocation, GameObject police)
    {
        Vector3 targetDir = targetLocation - transform.position;
        float lookAhead = 1;
        if (police.CompareTag("Player"))
        {
            lookAhead = targetDir.magnitude / (_agent.speed + police.GetComponent<Drive>().currentSpeed);
        }
        else if(police.CompareTag("police"))
        {
            lookAhead = targetDir.magnitude / (_agent.speed + police.GetComponent<NavMeshAgent>().speed);
        }
        Flee(police.transform.position + police.transform.forward * lookAhead);
    }
    
    private void GetClosestPoliceman() 
    {
        float closestDistance = Mathf.Infinity;
        foreach (GameObject policeman in Policemans)
        {
            float PoliceDistance = Vector3.Distance(policeman.transform.position, transform.position);
            if (PoliceDistance < closestDistance)
            {
                closestDistance = PoliceDistance;
                ClosestPoliceman = policeman;
            }
        }
    }

    void FixedUpdate()
    {
        m_HitDetect = Physics.BoxCast(m_Collider.bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
        if (m_HitDetect)
        {
            //Si detecta al peatón, activa la función de SEEK
            if (m_Hit.collider.tag == "pedestrian")
            {
                //Debug.Log("Estoy colisionando con " + m_Hit.collider.tag);
                m_Hit.collider.gameObject.GetComponent<Pedestrian>().chasedBy = gameObject;
                m_Hit.collider.gameObject.GetComponent<Pedestrian>().isChased = true;
                chasedPedestrian = m_Hit.collider.gameObject;
                isEvading = false;
            }
        }
        if (isEvading)
        {
            GetClosestPoliceman();
            Evade(ClosestPoliceman.transform.position, ClosestPoliceman);
        }
        else if (!isEvading)
        {
            Seek(chasedPedestrian.transform.position);
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("police"))
        {
            Debug.Log("ATRAPADO");
            _agent.speed = 0;
        }
        if (collision.gameObject.CompareTag("pedestrian"))
        {
            GameObject colliderPedestrian = collision.gameObject;
            Debug.Log("TE ATRAPE!");
            colliderPedestrian.GetComponent<NavMeshAgent>().speed = 0;
            Destroy(colliderPedestrian);
            UIReference.UpdatePedestrianCount(1);
            isEvading = true;
        }
        
    }

    // ****************************** RAYCAST ********************************************************************************
    //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * m_Hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * m_Hit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * m_MaxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * m_MaxDistance, transform.localScale);
        }
    }

}