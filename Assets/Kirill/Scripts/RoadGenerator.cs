using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MapGenerator;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _roadsBeastPref;
    [SerializeField] private GameObject[] _roadsFishPref;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _maxRoadCount = 5;
    private GameObject[] _nowRoadsPref;


    private float _speed = 0;
    private List<GameObject> _roads = new List<GameObject>();

    public float Speed => _speed;

    static public RoadGenerator instanse;

    private void Awake()
    {
        instanse = this;
        _nowRoadsPref = _roadsBeastPref;
    }
    private void Start()
    {
        ResetLevel();
        
       
    }


    private void Update()
    {
        if (_speed == 0)
            return;

        if (_speed != _maxSpeed)
            _speed += (float)(0.1 * Time.deltaTime);

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

        int indexGroand = UnityEngine.Random.Range(0, _nowRoadsPref.Length);
        GameObject nextRoad = Instantiate(_nowRoadsPref[indexGroand], pos, Quaternion.identity);
        nextRoad.transform.SetParent(transform);
        _roads.Add(nextRoad);
    }
   public void StartLevel()
    {
        Score.instance.ResetScore();
        MapGenerator.instanse.SetBeast(BeastPool.Mouse);
        _speed = 5;
        PlayerController.instanse.RestartBeast();

        //PlayerController.instanse.Animator.speed = 1;
    }

    public void SetNextRoud(MapGenerator.BeastPool nextBeast)
    {
        if (nextBeast == BeastPool.Fox || nextBeast == BeastPool.Mouse)
        {
            _nowRoadsPref = _roadsBeastPref;
        }
        if (nextBeast == BeastPool.Fish)
        {
            _nowRoadsPref = _roadsFishPref;
        }
    }

    public void ReastartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

