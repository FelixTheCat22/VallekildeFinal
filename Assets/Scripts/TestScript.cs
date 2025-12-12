using UnityEngine;

public class TestScript : MonoBehaviour
{
    public AudioSource audioSource;
    private int _frameCounter;
    private double _dspTimeMusicStart;
    private float _lastTime;
    private float _avgDelay;
    private int _measurementCount;
    private float _delayTotal;

    void Start()
    {
        audioSource.Play();
    }
    
    void Update()
    {
        float currTime = audioSource.time;
        if (currTime > _lastTime)
        {
            _measurementCount++;
            _delayTotal += currTime - _lastTime;
            _lastTime = currTime;
        }

        if ((_frameCounter & 0x0f) == 0)
        {
            Debug.Log((_delayTotal/_measurementCount) - Time.deltaTime);
        }
        
        /*
        _frameCounter++;
        Debug.Log("Frame: " + _frameCounter);
        Debug.Log("DeltaTime: " + Time.deltaTime);
        Debug.Log("Time: " + audioSource.time);
        Debug.Log("TimeSamples: " +  (float) audioSource.timeSamples / audioSource.clip.frequency);
        Debug.Log("dspTime: " + (AudioSettings.dspTime - _dspTimeMusicStart));
        */
    }
}
