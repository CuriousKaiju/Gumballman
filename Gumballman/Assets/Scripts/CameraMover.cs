using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField] private Transform _playerTransform;

    [Header("CAMERA VARIABLES")]
    [SerializeField] private float _cameraShift_X;
    

    void Update()
    {
        if(_playerTransform != null)
        {
            transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y + _cameraShift_X, -5);
        }
    }
}
