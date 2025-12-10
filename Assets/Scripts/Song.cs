using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "Scriptable Objects/Song")]
public class Song : ScriptableObject
{
    public string title;
    public string artist;
    public Sprite cover;
    public AudioClip audioClip;
    public float bpm;
    public float offset;
}
