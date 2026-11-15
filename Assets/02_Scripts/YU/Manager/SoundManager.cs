using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager :  MonoBehaviour
{
    int audioSourceCount = 30;
    List<AudioSource> audioSources = new();

    AudioSource bgmSource; // BGM 전용, 풀과 별개

    Dictionary<string, AudioClip> audios = new();

    public void Init()
    {
        for (int i = 0; i < audioSourceCount; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSources.Add(source);
            source.playOnAwake = false;
        }

        // BGM 전용 소스 별도 생성
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;

        List<AudioClip> audioClips = Managers.Resource.GetAssets<AudioClip>();

        foreach (var clip in audioClips)
        {
            audios.Add(clip.name, clip);
        }
    }

    public void PlaySFX(string name)
    {
        if (!audios.TryGetValue(name, out AudioClip clip))
            return;

        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying && source.clip == clip)
                return;
        }

        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.Play();
                break;
            }
        }
    }

    public void PlayBGM(string name)
    {
        if (!audios.TryGetValue(name, out AudioClip clip))
            return;

        // 같은 곡이 이미 재생 중이면 다시 틀 필요 없음
        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return;

        bgmSource.Stop();      // 이전 BGM 명시적으로 정지
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}
