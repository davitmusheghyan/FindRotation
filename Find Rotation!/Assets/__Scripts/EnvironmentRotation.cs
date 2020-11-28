using UnityEngine;
using DG.Tweening;
using System.Collections;

public class EnvironmentRotation : MonoBehaviour
{
    [SerializeField] Transform myCamera;
    [SerializeField] Transform DL;
    [SerializeField] Transform background;

    float duration;

    void Start()
    {
        duration = 1.25f;
    }

    public void Rotate()
    {
        myCamera.DORotate(new Vector3(25, 45, 0), duration);
        DL.DORotate(new Vector3(50, 60, 0), duration);
        background.DORotate(new Vector3(0, 89f, 0), duration);
    }

    public IEnumerator ResetAll()
    {
        yield return new WaitForSeconds(0.5f);
        myCamera.DORotate(new Vector3(25, -45, 0), 0.1f);
        DL.DORotate(new Vector3(50, -30, 0), 0.1f);
        background.DORotate(new Vector3(0, 0, 0), 0.1f);
    }
}
