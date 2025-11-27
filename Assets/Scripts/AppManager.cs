using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    // Eventually, move scene loading from GameManager to here
    
    public static AppManager Instance;

    public float inputOffset;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        if (!SceneManager.GetSceneByName("MainGame").isLoaded)
        {
            SceneManager.LoadScene("MainGame");
        }
        SceneManager.UnloadSceneAsync("MainMenu");
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!gm)
        {
            Debug.LogError("GameManager not found. Maybe MainGame scene failed to load?");
            return;
        }
        gm.StartGame();
    }

    public void StartCalibrator()
    {
        SceneManager.LoadScene("Calibration");
    }

    public void MainMenu()
    {
        if (!SceneManager.GetSceneByName("MainGame").isLoaded)
        {
            SceneManager.LoadScene("MainGame");
        }
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
