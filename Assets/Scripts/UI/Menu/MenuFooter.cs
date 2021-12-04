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
    // Selectable Canvases

    [SerializeField] Canvas ShopCanvas;
    [SerializeField] Canvas achievementsCanvas;
    [SerializeField] Canvas MapCanvas;
    [SerializeField] Canvas UpgradeCanvas;
    [SerializeField] Canvas OptionCanvas;

    [SerializeField] GameObject statpannel;

    [SerializeField] GameObject MoneyUI;

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
        EnableMapCanvas();

        UpdateMoneyUI();
        UpgradeManager.Instance.GetUI();
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

    private void DisableAllCanvas()
    {

        statpannel.gameObject.SetActive(false);
        ShopCanvas.gameObject.SetActive(false);
        achievementsCanvas.gameObject.SetActive(false);
        MapCanvas.gameObject.SetActive(false);
        UpgradeCanvas.gameObject.SetActive(false);
        OptionCanvas.gameObject.SetActive(false);
    }
    public void EnableShopCanvas()
    {
        DisableAllCanvas();
        ShopCanvas.gameObject.SetActive(true);
    }
    public void EnableAchievementsCanvas() 
    {
        DisableAllCanvas();
        achievementsCanvas.gameObject.SetActive(true);
    }
    public void EnableMapCanvas()
    {
        DisableAllCanvas();
        MapCanvas.gameObject.SetActive(true);
    }
    public void EnableUpgradesCanvas()
    {
        DisableAllCanvas();
        UpgradeCanvas.gameObject.SetActive(true);
        //UpgradeManager.Instance.UpdateUI();
    }
    public void EnableSettingsCanvas()
    {
        DisableAllCanvas();
        OptionCanvas.gameObject.SetActive(true);
    }

    public void UpdateMoneyUI()
    {
        MoneyUI.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.instance.money.ToString();
    }
}
