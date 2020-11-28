using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    public bool gameStart;
    public bool gameWin;

    public int currentLevel;
    public int nextLevel;

    bool callOneTime;
    public bool platformIsPhone;

    void Start()
    {
        callOneTime = true;
        currentLevel = PlayerPrefs.GetInt("currentLevel");
        if(currentLevel == 0)
        {
            currentLevel = 1;
            PlayerPrefs.SetInt("currentLevel", currentLevel);
        }
        nextLevel = currentLevel + 1;
        uiManager.menuLevel_txt.text = currentLevel.ToString() + " - " + nextLevel.ToString();
    }

    void Update()
    {
        platformIsPhone = (Application.platform == RuntimePlatform.Android) ? true : false;

        if (gameStart)
            callOneTime = true;

        if (gameWin)
            GameWin();
    }

    void GameWin()
    {
        gameStart = false;
        if(callOneTime)
        {
            uiManager.ProgressBar();
            StartCoroutine(changeLevelValue());
            callOneTime = false;
        }
    }
    IEnumerator changeLevelValue()
    {
        yield return new WaitForSeconds(0.5f);
        currentLevel++;
        PlayerPrefs.SetInt("currentLevel", currentLevel);
    }
}
