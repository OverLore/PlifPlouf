using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager instance;
    public static UpgradeManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //CreateUI();
        //onClickEvt.AddListener()
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool BuyUpgrade(int _ID)
    {
        var upgrade = UpgradesList[_ID];

        if( GameManager.instance.money - upgrade.TierList[upgrade.CurrentTier+1].price >= 0)
        {
            GameManager.instance.ChangeMoney(-upgrade.TierList[upgrade.CurrentTier+1].price);
            upgrade.CurrentTier++;
            UpgradesList[_ID] = upgrade;
            return true;
        }
        return false;
    }

    #region UI
    [Header("UI Elements")]
    [SerializeField] private GameObject UpgradesPrefab;
    [SerializeField] private GameObject UpgradesCanvas;
    [SerializeField] private GameObject RootPrefab;
    private GameObject UpgradeRoot;

    [SerializeField] private GameObject NotEnoughtMoneyMessage;

    private UnityEvent onClickEvt = new UnityEvent();

    [System.Serializable]
    public struct Upgrade
    {
        public string name;                            // Inspector Element Name
        public Sprite icon;
        [SerializeField] private int currentTier;
        public int CurrentTier
        {
            get => currentTier;
            set
            {
                currentTier = Mathf.Clamp(value, 0, TierList.Count - 1);
            }
        }

        [System.Serializable]
        public struct Tier
        {
            public int price;
            public float value;
        }
        public List<Tier> TierList;
    }

    [SerializeField] public List<Upgrade> UpgradesList;

    public void CreateUI()
    {
        GameObject tmp = UpgradesCanvas.transform.Find("Root")?.gameObject;
        if (UpgradeRoot != null || tmp != null)
        {
            DestroyUI();
        }

        Debug.Log("Upgrade UI created");

        GameObject root = Instantiate(RootPrefab, UpgradesCanvas.transform);
        root.name = "Root";
        UpgradeRoot = root.transform.Find("Panel").gameObject;
        var panelRect = UpgradeRoot.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, 460.0f * UpgradesList.Count);

        for (int i = 0; i < UpgradesList.Count; i++)
        {
            Upgrade upgradeInfo = UpgradesList[i];

            GameObject upgradeGO = Instantiate(UpgradesPrefab, UpgradeRoot.transform);
            upgradeGO.name = upgradeInfo.name;

            // get elements 
            GameObject background = upgradeGO.transform.Find("Background").gameObject;
            Image icon = background.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI name = background.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            icon.sprite = upgradeInfo.icon;
            name.text = upgradeInfo.name;
            GameObject button = upgradeGO.transform.Find("Button").gameObject;
            button.GetComponent<UpgradeDummyScript>().ID = i;

            UpdateUpgradeTextsAndButton(upgradeGO, i);
        }
    }

    public void DestroyUI()
    {
        Debug.Log("Upgrade UI destroyed");
        if (Application.isEditor)
        {
            if (UpgradeRoot != null)
            {
                Object.DestroyImmediate(UpgradeRoot.transform.parent.gameObject);
            }
            else
            {
                var root = UpgradesCanvas.transform.Find("Root").gameObject;
                if (root)
                {
                    Object.DestroyImmediate(root);
                }
            }
        }
        else
        {
            if (UpgradeRoot != null)
            {
                Object.Destroy(UpgradeRoot.transform.parent.gameObject);
            }
            else
            {
                var root = UpgradesCanvas.transform.Find("Root").gameObject;
                if (root)
                {
                    Object.Destroy(root);
                }
            }

        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UpgradeManager)), CanEditMultipleObjects]
    class UpgradeManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UpgradeManager upgradeManager = (UpgradeManager)target;

            if (GUILayout.Button("Create UI"))
            {
                upgradeManager.CreateUI();
            }
            else if (GUILayout.Button("Destroy UI"))
            {
                upgradeManager.DestroyUI();
            }
        }
    }
#endif

public void UpdateUpgradeTextsAndButton(GameObject _upgradeGO, int _ID)
    {
        Upgrade upgradeInfo = UpgradesList[_ID];

        GameObject background = _upgradeGO.transform.Find("Background").gameObject;
        TextMeshProUGUI now = background.transform.Find("Now").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI next = background.transform.Find("Next").GetComponent<TextMeshProUGUI>();
        // get button
        GameObject button = _upgradeGO.transform.Find("Button").gameObject;
        var price = button.transform.Find("Price").GetComponent<TextMeshProUGUI>();

        if (upgradeInfo.CurrentTier + 1 < upgradeInfo.TierList.Count)
        {
            now.text = $"now : {upgradeInfo.TierList[upgradeInfo.CurrentTier].value}";
            next.text = $"next : {upgradeInfo.TierList[upgradeInfo.CurrentTier + 1].value}";
            price.text = $"{upgradeInfo.TierList[upgradeInfo.CurrentTier + 1].price}";
        }
        else
        {
            now.text = $"now    max : {upgradeInfo.TierList[upgradeInfo.CurrentTier].value}";
            next.text = "";
            button.SetActive(false);
            now.GetComponent<RectTransform>().anchorMin
                = new Vector2(0.5f, 0.25f);
            now.GetComponent<RectTransform>().anchorMax
                = new Vector2(0.5f, 0.25f);
            Vector2 ogSize = now.GetComponent<RectTransform>().sizeDelta;
            now.GetComponent<RectTransform>().sizeDelta
                = new Vector2(ogSize.x * 2.5f, ogSize.y);
            now.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    public void OnClick(GameObject _upgradeGO, int _ID)
    {
        if (!BuyUpgrade(_ID))
        {
            NotEnoughtMoneyMessage.GetComponent<Animator>().SetBool("IsShown", true);
        }
        else
        {
            GameObject.FindObjectOfType<MenuFooter>().UpdateMoneyUI();
            UpgradeManager.instance.UpdateUpgradeTextsAndButton(_upgradeGO, _ID);
        }
    }

    #endregion
}
