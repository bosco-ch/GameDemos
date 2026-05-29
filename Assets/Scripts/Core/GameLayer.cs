using UnityEngine;

public static class GameLayer
{
    public static readonly int Player = LayerMask.NameToLayer("player");

    public static readonly int Wall = LayerMask.NameToLayer("wall");
    public static readonly int Enemy = LayerMask.NameToLayer("enemy");


    public static readonly LayerMask PlayerMask = 1 << Player;
    public static readonly LayerMask WallMask = 1 << Wall;
    public static readonly LayerMask EnemyMask = 1 << Enemy;

    public static readonly LayerMask PlayerOrWall = PlayerMask | WallMask;
    public static readonly LayerMask WallOrEnemy = WallMask |EnemyMask;
    public static readonly LayerMask PlayerOrWallOrEnemy = PlayerMask | WallMask |EnemyMask;
}