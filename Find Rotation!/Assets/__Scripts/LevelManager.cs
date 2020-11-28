using System.Collections;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [SerializeField] EnvironmentRotation environmentRotation;
    [SerializeField] UIManager uiManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] AudioManager audioManager;
    Model model_1;
    Model model_2;

    [SerializeField] GameObject[] models;

    [SerializeField] ParticleSystem backgroundPS;

    [SerializeField] ColorPallets[] colors;

    [SerializeField] Material backgroundMaterials;
    [SerializeField] Material modelMaterial;

    public int partsCount;
    int currentModel;
    int randomModel;
    int randomChance;
    int randomStartColor;
    int randomEndColor;

    float colorChangeDuration;

    [SerializeField] Material fogColor;
    [SerializeField] Material backgroundPSColor;

    public bool partWin;
    bool resetAll;
    bool setColor;

    void Start()
    {
        randomChance = (PlayerPrefs.GetInt("currentLevel") <= 5) ? 1 : Random.Range(1, 11);
        partsCount = (randomChance <= 3) ? 1 : 2;

        randomStartColor = Random.Range(0, colors.Length);
        randomEndColor = Random.Range(0, colors.Length);

        uiManager.ProgressBar();
        for (int i = 0; i < partsCount; i++)
        {
            LoadModel();
        }

        colorChangeDuration = 1.25f;
        
        backgroundPS.startColor = colors[randomStartColor].pallet[5];
        backgroundPS.Play();

        SetColors();
    }

    void Update()
    {
        if(randomEndColor == randomStartColor)
            randomEndColor = Random.Range(0, colors.Length);

        if (partWin)
        {
            if(partsCount == 1)
            {
                if (model_1.allowRoataion)
                {
                    if (model_1 != null)
                        model_1.Win();
                    gameManager.gameWin = true;
                }
            } else {
                if (model_1.allowRoataion)
                {
                    StartCoroutine(PartWin());
                    Destroy(model_1.transform.parent.gameObject, 3);
                }
                else
                {
                    if(model_2 != null)
                        model_2.Win();
                    gameManager.gameWin = true;
                }
            }
        }

        RenderSettings.fogColor = fogColor.color;
    }

    void LoadModel()
    {
        randomModel = Random.Range(0, models.Length);
        if (currentModel == 0)
        {
            uiManager.Models(randomModel);
            GameObject first = Instantiate(models[randomModel], Vector3.zero, Quaternion.identity);
            model_1 = first.transform.GetChild(0).GetComponent<Model>();
            model_1.allowRoataion = true;
        } else
        {
            GameObject second = Instantiate(models[randomModel], new Vector3(6, 0, 0), Quaternion.identity);
            model_2 = second.transform.GetChild(0).GetComponent<Model>();
            model_2.transform.parent.transform.rotation = Quaternion.Euler(0, 90, 0);
            model_2.allowRoataion = false;
        }

        currentModel++;
    }

    void SetColors()
    {
        if (!setColor)
        {
            backgroundMaterials.color = colors[randomStartColor].pallet[0];
            Camera.main.backgroundColor = colors[randomStartColor].pallet[1];
            fogColor.color = colors[randomStartColor].pallet[1];
            modelMaterial.color = colors[randomStartColor].pallet[2];
            uiManager.blur_img.color = colors[randomStartColor].pallet[3];
            uiManager.currentModel_img.color = colors[randomStartColor].pallet[4];
            uiManager.progressBar_img.color = colors[randomStartColor].pallet[6];
            setColor = true;
        } else
        {
            backgroundMaterials.DOColor(colors[randomEndColor].pallet[0], colorChangeDuration);
            Camera.main.DOColor(colors[randomEndColor].pallet[1], colorChangeDuration);
            fogColor.DOColor(colors[randomEndColor].pallet[1], colorChangeDuration);
            modelMaterial.DOColor(colors[randomEndColor].pallet[2], colorChangeDuration);
            uiManager.blur_img.DOColor(colors[randomEndColor].pallet[3], colorChangeDuration);
            uiManager.currentModel_img.DOColor(colors[randomEndColor].pallet[4], colorChangeDuration);
            backgroundPSColor.DOColor(colors[randomEndColor].pallet[5], 0.25f);
            uiManager.progressBar_img.DOColor(colors[randomEndColor].pallet[6], colorChangeDuration);
        }
    }

    IEnumerator PartWin()
    {
        uiManager.ProgressBar();
        gameManager.gameStart = false;
        model_1.Win();
        audioManager.PartChange();
        yield return new WaitForSeconds(0.25f);
        backgroundPS.Stop();
        yield return new WaitForSeconds(0.75f);
        SetColors();
        backgroundPS.startColor = backgroundPSColor.color;
        backgroundPS.Play();
        environmentRotation.Rotate();
        uiManager.Models(randomModel);
        model_1.allowRoataion = false;
        model_2.allowRoataion = true;
        partWin = false;
        gameManager.gameStart = true;
    }

    public void Continue()
    {
        if(!resetAll)
        {
            StartCoroutine(ResetAll(true));
            resetAll = true;
        }
    }
    public void Menu()
    {
        if (!resetAll)
        {
            StartCoroutine(uiManager.Transition(false, true, false));
            StartCoroutine(ResetAll(false));
            resetAll = true;
        }
    }
    IEnumerator ResetAll(bool gameStart)
    {
        setColor = false;
        StartCoroutine(environmentRotation.ResetAll());
        yield return new WaitForSeconds(0.55f);
        currentModel = 0;
        partWin = false;
        gameManager.gameWin = false;
        gameManager.gameStart = gameStart;

        randomChance = (PlayerPrefs.GetInt("currentLevel") <= 5) ? 1 : Random.Range(1, 11);
        partsCount = (randomChance <= 3) ? 1 : 2;

        randomStartColor = Random.Range(0, colors.Length);
        randomEndColor = Random.Range(0, colors.Length);

        uiManager.progressBar_img.fillAmount = 0;
        uiManager.ProgressBar();
        uiManager.callOneTime = false;
        for (int i = 0; i < partsCount; i++)
        {
            LoadModel();
        }

        colorChangeDuration = 1.25f;

        backgroundPS.startColor = colors[randomStartColor].pallet[5];
        backgroundPS.Stop();
        backgroundPS.Play();

        SetColors();
        resetAll = false;
    }
}

[System.Serializable]
class ColorPallets
{
    public Color[] pallet;
}
