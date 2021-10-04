using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    private HighScoreData highScore;
    public string playerName;
    public int currentHighScore;
    public string currentHighScoreName;

   
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(Instance);
    }

    private void Start()
    {
        highScore = new HighScoreData();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetRanking()
    {

    }
   
}
