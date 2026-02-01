using UnityEngine;
using System.Collections;

public class CoinMove : MonoBehaviour
{
    public float timeToClose = 1.4f;
    void OnEnable()
    {
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(timeToClose);
        transform.gameObject.SetActive(false);
    }
}
