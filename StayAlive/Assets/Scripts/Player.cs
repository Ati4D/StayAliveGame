using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _health = 100;


    public void AddDamage(int hp)
    {
        _health -= hp;
        Debug.Log("Damaged Player: " + _health);

        if (_health <= 0)
        {
            PlayerDied();
        }
    }

    public void PlayerDied()
    {
        Debug.Log("Player is dead");
        SceneManager.LoadScene(0);
    }

    public bool isAlive() => _health >= 1;
}
