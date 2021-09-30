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

    Dictionary<MenuTab, FooterButtonSizeController> Tabs = new Dictionary<MenuTab, FooterButtonSizeController>();

    private void Start()
    {
        Tabs.Add(MenuTab.Shop, ButtonsList[0].GetComponent<FooterButtonSizeController>());
        Tabs.Add(MenuTab.Achievements, ButtonsList[1].GetComponent<FooterButtonSizeController>());
        Tabs.Add(MenuTab.Home, ButtonsList[2].GetComponent<FooterButtonSizeController>());
        Tabs.Add(MenuTab.Skins, ButtonsList[3].GetComponent<FooterButtonSizeController>());
        Tabs.Add(MenuTab.Settings, ButtonsList[4].GetComponent<FooterButtonSizeController>());

        DefaultSize = MainCanvas.GetComponent<RectTransform>().rect.width / (ButtonsList.Count + 1);
        SelectedSize = DefaultSize * 2;

        foreach (KeyValuePair<MenuTab, FooterButtonSizeController> tab in Tabs)
        {
            tab.Value.SetDefault(DefaultSize, NormalColor);
        }

        ForceTab(SelectedTab);
    }

    public void OpenTab(MenuTab tab)
    {
        if (tab == SelectedTab)
        {
            return;
        }

        Tabs[SelectedTab].SetTargetSize(DefaultSize, NormalColor);

        SelectedTab = tab;

        Tabs[SelectedTab].SetTargetSize(SelectedSize, SelectColor);
    }

    public void ForceTab(MenuTab tab)
    {
        Tabs[SelectedTab].SetTargetSize(DefaultSize, NormalColor);

        SelectedTab = tab;

        Tabs[SelectedTab].ForceTarget(SelectedSize, SelectColor);
    }

    public void OpenTab(int tabID)
    {
        OpenTab((MenuTab)tabID);
    }
}
