using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject _paticleBlow;
    public void Destroyer()
    {
        Instantiate(_paticleBlow, transform.position, Quaternion.identity);
        AddHealth();
        Destroy(gameObject);
    }
    private void AddHealth()
    {
        GameEvents.CallHpChangeEventMinus(1);
    }
}
