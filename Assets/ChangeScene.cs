using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �����л�����
/// </summary>
public class ChangeScene : MonoBehaviour
{
    private AsyncOperation asyncOperation;

    public int index;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(asyncOperation.progress);
    }

    IEnumerator LoadScene(int index)
    {
        asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitForSeconds(1f);
    }

    public void UpdateScene()
    {
        StartCoroutine(LoadScene(index));
    }

    public void ActiveScene()
    {
        asyncOperation.allowSceneActivation = true;
    }
}