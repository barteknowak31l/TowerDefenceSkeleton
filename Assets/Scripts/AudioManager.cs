using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
        [SerializeField] AudioSource musicSource;
        public AudioClip musicClip;
    private void Start()
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
