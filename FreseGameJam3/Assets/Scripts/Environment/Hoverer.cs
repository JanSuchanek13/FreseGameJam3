using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverer : MonoBehaviour
{
    [SerializeField] float hoverHeight = 2f;
    [SerializeField] float speed = 1f;
    [SerializeField] GameObject _hoverObject;

    private Vector3 _startPosition;

    private void OnEnable()
    {
        if (_hoverObject != null)
        {
            _startPosition = _hoverObject.transform.position;
        }else
        {
            _startPosition = transform.position;
        }
    }

    void Update()
    {
        if(_hoverObject != null)
        {
            Vector3 targetPosition = _startPosition + Vector3.up * hoverHeight * Mathf.PingPong(Time.time, 1f) * speed;
            _hoverObject.transform.position = targetPosition;
        }else
        {
            Vector3 targetPosition = _startPosition + Vector3.up * hoverHeight * Mathf.PingPong(Time.time, 1f) * speed;
            transform.position = targetPosition;
        }
    }
}
