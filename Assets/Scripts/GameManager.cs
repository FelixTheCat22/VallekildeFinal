using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Song song;
    public Metronome metronome;
    public AudioSource audioSource;
    public Player player;
    public Enemy enemyPrefab;
    
    public TMPro.TMP_Text beatText;

    public Vector2 EnemyAvgPos {private set; get;}
    
    private List<Enemy> _enemies;

    private void Awake()
    {
        _enemies = new List<Enemy>();
    }
    
    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemy();
        }
        
        StartSong();
    }
    
    private void StartSong()
    {
        metronome.Song = song;
        audioSource.clip = song.audioClip;
        audioSource.Play();
    }

    private void SpawnEnemy()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        Vector3 pos = new Vector3(x, y, 0f).normalized * 7;
        SpawnEnemy(pos);
    }
    
    private void SpawnEnemy(Vector3 position)
    {
        Enemy enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemy.player = player;
        enemy.gameManager = this;
        _enemies.Add(enemy);
    }

    public void OnBeat(int lastBeat) // Called by Metronome every beat
    {
        beatText.text = lastBeat.ToString();

        if (lastBeat == 0 && _enemies.Count < 8)
        {
            SpawnEnemy();
        }
    }
}
