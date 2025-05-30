using UnityEngine;
using System.Collections;

public class ImageFadeController : MonoBehaviour
{
    public CanvasGroup imageA;  // 第一張圖
    public CanvasGroup imageB;  // 第二張圖

    public float fadeDuration = 1.5f;
    public float fadeStartTime = 25f;
    public float secondImageDisappearTime = 45f;

    public Vector3 initialScale = Vector3.one;
    public Vector3 finalScale = new Vector3(1.5f, 1.5f, 1f);  // 最終放大倍率

    void Start()
    {
        // 一開始 A 完全顯示，B 隱藏
        imageA.alpha = 1;
        imageB.alpha = 0;
        imageB.transform.localScale = initialScale;  // 設定初始大小

        StartCoroutine(ImageSequence());
    }

    IEnumerator ImageSequence()
    {
        // 等到指定時間再開始切換
        yield return new WaitForSeconds(fadeStartTime);

        // A 淡出、B 淡入
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            imageA.alpha = Mathf.Lerp(1, 0, t);
            imageB.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        imageA.alpha = 0;
        imageB.alpha = 1;

        float scalingTime = secondImageDisappearTime - (fadeStartTime + fadeDuration);
        StartCoroutine(ScaleOverTime(imageB.transform, initialScale, finalScale, scalingTime));

        // 等待到該消失的時間
        yield return new WaitForSeconds(scalingTime);

        // B 淡出
        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            imageB.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        imageB.alpha = 0;
    }

    IEnumerator ScaleOverTime(Transform target, Vector3 start, Vector3 end, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            target.localScale = Vector3.Lerp(start, end, t);
            yield return null;
        }

        target.localScale = end;
    }
}
