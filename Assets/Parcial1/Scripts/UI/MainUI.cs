using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public class MainUI : MonoBehaviour
{
    [SerializeField] private TMP_Text pedestriansCountText;
    [SerializeField] private int numberOfPedestrians = 10;

    private void Update()
    {
        pedestriansCountText.text = ":" + numberOfPedestrians.ToString();
        if (numberOfPedestrians <= 0)
        {
            SceneManager.LoadScene("GameOver" ,LoadSceneMode.Single);
        }
    }

    public void UpdatePedestrianCount(int i)
    {
        numberOfPedestrians -= i;
    }
    
    
    
}
