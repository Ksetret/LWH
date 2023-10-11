using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    
    [SerializeField] private TMP_Text _scoreText;

    private int _time;

    private bool _startGame = false;

    //[SerializeField]private bool _startGame = false;


    static public Score instance;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {


        if (_startGame) {
            _scoreText.text = ((_time++) / 5).ToString();
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

;
    }

}
