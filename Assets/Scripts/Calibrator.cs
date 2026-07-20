using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calibrator : MonoBehaviour
{
    public Song calibrationSong;
    public TMPro.TMP_Text offsetText;
    public ArcadeInputType exitCalibrator;

    private List<float> _offsets = new List<float>();
    
    void Start()
    {
        Metronome.Instance.audioSource.mute = false;
        AppManager.Instance.PlaySong(calibrationSong, false);
    }
    
    void Update()
    {
        if (ArcadeInput.InputInitiated(0, exitCalibrator))
        {
            PlayerPrefs.SetFloat("killswitchInputOffset", AppManager.Instance.inputOffset);
            SceneManager.LoadScene("MainMenu");
            return;
        }
        
        if (ArcadeInput.AnyInputInitiated(0))
        {
            float offset = Metronome.Instance.NearestBeatOffset();
            _offsets.Add(offset);
            //Debug.Log(offset);
        }
        
        float averageOffset = AverageOffset();
        offsetText.text = $"Average offset: {averageOffset:0.00} ms";
        AppManager.Instance.inputOffset = averageOffset;
    }

    private float AverageOffset()
    {
        float sum = 0;
        foreach (float offset in _offsets)
        {
            sum += offset;
        }
        float avg = sum / _offsets.Count;
        return float.IsNaN(avg) ? 0 : avg;
    }
}
