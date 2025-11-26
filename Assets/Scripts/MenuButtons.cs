using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public TMP_Text offsetText;
    
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void LoadCalibrator()
    {
        SceneManager.LoadScene("Calibration");
    }

    private void Update()
    {
        offsetText.text = AppManager.Instance.inputOffset + " ms";
    }
}
