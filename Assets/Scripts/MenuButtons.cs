using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public TMP_Text offsetText;
    
    public void StartGame()
    {
        AppManager.Instance.StartGame();
    }

    public void LoadCalibrator()
    {
        AppManager.Instance.StartCalibrator();
    }

    public void QuitGame()
    {
        AppManager.Instance.QuitGame();
    }

    private void Update()
    {
        offsetText.text = AppManager.Instance.inputOffset + " ms";
    }
}
