using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class EelMove : MonoBehaviour
{
    [SerializeField] int pathChoose;
    [SerializeField] float delayMax;
    public float EelSpeed;
    public int EelPhase;
    [Range(0f, 1f), SerializeField] float delayRandMax;
    [SerializeField] GameObject[] boneObject;
    DangerSign warningSign;
    bool isWarningFirstTime;
    Vector2 warningSignSize;
    Vector2 screenSize;
    ParticleSystem warningSignParticleSystem;

    public GameObject pathsObject;
    List<List<Transform>> pathsf;

    float delayNextPath = 0f;
    float delayMaxUse;
    float timerSpawn = 0f;
    float timerMax = 0.09f;
    int randomPath = 0;
    int actualBone = 0;
    int nbPaths;
    int[] nbChildPath;
    bool IsMoving = false;
    Transform[] actualPath;
    Vector2[] colliderPoints;


    void ChoosePath()
    {
        if (pathChoose == -1)
        {
            switch (EelPhase)
            {
                case 0:
                    randomPath = Random.Range(0, 3);
                    EelSpeed = 0.3f;
                    break;
                case 1:
                    randomPath = Random.Range(2, 5);
                    EelSpeed = 0.35f;
                    break;
                case 2:
                    randomPath = Random.Range(4, 7);
                    EelSpeed = 0.4f;
                    break;
                case 3:
                    randomPath = Random.Range(6, 9);
                    EelSpeed = 0.45f;
                    break;
                case 4:
                    randomPath = Random.Range(0, nbPaths);
                    EelSpeed = 0.5f;
                    break;
                default:
                    break;
            }
        }
        else
        {
            randomPath = pathChoose;
        }

        for (int i = 0; i < boneObject.Length; i++)
        {
            boneObject[i].transform.position = pathsf[randomPath][0].GetChild(0).transform.position;
        }
    }
    void InitAllPath()
    {
        for (int i = 0; i < nbPaths; i++)
        {
            List<Transform> temp = new List<Transform>();
            temp.Add(pathsObject.transform.GetChild(i));
            pathsf.Add(temp);
            for (int j = 0; j < pathsObject.transform.GetChild(i).transform.childCount; j++)
            {
                nbChildPath[i] = pathsObject.transform.GetChild(i).transform.childCount;
                if (j == 0)
                {
                    pathsf[i][j] = (pathsObject.transform.GetChild(i).transform.GetChild(j));
                }
                else
                {
                    pathsf[i].Add(pathsObject.transform.GetChild(i).transform.GetChild(j));
                }
            }
        }
    }
    
    bool IsPathEnd()
    {
        for (int i = 0; i < boneObject.Length; i++)
        {
            int cur = 0;
            int size = 0;
            if (boneObject[i].GetComponent<SplinePathFollow>() != null)
            {
                size = boneObject[i].GetComponent<SplinePathFollow>().paths.Length - 1;
                cur = boneObject[i].GetComponent<SplinePathFollow>().currentPath;
                if (cur <= size)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    void UpdateBodyPart()
    {
        if (actualBone < boneObject.Length)
        {
            timerSpawn += Time.deltaTime * GameManager.instance.timeScale;
            if (timerSpawn >= ( timerMax / EelSpeed + timerMax) * timerMax + timerMax)
            {
                SplinePathFollow pathFollow;
                if (boneObject[actualBone].GetComponent<SplinePathFollow>() == null)
                {
                    boneObject[actualBone].AddComponent<SplinePathFollow>();
                    pathFollow = boneObject[actualBone].GetComponent<SplinePathFollow>();
                    pathFollow.speed = EelSpeed;
                   
                    pathFollow.correction = 90;
                    pathFollow.isDestroyedAtEnd = false;
                }
                else
                {
                    pathFollow = boneObject[actualBone].GetComponent<SplinePathFollow>();
                    pathFollow.currentPath = 0;
                    pathFollow.speed = EelSpeed;
                }
                pathFollow.paths = new Transform[actualPath.Length];
                for (int i = 0; i < actualPath.Length; i++)
                {
                    pathFollow.paths.SetValue(actualPath[i], i);
                }
                actualBone++;
                timerSpawn -= timerMax;
            }
        }
        if (!IsPathEnd())
        {
            delayNextPath += Time.deltaTime;

            if (delayNextPath >= delayMaxUse)
            {
                delayMaxUse = delayMax + Random.Range(-delayRandMax, delayRandMax);
                actualBone = 0;
                IsMoving = false;
                delayNextPath = 0;
            }
        }
    }
    void UpdateUsePath()
    {
        actualPath = new Transform[pathsf[randomPath].Count];
        for (int i = 0; i < pathsf[randomPath].Count; i++)
        {
            actualPath[i] = pathsf[randomPath][i].transform;
            IsMoving = true;
        }
    }
    
    void UpdateCollision()
    {
        for (int i = 0; i < boneObject.Length; i++)
        {
            colliderPoints = GetComponent<EdgeCollider2D>().points;
            colliderPoints[i].x = boneObject[i].transform.position.x;
            colliderPoints[i].y = boneObject[i].transform.position.y;
            GetComponent<EdgeCollider2D>().points = colliderPoints;
        }
    }

    bool BonneIsOnScreen()
    {
        for (int i = 0; i < boneObject.Length / boneObject.Length; i++)
        {
            Vector3 bonne = boneObject[i].transform.position;

            if ((bonne.x > -screenSize.x + Camera.main.transform.position.x && bonne.x < screenSize.x + Camera.main.transform.position.x &&
                 bonne.y > -screenSize.y + Camera.main.transform.position.y && bonne.y < screenSize.y + Camera.main.transform.position.y))
            {
                Debug.Log("Ta mere la pute fonctione gros chien");
                return true;
            }
        }
        return false;
    }

    void UpdateSign()
    {
        Transform head = boneObject[0].transform;
        warningSign.transform.rotation = head.rotation;
        Vector3 tranformSign = head.position;
        tranformSign -= warningSign.transform.right * 2;
        warningSign.transform.position = tranformSign;

        if (BonneIsOnScreen())
        {
            //warningSign.GetComponent<SpriteRenderer>().enabled = false;
            if (!isWarningFirstTime)
            {
                Debug.Log("stop danger sign");
                warningSign.gameObject.SetActive(false);
                warningSignParticleSystem.Stop();
                isWarningFirstTime = true;
            }
        }
        else
        {
            Vector3 pos = Vector3.zero;

            if (isWarningFirstTime)
            {
                Debug.Log("Play danger sign");
                warningSign.gameObject.SetActive(true);
                warningSignParticleSystem.Play();
                isWarningFirstTime = false;
                Debug.Log(warningSignParticleSystem.time);
            }
            //the head is :
            //over right of the screen
            if (head.position.x >= -screenSize.x + Camera.main.transform.position.x)
            {
                pos.x = screenSize.x + Camera.main.transform.position.x - warningSignSize.x / 2;

                //under the screen
                if (head.position.y <= -screenSize.y + Camera.main.transform.position.y)
                {
                    pos.y = -screenSize.y + Camera.main.transform.position.y + warningSignSize.y;
                }
                //over the screen
                else if (head.position.y >= screenSize.y + Camera.main.transform.position.y)
                {
                    pos.y = screenSize.y + Camera.main.transform.position.y - warningSignSize.y;
                }
                //in between
                else
                {
                    pos.y = head.position.y;
                }
            }
            //over the left of the screen
            else
            {
                pos.x = -screenSize.x + Camera.main.transform.position.x + warningSignSize.x / 2;

                //under the screen
                if (head.position.y <= -screenSize.y + Camera.main.transform.position.y)
                {
                    pos.y = -screenSize.y + Camera.main.transform.position.y + warningSignSize.y;
                }
                //over the screen
                else if (head.position.y >= screenSize.y + Camera.main.transform.position.y)
                {
                    pos.y = screenSize.y + Camera.main.transform.position.y - warningSignSize.y;
                }
                //in between
                else
                {
                    pos.y = head.position.y;
                }
            }
            warningSign.transform.position = pos;
            //warningSign.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        nbPaths = pathsObject.transform.childCount;
        pathsf = new List<List<Transform>>();
        nbChildPath = new int[nbPaths];
        delayMaxUse = delayMax + Random.Range(-delayRandMax, delayRandMax);
        InitAllPath();
        ChoosePath();
        warningSign = DangerSignManager.instance.GetEelDangerSign(gameObject.transform.position);
        warningSignParticleSystem = DangerSignManager.instance.GetEelDangerSignParticleSystem(warningSign);
        warningSignSize = DangerSignManager.instance.GetEelDangerSignSize(warningSign);
        isWarningFirstTime = true;
        screenSize = new Vector2(ScreenSize.GetScreenToWorldWidth / 2, ScreenSize.GetScreenToWorldHeight / 2);
        Debug.Log(warningSignSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMoving)
        {
            ChoosePath();
            UpdateUsePath();
        }
        else
        {
            UpdateBodyPart();
        }
        UpdateCollision();
        UpdateSign();
        if (GetComponent<Enemy>().PV <= 0)
        {
            LevelManager.instance.state = LevelState.BossEnd;
        }
    }

     private void OnTriggerEnter2D(Collider2D collision)
     {

     }

}
