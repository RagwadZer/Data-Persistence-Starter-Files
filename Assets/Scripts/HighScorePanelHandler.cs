using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScorePanelHandler : MonoBehaviour
{
    private Transform entryTemplate; 
    private Transform entryContainer;// le  transform parent de la template
    private List<Transform> highScoreEntryTransform;

    private Button ResetHighScoreListButton;
    
    private int templateHeight = 20;

    private void Awake()
    {

        entryContainer = transform.Find("highScoreEntryContainer");
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        
        highScoreEntryTransform = new List<Transform>();

        //ajouter les score du jeu précédent avant d'utliser la save 
        CheckScoreToAdd();

        //pour load une liste de highScore s'il y en a 
        string JsonString = PlayerPrefs.GetString("highScoreTable");
       
        HighScore highScores = JsonUtility.FromJson<HighScore>(JsonString);


        /*
         * zone Test 
         * */

            //AddHighScoreEntry(10, "Joi");
            
            Debug.Log($"the first is {highScores.highScoreEntryList[1].name},  {highScores.highScoreEntryList[1].score}");
        
            Debug.Log($"EntryListCount : {highScores.highScoreEntryList.Count}");


            Debug.Log($"transformListCount :{highScoreEntryTransform.Count}");


        /*
         * fin zone test 
         * */

        //ranger l'ordre de la liste
        for (int i = 0; i < highScores.highScoreEntryList.Count; i++)
        {
            for (int j = i+1; j < highScores.highScoreEntryList.Count; j++)
            {
                if (highScores.highScoreEntryList[i].score < highScores.highScoreEntryList[j].score)
                {
                    HighScoreEntry temp = highScores.highScoreEntryList[i];
                    highScores.highScoreEntryList[i] = highScores.highScoreEntryList[j];
                    highScores.highScoreEntryList[j] = temp;

                }
            }
        }

       
        GameManager.Instance.firstPosScore = highScores.highScoreEntryList[0].score;
        GameManager.Instance.firstPosName = highScores.highScoreEntryList[0].name;
        Debug.Log($"fps :{GameManager.Instance.firstPosName} ,fpn: {GameManager.Instance.firstPosScore}");

        MaxRanking();
        
        ResetHighScoreListButton = transform.Find("resetHighScoreButton").GetComponent<Button>();
        ResetHighScoreListButton.onClick.AddListener(() => { ResetHighScoreList(); });

     


        foreach (HighScoreEntry highScore in highScores.highScoreEntryList)
        {
            CreateHighScoreEntryTransform(highScore, entryContainer, highScoreEntryTransform);
            Debug.Log($"{highScore.name} : {highScore.score}" );
        }
        
        
        
    }

    private void CheckScoreToAdd()
    {
        /* voir s'il y a des score a ajouter depuis le GM
     * flem de créer une liste de ranking sur le Gm donc on le fait juste retenir 
     * les score a chaque Game over et lorqu'on revient sur le menu
     * on ajoute ces score a la liste
     * */
        List<GameManager.ScoreToAdd> scoreToAdd = GameManager.Instance.ScoreToAddToHighScore;
        if (scoreToAdd != null)
        {
            List<GameManager.ScoreToAdd> scorelist = scoreToAdd;
            foreach (var score in scorelist)
            {
                AddHighScoreEntry(score.score, score.name);
            }
            scoreToAdd.Clear();
            
        }
    }

    private void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
      

        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count +1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;
            case 1:
                rankString = "1ST"; break;
            case 2:
                rankString = "2ND"; break;
            case 3:
                rankString = "3TH"; break;
        }



        entryTransform.Find("position").GetComponent<Text>().text = rankString;

        int score = highScoreEntry.score;
        entryTransform.Find("score").GetComponent<Text>().text = score.ToString();

        string name = highScoreEntry.name;
        entryTransform.Find("name").GetComponent<Text>().text = name;

        //on active le back ground si le rank est impaire
        entryTransform.Find("backGround").gameObject.SetActive(rank % 2 == 1);

        if (rank == 1)
        {
            //met en évidence le 1er
            entryTransform.Find("name").GetComponent<Text>().color = Color.green;
            entryTransform.Find("score").GetComponent<Text>().color = Color.green;
            entryTransform.Find("position").GetComponent<Text>().color = Color.green;
        }
        transformList.Add(entryTransform);
    }

    private void MaxRanking()
    {
        string JsonString = PlayerPrefs.GetString("highScoreTable");
        HighScore highScores = JsonUtility.FromJson<HighScore>(JsonString);

        for (int i = 9; i < highScores.highScoreEntryList.Count; i++)
        {
            HighScoreEntry highScoreEntry = highScores.highScoreEntryList[i];
            highScores.highScoreEntryList.Remove(highScoreEntry);
            string Json = JsonUtility.ToJson(highScores);

            PlayerPrefs.SetString("highScoreTable", Json);
            PlayerPrefs.Save();
        }             
    }


    //pour ajouter un nouveau score dans la liste des high score
    public void AddHighScoreEntry(int score, string name)
    {
        HighScoreEntry highScoreEntry = new HighScoreEntry { score = score, name = name };
        //ici on fait ça parce qu'on veut ajouter le nouveau score dans la liste de la save aussi
        //on recup la liste sur la save
        string JsonString = PlayerPrefs.GetString("highScoreTable");
        HighScore highScores = JsonUtility.FromJson<HighScore>(JsonString);
        //on ajoute le score
        highScores.highScoreEntryList.Add(highScoreEntry);
        //et on resave

        string Json = JsonUtility.ToJson(highScores);

        PlayerPrefs.SetString("highScoreTable", Json);
        PlayerPrefs.Save();
        
      

    }

    private void ResetHighScoreList()
    {
        string JsonString = PlayerPrefs.GetString("highScoreTable");
        HighScore highScores = JsonUtility.FromJson<HighScore>(JsonString);


        highScores.highScoreEntryList.Clear();
        highScoreEntryTransform.Clear();
        
        
        string Json = JsonUtility.ToJson(highScores);

        PlayerPrefs.SetString("highScoreTable", Json);
        PlayerPrefs.Save();

        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);

    }
    

    private class HighScore
    {
        public List<HighScoreEntry> highScoreEntryList;
    }

    /*
     * represent a single entry
     * le résultat d'un joueur
     * */
    [System.Serializable]
    public class HighScoreEntry
    {
        public int score;
        public string name;
    }
}
