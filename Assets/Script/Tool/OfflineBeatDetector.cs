using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class OfflineBeatDetector
{
    public List<float> DetectBeats(AudioClip audioClip, float threshold = 1.5f)
    {
        List<float> beatsTimes = new List<float>();
        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);
        int windowSize = 1024;
        int windows
             = samples.Length / windowSize;
        float[] energies = new float[windows];
        for (int w = 0; w < windows; w++)
        {
            float energy = 0;
            for (int i = 0; i < windowSize; i++)
            {
                int index = w * windowSize + i;
                energy += samples[index] * samples[index];
            }
            energies[w] = energy / windowSize;
        }

        //check beat
        float previousEnergy = energies[0];
        for (int i = 1; i < energies.Length; i++)
        {
            if (energies[i] > previousEnergy * threshold)
            {
                float time = (i * windowSize) / (float)audioClip.frequency;
                beatsTimes.Add(time);
            }
            previousEnergy = energies[i];
        }

        return beatsTimes;
    }

    public List<float> PostProcessBeats(List<float> rawTimes)
    {

        List<float> processed = new List<float>();

        // 1. ʶ����Ҫ���ļ��
        float tempo = EstimateTempo(rawTimes); // BPM����

        // 2. ����Ԥ��BPM��������
        for (int i = 0; i < rawTimes.Count; i++)
        {
            float expectedTime = i * 60f / tempo;

            // �����100msʱ�䴰
            float tolerance = 0.1f;
            if (Mathf.Abs(rawTimes[i] - expectedTime) < tolerance)
            {
                processed.Add(expectedTime); // ���뵽���۽��ĵ�
            }
        }

        return processed;
    }
    float EstimateTempo(List<float> beatTimes)
    {
        // ����ʱ����
        List<float> intervals = new List<float>();
        for (int i = 1; i < beatTimes.Count; i++)
        {
            intervals.Add(beatTimes[i] - beatTimes[i - 1]);
        }

        // �ҳ�����ܵ�ʱ��������λ����
        intervals.Sort();
        float medianInterval = intervals[intervals.Count / 2];

        return 60f / medianInterval; // ת��ΪBPM
    }
    //�������2��
    /// <summary>
    /// ������֪��BPM����ý���
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="secondPerBeat"></param>
    /// <returns></returns>
    public List<float> GenerateTimeLine(AudioSource audioSource, float secondPerBeat)
    {
        List<float> timeLine = new();
        float duration = audioSource.clip.length;//�����ܵĳ��ȣ�
        //float beatInterVal = secondPerBeat / 4;
        for (float time = 0f; time < duration; time += secondPerBeat)
        {
            timeLine.Add(time);
        }
        return timeLine;
    }
}

