using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMover : MonoBehaviour
{
    Transform _transform;

    [Header("MOVEMENT VARIABLES")]
    [SerializeField] GameObject _slimeParent;
    [SerializeField] GameObject _slimeDestroyParticle;
    [SerializeField] Transform _firstPosition;
    [SerializeField] Transform _secondPosition;
    [SerializeField] private bool _fromFirstToSecond = true;

    [SerializeField] private float _time;
    private float _timeForDirection;


    void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
        _transform.position = _firstPosition.position;
        _transform.localScale = _firstPosition.localScale;
        _timeForDirection = 0;
    }
    public void Destroyer()
    {
        Instantiate(_slimeDestroyParticle, transform.position, Quaternion.identity);
        Destroy(_slimeParent);
    }

    void Update()
    {
        if(_fromFirstToSecond)
        {
            if(_timeForDirection < _time)
            {
                var _normalizeTime = _timeForDirection / _time;
                _transform.position = Vector2.Lerp(_firstPosition.position, _secondPosition.position, _normalizeTime);
                _timeForDirection += Time.deltaTime;
            }
            else
            {
                _fromFirstToSecond = !_fromFirstToSecond;
                _transform.localScale = _secondPosition.localScale;
                _timeForDirection = 0;
            }
        }
        else
        {
            if (_timeForDirection < _time)
            {
                var _normalizeTime = _timeForDirection / _time;
                _transform.position = Vector2.Lerp(_secondPosition.position, _firstPosition.position, _normalizeTime);
                _timeForDirection += Time.deltaTime;
            }
            else
            {
                _fromFirstToSecond = !_fromFirstToSecond;
                _transform.localScale = _firstPosition.localScale;
                _timeForDirection = 0;
            }
        }
    }
}
