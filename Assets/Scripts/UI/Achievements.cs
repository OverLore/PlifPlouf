using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class Achievements : MonoBehaviour
{

    [SerializeField] public GameObject AchivmentPrefab;
    //GameObject[] AchivmentTab;
    [SerializeField] List<AchivmentStruct> tabAchivementStruc;
    [SerializeField] List<GameObject> tabGameObject;

    [SerializeField] Sprite UnactivatedMedal;
    [SerializeField] Sprite ActivatedMedal;

    void Start()
    {
        Vector3 prefPos = gameObject.transform.position;
        prefPos.y += 1600;
        for (int i = 0; i < tabAchivementStruc.Count ; i++)
        {
            GameObject go = Instantiate(AchivmentPrefab, prefPos, Quaternion.identity, gameObject.transform);

            go.transform.Find("Title").GetComponent<Text>().text = tabAchivementStruc[i].Name;
            go.transform.Find("Desc").GetComponent<Text>().text = tabAchivementStruc[i].Desc;

            prefPos.y -= 300;
            // tabGameObject.Add = go;
            tabGameObject.Add(go);
        }



    }
    void Update()
    {
        for (int i = 0; i < tabAchivementStruc.Count; i++)
        {
            if(tabAchivementStruc[i].obtained == 1)
            {
                //tabGameObject[i].GetComponent<SpriteRenderer>().sprite = ActivatedMedal;
               // tabGameObject[i].transform.Find("Image1").GetComponent<SpriteRenderer>().sprite = ActivatedMedal;
            }
            if (tabAchivementStruc[i].obtained == 2)
            {
                //tabGameObject[i].GetComponent<SpriteRenderer>().sprite = ActivatedMedal;
            }
            if (tabAchivementStruc[i].obtained == 3)
            {
                //tabGameObject[i].GetComponent<SpriteRenderer>().sprite = ActivatedMedal;
            }
        }
    }
    [System.Serializable]
    struct AchivmentStruct
    {
        
        [SerializeField] public string Name;
        [SerializeField] public string Desc;
        [SerializeField] public Vector3 Pos;
        [SerializeField] public int obtained;
    }

}
