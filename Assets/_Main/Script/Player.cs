using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
            health = 0;
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > 100)
            health = 100;
    }
}
