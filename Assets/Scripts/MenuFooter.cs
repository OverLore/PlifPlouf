using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFooter : MonoBehaviour
{
    [SerializeField] List<Image> ButtonsList = new List<Image>();
    [SerializeField] Canvas MainCanvas;

    void Update()
    {
        float width = MainCanvas.GetComponent<RectTransform>().rect.width;
        float height = MainCanvas.GetComponent<RectTransform>().rect.height;
        float imgsScale = width / ButtonsList.Count;

        for (int i = 0; i < ButtonsList.Count; i++)
        {
            Image img = ButtonsList[i];

            img.rectTransform.sizeDelta = new Vector2(imgsScale, imgsScale);
            img.transform.localPosition = new Vector3((-width / 2) + imgsScale / 2 + (i * imgsScale), -height / 2 + imgsScale / 2, 0);
        }
    }
}
