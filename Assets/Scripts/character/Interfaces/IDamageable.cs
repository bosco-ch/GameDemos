using UnityEngine;

//can damage
namespace character.Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(float damage);
        void knockback(Vector2 direction, float force);
        bool IsDead { get; }
    }
}