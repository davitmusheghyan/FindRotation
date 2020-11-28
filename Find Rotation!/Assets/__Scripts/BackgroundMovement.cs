using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] Vector3 movePosition_1;
    [SerializeField] Vector3 movePosition_2;

    [SerializeField] float moveDuration;

    [SerializeField] bool rightOrLeft;
    bool stop;

    void Awake()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while(!stop)
        {
            if(rightOrLeft)
            {
                transform.DOLocalMove(movePosition_1, moveDuration);
                yield return new WaitForSeconds(moveDuration + 2);
                rightOrLeft = false;
            } else
            {
                transform.DOLocalMove(movePosition_2, moveDuration);
                yield return new WaitForSeconds(moveDuration + 2);
                rightOrLeft = true;
            }
        }
    }
}
