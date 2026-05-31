using System.Collections;
using character.Interfaces;
using UnityEngine;

public class PlayerVision : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    public float viewRadius = 10f;
    public float moveSpeed = .05f;
    public int viewAngle = 60; //视野角度
    public int rayCount = 5; //射线数量 越多检测的越精确 但是性能越差
    public float lineLength = 10f; //瞄准线的长度
    private Vector3 _moveDirection = Vector3.up;
    float angleOffet = 0;
    [Header("子弹设置")] public float bulletHitColdTime = 5f; //秒
    public float bulletFlySpeed = .1f;
    private Transform bullet;
    private float bulletAngleOffet = 60; //初始子弹方向
    private Bounds playerBounds;
    LineRenderer lineRenderer;
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.widthMultiplier = .05f;
        //拿到子弹的对象
        if (bullet == null)
        {
            bullet = GameObject.Find("bullet").transform;
            bullet.gameObject.SetActive(false);
        }
        playerBounds = GetComponent<BoxCollider2D>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _moveDirection = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _moveDirection = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _moveDirection = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _moveDirection = Vector3.right;
        }
        else
        {
            _moveDirection = Vector3.zero;
        }
        //角色朝向
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 lookDir = mousePos - transform.position;
        angleOffet = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angleOffet);
        lineRenderer.SetPosition(0, transform.position);
        //设置linerender的长度为很长
        lookDir = lookDir.normalized;
        Vector3 lineEndpos = transform.position + lookDir * lineLength;
        RaycastHit2D collider = lineRaycast(lookDir);
        if (collider.collider == null)
        {
            lineRenderer.SetPosition(1, lineEndpos);
        }
        else
        {
            lineRenderer.SetPosition(1, collider.point);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!bullet.gameObject.activeSelf)
            {
                FixedBullet();
                bullet.gameObject.SetActive(true);
                if (collider.collider == null)
                {
                    StartCoroutine(Hit(transform.position, lineEndpos));
                }
                else
                {
                    StartCoroutine(Hit(transform.position, collider.point));
                }
            }
        }
    }
    RaycastHit2D lineRaycast(Vector2 lookDir)
    {
        var hit = Physics2D.Raycast(transform.position, lookDir, viewRadius, 1 << 6);
        return hit;
    }

    void FixedBullet()
    {
        bullet.rotation = Quaternion.Euler(0, 0, angleOffet + bulletAngleOffet);
    }

    //设置扇形区域发射多条射线
    void VisionRact()
    {
        float halfViewAngle = viewAngle / 2f;
        for (int i = 0; i < rayCount; i++)
        {
            var direction = Quaternion.Euler(0f, 0f, halfViewAngle / rayCount * i) * _moveDirection; //将方向沿z轴进行旋转
            VisionRaycast(direction);
        }

        for (int i = 0; i < rayCount; i++)
        {
            var direction = Quaternion.Euler(0f, 0f, -halfViewAngle / rayCount * i) * _moveDirection; //将方向沿z轴进行旋转
            VisionRaycast(direction);
        }
    }

    void VisionRaycast(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewRadius);
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, hit.point, Color.blue);
        }
        else
        {
            Debug.DrawRay(transform.position, direction * viewRadius, Color.blue);
        }
    }

    IEnumerator Hit(Vector3 start, Vector3 end)
    {
        bullet.transform.position = start;
        float totalDistance = Vector3.Distance(start, end),
            distance = Vector3.Distance(bullet.transform.position, start);
        Vector3 direction = (end - bullet.transform.position).normalized;
        while (distance < totalDistance)
        {
            bullet.transform.position += direction * bulletFlySpeed;
            distance = Vector3.Distance(bullet.transform.position, start);
            yield return null;
        }

        bullet.gameObject.SetActive(false);
    }

    bool CanMove(Vector2 direction)
    {
        Vector2 size = playerBounds.size;
        RaycastHit2D hit2D = Physics2D.BoxCast(
            transform.position,
            size,
            0,
            direction,
            moveSpeed * 1.05f, // 修复检测距离
            1 << 11
        );
        return hit2D.collider == null;
    }
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0)
            return;
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    public void knockback(Vector2 direction, float force)
    {
        throw new System.NotImplementedException();
    }

    public bool IsDead { get; }
}