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

    public void CloseBox()
    {
        //Debug.Log("skins tab");
        //gameObject.SetActive(true);
        //GetComponent<Animator>().SetTrigger("IsClosing");
        //DisableIsShown();
        //gameObject.SetActive(false);
    }

    public void DisableIsShown()
    {
        Debug.Log("Disable is shown");
        transform.SetAsLastSibling();
        GetComponent<Animator>().SetBool("IsShown", false);
    }

    public void OnClickEvent()
    {
        UpgradeManager.Instance.OnClick(this.transform.parent.gameObject, ID);
    }
}
