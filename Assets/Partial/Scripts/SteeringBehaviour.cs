using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SteeringBehaviour : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Drive _drive;
    [SerializeField] private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        //Obtenemos en on start el script de Drive para sacar los valores de ahi siempre
        //ya que el metodo GetComponent es bastante "expensive" (en computing power)
        _drive = target.GetComponent<Drive>();
        _agent = this.GetComponent<NavMeshAgent>();
    }
    void Seek(Vector3 targetLocation)
    {
        _agent.SetDestination(targetLocation);
    }

    void Flee(Vector3 targetLocation)
    {
        Vector3 fleeVector = targetLocation - this.transform.position;
        _agent.SetDestination(this.transform.position - fleeVector);
    }
    void Persue(Vector3 targetLocation)
    {
        Vector3 targetDirection = targetLocation - this.transform.position;
        
        if(_drive.currentSpeed < 0.01f){
            Seek(target.transform.position);
            return;
        }

        float lookahead = targetDirection.magnitude / (_agent.speed + _drive.currentSpeed);
        Seek(target.transform.position + target.transform.forward * lookahead * 5);
    }
    
    void Evade(Vector3 targetLocation)
    {
        Vector3 targetDir = targetLocation - this.transform.position;
        float lookahead = targetDir.magnitude / (_agent.speed + _drive.currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookahead * 15);
    }
    

    private Vector3 wanderTarget = Vector3.zero;
    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 20;
        float wanderJitter = 5;
        
        wanderTarget += new Vector3(Random.Range(-1.0f,1.0f) * wanderJitter,0,Random.Range(-1.0f,1.0f)* wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);
        Seek(targetWorld);
    }
    
    void Update()
    {
        // Seek(target.transform.position);
        // Flee(target.transform.position);
        // Persue(target.transform.position);
        Wander();
    }
}