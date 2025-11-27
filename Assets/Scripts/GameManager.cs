using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Song song;
    public Metronome metronome;
    public AudioSource audioSource;
    public Player player;
    public Enemy enemyPrefab;
    
    public TMPro.TMP_Text beatText;
    
    private List<Enemy> _enemies;
    private bool _gameOver = true;

    private void Awake()
    {
        _enemies = new List<Enemy>();
    }

    public void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            SpawnEnemy();
        }
        
        StartSong();
        
        _gameOver = false;
    }

    private void Update()
    {
        // O(n), move out of update if possible
        CleanEnemyList();

        if (_enemies.Count == 0 && !_gameOver)
        {
            // Win
            _gameOver = true;
            audioSource.Stop();
            player.transform.position = new Vector3(0f, 0f, -1f);
            AppManager.Instance.MainMenu();
        }
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

    private void CleanEnemyList()
    {
        _enemies.RemoveAll(e => !e);
    }

    public void OnBeat(int lastBeat) // Called by Metronome every beat
    {
        beatText.text = lastBeat.ToString();
    }
}
