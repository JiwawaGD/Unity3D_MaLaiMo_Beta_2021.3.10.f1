using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

public class MoveMode : MonoBehaviour
{
    private float targetIntensity = 1f; // �ؼбj�׭�
    private float currentIntensity = 0.3f; // ��e�j�׭�
    public float changeSpeed = 1f; // �j�ק��ܳt��
    //[Header("�����ܼ�")] public Volume postProcessVolume;
    GameManager gameManager;
    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }
    public void StartMoveMode()
    {
        StartCoroutine(ChangeVignetteIntensity());
    }
    public IEnumerator ChangeVignetteIntensity()
    {
        Volume postProcessVolume = GameManager.instance.postProcessVolume;

        VolumeProfile profile = postProcessVolume.sharedProfile;

        if (profile.TryGet<Vignette>(out Vignette vignette) &&
            profile.TryGet<CloudLayer>(out CloudLayer cloudLayer))
        {
            float currentIntensity = 0.738f;
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                vignette.intensity.value = Mathf.Lerp(currentIntensity,
                                            targetIntensity, elapsedTime);
                vignette.smoothness.value = Mathf.Lerp(currentIntensity, 
                                            targetIntensity, elapsedTime);
                vignette.roundness.value = Mathf.Lerp(currentIntensity, 
                                            targetIntensity, elapsedTime);
                cloudLayer.opacity.value = Mathf.Lerp(currentIntensity, 
                                            targetIntensity, elapsedTime);

                elapsedTime += Time.deltaTime * changeSpeed;
                yield return null;
            }
            gameManager.playerCtrlr.m_bCanControl = true;
            gameManager.playerCtrlr.m_bLimitRotation = false;
            KeepstoryReadding();
        }
    }
    public void KeepstoryReadding()
    {
        Volume postProcessVolume = GameManager.instance.postProcessVolume;

        VolumeProfile profile = postProcessVolume.sharedProfile;
        CloudLayer cloudLayer = null;

        if (!profile.TryGet<Vignette>(out var vignette) &&
            !profile.TryGet<CloudLayer>(out cloudLayer))
        {
            vignette = profile.Add<Vignette>(false);
            cloudLayer = profile.Add<CloudLayer>(false);
        }
        vignette.intensity.value = .1f;
        vignette.smoothness.value = 0;
        vignette.roundness.value = 0;
        if (cloudLayer != null)
        {
            cloudLayer.opacity.value = 0;
        }
    }
}
