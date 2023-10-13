using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPref;
    [SerializeField] private float _spawnDelay;
    


    private Vector3 _whereToSpawn;
    private float _randomX;
    private float _randomZ;
    private float _nextSpawn = 0.0f;

    private void Update()
    {
        if(Time.time > _nextSpawn)
        {
            _nextSpawn = Time.time + _spawnDelay;
            _randomX = Random.RandomRange(transform.position.x -30, transform.position.x + 30);
            _randomZ = Random.RandomRange(transform.position.z  -15, transform.position.z + 15);
            _whereToSpawn = new Vector3(_randomX, transform.position.y, _randomZ);
            GameObject Enemy = Instantiate(_enemyPref, _whereToSpawn,Quaternion.identity);
            Destroy(Enemy, 5.0f);
        }
    }

}
