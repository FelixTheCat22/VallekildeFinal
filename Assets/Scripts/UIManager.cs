using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text healthText;
    public TMP_Text beatText;
    public TMP_Text levelText;

    private void Awake()
    {
        Instance = this;
        /*
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        */
    }
}
