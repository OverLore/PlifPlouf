using UnityEngine;
using UnityEngine.UI;

public class FooterButtonSizeController : MonoBehaviour
{
    Image img;
    RectTransform rect;

    float t = 0;
    float defaultVal;
    float transitionTime = .15f;
    float targetSize;
    float lastSize;

    Color targetColor;
    Color lastColor;

    private void Start()
    {
        img = gameObject.GetComponent<Image>();
        rect = gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        t += Time.deltaTime / transitionTime;

        float val = Mathf.Lerp(lastSize, targetSize, t);
        Color col = Color.Lerp(lastColor, targetColor, t);

        rect.sizeDelta = new Vector2(val, defaultVal);
        img.color = col;
    }

    public void SetDefault(float target, Color targetC)
    {
        targetSize = target;
        lastSize = target;

        targetColor = targetC;
        lastColor = targetC;

        defaultVal = target;
    }

    public void SetTargetSize(float target, Color targetC)
    {
        if (target == targetSize)
        {
            return;
        }

        lastSize = targetSize;
        targetSize = target;

        lastColor = targetColor;
        targetColor = targetC;

        t = 0;
    }

    public void ForceTarget(float target, Color targetC)
    {
        lastSize = target;
        targetSize = target;

        lastColor = targetC;
        targetColor = targetC;
    }
}
