using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public TMP_Text offsetText;
    public TMP_Text hiScoreText;
    
    public void StartGame()
    {
        AppManager.Instance.StartFirstLevel();
    }

    public void LoadCalibrator()
    {
        SceneManager.LoadScene("Calibration");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        offsetText.text = "Calibration: " + AppManager.Instance.inputOffset + " ms";
        hiScoreText.text = "High Score: " + AppManager.Instance.hiScore;
    }

    
}
