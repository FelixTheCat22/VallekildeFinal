using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    // Eventually, move scene loading from GameManager to here
    
    public static AppManager Instance;
    
    public float inputOffset;
    
    public Level[] levels;
    private int _currentLevelIndex;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ArcadeInput.Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
        
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
