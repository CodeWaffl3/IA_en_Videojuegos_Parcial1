using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberSteering : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        _hidingPlaces = getHidingPlaces();
    }

    public void Seek(Vector3 targetLocation)
    {
        agent.SetDestination(targetLocation);
    }
    
    public void Flee(Vector3 targetLocation)
    {
        Vector3 fleeVector = targetLocation - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    public void Pursue(Vector3 targetLocation, GameObject target)
    {
        Vector3 targetDir = targetLocation - this.transform.position;

        if (target.GetComponent<Drive>().currentSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }
        float lookahead = targetDir.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookahead * 5);
    }

    public void Evade(Vector3 targetLocation, GameObject target)
    {
        Vector3 targetDir = targetLocation - this.transform.position;
        float lookahead = targetDir.magnitude / (agent.speed + target.GetComponent<Drive>().currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookahead * 15);
    }

    
    public void Wander(Vector3 wanderTarget)
    {
        float wanderRadius = 10;
        float wanderDistance = 20;
        float wanderJitter = 5;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter,
            0,
            Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    GameObject[] getHidingPlaces()
    {
        GameObject[] hidingSpots;
        hidingSpots = GameObject.FindGameObjectsWithTag("hide");
        return hidingSpots;
    }

    private GameObject[] _hidingPlaces;

    public void Hide(Transform target)
    {
        float closestDistance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        GameObject[] hidingPlaces = _hidingPlaces;

        for (int i = 0; i < hidingPlaces.Length; i++)
        {
            Vector3 hideDirection = hidingPlaces[i].transform.position - target.transform.position;
            Vector3 hidePosition = hidingPlaces[i].transform.position + hideDirection.normalized * 5;

            if (Vector3.Distance(this.transform.position, hidePosition) < closestDistance)
            {
                closestDistance = Vector3.Distance(this.transform.position, hidePosition);
                chosenSpot = hidePosition;
            }
        }
        Seek(chosenSpot);
    }
}
