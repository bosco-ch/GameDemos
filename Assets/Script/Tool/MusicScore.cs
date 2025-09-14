using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class MusicScore
{
    public float beatTimes; //击打时间点
    public float currectProcess;//当前进度
    public float durationTime;//当长按的时候需要持续的时间//其实更多算是蓄力时间 --- 这点我还在考虑如何去实现比较好,暂时考虑的是 蓄力还击,但是蓄力的话,就要加快球速,可能会影响节奏
    public string lineLevels;

    public bool LoogPress;//是否长按
    public int interferType;//干扰物类型:云层,树林,有时候可以考虑广告牌，这样就可以做成广告位了
    public string sayWhenSpecial;//特殊球类时候,发送语音文字,最好上配音
    public Vector3 AIPosition;//对方玩家的位置2,0,6(近)  2,0,18(中) 2,0,50(远)
    public Vector3 point1;//贝塞尔曲线 点1  -1.07,3,6 1.2,3,6   (近:两个点)     -1.4,3,10 . 0.3,3,17 (中:两个点) -1.4,3,9  0.3,3,36 (远:两个点)
    public Vector3 point2;//贝塞尔曲线 点2 用来修改Ball的飞行曲线
    public string flyLevel;//飞行等级，用来确定球的飞行路线
}
public class MusicInfo
{
    public string ID;
    public string Name;//音乐名称
    public string Author;//作者
    public string ResFileUrl;//只记录 resoreces后面的文件位置

    public List<MusicScore> ScoreList;
    public MusicInfo()
    {
        ScoreList = new List<MusicScore>();
    }
}
