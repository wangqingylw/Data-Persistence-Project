using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverObj;
    public GameObject GameOverHigh;
    public GameObject GameOverTips;
    public TMP_InputField PlayerNameInput;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        RefreshHighScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                BackToMenu();
            }
        }
        else if (m_GameOver)
        {
            // if game over high show, do not recognize other input
            if (GameOverHigh.activeSelf)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ReStart();
            } else if (Input.GetKeyDown(KeyCode.Escape))
            {
                BackToMenu();
            }
        }
    }

    private void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverObj.SetActive(true);
        CheckHighScore();
    }

    private void CheckHighScore()
    {
        bool isHigh = m_Points > InfoManager.Instance.HighScore;
        GameOverHigh.SetActive(isHigh);
        GameOverTips.SetActive(!isHigh);
        if (isHigh)
        {
            PlayerNameInput.text = InfoManager.Instance.PlayerName;
            InfoManager.Instance.SetHighScore(m_Points); // 先存一下，避免用户直接退出
        }
    }

    private void RefreshHighScore()
    {
        HighScoreText.text = InfoManager.Instance.ShowInfo();
    }

    public void EndEditPlayerName()
    {
        string name = PlayerNameInput.text;
        int score = m_Points;

        InfoManager.Instance.SetHighScore(score, name);
        ReStart();
    }

    public bool CanPaddleMove()
    {
        return m_Started && !m_GameOver;
    }
}
