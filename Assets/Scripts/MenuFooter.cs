using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFooter : MonoBehaviour
{
    public enum MenuTab
    {
        Shop,
        Achievements,
        Home,
        Skins,
        Settings
    }

    float DefaultSize;
    float SelectedSize;

    [Header("Settings")]
    [SerializeField] Canvas MainCanvas;
    [SerializeField] Color NormalColor;
    [SerializeField] Color SelectColor;
    [SerializeField] MenuTab SelectedTab;
    [SerializeField] List<Image> ButtonsList = new List<Image>();

    Dictionary<MenuTab, Image> Tabs = new Dictionary<MenuTab, Image>();

    private void Awake()
    {
        Tabs.Add(MenuTab.Shop, ButtonsList[0]);
        Tabs.Add(MenuTab.Achievements, ButtonsList[1]);
        Tabs.Add(MenuTab.Home, ButtonsList[2]);
        Tabs.Add(MenuTab.Skins, ButtonsList[3]);
        Tabs.Add(MenuTab.Settings, ButtonsList[4]);

        DefaultSize = MainCanvas.GetComponent<RectTransform>().rect.width / (ButtonsList.Count + 1);
        SelectedSize = DefaultSize * 2;

        foreach (Image img in ButtonsList)
        {
            img.color = NormalColor;
        }

        OpenTab(SelectedTab);
    }

    private void Update()
    {
        DefaultSize = MainCanvas.GetComponent<RectTransform>().rect.width / (ButtonsList.Count + 1);
        SelectedSize = DefaultSize * 2;

        OpenTab(SelectedTab);
    }

    public void OpenTab(MenuTab tab)
    {
        Tabs[SelectedTab].rectTransform.sizeDelta = new Vector2(DefaultSize, DefaultSize);
        Tabs[SelectedTab].color = NormalColor;


        SelectedTab = tab;

        Tabs[SelectedTab].rectTransform.sizeDelta = new Vector2(SelectedSize, DefaultSize);
        Tabs[SelectedTab].color = SelectColor;
    }

    public void OpenTab(int tabID)
    {
        OpenTab((MenuTab)tabID);
    }
}
