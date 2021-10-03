using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [Header("COIN VARIABLES")]
    [SerializeField] private int _playerScoreAdd;
    [SerializeField] GameObject _paticleBlow;
    private void AddScore(int score)
    {
        GameEvents.CallScoreChangeEvent(score);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(_paticleBlow, transform.position, Quaternion.identity);
            AddScore(_playerScoreAdd);
            Destroy(gameObject);
        }
    }
}
