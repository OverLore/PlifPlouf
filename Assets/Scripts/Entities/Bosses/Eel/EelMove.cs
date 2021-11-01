using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class EelMove : MonoBehaviour
{
    [SerializeField] float speed;
    Transform rootBone;
    Transform[] boneTransforms;
    int nbBone;
       // Start is called before the first frame update
       void Start()
    {
        rootBone = GetComponent<SpriteSkin>().rootBone;
        boneTransforms = GetComponent<SpriteSkin>().boneTransforms;
        nbBone = boneTransforms.Length;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(nbBone);
        for (int i = nbBone-1; i > 0; i--)
        {

            Debug.Log(i);
            Vector3 posA = boneTransforms[i].position;
            Vector3 posB = boneTransforms[i-1].position;
            Vector3 newPos = new Vector3(Mathf.Lerp(posA.x, posB.x, speed), Mathf.Lerp(posA.y, posB.y, speed), 0);
            boneTransforms[i].LookAt(boneTransforms[i - 1]);
            boneTransforms[i].localRotation = Quaternion.Euler(boneTransforms[i].rotation.x, 0, boneTransforms[i].rotation.y);
            boneTransforms[i].position = newPos;
        }
}
}
