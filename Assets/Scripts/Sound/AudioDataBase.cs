using UnityEngine;
using UnityEngine.Audio;

public abstract class AudioDataBase : ScriptableObject
{
    [Header("Šî–{Ý’è")]
    [SerializeField]
    private AudioClip audioClip;
    public AudioClip AudioClip => audioClip;

    [SerializeField]
    private AudioMixerGroup audioMixerGroup;
    public AudioMixerGroup AudioMixerGroup => audioMixerGroup;


    [Header("Ä¶Ý’è")]
    [SerializeField, Range(0f, 1f)]
    private float volume = 1f;
    public float Volume => volume;

    [SerializeField, Range(0.1f, 3f)]
    private float pitch = 1f;
    public float Pitch => pitch;

    [Header("Ú×Ý’è")]
    [SerializeField, Range(0, 256)]
    private int priority = 128;
    public int Priority => priority;
}
