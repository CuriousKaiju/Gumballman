using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollision : MonoBehaviour
{

    [Header("FIRE VARIABLES")]
    [SerializeField] Transform _spawnZone;
    [SerializeField] int _damegeForPlayer;

    private Vector2 _worldSpawnZone;
    void Start()
    {
        _worldSpawnZone = _spawnZone.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().GetDamage();
            collision.gameObject.GetComponent<PlayerController>().PlayerGetDamageFromFire();
            StartCoroutine(PlayerSpawner(collision.gameObject, _worldSpawnZone));
        }
    }
    IEnumerator PlayerSpawner(GameObject player, Vector2 spawnPos)
    {
        yield return new WaitForSeconds(2);
        
        if(player != null)
        {
            player.SetActive(true);
            player.transform.position = spawnPos;
        }
        

    }
}
