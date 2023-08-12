using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector = Vector3.left;
    [SerializeField] private float speed = 1f;
    //new variables
    [SerializeField] bool partialRotation = false;
    [SerializeField] float maxRotationLeft = 100;
    [SerializeField] float maxRotationRight = -100;

    [SerializeField] GameObject _rotatorObject;
    [SerializeField] Transform _rotationPointTransform;
    Vector3 _rotPoint;

    bool _turnLeft = true;
    bool _turnRight = false;

    private void OnEnable()
    {
        _rotPoint = _rotationPointTransform.position;
    }
    void Update()
    {
        if(_rotatorObject == null)
        {
            if (!partialRotation)
            {
                transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);
            }
            else
            {
                //transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);
                if (transform.rotation.y < maxRotationLeft)
                {
                    _turnLeft = true;
                    _turnRight = false;
                    //Debug.Log("Rotation threshold reached");
                    transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);

                }
                else
                {
                    _turnLeft = false;
                    _turnRight = true;
                    transform.Rotate(rotationVector * -1, speed * Time.deltaTime, Space.World);

                }
                if (transform.rotation.y >= maxRotationLeft || transform.rotation.y <= maxRotationRight)
                {
                    //Debug.Log("Rotation threshold reached");
                    rotationVector *= -1; // invert direction 
                }
            }
        }
        else
        {
            if (!partialRotation)
            {
                _rotatorObject.transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);
            }
            else
            {
                //transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);
                if (_rotatorObject.transform.rotation.y < maxRotationLeft)
                {
                    _turnLeft = true;
                    _turnRight = false;
                    //Debug.Log("Rotation threshold reached");
                    _rotatorObject.transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);

                }
                else
                {
                    _turnLeft = false;
                    _turnRight = true;
                    _rotatorObject.transform.Rotate(rotationVector * -1, speed * Time.deltaTime, Space.World);

                }
                if (_rotatorObject.transform.rotation.y >= maxRotationLeft || _rotatorObject.transform.rotation.y <= maxRotationRight)
                {
                    //Debug.Log("Rotation threshold reached");
                    rotationVector *= -1; // invert direction 
                }
            }
        }
        
    }
}
