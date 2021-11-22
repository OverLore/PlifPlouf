using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDummyScript : MonoBehaviour
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

    public void DisableIsShown()
    {
        transform.SetAsLastSibling();
        GetComponent<Animator>().SetBool("IsShown", false);
    }

    public void OnClickEvent()
    {
        UpgradeManager.Instance.OnClick(this.transform.parent.gameObject, ID);
    }
}
