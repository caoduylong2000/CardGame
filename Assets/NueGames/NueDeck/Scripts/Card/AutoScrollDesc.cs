using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoScrollDesc : MonoBehaviour
{
    public float delayTime; 
    public float scrollSpeed;

    [SerializeField] private Scrollbar scrollbar;
    private Coroutine currentScrollCoroutine;

    public void ActiveScroll()
    {
        currentScrollCoroutine = StartCoroutine(ScrollRoutine());
    }

    IEnumerator ScrollRoutine()
    {
        while (true)
        {
            // Scroll down
            yield return ScrollVertical(scrollSpeed, 0f);

            yield return new WaitForSeconds(delayTime);

            // Scroll up
            yield return ScrollVertical(scrollSpeed, 1f);

            yield return new WaitForSeconds(delayTime);
        }
    }

    IEnumerator ScrollVertical(float speed, float targetNormalizedPosition)
    {
        while (!Mathf.Approximately(scrollbar.value, targetNormalizedPosition))
        {
            scrollbar.value = Mathf.MoveTowards(scrollbar.value, targetNormalizedPosition, (speed / 1000) * Time.deltaTime);
            yield return null;
        }
    }

    void OnDisable()
    {
        if (currentScrollCoroutine != null)
            StopCoroutine(currentScrollCoroutine);
    }
}
