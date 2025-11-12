using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Song song;
    public Metronome metronome;
    public AudioSource audioSource;
    
    private void Start()
    {
        metronome.song = song;
        audioSource.clip = song.audioClip;
        audioSource.Play();
    }
}
