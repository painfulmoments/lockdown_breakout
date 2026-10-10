using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteAudioForSeconds : MonoBehaviour
{
    public float muteDuration = 1.0f; // 静音持续时间
    private List<AudioSource> audioSources;

    void Start()
    {
        // 获取场景中所有的AudioSource组件
        audioSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());

        // 静音所有音频源
        MuteAllAudio(true);

        // 在指定时间后恢复音频
        StartCoroutine(UnmuteAfterSeconds(muteDuration));
    }

    void MuteAllAudio(bool mute)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = mute;
        }
    }

    IEnumerator UnmuteAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        MuteAllAudio(false);
    }
}
