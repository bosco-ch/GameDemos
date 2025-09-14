using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// ≤‚ ‘«–ªª≥°æ∞
/// </summary>
public class ChangeScence : MonoBehaviour
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
        Debug.Log(asyncOperation.progress);
    }
    IEnumerator LoadScence(int index)
    {
        asyncOperation = SceneManager.LoadSceneAsync(index);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitForSeconds(1f);
    }
    public void updateScence()
    {
        StartCoroutine(LoadScence(index));
    }
    public void ActiveScence()
    {
        asyncOperation.allowSceneActivation = true;
    }
}
