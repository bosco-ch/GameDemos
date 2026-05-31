using UnityEngine;

public interface IEnemyState
{
    void OnEnter();
    void OnExit();
    void Update();

    protected void MoveFaceDirection(Vector3 direction)
    {
        direction = direction.normalized;
    }
}