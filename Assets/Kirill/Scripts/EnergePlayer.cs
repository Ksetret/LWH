using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnergePlayer : MonoBehaviour 
{
    [SerializeField] private Image _energeBar;

    [SerializeField] private float _currentEnerge = 0.5f;
    [SerializeField] private float _maxEnerge = 1.0f;
    [SerializeField] private float _rateEnerge = 0.002f;


    static public EnergePlayer instance;

    private void Awake()
    {
        instance = this;

        _energeBar.fillAmount = 1;

    }

    private void FixedUpdate()
    {
        _currentEnerge -= _rateEnerge;
        if (_currentEnerge <= 0)
            Debug.Log("Game Over");
        _energeBar.fillAmount = _currentEnerge;
    }

    public void EnergeBuster() {

        if (_currentEnerge >= _maxEnerge) return;

            _currentEnerge += 0.2f;
     
    }

    
}

