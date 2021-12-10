using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifeBar : MonoBehaviour
{
    [SerializeField] Transform min;
    [SerializeField] Transform max;

    [SerializeField] Color minColor;
    [SerializeField] Color maxColor;

    [SerializeField] GameObject mask;
    [SerializeField] GameObject lateMask;

    [SerializeField] SpriteRenderer fill;

    [SerializeField] Vector3 targetScale = new Vector3(1.0625f, 0.725f, 1);

    [Range(0f, 1f), SerializeField] float value = 1f;

    bool appeared = false;

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public void ShowBar()
    {
        StartCoroutine(Appear());
    }

    IEnumerator Appear()
    {
        AudioManager.Instance.PlaySound("Alarm");

        appeared = false;

        value = 0;

        mask.transform.position = min.position;
        lateMask.transform.position = min.position;
        fill.color = Color.Lerp(minColor, maxColor, value);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;

            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);

            yield return null;
        }

        transform.localScale = targetScale;

        while (t < 1.8)
        {
            t += Time.deltaTime;

            value = (t - 1) / .8f;

            mask.transform.position = Vector3.Lerp(min.position, max.position, value);
            fill.color = Color.Lerp(minColor, maxColor, value);

            yield return null;
        }

        appeared = true;

        lateMask.transform.position = max.position;

        SetValue(1);
    }

    private void Update()
    {
        lateMask.transform.position = Vector3.Lerp(lateMask.transform.position, mask.transform.position, Time.deltaTime * GameManager.instance.timeScale * 5);
    }

    public void SetValue(float v)
    {
        if (!appeared)
        {
            return;
        }

        value = v;

        mask.transform.position = Vector3.Lerp(min.position, max.position, value);
        
        fill.color = Color.Lerp(minColor, maxColor, value);
    }
}
