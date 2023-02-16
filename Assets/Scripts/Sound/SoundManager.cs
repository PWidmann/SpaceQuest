using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region Members
    public static SoundManager instance;
    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceSFX;
    private float timer = 0;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.5f)
        {
            audioSourceSFX.volume = GameManager.SfxVolume / 100;
            audioSourceMusic.volume = GameManager.MusicVolume / 100;
        }
    }
    #endregion

    #region Public Methods
    public void SetVolumeMusic(float value)
    {
        audioSourceMusic.volume = value;
    }
    public void SetVolumeSFX(float value)
    {
        audioSourceSFX.volume = value;
    }
    public void PlayMusic(int index)
    {
        if (audioSourceMusic.clip != null)
        {
            audioSourceMusic.Stop();
        }
        
        audioSourceMusic.clip = musicClips[index];
        audioSourceMusic.Play();
    }
    public void PlaySFX(int index)
    {
        audioSourceSFX.PlayOneShot(sfxClips[index]);
    }
    #endregion
}