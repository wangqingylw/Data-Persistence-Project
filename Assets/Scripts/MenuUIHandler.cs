using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI highScoreText;
    public TMP_InputField nameInputField;

    void Start()
    {
        InitInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        InfoManager.Instance.SaveInfo();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quie();
#endif
    }

    public void InitInfo()
    {
        // name
        nameInputField.text = InfoManager.Instance.PlayerName;

        // high score
        highScoreText.text = InfoManager.Instance.ShowInfo();
    }

    public void EndEditPlayerName()
    {
        InfoManager.Instance.SetPlayerName(nameInputField.text);
    }

}
