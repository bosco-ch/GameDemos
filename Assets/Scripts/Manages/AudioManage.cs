using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AudioManage : Singleton<AudioManage>
{
    private AudioClip _bgmClip;
    private AudioSource _audioSource;
    protected override void Awake()
    {
        base.Awake();
        if (this.transform.parent == null || this.transform.parent != GameRoot.Instance.transform)
        {
            this.transform.SetParent(GameRoot.Instance.transform);
        }
        if (_audioSource == null)
            _audioSource = this.gameObject.AddComponent<AudioSource>();
        Addressables.InitializeAsync();
    }
    public void PlayOneShot()
    {
        // if (audioSource == null)
        // {
        //     audioSource = this.gameObject.AddComponent<AudioSource>();
        // }
        // audioSource.PlayOneShot(bgmClip);
        //热加载
        Addressables.LoadAssetAsync<AudioClip>("click").Completed += (handle) =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                _audioSource.PlayOneShot(handle.Result);
            }
        };
    }

}
