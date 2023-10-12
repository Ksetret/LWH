using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject _scoreWindow;
    [SerializeField] private GameObject _autorWinfow;
    [SerializeField] private GameObject _menuWindow;
    [SerializeField] private AudioSource _fonSounde;
    [SerializeField] private GameObject _stopWindow;

    static public MainMenu instance;

    private void Awake()
    {
        instance = this;
    }
    public void StartGameButton()
    {
        RoadGenerator.instanse.StartLevel();
        this.gameObject.SetActive(false);
        _fonSounde.Play();
    }

    public void AutorButton()
    {
        _scoreWindow.SetActive(false);
        _autorWinfow.SetActive(true);
        _menuWindow.SetActive(false);
    }

    public void ScoreButton()
    {
        _scoreWindow.SetActive(true);
        _autorWinfow.SetActive(false);
        _menuWindow.SetActive(false);
    }

    public void MainMenuButton()
    {
        _scoreWindow.SetActive(false);
        _autorWinfow.SetActive(false);
        _menuWindow.SetActive(true);
    }

    public void RestartLvl()
    {
        RoadGenerator.instanse.ResetLevel();
        RoadGenerator.instanse.StartLevel();

        MapGenerator.instanse.RestartLvl();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StopGame()
    {
        
        Time.timeScale = 0;
        _stopWindow.SetActive(true);

    }
}
