using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip[] bgmClip;
    public float bgmVolume;
    AudioSource[] bgmPlayer;
    //public int bgmChannels;
    public enum Bgm
    {
        music_GoFarGoWideSnowDesert, music_IndifferentSlow, Title_music_LastSummer
    }

    [Header("#SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    //public int sfxChannels;
    AudioSource[] sfxPlayer;
    int sfxChannel_Index;
    public enum Sfx
    {
        morning, night4, TreeDeath, TreeFall, TreePain, StoneFall, StoneShatterd, 
        Eat, BuildingBuild, BuildingPackup, CraftingItem, RabbitPain, loading,
        CampfireIdle, CampfireKindle, wind_1, SkillAcquire, WeaponEquip, 
        PlayerAttack, StepSnow, PlayerFall, PlayerDeath_LostAndWintered, 
        NoHP_heartbeat
    }

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        Init();
    }

    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BGMPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = new AudioSource[bgmClip.Length];

        Debug.Log("BGM Channels: " + bgmPlayer.Length);

        for (int index = 0; index < bgmPlayer.Length; index++)
        {
            bgmPlayer[index] = bgmObject.AddComponent<AudioSource>();
            bgmPlayer[index].playOnAwake = false;
            bgmPlayer[index].volume = bgmVolume;
            bgmPlayer[index].loop = true;
        }
        Debug.Log("BGM Player Array Length: " + bgmPlayer.Length);

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayer = new AudioSource[sfxClip.Length];

        for(int index = 0; index < sfxPlayer.Length; index++)
        {
            sfxPlayer[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayer[index].playOnAwake = false;
            sfxPlayer[index].volume = sfxVolume;
        }
    }

    public void PlaySFX(Sfx sfx)
    {
        //사용시, AudioManager.instance.PlaySFX(AudioManager.Sfx.ClipName); 사용

        for(int i = 0; i < sfxPlayer.Length; i++)
        {
            int loop_Index = (i + sfxChannel_Index) % sfxPlayer.Length;

            if (sfxPlayer[loop_Index].isPlaying)
                continue;

            sfxChannel_Index = loop_Index;
            sfxPlayer[loop_Index].clip = sfxClip[(int)sfx];
            sfxPlayer[loop_Index].Play();
            break;
        }        
    }

    public void StopSFX(Sfx sfx) // 특정 SFX 종료
    {
        //사용시, AudioManager.instance.StopSFX(AudioManager.Sfx.CampfireIdle); 사용

        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            if (sfxPlayer[i].isPlaying && sfxPlayer[i].clip == sfxClip[(int)sfx])
            {
                sfxPlayer[i].Stop();
                break;
            }
        }
    }

    public void PlayBGM(Bgm bgm, int channel)
    {
        //사용시, AudioManager.instance.PlayBGM(AudioManager.Bgm.ClipName); 사용
        //정지시, AudioManager.instance.StopBGM();

        if (channel < 0 || channel >= bgmPlayer.Length)
        {
            Debug.LogError("BGM채널 인덱스 없음");
            return;
        }

        AudioSource bgmSource = bgmPlayer[channel];

        // 현재 재생 중인 BGM이 같은 경우 실행하지 않음
        if (bgmSource.isPlaying && bgmSource.clip == bgmClip[(int)bgm])
            return;

        // 현재 재생 중인 BGM이 있으면 중지
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }

        // 새로운 BGM 재생
        bgmSource.clip = bgmClip[(int)bgm];
        bgmSource.Play();
    }
    public void StopBGM()
    {
        foreach (AudioSource player in bgmPlayer)
        {
            if (player.isPlaying)
            {
                player.Stop();
            }
        }
    }
}
    
