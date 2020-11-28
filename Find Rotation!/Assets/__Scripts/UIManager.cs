using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] AudioManager audioManager;

    [SerializeField] Canvas menu_cnvs;
    [SerializeField] Canvas game_cnvs;
    [SerializeField] Canvas gameWin_cnvs;

    public bool callOneTime;

    [Space]
    [Header("Menu Canvas")]
    //>>>>>Menu<<<<<\\
    [SerializeField] Image settings_img;
    [SerializeField] Image[] volume_img;
    [SerializeField] Image[] vibration_img;
    [SerializeField] TextMeshProUGUI start_txt;
    public TextMeshProUGUI menuLevel_txt;
    bool settingsWindowON;

    [Space]
    [Header("Game Canvas")]
    //>>>>>Game<<<<<\\
    [SerializeField] Sprite[] models;
    [SerializeField] Sprite[] progressBars;
    public Image currentModel_img;
    public Image progressBar_img;
    [SerializeField] Image progressBarBCK_img;
    [SerializeField] TextMeshProUGUI currentLevel_txt;
    [SerializeField] TextMeshProUGUI nextLevel_txt;
    bool partWin;

    [Space]
    [Header("Game Win")]
    //>>>>>GameWin<<<<<\\
    [SerializeField] Image home_img;
    [SerializeField] TextMeshProUGUI home_txt;
    [SerializeField] TextMeshProUGUI continue_txt;
    [SerializeField] RectTransform homeObject;

    [Space]
    [Header("General Canvas")]
    //>>>>>General<<<<<\\
    public Image blur_img;
    [SerializeField] Image transitionPanel_img;
    [SerializeField] float panelTime;

    void Start()
    {
        if(PlayerPrefs.GetInt("volumeStatus") == 0)
        {
            volume_img[0].enabled = true;
            volume_img[1].enabled = false;
        } else
        {
            volume_img[0].enabled = false;
            volume_img[1].enabled = true;
        }
        if (PlayerPrefs.GetInt("vibrationStatus") == 0)
        {
            vibration_img[0].enabled = true;
            vibration_img[1].enabled = false;
        } else
        {
            vibration_img[0].enabled = false;
            vibration_img[1].enabled = true;
        }
        menuLevel_txt.text = PlayerPrefs.GetInt("currentLevel").ToString() + " - " + (PlayerPrefs.GetInt("currentLevel") + 1).ToString();
    }

    void Update()
    {
        if (partWin)
        {
            if (progressBar_img.fillAmount < 0.5f)
                progressBar_img.fillAmount += 0.02f;
            else
            {
                progressBar_img.fillAmount = 0.5f;
                partWin = false;
            }
            progressBar_img.fillAmount = Mathf.Clamp(progressBar_img.fillAmount, 0, 0.5f);
        }
        else if (gameManager.gameWin)
        {
            progressBar_img.fillAmount += 0.02f;
            if (!callOneTime)
            {
                StartCoroutine(GameWin());
                callOneTime = true;
            }
        }
    }
    //>>>>>>>>>>GameCanvas<<<<<<<<<\\
    public void ProgressBar()
    {
        progressBar_img.sprite = progressBarBCK_img.sprite = (levelManager.partsCount == 1) ? progressBars[0] : progressBars[1];
        currentLevel_txt.text = gameManager.currentLevel.ToString();
        nextLevel_txt.text = (gameManager.currentLevel + 1).ToString();
        if (levelManager.partWin)
            partWin = true;
    }

    public void Models(int number)
    {
        currentModel_img.sprite = models[number];
    }
    //>>>>>>>>>>GameCanvas<<<<<<<<<\\


    //>>>>>>>>>>GameWinCanvas<<<<<<<<<\\
    public void HomeButtonDown()
    {
        audioManager.ButtonDown();
        homeObject.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        home_txt.DOColor(new Color(home_txt.color.r, home_txt.color.g, home_txt.color.b, 0.4f), 0.2f);
        home_img.DOColor(new Color(home_img.color.r, home_img.color.g, home_img.color.b, 0.4f), 0.2f);
    }
    public void HomeButtonUp()
    {
        audioManager.ButtonUp();
        homeObject.DOScale(Vector3.one, 0.2f);
        home_txt.DOColor(new Color(home_txt.color.r, home_txt.color.g, home_txt.color.b, 0.58f), 0.2f);
        home_img.DOColor(new Color(home_img.color.r, home_img.color.g, home_img.color.b, 0.58f), 0.2f);
    }
    public void ContinueButtonDown()
    {
        audioManager.ButtonDown();
        continue_txt.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        continue_txt.DOColor(new Color(continue_txt.color.r, continue_txt.color.g, continue_txt.color.b, 0.5f), 0.2f);
    }
    public void ContinueButtonUp()
    {
        audioManager.ButtonUp();
        continue_txt.rectTransform.DOScale(Vector3.one, 0.2f);
        continue_txt.DOColor(new Color(continue_txt.color.r, continue_txt.color.g, continue_txt.color.b, 0.68f), 0.2f);
        StartCoroutine(Transition(false, false, true));
    }
    //>>>>>>>>>>GameWinCanvas<<<<<<<<<\\


    //>>>>>>>>>>MenuCanvas<<<<<<<<<<\\
    public void StartButtonDown()
    {
        audioManager.ButtonDown();
        start_txt.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        start_txt.DOColor(new Color(start_txt.color.r, start_txt.color.g, start_txt.color.b, 0.55f), 0.2f);
    }
    public void StartButtonUp()
    {
        gameManager.gameStart = true;
        audioManager.ButtonUp();
        start_txt.rectTransform.DOScale(Vector3.one, 0.2f);
        start_txt.DOColor(new Color(start_txt.color.r, start_txt.color.g, start_txt.color.b, 1), 0.2f);
        StartCoroutine(Transition(false, false, true));
    }

    public void SettingsButtonDown()
    {
        audioManager.ButtonDown();
        settings_img.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        settings_img.DOColor(new Color(settings_img.color.r, settings_img.color.g, settings_img.color.b, 0.55f), 0.2f);
    }
    public void SettingsButtonUp()
    {
        audioManager.ButtonUp();

        settings_img.rectTransform.DOScale(Vector3.one, 0.2f);
        settings_img.DOColor(new Color(settings_img.color.r, settings_img.color.g, settings_img.color.b, 1), 0.2f);

        if(!settingsWindowON)
        {
            settings_img.rectTransform.DORotate(new Vector3(0, 0, 180), 0.5f);
            volume_img[0].rectTransform.DOScale(Vector3.one, 0.5f);
            volume_img[1].rectTransform.DOScale(Vector3.one, 0.5f);
            vibration_img[0].rectTransform.DOScale(Vector3.one, 0.5f);
            vibration_img[1].rectTransform.DOScale(Vector3.one, 0.5f);

            StartCoroutine(resetSettingsRotation());

            settingsWindowON = true;
        } else
        {
            settings_img.rectTransform.DORotate(new Vector3(0, 0, -180), 0.5f);
            volume_img[0].rectTransform.DOScale(Vector3.zero, 0.5f);
            volume_img[1].rectTransform.DOScale(Vector3.zero, 0.5f);
            vibration_img[0].rectTransform.DOScale(Vector3.zero, 0.5f);
            vibration_img[1].rectTransform.DOScale(Vector3.zero, 0.5f);

            StartCoroutine(resetSettingsRotation());
            settingsWindowON = false;
        }
    }
    IEnumerator resetSettingsRotation()
    {
        yield return new WaitForSeconds(0.5f);
        settings_img.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void VolumeButtonDown()
    {
        audioManager.ButtonDown();
        volume_img[0].rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        volume_img[0].DOColor(new Color(volume_img[0].color.r, volume_img[0].color.g, volume_img[0].color.b, 0.55f), 0.2f);
        volume_img[1].rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        volume_img[1].DOColor(new Color(volume_img[1].color.r, volume_img[1].color.g, volume_img[1].color.b, 0.55f), 0.2f);
    }
    public void VolumeButtonUp()
    {
        audioManager.ButtonUp();
        audioManager.AudioOnOff();

        if(audioManager.audioStatus == 0)
        {
            volume_img[0].enabled = true;
            volume_img[1].enabled = false;
            volume_img[0].rectTransform.DOScale(Vector3.one, 0.2f);
            volume_img[0].DOColor(new Color(volume_img[0].color.r, volume_img[0].color.g, volume_img[0].color.b, 1), 0.2f);
        } else
        {
            volume_img[0].enabled = false;
            volume_img[1].enabled = true;
            volume_img[1].rectTransform.DOScale(Vector3.one, 0.2f);
            volume_img[1].DOColor(new Color(volume_img[1].color.r, volume_img[1].color.g, volume_img[1].color.b, 1), 0.2f);
        }
    }
    public void VibrationButtonDown()
    {
        audioManager.ButtonDown();
        vibration_img[0].rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        vibration_img[0].DOColor(new Color(vibration_img[0].color.r, vibration_img[0].color.g, vibration_img[0].color.b, 0.55f), 0.2f);
        vibration_img[1].rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        vibration_img[1].DOColor(new Color(vibration_img[1].color.r, vibration_img[1].color.g, vibration_img[1].color.b, 0.55f), 0.2f);
    }
    public void VibrationButtonUp()
    {
        audioManager.ButtonUp();
        audioManager.VibrationOnOff();

        if (audioManager.vibrationStatus == 0)
        {
            vibration_img[0].enabled = true;
            vibration_img[1].enabled = false;
            vibration_img[0].rectTransform.DOScale(Vector3.one, 0.2f);
            vibration_img[0].DOColor(new Color(vibration_img[0].color.r, vibration_img[0].color.g, vibration_img[0].color.b, 1), 0.2f);
        } else
        {
            vibration_img[0].enabled = false;
            vibration_img[1].enabled = true;
            vibration_img[1].rectTransform.DOScale(Vector3.one, 0.2f);
            vibration_img[1].DOColor(new Color(vibration_img[1].color.r, vibration_img[1].color.g, vibration_img[1].color.b, 1), 0.2f);
        }
    }
    //>>>>>>>>>>MenuCanvas<<<<<<<<<<\\

    public IEnumerator Transition(bool gameWin, bool menu, bool game)
    {
        transitionPanel_img.raycastTarget = true;
        transitionPanel_img.DOColor(new Color(transitionPanel_img.color.r, transitionPanel_img.color.g, transitionPanel_img.color.b, 1), panelTime);
        yield return new WaitForSeconds(panelTime);
        gameWin_cnvs.enabled = gameWin;
        menu_cnvs.enabled = menu;
        game_cnvs.enabled = game;
        transitionPanel_img.raycastTarget = false;
        transitionPanel_img.DOColor(new Color(transitionPanel_img.color.r, transitionPanel_img.color.g, transitionPanel_img.color.b, 0), panelTime);
    }
    IEnumerator GameWin()
    {
        yield return new WaitForSeconds(1.25f);
        transitionPanel_img.raycastTarget = true;
        transitionPanel_img.DOColor(new Color(transitionPanel_img.color.r, transitionPanel_img.color.g, transitionPanel_img.color.b, 1), panelTime);
        yield return new WaitForSeconds(panelTime);
        game_cnvs.enabled = false;
        gameWin_cnvs.enabled = true;
        transitionPanel_img.raycastTarget = false;
        transitionPanel_img.DOColor(new Color(transitionPanel_img.color.r, transitionPanel_img.color.g, transitionPanel_img.color.b, 0), panelTime);
    }
}
