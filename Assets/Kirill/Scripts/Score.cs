using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static MapGenerator;
using System;

public class Score : MonoBehaviour
{
    
    [SerializeField] private TMP_Text _scoreText;

    private int _time;
    private int _timeMaps;

    private bool _startGame = false;

    private bool _foxCardReady = true;
    private bool _endGame = true;
    //[SerializeField]private bool _startGame = false;


    static public Score instance;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {


        if (_startGame) {
            _scoreText.text = Convert.ToInt32((((_time++) / 40 * RoadGenerator.instanse.Speed))).ToString();
        }
        if ((_timeMaps++) / 5  == 300 && _foxCardReady == true)
        {
            _foxCardReady = false;
            MapGenerator.instanse.SetBeast(BeastPool.Fox);
        }
       if ((_timeMaps++) / 5   == 600 && _endGame == true)
        {
            _endGame = false;
            MapGenerator.instanse.SetBeast(BeastPool.EndGame);
        }
    }

    

    public void StopScore()
    {
        _startGame = !_startGame;
        _startGame = false;
    }
    public void ResetScore()
    {
        _time = 0;
        _startGame = true;
    }


}
