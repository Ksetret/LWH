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

    [SerializeField] private float _currentEnerge = 1f;
    [SerializeField] private float _maxEnerge = 1.0f;
    [SerializeField] private float _rateEnerge = 0.002f;
    [SerializeField] private GameObject _deathWindow;
    [SerializeField] private Light _lightMouse;
    [SerializeField] private ParticleSystem _particleMouse;
    [SerializeField] private Light _lightFox;
    [SerializeField] private ParticleSystem _particleFox;
    [SerializeField] private Light _lightFish;
    [SerializeField] private ParticleSystem _particleFish;

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
        {
            Score.instance.StopScore();
            _deathWindow.SetActive(true);
            RoadGenerator.instanse.ResetLevel();
        }

        if(_particleMouse != null)
        _particleMouse.startSize = _currentEnerge * 5;

        if (_lightMouse != null)
            _lightMouse.intensity = _currentEnerge;

        if (_particleFox != null)
            _particleFox.startSize = _currentEnerge * 5;

        if (_lightFox != null)
            _lightFox.intensity = _currentEnerge;

        if (_particleFish != null)
            _particleFish.startSize = _currentEnerge * 5;

        if (_lightFish != null)
            _lightFish.intensity = _currentEnerge;
    }

    public void EnergeBuster() {

        if (_currentEnerge >= _maxEnerge) return;

            _currentEnerge += 0.2f;
     
    }

    
}

