using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public void AnayTelegram() { Application.OpenURL("https://web.telegram.org/k/#@anialavik"); }
    public void TimkekichTelegram() { Application.OpenURL("https://web.telegram.org/k/#@timkekich"); }
    public void BigimanButtonTelegram() { Application.OpenURL("https://web.telegram.org/k/#@BigimanBatton"); }
    public void BaufgtTelegram() { Application.OpenURL("https://web.telegram.org/k/#@Baufgt"); }
    public void FlametlTelegram() { Application.OpenURL("https://web.telegram.org/k/#@flametl"); }

    public void KeniuoTelegram() { Application.OpenURL("https://web.telegram.org/k/#@keniuo"); }
}


    