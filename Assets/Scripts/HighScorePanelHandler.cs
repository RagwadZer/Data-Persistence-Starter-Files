using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScorePanelHandler : MonoBehaviour
{
    private Transform entryTemplate;
    private Transform entryContainer;
    private int templateHeight = 20;

    private List<HighScoreEntry> highScoreEntryList;
    private List<Transform> highScoreEntryTransform;

    private void Awake()
    {
        entryContainer = transform.Find("highScoreEntryContainer");
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);


        /*    highScoreEntryList = new List<HighScoreEntry>()
            {
                new HighScoreEntry { score = 20, name = "vvrf" },
                 new HighScoreEntry { score = 50, name = "afr" },
                 new HighScoreEntry { score = 60, name = "cce" },
                 new HighScoreEntry { score = 550, name = "aaa" },
             };*/

        string JsonString = PlayerPrefs.GetString("highScoreTable");
        HighScore highScores = JsonUtility.FromJson<HighScore>(JsonString);
        highScoreEntryList = highScores.highScoreEntryList;

        //ranger l'ordre de la liste
        for (int i = 0; i < highScoreEntryList.Count; i++)
        {
            for (int j = i+1; j < highScoreEntryList.Count; j++)
            {
                if (highScoreEntryList[i].score < highScoreEntryList[j].score)
                {
                    HighScoreEntry temp = highScoreEntryList[i];
                    highScoreEntryList[i] = highScoreEntryList[j];
                    highScoreEntryList[j] = temp;
                }
            }
        }

        highScoreEntryTransform = new List<Transform>();
        foreach (HighScoreEntry highScoreEntry in highScoreEntryList)
        {
            CreateHighScoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransform);
        }

/*
        HighScore highScores = new HighScore { highScoreEntryList = highScoreEntryList };
        string Json = JsonUtility.ToJson(highScores);

        PlayerPrefs.SetString("highScoreTable", Json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highScoreTable"));*/

    }

    private void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();

        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
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

        transformList.Add(entryTransform);
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
