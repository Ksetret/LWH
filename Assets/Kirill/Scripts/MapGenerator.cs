using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;




public class MapGenerator : MonoBehaviour
{

    [SerializeField] private GameObject _foxTile;
    [SerializeField] private GameObject _fishTile;
    [SerializeField] private GameObject _endTile;

    private GameObject[] _nowObstacle;
    [SerializeField] private GameObject[] _obstacleMouse;
    [SerializeField] private GameObject[] _obstacleFox;
    [SerializeField] private GameObject[] _obstacleFish;
    [SerializeField] private GameObject _rampPrefab;

    [SerializeField] private GameObject _energePrefab;

    [SerializeField] private List<GameObject> _maps = new List<GameObject>();
    [SerializeField] private List<GameObject> _activeMaps = new List<GameObject>();

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
   // enum EnergeStyle { Line, Jump, Ramp};
    public enum BeastPool { Fox = 1, Mouse = 0, EndGame = 2, Fish = 3};

    public BeastPool _nowBeast = BeastPool.Mouse;
    struct MapItem
    {
        public void SetValues(GameObject obstacle, TrackPos trackPos)
        {
            this.obstacle = obstacle; this.trackPos = trackPos; 
        }

        public GameObject obstacle;
        public TrackPos trackPos;
        
    }

    static public MapGenerator instanse;
    private void Awake()
    {

        _nowObstacle = _obstacleMouse;

        instanse = this;
        _mapSize = _itemCountInMap * _itemSpace;
        _roadGeneratorScript = _roadGenerator.GetComponent<RoadGenerator>();

       

        foreach (GameObject map in _maps)
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

        foreach (GameObject map in _activeMaps) 
        {
            map.transform.position -= new Vector3(0, 0, _roadGeneratorScript.Speed * Time.deltaTime);
        }

        if (_activeMaps[0].transform.position.z < - _mapSize)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
    }

    private void RemoveFirstActiveMap()
    {

        _activeMaps[0].SetActive(false);

        if (_maps.Count > 3)
        {
            Destroy(_activeMaps[0].gameObject);
            _activeMaps.RemoveAt(0);
        }
        else
        {
            _maps.Add(MakeMap());
            _activeMaps.RemoveAt(0);
            
        }
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.active == false)
                Destroy(child.gameObject);
        }

    }



    public void ResetMaps()
    {
        while(_activeMaps.Count > 0)
        {
            RemoveFirstActiveMap();
        }

        /* AddActiveMap();
         AddActiveMap();*/
    }

    private void AddActiveMap()
    {
        int rand = UnityEngine.Random.Range(0, _maps.Count);
        GameObject nextMap = _maps[rand];
        nextMap.SetActive(true);
        foreach (Transform child in nextMap.transform)
        {
            child.gameObject.SetActive(true);
        }

        nextMap.transform.position = _activeMaps.Count > 0 ?
                                        _activeMaps[_activeMaps.Count - 1].transform.position + Vector3.forward * _mapSize :
                                        new Vector3(0, 0, 10);
        _maps.RemoveAt(rand);
        _activeMaps.Add(nextMap);
    }

    private GameObject MakeMap()
    {
        GameObject result = new GameObject("Map");
        result.transform.SetParent(transform);
        for (int i = 0; i < _itemCountInMap; i++)
        {
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;
          
            int rand = UnityEngine.Random.Range(0, 3);

            int randObstacle = UnityEngine.Random.Range(0, _nowObstacle.Length);



            obstacle = _nowObstacle[randObstacle];

            

            if (rand == 0) { trackPos = TrackPos.Left;}
            else if (rand == 1) { trackPos = TrackPos.Right;}
            else if (rand == 2) { trackPos = TrackPos.Center;}

            Vector3 obstaclePos = new Vector3((int)trackPos * _laneOffset, 1f, i * _itemSpace);
          

            if (obstacle != null)
            {
                GameObject nextObstacle = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                nextObstacle.transform.SetParent(result.transform);
            }
        }
        return result;
    }

    private GameObject MakeMap(GameObject _beast)
    {
        GameObject result = new GameObject("Map");
        result.transform.SetParent(transform);
        
            GameObject obstacle = null;
            TrackPos trackPos = TrackPos.Center;

            int rand = UnityEngine.Random.Range(0, 3);

            int randObstacle = UnityEngine.Random.Range(0, _nowObstacle.Length);



            obstacle = _beast;


            trackPos = TrackPos.Center; 

            Vector3 obstaclePos = new Vector3((int)trackPos * _laneOffset, 1f, _itemSpace);


            if (obstacle != null)
            {
                GameObject nextObstacle = Instantiate(obstacle, obstaclePos, Quaternion.identity);
                nextObstacle.transform.SetParent(result.transform);
            }
        
        return result;
    }

    /*private void CreatCoints(EnergeStyle style, Vector3 pos, GameObject parentObject)
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
    }*/

    public void SetBeast(BeastPool nextBeast)
    {
       _maps.Clear();
        _nowBeast = nextBeast;
        

        Debug.Log("SetBeast");
        if (_nowBeast == BeastPool.Mouse)
        {
            _nowObstacle = _obstacleMouse;

            /*foreach (Transform child in this.transform)
                Destroy(child.gameObject);*/

            _maps.Add(MakeMap());
            _maps.Add(MakeMap());
            
           /* maps.Add(MakeMap());

            AddActiveMap();*/
            AddActiveMap();
            AddActiveMap();

        }

        else if (_nowBeast == BeastPool.Fox)
        {
            _nowObstacle = _obstacleFox;

            /*foreach (Transform child in this.transform)
                 Destroy(child.gameObject);*/
            _maps.Add(MakeMap(_foxTile));
            _maps.Add(MakeMap());
            _maps.Add(MakeMap());
           /* maps.Add(MakeMap());

           
            AddActiveMap();*/
            AddActiveMap();
            AddActiveMap();
            AddActiveMap();
        }
        else if (_nowBeast == BeastPool.Fish)
        {
            _nowObstacle = _obstacleFish;

            _maps.Add(MakeMap(_fishTile));
            _maps.Add(MakeMap());
            _maps.Add(MakeMap());

            AddActiveMap();
            AddActiveMap();
            AddActiveMap();

        }

        else if (_nowBeast == BeastPool.EndGame)
        {
            _nowObstacle = _obstacleFox;

           
            _maps.Add(MakeMap(_endTile));
            _maps.Add(MakeMap());
            _maps.Add(MakeMap());

            AddActiveMap();
            AddActiveMap();
            AddActiveMap();
        }
    }

    public void RestartLvl()
    {
        foreach (Transform child in this.transform)
            Destroy(child.gameObject);
        ResetMaps();
        _maps.Clear();
        _activeMaps.Clear();
        SetBeast(BeastPool.Mouse);
        
    }

  
}
