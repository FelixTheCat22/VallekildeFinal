using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
    public string levelSceneName;
    public string levelPrettyName;
    public bool randomSong;
    public Song song;

    public void Load()
    {
        SceneManager.LoadScene(levelSceneName);
        SceneManager.LoadScene("LevelShared", LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LevelShared")
        {
            UIManager.Instance.levelText.text = levelPrettyName;
            
            SceneManager.sceneLoaded -= OnSceneLoad;
        }
    }
}
