using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using character.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BulletController : MonoBehaviour
{
    private float _attackRange;
    private float _flySpeed;
    private Vector2 _direction;
    private const string FireballKey = "Assets/Prefabs/Weapons/Blood.prefab";
    private AssetReference _bloodPrefab;

    public void Initialize(float flySpeed, float lifeTime, Vector2 direction)
    {
        this._flySpeed = flySpeed;
        this._direction = direction.normalized;
        Destroy(gameObject, lifeTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        _attackRange = GetComponent<SpriteRenderer>().bounds.extents.x;
        Debug.Log(_attackRange);
    }

    // Update is called once per frame
    void Update()
    {
        Fly(transform.position, _flySpeed, GameLayer.EnemyMask);
    }

    private void Fly(Vector3 endPos, float speed, LayerMask mask)
    {
        MoveUtil.MoveGeneral(transform, speed, out bool isDone, _direction);
        var checkPlayerInCircle = Detector.CheckPlayerInCircle(
            transform.position,
            .1f,
            mask
        );
        if (checkPlayerInCircle != null && checkPlayerInCircle.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("击中");
            damageable.TakeDamage(999);
            //blood
            Addressables.InstantiateAsync(FireballKey, transform.position, Quaternion.identity).Completed += (op) =>
            {
                if (op.Result != null)
                {
                    op.Result.GetComponent<ParticleSystem>().Play();
                }
            };
            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, .1f);
    }
}