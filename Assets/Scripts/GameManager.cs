using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public EnemySpawnDescriptor[] enemySpawns;

    public bool Running { private set; get; }

    private List<Enemy> _enemies;
    private bool _gameOver = true;

    private void Awake()
    {
        Metronome.Instance.onBeat += OnBeat;
        _enemies = new List<Enemy>();
        Enemy[] presetEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy presetEnemy in presetEnemies)
        {
            _enemies.Add(presetEnemy);
        }
        //Debug.Log(_enemies.Count);
    }

    private void OnDestroy()
    {
        Metronome.Instance.onBeat -= OnBeat;
    }

    private void Start()
    {
        OnPlayerHealthChanged(); // Update health display
        
        StartGame();
    }

    public void StartGame()
    {
        SpawnEnemies();

        AppManager.Instance.PlayCurrentLevelSong();
        
        _gameOver = false;
        Running = false;
    }

    private void Update()
    {
        if (!Running && !_gameOver &&
            Metronome.Instance.audioSource.time > AppManager.Instance.CurrentLevel.song.levelStartTime)
        {
            Running = true;
        }
        
        // Has to iterate entire list each frame, move out of update if possible
        CleanEnemyList();

        if (Input.GetKey(KeyCode.Escape))
        {
            // Implement pause later. For now it is basically a surrender
            EndGame(false);
        }

        if (_enemies.Count == 0 && !_gameOver)
        {
            // Win
            EndGame(true);
        }
    }
    
    private void EndGame(bool playerWon)
    {
        _gameOver = true;
        Running = false;
        if (playerWon)
        {
            AppManager.Instance.NextLevel();
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    
    private void SpawnEnemies()
    {
        foreach (EnemySpawnDescriptor enemySpawn in enemySpawns)
        {
            for (int i = 0; i < enemySpawn.count; i++)
            {
                if (enemySpawn.randomSpawn)
                {
                    SpawnEnemy(enemySpawn.prefab, enemySpawn.randomSpawnRadius);
                }
                else
                {
                    SpawnEnemy(enemySpawn.prefab, enemySpawn.position);
                }
            }
        }
    }
    
    private void SpawnEnemy(Enemy prefab, float radius)
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        Vector3 pos = new Vector3(x, y, 0f).normalized * radius;
        SpawnEnemy(prefab, pos);
    }
    
    private void SpawnEnemy(Enemy prefab, Vector3 position)
    {
        Enemy enemy = Instantiate(prefab, position, Quaternion.identity);
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
        #if UNITY_EDITOR
            UIManager.Instance.beatText.text = "Debug | Beat: " + lastBeat;
        #else
            UIManager.Instance.beatText.text = "";
        #endif
    }

    public void OnPlayerHealthChanged()
    {
        UIManager.Instance.healthText.text = "Health: " + player.Health;
        if (player.Health <= 0)
        {
            // Lose
            EndGame(false);
        }
    }
}

[System.Serializable]
public class EnemySpawnDescriptor
{
    public int count = 1;
    public Enemy prefab;
    public bool randomSpawn;
    [Tooltip("Ignored if Random Spawn is false")]
    public float randomSpawnRadius;
    [Tooltip("Ignored if Random Spawn is true")]
    public Vector2 position;
    
}
