using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image overlay;         // Imagen negra full-screen (alpha 0 al inicio)
    public float duration = 0.35f;

    public IEnumerator FadeOut()
    {
        yield return Fade(0f, 1f);
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(1f, 0f);
    }

    IEnumerator Fade(float from, float to)
    {
        if (overlay == null) yield break;
        Color c = overlay.color;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(from, to, t / duration);
            overlay.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        overlay.color = new Color(c.r, c.g, c.b, to);
    }
}
