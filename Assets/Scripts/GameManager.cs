using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Song song;
    public Metronome metronome;
    public AudioSource audioSource;
    
    public TMPro.TMP_Text beatText;
    
    private void Start()
    {
        metronome.Song = song;
        audioSource.clip = song.audioClip;
        audioSource.Play();
    }

    public void OnBeat(int lastBeat) // Called by Metronome every beat
    {
        beatText.text = lastBeat.ToString();
    }
}
