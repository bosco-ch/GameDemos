using UnityEngine;

//数据检测
public static class Detector
{
    /// <summary>
    /// 视角射线
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dirtion"></param>
    /// <param name="distance"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static RaycastHit2D RayCast(Vector2 pos, Vector2 dirtion, float distance, LayerMask layer)
    {
        return Physics2D.Raycast(pos
            , dirtion
            , distance
            , layer);
    }
        
    public static Collider2D CheckPlayerInCircle(Vector2 pos, float viewPos, LayerMask layer)
    {
        return Physics2D.OverlapCircle(
            pos,
            viewPos,
            layer
        );
    }
}