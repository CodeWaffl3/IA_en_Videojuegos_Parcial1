using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Cop : MonoBehaviour
{
    // Start is called before the first frame update
    private enum CopState
    {
        Catch,
        Flee
    }
    [Header("Robbers")]
    [SerializeField] private GameObject[] _Robbers;
    [SerializeField] private NavMeshAgent[] _RobbersNavMeshes;
    
    [Header("Cop Related")]
    [SerializeField] private float CatchDistance = 2;
    [SerializeField] private CopState _copState = CopState.Catch;
    [SerializeField] private float copHealth = 1;
    
    [SerializeField] private float _catches = 5;
    
    void CheckRadius()
    {
        foreach (GameObject robber in _Robbers)
        {
            if (Vector3.Distance(robber.transform.position, this.transform.position) < CatchDistance)
            {
                if (_copState == CopState.Catch)
                {
                    robber.GetComponent<NavMeshAgent>().speed = 0;
                }
                else if (_copState == CopState.Flee)
                {
                    copHealth--;
                }
            }
        }
    }
    

    void CheckWinLvl1()
    {
        float current_catches = 0;
        foreach (var robber in _RobbersNavMeshes)
        {
            if (robber.speed == 0)
            {
                current_catches++;
            }
        }

        if (current_catches == _catches)
        {
            SceneManager.LoadScene("Level2", LoadSceneMode.Single);
        }
    }

    void CheckLoseLvl2()
    {
        if (copHealth <= 0)
        {
            SceneManager.LoadScene("GameOver" ,LoadSceneMode.Single);
        }
    }
    private void Update()
    {
        if (_copState == CopState.Catch)
        {
            CheckWinLvl1();
        }
        else
        {
            CheckLoseLvl2();
        }
        CheckRadius();
        
    }
}
