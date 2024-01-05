using System.Collections;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    [SerializeField]
    private int _health = 100;

    [SerializeField]
    private float _speed = 1f;
    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private Animator _anim;

    [SerializeField]
    private Rigidbody thisRB;

    [SerializeField]
    private BoxCollider thisBC;
    [SerializeField]
    private BoxCollider thisTriggerBC;

    [SerializeField]
    private float _moveDistance = 10;

    [SerializeField]
    private float _attackDistance = 1.7f;

    [SerializeField]
    private int _damageToPlayer = 20;

    private bool isCoroutineRunning = false;

    public void Damaged(int hp)
    {
        if (isAlive())
        {
            _anim.SetTrigger("isDamaged");
            AddDamage(hp);
            if (!isAlive())
            {
                _anim.SetTrigger("isDead");
                thisRB.constraints = RigidbodyConstraints.FreezeAll;

                Vector3 v3s = new Vector3(0.5f, 0.5f, 2f);
                Vector3 v3c = new Vector3(0f, 0f, -1f);

                thisBC.size = v3s;
                thisBC.center = v3c;
                thisTriggerBC.size = v3s;
                thisTriggerBC.center = v3c;
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
            Vector3 directionToTarget = _player.transform.position - transform.position;
            float distance = directionToTarget.magnitude;

            if (distance <= _moveDistance && distance > _attackDistance)
            {
                _anim.SetBool("isMove", true);
                directionToTarget.y = 0;
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                }
                transform.Translate(Vector3.forward * _speed * Time.deltaTime);
            }
            else if (distance <= _attackDistance)
            {
                if (!isCoroutineRunning) PlayAnimationWithAction();
            }
            else
            {
                _anim.SetBool("isMove", false);
            }
        }
    }

    private IEnumerator PlayAnimationAndDoAction()
    {
        isCoroutineRunning = true;
        _anim.SetTrigger("isAttack");
        var player_sc = _player.GetComponent<Player>();
        player_sc.AddDamage(_damageToPlayer);
        yield return new WaitForSeconds(_anim.GetCurrentAnimatorStateInfo(0).length);
        isCoroutineRunning = false;
    }

    private void PlayAnimationWithAction() => StartCoroutine(PlayAnimationAndDoAction());

    public void AddDamage(int damage) => _health -= damage;

    public bool isAlive() => _health >= 1;
}