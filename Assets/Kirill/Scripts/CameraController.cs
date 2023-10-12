using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private Transform _cameraPlace;

    [SerializeField] private Vector3 _cameraOffset;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _cameraPlace.position + _cameraOffset, _speed * Time.deltaTime);
    }
}
