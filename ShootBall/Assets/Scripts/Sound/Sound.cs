using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    NONE,
    BACKGROUND_SOUND,   // 배경 음악
    INGAME_SOUND        // 게임에서 나는 모든 소리(배경 음악 제외)
}

class SoundChannel
{
    public SoundType soundType;
    public AudioSource audioSource;

    public void Stop()
    {
        if (audioSource == null)
            return;

        audioSource.Stop();
    }

    public void Play()
    {
        if (audioSource == null)
            return;

        audioSource.Play();
    }

    public bool IsPlaying()
    {
        if (audioSource == null) 
            return false;

        return audioSource.isPlaying;
    }
}

public class Sound : MonoSingleTon<Sound>
{
    const int channelCount = 10;
    SoundChannel[] soundChannels = null;
    SoundChannel soundBGMChannel = null;

    Dictionary<string, AudioClip> soundResourceDic = new Dictionary<string, AudioClip>();

    protected override void Init()
    {
        soundResourceDic.Clear();
        soundChannels = new SoundChannel[channelCount];

        for(int i = 0; i < channelCount; i++)
        {
            soundChannels[i] = new SoundChannel();
            soundChannels[i].audioSource = gameObject.AddComponent<AudioSource>();
            soundChannels[i].soundType = SoundType.INGAME_SOUND;
            soundChannels[i].audioSource.loop = false;
            soundChannels[i].audioSource.playOnAwake = false;
            soundChannels[i].audioSource.volume = 1f;
        }

        soundBGMChannel = new SoundChannel();
        soundBGMChannel.audioSource = gameObject.AddComponent<AudioSource>();
        soundBGMChannel.soundType = SoundType.BACKGROUND_SOUND;
        soundBGMChannel.audioSource.loop = true;
        soundBGMChannel.audioSource.playOnAwake = false;
        soundBGMChannel.audioSource.volume = 1f;
    }

    public override void StartSingleTon()
    {

    }

    public void PlayClip(string clip, SoundType type)
    {
        int channelIndex = FindAvailableChannel();

        if (channelIndex == -1)
        {
            Debug.Log("sound file not exits");
            return;
        }

        if(IsPossiblePlaySound(type) == false)
        {
            Debug.Log("not use sound : " + type);
            return;
        }

        var clipOb = GetClip(clip);

        if (type == SoundType.BACKGROUND_SOUND)
        {
            PlayBGM(clipOb);
        }
        else
        {
            PlayInGameSound(channelIndex, clipOb);
        }
    }

    void PlayBGM(AudioClip clip)
    {
        soundBGMChannel.audioSource.clip = clip;
        soundBGMChannel.Play();
    }

    void PlayInGameSound(int index, AudioClip clip)
    {
        soundChannels[index].audioSource.clip = clip;
        soundChannels[index].Play();
    }

    public void StopBGM()
    {
        soundBGMChannel.Stop();
    }

    public void StopAllSound()
    {
        StopBGM();

        for(int i = 0; i < channelCount; i++)
        {
            soundChannels[i].Stop();
        }
    }

    AudioClip GetClip(string clip)
    {
        if (clip == null)
            return null;

        if (soundResourceDic.ContainsKey(clip))
            return soundResourceDic[clip];

        var clipComp = Resources.Load(clip) as AudioClip;

        if (clipComp == null)
            return null;

        soundResourceDic.Add(clip, clipComp);
        return clipComp;
    }

    int FindAvailableChannel()
    {
        for(int i = 0; i < channelCount; i++)
        {
            if (soundChannels[i].audioSource == null || soundChannels[i].IsPlaying())
                continue;
            return i;
        }
        return -1;
    }

    protected virtual bool IsPossiblePlaySound(SoundType type)
    {
        switch (type)
        {
            case SoundType.BACKGROUND_SOUND:
                {
                    if (GlobalData.soundSettings.backgroundSound == false)
                    {
                        return false;
                    }
                }
                break;
            case SoundType.INGAME_SOUND:
                {
                    if (GlobalData.soundSettings.inGameSound == false)
                    {
                        return false;
                    }
                }
                break;
            default:
                break;
        }
        return true;
    }
}
