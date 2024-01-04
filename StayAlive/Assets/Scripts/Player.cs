using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _health = 100;

    private void Update()
    {
        Debug.Log("Player: "+_health);
    }

    public void AddDamage(int hp)
    {
        _health -= hp;
    }
    public bool isAlive() => _health >= 1;
}
