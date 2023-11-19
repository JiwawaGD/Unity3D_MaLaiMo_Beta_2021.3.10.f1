using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class testaudAss : MonoBehaviour
{
    [SerializeField] AssetReference soundAddressableName ;

    void Start()
    {
        LoadAndPlaySound();
    }

    void LoadAndPlaySound()
    {
        // 使用Addressables API 非同步加載音效
        Addressables.LoadAssetAsync<AudioClip>(soundAddressableName).Completed += OnSoundLoaded;
    }

    void OnSoundLoaded(AsyncOperationHandle<AudioClip> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // 獲取加載的音效
            AudioClip audioClip = handle.Result;

            // 播放音效
            AudioSource.PlayClipAtPoint(audioClip, transform.position);

            // 釋放資源
            Addressables.Release(handle);
        }
        else
        {
            Debug.LogError("Failed to load sound from Addressables: " + handle.DebugName);
        }
    }
}
