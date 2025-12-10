using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using XNode.Examples;

public class PanelController : MonoBehaviour
{
    public static PanelController Instance;
    
    public MarqueeText titleText;
    public MarqueeText artistText;
    public Image coverImage;
    public Animator coverAnimator;
    public bool animate;
    [Tooltip("Must be a power of two and not less than 64 or larger than 8192.")]
    public int spectrumSampleCount = 64;
    public RectTransform visualizerPrefab;
    public int visualizerBarCount;
    public HorizontalLayoutGroup visualizerLayoutGroup;
    
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public Song song;
    
    private float _smoothMaxValue = 0.01f;
    private bool _visualizerInitiated = false;
    private RectTransform[] _visualizerBars;

    public void Awake()
    {
        Metronome.Instance.onBeat += OnBeat;
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
            if (!ValidateValues())
            {
                Debug.LogError("Invalid values provided to PanelController", gameObject);
                this.enabled = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateVisualizer();
    }

    private void OnDestroy()
    {
        Metronome.Instance.onBeat -= OnBeat;
    }

    private void OnBeat(int beat)
    {
        if (!animate) return;
        coverAnimator.Play("OnBeat");
    }
    
    private bool ValidateValues()
    {
        if (!Mathf.IsPowerOfTwo(spectrumSampleCount)
            || spectrumSampleCount < 64
            || spectrumSampleCount > 8192)
        {
            return false;
        }
        
        return true;
    }
    
    public void UpdateSongDetails()
    {
        titleText.text.text = song.title;
        titleText.Initialize();
        artistText.text.text = song.artist;
        artistText.Initialize();
        coverImage.sprite = song.cover;
    }

    public void InitializeVisualizer()
    {
        if (_visualizerInitiated) return;
        _visualizerBars = new RectTransform[visualizerBarCount];
        for (int i = 0; i < visualizerBarCount; i++)
        {
            _visualizerBars[i] = Instantiate(visualizerPrefab, visualizerLayoutGroup.transform);
        }
        
        _visualizerInitiated = true;
    }
    
    public void UpdateVisualizer()
    {
        if (!_visualizerInitiated) { InitializeVisualizer(); }
        float[] spectrum = new float[spectrumSampleCount];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        
        float[] binAverages = new float[visualizerBarCount];
        float maxAverage = float.MinValue;
        for (int i = 0; i < visualizerBarCount; i++)
        {
            
            // Perceptual spacing
            float minFreq = 20f; //Hz
            float maxFreq = 20_000f; //Hz
            float sampleRate = audioSource.clip.frequency;
            
            float freqStart = minFreq * Mathf.Pow(maxFreq/minFreq, (float)i / visualizerBarCount);
            float freqEnd = minFreq * Mathf.Pow(maxFreq/minFreq, (float)(i+1) / visualizerBarCount);
            
            int startIndex = Mathf.FloorToInt(freqStart / (sampleRate / 2f) * spectrumSampleCount);
            int endIndex = Mathf.FloorToInt(freqEnd / (sampleRate / 2f) * spectrumSampleCount);
            
            startIndex = Mathf.Clamp(startIndex, 0, spectrum.Length - 1);
            endIndex = Mathf.Clamp(endIndex, startIndex + 1, spectrum.Length);
        
            /*
            // Simple log binning
            int startIndex = (int)Mathf.Pow(2, i * Mathf.Log(spectrumSampleCount, 2) / visualizerBarCount);
            int endIndex = (int)Mathf.Pow(2, (i + 1) * Mathf.Log(spectrumSampleCount, 2) / visualizerBarCount);

            endIndex = Mathf.Min(endIndex, spectrum.Length - 1);
            if (endIndex <= startIndex)
            {
                endIndex = startIndex + 1;
            }
            */
            
            float total = 0;
            for (int j = startIndex; j < endIndex; j++)
            {
                total += spectrum[j];
            }

            float avg = total / (endIndex - startIndex);
            if (avg > maxAverage) { maxAverage = avg; }
            binAverages[i] = avg;

        }

        _smoothMaxValue = Mathf.Lerp(_smoothMaxValue, maxAverage, 0.1f);
        _smoothMaxValue = Mathf.Max(_smoothMaxValue, 0.0001f);
        
        for (int i = 0; i < visualizerBarCount; i++)
        {
            float normalizedValue = Mathf.Clamp01(binAverages[i] / _smoothMaxValue);
            _visualizerBars[i].localScale = new Vector3(1, normalizedValue, 1);
        }
    }
}
