using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "Scriptable Objects/Song")]
public class Song : ScriptableObject
{
    public AudioClip audioClip;
    public float bpm;
    public float offset;
}
