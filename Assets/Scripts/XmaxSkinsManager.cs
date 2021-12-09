using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XmaxSkinsManager : MonoBehaviour
{
    [System.Serializable]
    public struct EventHolder
    {
        public List<SpriteRenderer> renderers;
        public List<Animator> animators;
        public List<string> animatorString;
        public List<Material> materials;
    }

    public EventHolder normal;
    public EventHolder christmas;

    void ClearDefault()
    {
        foreach(SpriteRenderer renderer in normal.renderers)
        {
            renderer.enabled = false;
        }

        foreach(SpriteRenderer renderer in christmas.renderers)
        {
            renderer.enabled = false;
        }
    }

    void Start()
    {
        ClearDefault();

        if (Database.SeasonSkin != null && Database.SeasonSkin.is_activated)
        {
            for(int i = 0; i < christmas.renderers.Count; i++)
            {
                christmas.renderers[i].enabled = true;

                christmas.animators[i].SetTrigger(christmas.animatorString[i]);
                christmas.renderers[i].material = christmas.materials[i];
            }
        }
        else
        {
            for (int i = 0; i < normal.renderers.Count; i++)
            {
                //Debug.Log(i);

                normal.renderers[i].enabled = true;

                normal.animators[i].SetTrigger(normal.animatorString[i]);
                normal.renderers[i].material = normal.materials[i];
            }
        }
    }
}
