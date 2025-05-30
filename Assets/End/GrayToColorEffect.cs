using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class GrayToColorEffect : MonoBehaviour
{
    public Material effectMaterial;
    private float timer = 0f;
    private Camera cam;

    void OnEnable()
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode |= DepthTextureMode.Depth;
        timer = 0f;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (effectMaterial == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        timer += Time.deltaTime;

        float radius = 0f;
        Vector2 center = new Vector2(0.5f, 0.5f); // 預設中心

        if (timer < 20f)
        {
            // 初始灰暗
            radius = 0f;
        }
        else if (timer >= 20f && timer < 30f)
        {
            // 第一次漸變到彩色
            radius = Mathf.Clamp01((timer - 20f) / 5f); // 0~1
        }
        else if (timer >= 30f && timer < 32f)
        {
            // 突然變回灰暗
            radius = 0f;
        }
        else if (timer >= 32f && timer < 40f)
        {
            // 漸變彩色（更慢）
            radius = (timer - 32f) / 6f;
            center = new Vector2(1.0f, 0.0f); // 從右下角開始
        }
        else
        {
            // 完全彩色
            radius = 1f;
        }

        effectMaterial.SetFloat("_Radius", radius);
        effectMaterial.SetVector("_Center", center);

        Graphics.Blit(src, dest, effectMaterial);
    }
}
