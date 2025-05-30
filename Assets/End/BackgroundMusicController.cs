using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioSource audioSource;
    public float playTime = 15f; // 幾秒後播放
    private bool hasPlayed = false;

    void Update()
    {
        if (!hasPlayed && Time.time >= playTime)
        {
            hasPlayed = true;
            audioSource.Play();
        }
    }
}
