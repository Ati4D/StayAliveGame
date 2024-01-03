using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private Transform _player;
    private Animator _anim;

    [SerializeField]
    private float _moveDistance = 10;

    [SerializeField]
    private float _attackDistance = 1.7f;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = (_player.position - transform.position).magnitude;
        
        if(distance <= _moveDistance && distance > _attackDistance)
        {
            _anim.SetBool("isMove", true);
            transform.LookAt(_player);
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }
        else if(distance <= _attackDistance)
        {
            _anim.SetTrigger("isAttack");
        }
        else
        {
            _anim.SetBool("isMove", false);
        }
        
    }
}