using System;
using Unity.VisualScripting;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    // Time is in milliseconds
    
    public AudioSource audioSource;
    [Tooltip("Margin on error on either side of the beat, in ms")]
    public int margin;
    [HideInInspector]
    public Song song;
    private float beatDuration;
    private float lastBeat;
    private float nextBeatPosition;
    private float lastBeatTimeoutPosition;
    
    public TMPro.TMP_Text beatText;


    private void InitializeValues()
    {
        beatDuration = 60f / song.bpm * 1000f;
        lastBeat = 0;
        nextBeatPosition = beatDuration + song.offset;
    }

    private void Start()
    {
        InitializeValues();
    }
    
    private void Update()
    {
        float position = audioSource.time * 1000f;
        if (position >= nextBeatPosition)
        {
            lastBeat = (lastBeat + 1) % 4; // 0-indexed
            nextBeatPosition += beatDuration;
            beatText.text = lastBeat.ToString();
        }
    }
}
