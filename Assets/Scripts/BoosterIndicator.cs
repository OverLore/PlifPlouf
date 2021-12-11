using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IndicatorType
{
    Damage,
    Speed,
    Horizontal,
    Number,
    Shield
}

public class BoosterIndicator : MonoBehaviour
{
    [SerializeField] IndicatorType type;

    [SerializeField] Image clock;

    [SerializeField] Image[] indicatorImgs;
    List<Color> indicatorImgsColors = new List<Color>();

    private void Start()
    {
        foreach (Image img in indicatorImgs)
        {
            indicatorImgsColors.Add(img.color);
        }
    }

    void Update()
    {
        if (GameManager.instance.timeScale < 1f)
        {
            switch (type)
            {
                case IndicatorType.Damage:
                    clock.fillAmount = GameManager.instance.GetPlayer().AttackDamageLeft / 20f;
                    break;
                case IndicatorType.Speed:
                    clock.fillAmount = GameManager.instance.GetPlayer().AttackSpeedLeft / 20f;
                    break;
                case IndicatorType.Horizontal:
                    clock.fillAmount = GameManager.instance.GetPlayer().HorizontalShotLeft / 20f;
                    break;
                case IndicatorType.Number:
                    clock.fillAmount = GameManager.instance.GetPlayer().NumberShotLeft / 20f;
                    break;
                case IndicatorType.Shield:
                    clock.fillAmount = GameManager.instance.GetPlayer().ShieldLeft / GameManager.instance.GetPlayer().shieldDuration;
                    break;
            }
        }

        for (int i = 0; i < indicatorImgs.Length; i++)
        {
            indicatorImgs[i].color =
                new Color(indicatorImgsColors[i].r, indicatorImgsColors[i].g,
                indicatorImgsColors[i].b, 1 - (GameManager.instance.timeScale * 1.1f));
        }
    }
}
