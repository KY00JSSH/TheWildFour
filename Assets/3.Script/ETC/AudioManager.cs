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
    public enum Bgm
    {
        music_GoFarGoWideSnowDesert, music_IndifferentSlow, Title_music_LastSummer
    }

    [Header("#SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayer;
    int channel_Index;
    public enum Sfx
    {
        TreeDeath, TreeFall, TreePain, StoneFall, StoneShatterd, 
        Eat, PlayerThrow, BuildingBuild, BuildingPackup, CraftingItem, 
        CampfireIdle, CampfireKindle, wind_1, SkillAcquire, WeaponEquip, 
        PlayerAttack, StepSnow, PlayerFall, PlayerDeath_LostAndWintered, 
        NoHP_heartbeat, morning, night4
    }

    private void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BGMPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = new AudioSource[channels];

        for (int index = 0; index < bgmPlayer.Length; index++)
        {
            bgmPlayer[index] = bgmObject.AddComponent<AudioSource>();
            bgmPlayer[index].playOnAwake = false;
            bgmPlayer[index].volume = bgmVolume;
            bgmPlayer[index].loop = true;
        }             
        //bgmPlayer.clip = bgmClip;

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SFXPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayer = new AudioSource[channels];

        for(int index = 0; index < sfxPlayer.Length; index++)
        {
            sfxPlayer[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayer[index].playOnAwake = false;
            sfxPlayer[index].volume = sfxVolume;
        }

    }

    public void PlaySFX(Sfx sfx)
    {
        //사용시, AudioManager.instance.PlaySFX(AudioManaer.Sfx.ClipName); 사용

        for(int i = 0; i < sfxPlayer.Length; i++)
        {
            int loop_Index = (i + channel_Index) % sfxPlayer.Length;

            if (sfxPlayer[loop_Index].isPlaying)
                continue;

            channel_Index = loop_Index;
            sfxPlayer[loop_Index].clip = sfxClip[(int)sfx];
            sfxPlayer[loop_Index].Play();
            break;
        }        
    }

    public void PlayBGM(Bgm bgm)
    {
        //사용시, AudioManager.instance.PlayBGM(AudioManager.Bgm.ClipName); 사용
        //정지시, AudioManager.instance.StopBGM();

        // 현재 재생 중인 BGM이 같은 경우 실행하지 않음
        foreach (AudioSource player in bgmPlayer)
        {
            if (player.isPlaying && player.clip == bgmClip[(int)bgm])
                return;
        }

        // 새로운 BGM 재생을 위한 채널 선택
        for (int i = 0; i < bgmPlayer.Length; i++)
        {
            // 현재 재생 중인 다른 BGM 중지
            if (bgmPlayer[i].isPlaying)
            {
                bgmPlayer[i].Stop();
            }

            // 해당 채널에 새로운 BGM 할당 및 재생
            bgmPlayer[i].clip = bgmClip[(int)bgm];
            bgmPlayer[i].Play();
            break; // 새로운 BGM을 하나만 재생하므로 루프 탈출
        }
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
