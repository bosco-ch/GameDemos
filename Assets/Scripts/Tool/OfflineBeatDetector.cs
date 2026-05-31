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

        // 1. 识别主要节拍间隔
        float tempo = EstimateTempo(rawTimes); // BPM估计

        // 2. 根据预估BPM修正检测点
        for (int i = 0; i < rawTimes.Count; i++)
        {
            float expectedTime = i * 60f / tempo;

            // 允许±100ms时间窗
            float tolerance = 0.1f;
            if (Mathf.Abs(rawTimes[i] - expectedTime) < tolerance)
            {
                processed.Add(expectedTime); // 对齐到理论节拍点
            }
        }

        return processed;
    }
    float EstimateTempo(List<float> beatTimes)
    {
        // 计算时间间隔
        List<float> intervals = new List<float>();
        for (int i = 1; i < beatTimes.Count; i++)
        {
            intervals.Add(beatTimes[i] - beatTimes[i - 1]);
        }

        // 找出最可能的时间间隔（中位数）
        intervals.Sort();
        float medianInterval = intervals[intervals.Count / 2];

        return 60f / medianInterval; // 转换为BPM
    }
    //解决方案2：
    /// <summary>
    /// 根据已知的BPM来获得节拍
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="secondPerBeat"></param>
    /// <returns></returns>
    public List<float> GenerateTimeLine(AudioSource audioSource, float secondPerBeat)
    {
        List<float> timeLine = new();
        float duration = audioSource.clip.length;//歌曲总的长度；
        //float beatInterVal = secondPerBeat / 4;
        for (float time = 0f; time < duration; time += secondPerBeat)
        {
            timeLine.Add(time);
        }
        return timeLine;
    }
}

