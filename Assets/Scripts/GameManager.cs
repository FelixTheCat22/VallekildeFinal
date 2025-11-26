using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Song song;
    public Metronome metronome;
    public AudioSource audioSource;
    public Player player;
    public Enemy enemyPrefab;
    
    public TMPro.TMP_Text beatText;
    
    private List<Enemy> _enemies;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        _enemies = new List<Enemy>();
        //DontDestroyOnLoad(gameObject);
        
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    public void StartGame()
    {
        SceneManager.UnloadSceneAsync("MainMenu");
        
        for (int i = 0; i < 5; i++)
        {
            SpawnEnemy();
        }
        
        StartSong();
    }

    private void Update()
    {
        // O(n), move out of update if possible
        CleanEnemyList();
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

        if (lastBeat == 0 && _enemies.Count < 8)
        {
            SpawnEnemy();
        }
    }
}
