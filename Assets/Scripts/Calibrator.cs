using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calibrator : MonoBehaviour
{
    public AudioSource audioSource;
    public Metronome metronome;
    public Song calibrationSong;
    public TMPro.TMP_Text offsetText;
    
    private List<float> _offsets = new List<float>();

    void Start()
    {
        metronome.Song = calibrationSong;
        metronome.InitializeValues(false);
        audioSource.clip = calibrationSong.audioClip;
        audioSource.Play();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            float offset = metronome.NearestBeatOffset();
            _offsets.Add(offset);
            Debug.Log(offset);
        }
        
        float averageOffset = AverageOffset();
        offsetText.text = averageOffset + " ms";
        AppManager.Instance.inputOffset = averageOffset;
    }

    private float AverageOffset()
    {
        float sum = 0;
        foreach (float offset in _offsets)
        {
            sum += offset;
        }
        return sum / _offsets.Count;
    }
}
