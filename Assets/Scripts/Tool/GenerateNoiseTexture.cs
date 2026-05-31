
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 动态生成噪波图片
/// </summary>
[RequireComponent(typeof(MeshRenderer))]//必须有这个对象
public class GenerateNoiseTexture : MonoBehaviour
{
    public int textureSize = 256;
    public float noiseSacle = 20f;//噪波缩放
    public int octaves = 3; //噪波的层级
    public string filename = "Texture/test.png";
    Texture2D GeneratePictureNoiseTexture()
    {
        Texture2D texture2D = new Texture2D(textureSize, textureSize, TextureFormat.RGB24, false);
        texture2D.wrapMode = TextureWrapMode.Repeat;
        texture2D.filterMode = FilterMode.Bilinear;
        Color[] pixels = new Color[textureSize * textureSize];
        for (int i = 0; i < textureSize; i++)
        {
            for (int j = 0; j < textureSize; j++)
            {
                float xCoord = (float)i / textureSize * noiseSacle;
                float yCoord = (float)j / textureSize * noiseSacle;
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);
                //增加多层噪波，丰富一下细节
                for (int k = 1; k < octaves; k++)
                {
                    float frequency = Mathf.Pow(2, k);
                    float amplitude = Mathf.Pow(.5f, k);
                    noiseValue += Mathf.PerlinNoise(xCoord * frequency, yCoord * frequency) * amplitude;
                }
                //归一化
                noiseValue = Mathf.Clamp01(noiseValue);
                //设置灰度颜色 R=G=B=噪波值
                pixels[j * textureSize + i] = new Color(noiseValue, noiseValue, noiseValue);
            }
        }
        texture2D.SetPixels(pixels);
        texture2D.Apply();
        return texture2D;
    }
    void saveTextureToFiles(Texture2D texture, string filename)
    {
        Texture2D texture2D = GeneratePictureNoiseTexture();
        byte[] bytes = texture2D.EncodeToPNG();
        string path = Application.dataPath + "/" + filename;
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log("保存成功");
    }

    public void saveTextureToFiles()
    {
        Texture2D texture2D = GeneratePictureNoiseTexture();
        byte[] bytes = texture2D.EncodeToPNG();
        string path = Application.dataPath + "/" + filename;
        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log("保存成功");
    }
}
