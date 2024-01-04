using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    [SerializeField]
    private int _health = 100;

    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private GameObject _player;
    private Animator _anim;

    [SerializeField]
    private Rigidbody thisRB;

    [SerializeField]
    private float _moveDistance = 10;

    [SerializeField]
    private float _attackDistance = 1.7f;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Damaged(int hp)
    {
        if (isAlive())
        {
            _anim.SetTrigger("isDamaged");
            AddDamage(hp);
            Debug.Log("Damaged");
            if (!isAlive())
            {
                _anim.SetTrigger("isDead");
                thisRB.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        if (isAlive())
        {
            float distance = (_player.transform.position - transform.position).magnitude;

            if (distance <= _moveDistance && distance > _attackDistance)
            {
                _anim.SetBool("isMove", true);
                transform.LookAt(_player.transform);
                transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            }
            else if (distance <= _attackDistance)
            {
                _anim.SetTrigger("isAttack");
                _player.GetComponent<Player>().AddDamage(20);
            }
            else
            {
                _anim.SetBool("isMove", false);
            }
        }
    }

    public void AddDamage(int damage)
    {
        _health -= damage;
    }

    public bool isAlive() => _health >= 1;
}