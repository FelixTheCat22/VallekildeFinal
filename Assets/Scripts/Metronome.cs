using UnityEngine;

public class Metronome : MonoBehaviour
{
    // Time is in milliseconds
    
    public static Metronome Instance;
    
    public AudioSource audioSource;
    [Tooltip("Margin on error on either side of the beat, in ms")]
    public int margin;
    public int MaybeBeat { get; private set; } // -1 when not valid timing
    public int BeatCounter { get ; private set;}

    public delegate void OnBeat(int lastBeat);
    public event OnBeat onBeat;

    public int NearedBeatCounter
    {
        get
        {
            float nboffset = NearestBeatOffset();
            if (nboffset < 0)
            {
                return BeatCounter - 1;
            }
            return BeatCounter;
        }
    }

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

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void InitializeValues(bool useInputOffset = true)
    {
        BeatCounter = 0;
        _beatDuration = 60f / Song.bpm * 1000f;
        _lastBeat = 0;
        _nextBeatPosition = _beatDuration + Song.offset;
        float inputOffset = useInputOffset ? 
            !AppManager.Instance ? -60 : AppManager.Instance.inputOffset 
            : 0;
        _nextBeatPosition -= inputOffset;
    }

    // Negative if just after beat, positive if just before beat
    public float NearestBeatOffset()
    {
        float previousBeatPosition = _nextBeatPosition - _beatDuration;
        float position = audioSource.time * 1000f;
        
        float distanceToPrevious = position - previousBeatPosition;
        float distanceToNext = _nextBeatPosition - position;

        if (distanceToPrevious < distanceToNext)
        {
            return -distanceToPrevious;
        }
        else
        {
            return distanceToNext;
        }
    }

    private void Update()
    {
        if (!audioSource.isPlaying) return;
        
        float position = audioSource.time * 1000f;
        if (position >= _nextBeatPosition)
        {
            // Beat
            _lastBeat = (_lastBeat + 1) % 4; // 0-indexed
            _lastBeatTimeoutPosition = _nextBeatPosition + margin;
            _nextBeatPosition += _beatDuration;
            BeatCounter++;
            onBeat?.Invoke(_lastBeat);
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
