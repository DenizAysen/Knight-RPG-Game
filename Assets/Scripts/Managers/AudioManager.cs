using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private AudioSource menuMusic, gameMusic;
    [SerializeField] private AudioSource[] sfx; 
    #endregion
    #region Singleton
    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion
    public void PlayMenuMusic()
    {
        if (gameMusic.isPlaying)
        {
            gameMusic.Stop();
        }
        menuMusic.Play();
    }
    public void PlayGameMusic()
    {
        if (menuMusic.isPlaying)
        {
            menuMusic.Stop();
        }
        gameMusic.Play();
    }
    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
    public bool IsSFXPlaying(int playingSFX) => sfx[playingSFX].isPlaying;
}
