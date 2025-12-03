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
    public int enemySpawnCount;
    public TMPro.TMP_Text healthText;
    
    public TMPro.TMP_Text beatText;
    
    private List<Enemy> _enemies;
    private bool _gameOver = true;

    private void Awake()
    {
        _enemies = new List<Enemy>();
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        for (int i = 0; i < enemySpawnCount; i++)
        {
            SpawnEnemy();
        }
        
        StartSong();
        
        _gameOver = false;
    }

    private void Update()
    {
        // Has to iterate entire list each frame, move out of update if possible
        CleanEnemyList();

        if (Input.GetKey(KeyCode.CapsLock))
        {
            // Implement pause later. For now it is basically a surrender
            _gameOver = true;
            ResetGame();
            SceneManager.LoadScene("MainMenu");
        }

        if (_enemies.Count == 0 && !_gameOver)
        {
            // Win
            _gameOver = true;
            ResetGame();
            SceneManager.LoadScene("MainMenu");
        }
    }
    
    private void StartSong()
    {
        metronome.Song = song;
        audioSource.clip = song.audioClip;
        audioSource.Play();
    }

    private void ResetGame()
    {
        player.ResetHealth();
        audioSource.Stop();
        player.transform.position = new Vector3(0f, 0f, -1f);
        foreach (Enemy enemy in _enemies)
        {
            Destroy(enemy.gameObject);
            CleanEnemyList();
        }
    }

    private void SpawnEnemy()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        Vector3 pos = new Vector3(x, y, 0f).normalized * 9;
        SpawnEnemy(pos);
    }
    
    private void SpawnEnemy(Vector3 position)
    {
        Enemy enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        enemy.player = player;
        //enemy.gameManager = this;
        _enemies.Add(enemy);
    }

    private void CleanEnemyList()
    {
        _enemies.RemoveAll(e => !e);
    }

    public void OnBeat(int lastBeat) // Called by Metronome every beat
    {
        beatText.text = "Debug | Beat: " + lastBeat;
    }

    public void OnPlayerHealthChanged()
    {
        healthText.text = "Health: " + player.Health;
        if (player.Health <= 0)
        {
            // Lose
            _gameOver = true;
            ResetGame();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
