using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robber : MonoBehaviour
{
    private enum RobberState
    {
        Wander,
        Flee,
        Evade,
        Hide,
        Pursue,
        Seek
    }

    [SerializeField] private GameObject _target;
    [SerializeField] private RobberState _robberState = RobberState.Wander;
    private RobberSteering _robberSteering;
    private Vector3 wanderTarget = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        _robberSteering = GetComponent<RobberSteering>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_robberState)
        {
            case RobberState.Evade:
                _robberSteering.Evade(_target.transform.position,_target);
                break;
            case RobberState.Flee:
                _robberSteering.Flee(_target.transform.position);
                break;
            case RobberState.Wander:
                _robberSteering.Wander(wanderTarget);
                break;
            case RobberState.Hide:
                _robberSteering.Hide(_target.transform);
                break;
            case RobberState.Pursue:
                _robberSteering.Pursue(_target.transform.position,_target);
                break;
            case RobberState.Seek:
                _robberSteering.Seek(_target.transform.position);
                break;
        }
    }
}
