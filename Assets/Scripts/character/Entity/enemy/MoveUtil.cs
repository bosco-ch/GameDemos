using UnityEngine;
using UnityEngine.AI;


public static class MoveUtil
{
    /// <summary>
    /// 2d物体的移动 (不需要刚体)
    /// </summary>
    /// <param name="transform">需要移动的物体</param>
    /// <param name="direction">移动方向</param>
    /// <param name="speed">移动速度</param>
    public static void Move2DWithoutBody(Transform transform, Vector3 direction, float speed)
    {
        direction = direction.normalized;
        transform.position += CalculateMoveDistance(speed) * direction;
    }

    public static void Move(Transform self, Vector3 targetpos, float speed, out bool isdone)
    {
        isdone = false;
        if (Vector2.Distance(self.transform.position, targetpos) < .1f)
        {
            isdone = true;
        }
        else
        {
            self.position += CalculateMoveDistance(speed) * (targetpos - self.transform.position).normalized;
        }
    }

    public static void MoveNavMesh(NavMeshAgent agent, Vector3 targetpos, float speed, out bool isdone)
    {
        if (!agent.isOnNavMesh)
        {
            Debug.Log("不存在这个");
            isdone = false;
            return;
        }

        isdone = false;
        agent.SetDestination(targetpos);
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // 获取移动方向

                isdone = true;
            }
        }
    }

    /// <summary>
    /// 移动通用
    /// </summary>
    /// <param name="self"></param>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    /// <param name="targetpos"></param>
    /// <param name="isdone"></param>
    public static void MoveGeneral(Transform self,
        float speed,
        out bool isdone,
        Vector3 direction = default,
        Vector3? targetpos = null)
    {
        Vector3 moveDir = Vector3.zero;
        if (targetpos != null)
        {
            moveDir = (targetpos.Value - self.transform.position).normalized;
            if (Vector3.Distance(self.transform.position, targetpos.Value) < .01f)
            {
                isdone = true;
            }
        }
        else
        {
            moveDir = direction.normalized;
        }

        self.position += CalculateMoveDistance(speed) * moveDir;
        isdone = false;
    }

    /// <summary>
    /// 先转到需要移动的目标点，在开始移动
    /// </summary>
    /// <param name="self"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="rotateSpeed"></param>
    /// <param name="direction"></param>
    /// <param name="targetpos"></param>
    public static void RotateThenMove(Transform self,
        float moveSpeed,
        float rotateSpeed,
        out bool success,
        Vector3 direction = default,
        Vector3? targetpos = null)
    {
        LookAt2D(self, rotateSpeed, out bool isTurnDone, targetpos, direction);
        if (isTurnDone)
        {
            MoveGeneral(self, moveSpeed, out bool done, direction, targetpos);
            {
                success = done;
            }
        }

        success = false;
    }

    /// <summary>
    /// 移动距离
    /// </summary>
    /// <param name="speed">移动速度</param>
    /// <returns></returns>
    public static float CalculateMoveDistance(float speed)
    {
        return speed * Time.deltaTime;
    }

    public static void StopMove2D(Rigidbody2D rb)
    {
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// 转向
    /// </summary>
    /// <param name="self">需要旋转的本体</param>
    /// <param name="targetPos">需要朝向的目标，可为空</param>
    /// <param name="rotateSpeed">旋转的速率</param>
    /// <param name="isTurnDone">是否旋转完成</param>
    /// <param name="direction">旋转朝向（可选）</param>
    public static void LookAt2D(Transform self,
        float rotateSpeed,
        out bool isTurnDone,
        Vector2? targetPos = null,
        Vector2 direction = default)
    {
        // 1. 计算方向
        Vector2 dir = targetPos.HasValue
            ? (targetPos.Value - (Vector2)self.position).normalized
            : direction.normalized;
        // 2. 计算目标角度
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float currentAngle = self.eulerAngles.z;
        // 3. 平滑旋转
        currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotateSpeed * Time.deltaTime);
        self.rotation = Quaternion.Euler(0, 0, currentAngle);
        // 4. 判断是否转向完成（给 out 赋值）
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(self.eulerAngles.z, targetAngle));
        isTurnDone = angleDiff < .1f;
        if (isTurnDone)
            self.rotation = Quaternion.Euler(0, 0, targetAngle);
    }
}