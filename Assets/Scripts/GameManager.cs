using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    [SerializeField] DartTagPlayer player;
    [SerializeField] int enemyCount = 1;
    [SerializeField] int maxEnemies = 10;
    [SerializeField] List<Transform> spawnPositions;
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] List<DartTagPlayer> dartTagPlayers;
    bool gameEnded;
    float gameLength = 300f;
    float gameTime = 0f;
    public int gameMinutes { get; private set; }
    public int gameSeconds { get; private set; }
    [SerializeField] GameObject endGameMenu;
    public event Action OnGameEnd = delegate { };
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        if (PlayerPrefs.HasKey("enemyCount"))
        {
            enemyCount = PlayerPrefs.GetInt("enemyCount");
        }
        dartTagPlayers.Add(player);
        var random = new System.Random();
        var randomInd = random.Next(enemyPrefabs.Count);
        for (int i = 0; i < enemyCount; i++)
        {
            var go = Instantiate(enemyPrefabs[randomInd], spawnPositions[i]);
            dartTagPlayers.Add(go.GetComponent<DartTagPlayer>());
            randomInd = random.Next(enemyPrefabs.Count);
        }

        var randomItInd = random.Next(dartTagPlayers.Count);
        dartTagPlayers[randomItInd].GetShot(true);
    }
    void Update()
    {
        if (!gameEnded)
        {
            gameMinutes = Mathf.FloorToInt(gameTime / 60);
            gameSeconds = Mathf.RoundToInt(gameTime % 60);

            if (gameTime == gameLength)
            {
                EndGame();
            }
            else
            {
                gameTime = Mathf.Clamp(gameTime + Time.deltaTime, 0, gameLength);
            }
        }
    }

    void EndGame()
    {
        gameEnded = true;
        OnGameEnd();
        Cursor.lockState = CursorLockMode.None;
        SaveScores();
        endGameMenu.SetActive(true);
        StartCoroutine(FadeInEndMenu());
    }

    IEnumerator FadeInEndMenu()
    {
        var canvasGroup = endGameMenu.GetComponent<CanvasGroup>();
        var seconds = 0f;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, seconds);
            seconds += Time.deltaTime;
            yield return null;
        }
        Cursor.lockState = CursorLockMode.None;
    }

    void SaveScores()
    {
        if (PlayerPrefs.HasKey("leaderboard"))
        {
            var leaderboardPairs = new List<string>(PlayerPrefs.GetString("leaderboard").Split(','));
            var names = new List<string>();
            var scores = new List<int>();
            foreach (var leaderboardPair in leaderboardPairs)
            {
                string[] split = leaderboardPair.Split(':');
                names.Add(split[0]);
                scores.Add(Int32.Parse(split[1]));
            }

            foreach (var p in dartTagPlayers)
            {
                int score = Mathf.RoundToInt(p.score);
                for (int i = 0; i < scores.Count; i++)
                {
                    if (score > scores[i])
                    {
                        scores.Insert(i, score);
                        names.Insert(i, p.gameObject.name);
                        break;
                    }
                }
                scores.Add(score);
                names.Add(p.gameObject.name);
            }

            var newLeaderboard = "";
            var numScoresToSave = names.Count < 10 ? names.Count : 10;
            for (int i = 0; i < numScoresToSave; i++)
            {
                newLeaderboard += String.Format("{0}:{1}", names[i], scores[i]);
                if (i != numScoresToSave - 1)
                {
                    newLeaderboard += ",";
                }
            }
            PlayerPrefs.SetString("leaderboard", newLeaderboard);
        }
        else
        {
            dartTagPlayers.Sort((a, b) => b.score.CompareTo(a.score));

            var newLeaderboard = "";
            var numScoresToSave = dartTagPlayers.Count < 10 ? dartTagPlayers.Count : 10;

            for (int i = 0; i < numScoresToSave; i++)
            {
                newLeaderboard += String.Format("{0}:{1}", dartTagPlayers[i].name, Mathf.RoundToInt(dartTagPlayers[i].score));
                if (i != numScoresToSave - 1)
                {
                    newLeaderboard += ",";
                }
            }
            PlayerPrefs.SetString("leaderboard", newLeaderboard);
        }
    }
}
