using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreMenuControl : MonoBehaviour
{
    [SerializeField] string version;
    [SerializeField] TMP_Text versionTxt;

    [SerializeField] TMP_Text loading;

    [SerializeField] Image title;
    [SerializeField] Sprite normalTitle;
    [SerializeField] Sprite noelTitle;

    [SerializeField] Button btn;

    float t = 3.5f;

    private void Start()
    {
        btn.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        btn.interactable = t < 0;

        loading.color = new Color(1, 1, 1, (Mathf.Cos(Time.time * 5) + 1) / 3f + .5f);

        if (t > 0)
        {
            loading.text = "Loading...";
        }
        else
        {
            loading.text = "";
        }

        t -= Time.deltaTime;

        if (Database.SeasonSkin != null && Database.SeasonSkin.is_activated)
        {
            title.sprite = noelTitle;

            versionTxt.text = version + " (Christmas event)";
        }
        else
        {
            title.sprite = normalTitle;

            versionTxt.text = version;
        }
    }
}
