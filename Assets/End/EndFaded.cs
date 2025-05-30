using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndFaded : MonoBehaviour
{
    public CanvasGroup canvasGroup;             // UI 黑幕
    public float fadeDuration = 2f;             // 淡入、淡出時間
    public float endFadeStartTime = 65f;        // 幾秒後畫面再暗下來

    void Start()
    {
        StartCoroutine(FlickerSequence());
    }

    IEnumerator FlickerSequence()
    {
        // 初始畫面黑
        canvasGroup.alpha = 1;

        // 慢慢亮起來
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;

        // 等待直到該暗下來的時間
        float timeUntilEndFade = endFadeStartTime - fadeDuration;
        if (timeUntilEndFade > 0)
            yield return new WaitForSeconds(timeUntilEndFade);

        // 慢慢再暗下來
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1;
    }
}
