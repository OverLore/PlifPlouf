using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOnClick : MonoBehaviour
{

    public int ID;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickEvent()
    {
        UpgradeManager.Instance.OnClick(this.transform.parent.gameObject, ID);
    }
}
