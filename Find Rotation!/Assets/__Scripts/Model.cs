using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class Model : MonoBehaviour
{
    Touch touch_1;
    Touch touch_2;

    [SerializeField] GameManager gameManager;
    [SerializeField] LevelManager levelManager;
    
    [SerializeField] float rotSpeed;
    [SerializeField] float duration;
    float angleX;
    float angleY;
    float angleZ;

    [SerializeField] Vector3 minRotation;
    [SerializeField] Vector3 maxRotation;

    [SerializeField] GameObject[] meshes;

    public ParticleSystem partWinPS;

    [SerializeField] Material modelMaterial;

    public bool allowRoataion;
    bool playPS;

    void Start()
    {
        rotSpeed = 4;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        RandomRotation();
        partWinPS.startColor = modelMaterial.color;
    }

    void Update()
    {
        if(gameManager.platformIsPhone)
        {
            if(gameManager.gameStart && allowRoataion)
            {
                if (Input.touchCount > 0)
                {
                    touch_1 = Input.GetTouch(0);
                    if (touch_1.phase == TouchPhase.Moved)
                    {
                        Rotate();
                        StartCoroutine(CheckWin());
                    }
                }
            }
        } else
        {
            if(Input.GetMouseButton(0) && gameManager.gameStart && allowRoataion)
            {
                Rotate();
                StartCoroutine(CheckWin());
            }
        }
        partWinPS.startColor = modelMaterial.color;
        Angle();
    }

    void RandomRotation()
    {
        int randomX = Random.Range(-360, 360);
        if (randomX > minRotation.x && randomX < maxRotation.x)
            randomX += 20;

        int randomY = Random.Range(-360, 360);
        if (randomY > minRotation.y && randomY < maxRotation.y)
            randomY += 20;

        int randomZ = Random.Range(-360, 360);
        if (randomZ > minRotation.z && randomY < maxRotation.z)
            randomZ += 20;

        transform.rotation = new Quaternion(randomX, randomY, randomZ, 1);
    }

    void Rotate()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;

        transform.RotateAround(Vector3.up, -rotX);
        transform.RotateAround(Vector3.right, rotY);
    }

    void Angle()
    {
        angleX = transform.localEulerAngles.x;

        angleX = (angleX > 180) ? angleX - 360 : angleX;

        angleY = transform.localEulerAngles.y;

        angleY = (angleY > 180) ? angleY - 360 : angleY;

        angleZ = transform.localEulerAngles.z;

        angleZ = (angleZ > 180) ? angleZ - 360 : angleZ;
    }
       
    IEnumerator CheckWin()
    {
        yield return new WaitForSeconds(0.1f);

        levelManager.partWin = (angleX > minRotation.x && angleX < maxRotation.x &&
                               angleY > minRotation.y && angleY < maxRotation.y &&
                               angleZ > minRotation.z && angleZ < maxRotation.z) ? true : false;
    }

    public void Win()
    {
        if(!playPS)
        {
            partWinPS.startColor = modelMaterial.color;
            partWinPS.Play();
            playPS = true;
        }
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].transform.DOLocalMove(Vector3.zero, duration);
            meshes[i].transform.DOScale(Vector3.one, duration);
        }
        transform.DOLocalRotate(new Vector3(0, 0, 0), duration);
        Destroy(transform.parent.gameObject, 1.75f);
    }
}
