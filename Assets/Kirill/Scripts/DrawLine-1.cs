using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer _line;
    [SerializeField] private Transform _flourPosition;
    [SerializeField] private GameObject _middleLine;
    [SerializeField] private GameObject _bottomLine;
    [SerializeField] private Transform _targetTransform;

    private void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    private void Update()
    {


        _line.SetPosition(0, transform.position);
        _line.SetPosition(1, _flourPosition.position);

        Vector3 flourTargetPosition = new Vector3(_targetTransform.position.x, _flourPosition.position.y, _targetTransform.position.z);

        _middleLine.GetComponent<LineRenderer>().SetPosition(0, _flourPosition.position);
        _middleLine.GetComponent<LineRenderer>().SetPosition(1, flourTargetPosition);

        _bottomLine.GetComponent<LineRenderer>().SetPosition(0, flourTargetPosition);
        _bottomLine.GetComponent<LineRenderer>().SetPosition(1, _targetTransform.position);
    }
}
