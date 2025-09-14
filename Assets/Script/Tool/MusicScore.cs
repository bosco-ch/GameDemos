using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class MusicScore
{
    public float beatTimes; //����ʱ���
    public float currectProcess;//��ǰ����
    public float durationTime;//��������ʱ����Ҫ������ʱ��//��ʵ������������ʱ�� --- ����һ��ڿ������ȥʵ�ֱȽϺ�,��ʱ���ǵ��� ��������,���������Ļ�,��Ҫ�ӿ�����,���ܻ�Ӱ�����
    public string lineLevels;

    public bool LoogPress;//�Ƿ񳤰�
    public int interferType;//����������:�Ʋ�,����,��ʱ����Կ��ǹ���ƣ������Ϳ������ɹ��λ��
    public string sayWhenSpecial;//��������ʱ��,������������,���������
    public Vector3 AIPosition;//�Է���ҵ�λ��2,0,6(��)  2,0,18(��) 2,0,50(Զ)
    public Vector3 point1;//���������� ��1  -1.07,3,6 1.2,3,6   (��:������)     -1.4,3,10 . 0.3,3,17 (��:������) -1.4,3,9  0.3,3,36 (Զ:������)
    public Vector3 point2;//���������� ��2 �����޸�Ball�ķ�������
    public string flyLevel;//���еȼ�������ȷ����ķ���·��
}
public class MusicInfo
{
    public string ID;
    public string Name;//��������
    public string Author;//����
    public string ResFileUrl;//ֻ��¼ resoreces������ļ�λ��

    public List<MusicScore> ScoreList;
    public MusicInfo()
    {
        ScoreList = new List<MusicScore>();
    }
}
