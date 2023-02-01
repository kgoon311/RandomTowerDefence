using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpLearn : MonoBehaviour
{
    public Transform objTransform;
    public Transform[] targetPos;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("asd");
            StartCoroutine(LerpCoroutine(targetPos[0], targetPos[1], 3));
        }
    }
    IEnumerator LerpCoroutine(Transform start, Transform end, float duration)
    {
        float timer = 0f;


        while (timer < duration)
        {
            float x = timer / duration;
            objTransform.position = Vector3.Lerp(start.position, end.position, x * x * x * x);
            timer += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
