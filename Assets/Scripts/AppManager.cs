using UnityEngine;

public class AppManager : MonoBehaviour
{
    // Eventually, move scene loading from GameManager to here
    
    public static AppManager Instance;

    public float inputOffset;
    
    private void Awake()
    {
        Instance = this;
    }
}
