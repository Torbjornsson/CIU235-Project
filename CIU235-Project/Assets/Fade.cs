using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public CanvasGroup canvas_group;

    public bool fading;
    public float fade_speed;
    public int fade_dir;
    public bool fade_done;

    public void StartFade(float speed, int dir)
    {
        if (canvas_group == null) canvas_group = GetComponent<CanvasGroup>();
        canvas_group.alpha = (dir > 0) ? 0 : 1;
        fading = true;
        fade_speed = speed;
        fade_dir = dir;

        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        while ((fade_dir == 1 && canvas_group.alpha < 1) || (fade_dir == -1 && canvas_group.alpha > 0))
        {
            canvas_group.alpha += Time.deltaTime * fade_speed * fade_dir;
            yield return null;
        }
        fade_done = true;
        yield return null;
    }

    public void Reset()
    {
        fading = false;
        fade_done = false;
    }
}
