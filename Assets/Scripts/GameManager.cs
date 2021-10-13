using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<ScoreToAdd> ScoreToAddToHighScore;
    public string playerName;
    //1er place player
    public int firstPosScore;
    public string firstPosName;

   
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

        ScoreToAddToHighScore = new List<ScoreToAdd>();
        DontDestroyOnLoad(Instance);
        
    }


    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public class ScoreToAdd
    {
        public int score;
        public string name;
    }
}
