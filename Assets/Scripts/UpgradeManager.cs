using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UI
    [Header("UI Elements")]
    [SerializeField] private GameObject UpgradesPrefab;
    [SerializeField] private GameObject UpgradesCanvas;
    private GameObject UpgradeRoot;

    [System.Serializable]
    private struct Upgrade
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

    [SerializeField] private List<Upgrade> UpgradesList;

    public void CreateUI()
    {
        if (UpgradeRoot != null && UpgradesCanvas.transform.Find("Root").gameObject != null)
        {
            DestroyUI();
        }

        Debug.Log("Upgrade UI created");

        UpgradeRoot = new GameObject("Root");
        UpgradeRoot.transform.SetParent(UpgradesCanvas.transform);
        RectTransform transform = UpgradeRoot.AddComponent<RectTransform>();
        transform.anchorMax = new Vector2(0.5f, 1.0f);
        transform.anchorMin = new Vector2(0.5f, 1.0f);
        transform.pivot = new Vector2(0.5f, 1.0f);
        transform.anchoredPosition = Vector2.zero;

        for (int i = 0; i < UpgradesList.Count; i++)
        {
            Upgrade upgradeInfo = UpgradesList[i];

            GameObject upgradeGO = Instantiate(UpgradesPrefab, UpgradeRoot.transform);
            upgradeGO.name = upgradeInfo.name;
            upgradeGO.transform.Translate(new Vector3(0, -i * upgradeGO.GetComponent<RectTransform>().sizeDelta.y * 1.5f));

            // get elements 
            GameObject background = upgradeGO.transform.Find("Background").gameObject;
            Image icon = background.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI name = background.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI values = background.transform.Find("Values").GetComponent<TextMeshProUGUI>();
            // get button
            GameObject button = upgradeGO.transform.Find("Button").gameObject;
            button.GetComponent<Button>().onClick.AddListener(() => { OnClick(i); });
            var price = button.transform.Find("Price").GetComponent<TextMeshProUGUI>();

            icon.sprite = upgradeInfo.icon;
            name.text = upgradeInfo.name;
            if (upgradeInfo.CurrentTier + 1 < upgradeInfo.TierList.Count)
            {
                values.text = $"now : {upgradeInfo.TierList[upgradeInfo.CurrentTier].value}\t" +
                    $"next : {upgradeInfo.TierList[upgradeInfo.CurrentTier + 1].value}";
                price.text = $"{upgradeInfo.TierList[upgradeInfo.CurrentTier + 1].price}";
            }
            else
            {
                values.text = $"max now {upgradeInfo.TierList[upgradeInfo.CurrentTier].value}";
                button.SetActive(false);
            }
        }
    }

    public void DestroyUI()
    {
        Debug.Log("Upgrade UI destroyed");
        if (Application.isEditor)
        {
            if (UpgradeRoot != null)
            {
                Object.DestroyImmediate(UpgradeRoot);
            }
            else
            {
                var root = UpgradesCanvas.transform.Find("Root").gameObject;
                if (root)
                {
                    Object.DestroyImmediate(UpgradesCanvas.transform.Find("Root").gameObject);
                }
            }
        }
        else
        {
            if (UpgradeRoot != null)
            {
                Object.Destroy(UpgradeRoot);
            }
            else
            {
                var root = UpgradesCanvas.transform.Find("Root").gameObject;
                if (root)
                {
                    Object.Destroy(UpgradesCanvas.transform.Find("Root").gameObject);
                }
            }

        }
    }

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

    private void OnClick(int _ID)
    {

    }

    #endregion
}
