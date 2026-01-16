using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Debug = MyDebugLogger;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoSingleton<AudioManager>
{
    [Header("設定")]
    [SerializeField]
    private SoundDatabase soundDatabase;
    [SerializeField]
    private AudioMixerGroup bgmMixerGroup;
    [SerializeField]
    private AudioMixerGroup seMixerGroup;

    [Header("オーディオソース設定")]
    [SerializeField]
    private int seSourcePoolSize = 10;

    private AudioSource bgmSource;
    private AudioSource[] seSources;
    private int currentSeIndex = 0;

    private BGM_ID? currentBgmID;
    private Coroutine bgmFadeCoroutine;

    protected override void Awake()
    {
        Initialize();
        
    }

    private void Initialize()
    {
        soundDatabase.Initialize();

        // BGM用AudioSource作成
        bgmSource = gameObject.GetComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.outputAudioMixerGroup = bgmMixerGroup;

        // SE用AudioSourceプール作成
        seSources = new AudioSource[seSourcePoolSize];
        for (int i = 0; i < seSourcePoolSize; i++)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
            seSources[i].playOnAwake = false;
            seSources[i].outputAudioMixerGroup = seMixerGroup;
        }
    }

    public void PlayBGM(BGM_ID id, bool fade = true, float fadeTime = 1f)
    {
        if (currentBgmID == id && bgmSource.isPlaying) return;

        var data = soundDatabase.GetBGMData(id);
        if (data == null || data.AudioClip == null)
        {
            Debug.LogWarning($"BGM not found: {id}");
            return;
        }

        if (bgmFadeCoroutine != null)
        {
            StopCoroutine(bgmFadeCoroutine);
        }

        if (fade)
        {
            bgmFadeCoroutine = StartCoroutine(CrossFadeBGM(data, fadeTime));
        }
        else
        {
            PlayBGMImmediate(data);
        }

        currentBgmID = id;
    }

    private void PlayBGMImmediate(BGMData data)
    {
        bgmSource.clip = data.AudioClip;
        bgmSource.volume = data.Volume;
        bgmSource.pitch = data.Pitch;
        bgmSource.loop = data.Loop;
        bgmSource.priority = data.Priority;
        if (data.AudioMixerGroup != null)
        {
            bgmSource.outputAudioMixerGroup = data.AudioMixerGroup;
        }
        bgmSource.Play();
    }

    private IEnumerator CrossFadeBGM(BGMData newData, float fadeTime)
    {
        float startVolume = bgmSource.volume;

        // フェードアウト
        if (bgmSource.isPlaying)
        {
            float elapsed = 0f;
            while (elapsed < fadeTime)
            {
                elapsed += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeTime);
                yield return null;
            }
        }

        // 新しいBGMをセット
        bgmSource.clip = newData.AudioClip;
        bgmSource.pitch = newData.Pitch;
        bgmSource.loop = newData.Loop;
        bgmSource.priority = newData.Priority;
        if (newData.AudioMixerGroup != null)
        {
            bgmSource.outputAudioMixerGroup = newData.AudioMixerGroup;
        }
        bgmSource.Play();

        // フェードイン
        float targetVolume = newData.Volume;
        float elapsed2 = 0f;
        while (elapsed2 < fadeTime)
        {
            elapsed2 += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, elapsed2 / fadeTime);
            yield return null;
        }

        bgmSource.volume = targetVolume;
        bgmFadeCoroutine = null;
    }

    public void StopBGM(bool fade = true, float fadeTime = 1f)
    {
        if (fade)
        {
            if (bgmFadeCoroutine != null) StopCoroutine(bgmFadeCoroutine);
            bgmFadeCoroutine = StartCoroutine(FadeOutBGM(fadeTime));
        }
        else
        {
            bgmSource.Stop();
        }
        currentBgmID = null;
    }

    private IEnumerator FadeOutBGM(float fadeTime)
    {
        float startVolume = bgmSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeTime);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.volume = startVolume;
        bgmFadeCoroutine = null;
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }

    // ============================================
    // SE再生（2D）
    // ============================================
    public void PlaySE(SE_ID id)
    {
        var data = soundDatabase.GetSEData(id);
        if (data == null || data.AudioClip == null)
        {
            Debug.LogWarning($"SE not found: {id}");
            return;
        }

        AudioSource source = GetAvailableSESource();
        ConfigureSESource(source, data, Vector3.zero, false);
        source.Play();
    }

    // ============================================
    // SE再生（3D）
    // ============================================
    public void PlaySE3D(SE_ID id, Vector3 position)
    {
        var data = soundDatabase.GetSEData(id);
        if (data == null || data.AudioClip == null)
        {
            Debug.LogWarning($"SE not found: {id}");
            return;
        }

        if (!data.Is3D)
        {
            Debug.LogWarning($"SE {id} is not configured for 3D playback");
        }

        // 3D音響用の一時的なGameObjectを作成
        GameObject tempObj = new GameObject($"TempAudio_{id}");
        tempObj.transform.position = position;

        AudioSource source = tempObj.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = data.AudioMixerGroup ?? seMixerGroup;
        ConfigureSESource(source, data, position, true);
        source.Play();

        // 再生終了後に自動削除
        Destroy(tempObj, data.AudioClip.length / data.Pitch + 0.1f);
    }

    // AudioSourceに永続的に追従させる3D再生
    public AudioSource PlaySE3DAttached(SE_ID id, Transform parent)
    {
        var data = soundDatabase.GetSEData(id);
        if (data == null || data.AudioClip == null)
        {
            Debug.LogWarning($"SE not found: {id}");
            return null;
        }

        GameObject tempObj = new GameObject($"Audio_{id}");
        tempObj.transform.SetParent(parent);
        tempObj.transform.localPosition = Vector3.zero;

        AudioSource source = tempObj.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = data.AudioMixerGroup ?? seMixerGroup;
        ConfigureSESource(source, data, parent.position, true);
        source.Play();

        if (!data.Loop)
        {
            Destroy(tempObj, data.AudioClip.length / data.Pitch + 0.1f);
        }

        return source;
    }

    private void ConfigureSESource(AudioSource source, SEData data, Vector3 position, bool force3D)
    {
        source.clip = data.AudioClip;
        source.volume = data.Volume;
        source.loop = data.Loop;
        source.priority = data.Priority;

        // ピッチのランダム化
        if (data.RandomPitch)
        {
            source.pitch = data.Pitch + Random.Range(-data.PitchVariation, data.PitchVariation);
        }
        else
        {
            source.pitch = data.Pitch;
        }

        // 3D設定
        bool use3D = force3D || data.Is3D;
        source.spatialBlend = use3D ? data.SpatialBlend : 0f;

        if (use3D)
        {
            source.maxDistance = data.MaxDistance;
            source.rolloffMode = AudioRolloffMode.Linear;
        }
    }

    private AudioSource GetAvailableSESource()
    {
        // 再生中でないソースを探す
        for (int i = 0; i < seSources.Length; i++)
        {
            int index = (currentSeIndex + i) % seSources.Length;
            if (!seSources[index].isPlaying)
            {
                currentSeIndex = (index + 1) % seSources.Length;
                return seSources[index];
            }
        }

        // 全て再生中なら最も古いものを使う
        AudioSource source = seSources[currentSeIndex];
        currentSeIndex = (currentSeIndex + 1) % seSources.Length;
        return source;
    }

    // ============================================
    // ボリューム制御
    // ============================================
    public void SetBGMVolume(float volume)
    {
        if (bgmMixerGroup != null)
        {
            bgmMixerGroup.audioMixer.SetFloat("BGMVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
        }
    }

    public void SetSEVolume(float volume)
    {
        if (seMixerGroup != null)
        {
            seMixerGroup.audioMixer.SetFloat("SEVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
        }
    }

    public void SetMasterVolume(float volume)
    {
        if (bgmMixerGroup != null)
        {
            bgmMixerGroup.audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f);
        }
    }
}