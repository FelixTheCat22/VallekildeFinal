using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    // Eventually, move scene loading from GameManager to here
    
    public static AppManager Instance;
    
    public float inputOffset;

    public Song mainMenuSong;
    
    public Level[] levels;
    private int _currentLevelIndex;

    private bool _onArcadeMachine;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ArcadeInput.Initialize();
            // TODO: Make dependent on Arcade Machine
            SceneManager.LoadSceneAsync("LEDPanel", LoadSceneMode.Additive);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        PlaySong(mainMenuSong);
    }

    public void PlaySong(Song song, bool useInputOffset = true)
    {
        AudioSource audioSource = Metronome.Instance.audioSource;
        // Initialize metronome and audioSource
        Metronome.Instance.Song = song;
        if (!useInputOffset)
        {
            Metronome.Instance.InitializeValues(false);
        }
        audioSource.clip = song.audioClip;
        
        // Initialize LED Panel
        PanelController.Instance.song = song;
        PanelController.Instance.audioSource = audioSource;
        PanelController.Instance.UpdateSongDetails();
        PanelController.Instance.InitializeVisualizer();
        
        // Play
        audioSource.Play();
        
    }

    public void StartFirstLevel()
    {
        _currentLevelIndex = 0;
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
}
