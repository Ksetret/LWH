using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MapGenerator;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _roadsPref;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _maxRoadCount = 5;

    
    
    private float _speed = 0;
    private List<GameObject> _roads = new List<GameObject>();

    public float Speed => _speed;

    static public RoadGenerator instanse;

    private void Awake()
    {
        instanse = this;
    }
    private void Start()
    {
        ResetLevel();
        StartLevel();
    }


    private void Update()
    {
        if (_speed == 0)
            return;

        foreach(GameObject road in _roads)
        {
            road.transform.position -= new Vector3(0, 0, _speed * Time.deltaTime);
        }

        if (_roads[0].transform.position.z < -10f)
        {
            Destroy(_roads[0]);
            _roads.RemoveAt(0);
            CreateNextRoad();
        }

       
    }

    public void ResetLevel()
    {
      //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _speed = 0;
        while (_roads.Count > 0)
        {
            Destroy(_roads[0]);
            _roads.RemoveAt(0);
        }

        for (int i = 0; i < _maxRoadCount; i++)
        {
            CreateNextRoad();
        }

        //PlayerController.instanse.Animator.speed = 0;
        MapGenerator.instanse.ResetMaps();
    }

    private void CreateNextRoad()
    {

        Vector3 pos = Vector3.zero;
        if(_roads.Count > 0)
        {
            pos = _roads[_roads.Count - 1].transform.position + new Vector3(0, 0f, 10f);
        }

        int indexGroand = UnityEngine.Random.Range(0, _roadsPref.Length);
        GameObject nextRoad = Instantiate(_roadsPref[indexGroand], pos, Quaternion.identity);
        nextRoad.transform.SetParent(transform);
        _roads.Add(nextRoad);
    }
   public void StartLevel()
    {
        Score.instance.ResetScore();
        MapGenerator.instanse.SetBest(BeastPool.Mouse);
        _speed = _maxSpeed;
        PlayerController.instanse.RestartBeast();

        //PlayerController.instanse.Animator.speed = 1;
    }
}

