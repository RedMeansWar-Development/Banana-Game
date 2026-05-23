using System.Collections;
using BananaGame.BananaTime;
using BananaTimeTravel.BananaTime;
using UnityEngine;

namespace BananaGame.Managers;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Crossfade")]
    public float crossfadeDuration = 1.5f;

    private AudioSource _musicSource;
    private AudioSource _ambientSource;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Two AudioSources: one for music, one for ambient
        var sources = GetComponents<AudioSource>();
        _musicSource = sources.Length > 0 ? sources[0] : gameObject.AddComponent<AudioSource>();
        _ambientSource = sources.Length > 1 ? sources[1] : gameObject.AddComponent<AudioSource>();

        _musicSource.loop = true;
        _ambientSource.loop = true;
    }

    private void Start()
    {
        if (TimeController.Instance != null)
            TimeController.Instance.OnEraChanged += OnEraChanged;
    }

    private void OnDestroy()
    {
        if (TimeController.Instance != null)
            TimeController.Instance.OnEraChanged -= OnEraChanged;
    }

    private void OnEraChanged(EraDefinition era)
    {
        if (era.musicClip != null) StartCoroutine(Crossfade(_musicSource, era.musicClip));
        if (era.ambientClip != null) StartCoroutine(Crossfade(_ambientSource, era.ambientClip));
    }

    private IEnumerator Crossfade(AudioSource source, AudioClip newClip)
    {
        float elapsed = 0f;
        float startVol = source.volume;

        // Fade out
        while (elapsed < crossfadeDuration / 2f)
        {
            source.volume = Mathf.Lerp(startVol, 0f, elapsed / (crossfadeDuration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        source.clip = newClip;
        source.Play();
        elapsed = 0f;

        // Fade in
        while (elapsed < crossfadeDuration / 2f)
        {
            source.volume = Mathf.Lerp(0f, startVol, elapsed / (crossfadeDuration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        source.volume = startVol;
    }
}

