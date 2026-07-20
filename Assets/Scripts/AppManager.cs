using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;
    
    public float inputOffset;

    public Song mainMenuSong;

    public Song[] allSongs;
    
    public Level[] levels;
    private int _currentLevelIndex;
    private bool _onMainMenu;

    private bool _onArcadeMachine;
    private Song _currentSong;

    public Level CurrentLevel => levels[_currentLevelIndex];

    [HideInInspector]
    public int score;
    [HideInInspector]
    public int hiScore;
    private int _scoreMultiplier;
    public int maxMultiplier;

    private void Awake()
    {
        if (!Instance)
        {
            // Runs the first time main menu is loaded, i.e. on game start
            score = 0;
            hiScore = PlayerPrefs.GetInt("killswitchHiScore", 0);
            inputOffset = PlayerPrefs.GetFloat("killswitchInputOffset", 0);
            _scoreMultiplier = 1;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ArcadeInput.Initialize();
            // TODO: Make dependent on Arcade Machine, if possible
            #if !UNITY_WEBGL
                SceneManager.LoadSceneAsync("LEDPanel", LoadSceneMode.Additive);
            #endif
        }
        else
        {
            // Runs every time main menu is loaded, except the first.
            if (Instance.score > Instance.hiScore)
            {
                Instance.hiScore = Instance.score;
                PlayerPrefs.SetInt("killswitchHiScore", Instance.hiScore);
            }
            Destroy(gameObject);
        }
        
        // Runs every time main menu is loaded
        _onMainMenu = true;
    }

    private void Start()
    {
        PlaySong(mainMenuSong);
    }

    private void Update()
    {
        if (Metronome.Instance.audioSource.isPlaying == false
            && _currentSong.loop)
        {
            if (!_onMainMenu)
            {
                PlaySong(_currentSong);
            }
            else
            {
                PlayRandomSong();
            }
        }

        if (UIManager.Instance)
        {
            UIManager.Instance.multiplierText.text = "Multiplier: " + _scoreMultiplier + "x";
            UIManager.Instance.scoreText.text = "Score: " + score;
        }
    }

    public void PlaySong(Song song, bool useInputOffset = true)
    {
        _currentSong = song;
        
        AudioSource audioSource = Metronome.Instance.audioSource;
        // Initialize metronome and audioSource
        Metronome.Instance.Song = song;
        if (!useInputOffset)
        {
            Metronome.Instance.InitializeValues(false);
        }
        audioSource.clip = song.audioClip;
        
        #if !UNITY_WEBGL
        // Initialize LED Panel
        if (PanelController.Instance)
        {
            PanelController.Instance.song = song;
            PanelController.Instance.audioSource = audioSource;
            PanelController.Instance.UpdateSongDetails();
            PanelController.Instance.InitializeVisualizer();
        }
        #endif

        // Play
        audioSource.Play();
        
    }

    public Song PlayRandomSong()
    {
        int songIndex = Random.Range(0, allSongs.Length);
        Song song = allSongs[songIndex];
        PlaySong(song);
        return song;
    }

    public void PlayCurrentLevelSong()
    {
        Level level = CurrentLevel;
        if (level.randomSong)
        {
            level.song = PlayRandomSong();
        }
        else if (level.song)
        {
            PlaySong(level.song);
        }
        else
        {
            Debug.LogError("Level " + level.levelPrettyName + "has no song and is not set to random.");
        }
    }

    public void StartFirstLevel()
    {
        Metronome.Instance.audioSource.mute = false;
        _onMainMenu = false;
        _currentLevelIndex = 0;
        score = 0;
        levels[0].Load();
    }

    public void NextLevel()
    {
        _currentLevelIndex++;
        if (_currentLevelIndex < levels.Length)
        {
            levels[_currentLevelIndex].Load();
        }
        else
        {
            // Win
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void OnBeatMiss()
    {
        _scoreMultiplier = 1; 
    }

    public void IncreaseScore(int amount)
    {
        score += amount * _scoreMultiplier;
        _scoreMultiplier = Mathf.Clamp(_scoreMultiplier + 1, 1, maxMultiplier);
    }
}
