using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartSceneUIHandler : MonoBehaviour
{

    public Button startButton, quitButton;
    public InputField nameInputField;
    [SerializeField] private string playerName;

    void Start()
    {
        startButton.onClick.AddListener(() => { SceneManager.LoadScene(1); });
        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#else
            Application.Quit();            
#endif
        });

        nameInputField.onValueChanged.AddListener(delegate 
        {
            playerName = nameInputField.text.ToString();
            GameManager.Instance.playerName = playerName;
        });

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
