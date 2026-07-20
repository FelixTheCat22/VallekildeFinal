using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public LayoutElement quitButtonReplacement;
    public Image muteButton;
    public Sprite unMuted;
    public Sprite muted;
    public TMP_Text offsetText;
    public TMP_Text hiScoreText;

    #if UNITY_WEBGL
    private void Awake()
    {
        quitButton.gameObject.SetActive(false);
        quitButtonReplacement.gameObject.SetActive(true);
    }
    #endif
    
    private void Start()
    {
        if (AppManager.Instance.inputOffset == 0) // Game is not calibrated
        {
            startButton.interactable = false;
            SetStartButtonText("Please Calibrate");
        }
        else
        {
            startButton.interactable = true;
            SetStartButtonText("Start");
        }
    }
    
    private void SetStartButtonText(string text)
    {
        startButton.GetComponentInChildren<TMP_Text>().text = text;
    }
    
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

    public void ToggleMute()
    {
        AudioSource audioSource = Metronome.Instance.audioSource;
        audioSource.mute = !audioSource.mute;
        if (audioSource.mute)
        {
            muteButton.sprite = muted;
        }
        else
        {
            muteButton.sprite = unMuted;
        }
    }
    
    private void Update()
    {
        offsetText.text = $"Calibration: {AppManager.Instance.inputOffset:0.00} ms";
        hiScoreText.text = $"High Score: {AppManager.Instance.hiScore}";
    }
}
