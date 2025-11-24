using System;
using Unity.VisualScripting;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    // Seems to sometimes be a bit off. Hardware lag? TODO: Figure out why
    
    // Time is in milliseconds
    
    public AudioSource audioSource;
    public GameManager gameManager;
    [Tooltip("Margin on error on either side of the beat, in ms")]
    public int margin;
    public int MaybeBeat { get; private set; } // -1 when not valid timing

    private Song _song;
    private float _beatDuration;
    private int _lastBeat;
    private float _nextBeatPosition;
    private float _lastBeatTimeoutPosition;

    public Song Song
    {
        get => _song;
        set {
            _song = value;
            InitializeValues();
        }
    }
    
    private void InitializeValues()
    {
        _beatDuration = 60f / Song.bpm * 1000f;
        _lastBeat = 0;
        _nextBeatPosition = _beatDuration + Song.offset;
    }

    private void Update()
    {
        float position = audioSource.time * 1000f;
        if (position >= _nextBeatPosition)
        {
            // Beat
            _lastBeat = (_lastBeat + 1) % 4; // 0-indexed
            _lastBeatTimeoutPosition = _nextBeatPosition + margin;
            _nextBeatPosition += _beatDuration;
            gameManager.OnBeat(_lastBeat);
        }

        if (position >= _lastBeatTimeoutPosition)
        {
            MaybeBeat = -1;
        }
        if (position >= _nextBeatPosition - margin)
        {
            MaybeBeat = (_lastBeat + 1) % 4;
        }
        
    }
}
