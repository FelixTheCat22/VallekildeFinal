using UnityEngine;

public class TestScript : MonoBehaviour
{
    public AudioSource audioSource;
    
    public TMPro.TMP_Text dspText;
    public TMPro.TMP_Text timeText;

    void Update()
    {
        Debug.Log("dsp:  " + AudioSettings.dspTime + "\ntime: " + audioSource.time);

        dspText.text = "dsp: " + AudioSettings.dspTime;
        timeText.text = "time: " + audioSource.time;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.Stop();
            audioSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (audioSource.isPlaying) {
                audioSource.Pause();
            } else
            {
                audioSource.Play();
            }
        }
    }
}
