using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;




public class MapGenerator : MonoBehaviour
{

    /* [SerializeField] private GameObject _obstacleTopPrefab;
     [SerializeField] private GameObject _obstacleBottomPrefab;
     [SerializeField] private GameObject _obstacleFullPrefab;*/
    private GameObject[] _nowObstacle;
    [SerializeField] private GameObject[] _obstacleMouse;
    [SerializeField] private GameObject[] _obstacleMonkey;
    [SerializeField] private GameObject _rampPrefab;

    [SerializeField] private GameObject _energePrefab;

    [SerializeField] private List<GameObject> maps = new List<GameObject>();
    [SerializeField] private List<GameObject> activeMaps = new List<GameObject>();

    [SerializeField]private int _itemSpace = 20;
    [SerializeField] private int _itemCountInMap = 5;
    [SerializeField] private float _laneOffset = 2.5f;

    [SerializeField] private int energeCountInItem = 6;
    [SerializeField] private float _energeHeight = 0.5f;

    private int _mapSize;

    [SerializeField] private GameObject _roadGenerator;
    private RoadGenerator _roadGeneratorScript;

    public float LaneOffset => _laneOffset;
    enum TrackPos { Left = -1, Center = 0, Right = 1};
    enum EnergeStyle { Line, Jump, Ramp};
    public enum BeastPool { Monkey = 1, Mouse = 0};

    public BeastPool _nowBeast = BeastPool.Mouse;
    struct MapItem
    {
        public void SetValues(GameObject obstacle, TrackPos trackPos, EnergeStyle energeStyle)
        {
            this.obstacle = obstacle; this.trackPos = trackPos; this.energiStyle = energeStyle;
        }

        public GameObject obstacle;
        public TrackPos trackPos;
        public EnergeStyle energiStyle;
    }

    static public MapGenerator instanse;
    private void Awake()
    {
        _nowObstacle = _obstacleMouse;

        instanse = this;
        _mapSize = _itemCountInMap * _itemSpace;
        _roadGeneratorScript = _roadGenerator.GetComponent<RoadGenerator>();
        maps.Add(MakeMap());
        maps.Add(MakeMap());
        maps.Add(MakeMap());
        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }
    }


    private void Start()
    {
        
        _nowBeast = BeastPool.Mouse;
    }
    private void Update()
    {
        if (_roadGeneratorScript.Speed == 0) return;

        foreach (GameObject map in activeMaps) 
        {
            map.transform.position -= new Vector3(0, 0, _roadGeneratorScript.Speed * Time.deltaTime);
        }

        if (activeMaps[0].transform.position.z < - _mapSize)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
    }

    private void RemoveFirstActiveMap()
    {
        activeMaps[0].SetActive(false);
        maps.Add(activeMaps[0]);
        activeMaps.RemoveAt(0);
    }

    public void ResetMaps()
    {
        while(activeMaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }

         AddActiveMap();
        AddActiveMap();
    }

    private void AddActiveMap()
    {
        int rand = UnityEngine.Random.Range(0, maps.Count);
        GameObject nextMap = maps[rand];
        nextMap.SetActive(true);
        foreach (Transform child in nextMap.transform)
        {
            child.gameObject.SetActive(true);
        }

        nextMap.transform.position = activeMaps.Count > 0 ?
                                        activeMaps[activeMaps.Count - 1].transform.position + Vector3.forward * _mapSize :
                                        new Vector3(0, 0, 10);
        maps.RemoveAt(rand);
        activeMaps.Add(nextMap);
    }

    private GameObject MakeMap()
    {
        GameObject result = new GameObject("Map");
        result.transform.SetParent(transform);
        for (int i = 0; i < _itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
            EnergeStyle energiStyle = EnergeStyle.Line;

            int rand = UnityEngine.Random.Range(0, 3);

            int randObstacle = UnityEngine.Random.Range(0, _nowObstacle.Length);



            obstacle = _nowObstacle[randObstacle];

            

            if (rand == 0) { trackPos = TrackPos.Left;}
            else if (rand == 1) { trackPos = TrackPos.Right;}
            else if (rand == 2) { trackPos = TrackPos.Center;}

            Vector3 obstaclePos = new Vector3((int)trackPos * _laneOffset, 1f, i * _itemSpace);
            CreatCoints(energiStyle, obstaclePos, result);

            if (obstacle != null)
            {
                GameObject nextObstacle = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                nextObstacle.transform.SetParent(result.transform);
            }
        }
        return result;
    }

    private void CreatCoints(EnergeStyle style, Vector3 pos, GameObject parentObject)
    {
        Vector3 energePos = Vector3.zero;
        if (style == EnergeStyle.Line)
        {
            for (int i = -energeCountInItem / 2; i < energeCountInItem / 2; i++)
            {
                energePos.y = _energeHeight;
                energePos.z = i * ((float)_itemSpace / energeCountInItem);
                GameObject nextEnerge = Instantiate(_energePrefab, energePos + pos, Quaternion.identity);
                nextEnerge.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == EnergeStyle.Jump)
        {
            for (int i = -energeCountInItem / 2; i < energeCountInItem / 2; i++)
            {
                energePos.y = Mathf.Max(-1/2f * Mathf.Pow(i,2) + 3, _energeHeight);
                energePos.z = i * ((float)_itemSpace / energeCountInItem);
                GameObject nextEnerge = Instantiate(_energePrefab, energePos + pos, Quaternion.identity);
                nextEnerge.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == EnergeStyle.Ramp)
        {
            for (int i = -energeCountInItem / 2; i < energeCountInItem / 2; i++)
            {
                energePos.y = Mathf.Min(Mathf.Max(0.7f * (i+2), _energeHeight), 3.0f);
                energePos.z = i * ((float)_itemSpace / energeCountInItem);
                GameObject nextEnerge = Instantiate(_energePrefab, energePos + pos, Quaternion.identity);
                nextEnerge.transform.SetParent(parentObject.transform);
            }
        }
    }

    public void SetBest(BeastPool nextBeast)
    {
        maps.Clear();
        activeMaps.Clear();

      //  MakeMap();
       
        _nowBeast = nextBeast;
    

        Debug.Log("SetBeast");
        if (_nowBeast == BeastPool.Mouse)
        {
            _nowObstacle = _obstacleMouse;

            foreach (Transform child in this.transform)
                Destroy(child.gameObject);

            maps.Add(MakeMap());
            maps.Add(MakeMap());
            maps.Add(MakeMap());

            AddActiveMap();
            AddActiveMap();
            AddActiveMap();

        }

        else if (_nowBeast == BeastPool.Monkey)
        {
            _nowObstacle = _obstacleMonkey;

            foreach (Transform child in this.transform)
                Destroy(child.gameObject);

            maps.Add(MakeMap());
            maps.Add(MakeMap());
            maps.Add(MakeMap());

           
            AddActiveMap();
            AddActiveMap();
            AddActiveMap();

        }

    }

    

  
}
