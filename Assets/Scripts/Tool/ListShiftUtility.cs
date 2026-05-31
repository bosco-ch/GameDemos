using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于控制list内部的成员的位置
/// 前面的可以到后面去，后面可以前面来
/// </summary>
public class ListShiftUtility<T>
{
    public static void moveFront(List<T> list)
    {
        if (list.Count == 0) return;
        T firstElement = list[0];
        list.RemoveAt(0);
        list.Add(firstElement);
    }
    public static void moveBack(List<T> list)
    {
        if (list.Count == 0) return;
        T lastElement = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        list.Insert(0, lastElement);
    }
}
