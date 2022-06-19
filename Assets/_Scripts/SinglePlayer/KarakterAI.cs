using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public enum Charachter
{
    sıfır = 0,
    iki = 2
}

public class KarakterAI : MonoBehaviour
{
    public Charachter charachterEnum;
    public GameObject targetsParent;
    public List<GameObject> targets = new List<GameObject>();
    //public List<GameObject> ropes = new List<GameObject>();
    // public Transform[] ropes;
    public List<Transform> ropes = new List<Transform>();
    public float radius = 2;
    public Transform ToplanacaklarAnaObjesi;
    public GameObject prevObject;
    public List<GameObject> cubes = new List<GameObject>();
    private Animator animator;
    private NavMeshAgent agent;
    private bool haveTarget = false;
    private Vector3 targetTransform;
    private Transform targetT = null;
    public Transform ttt = null;
    public int collectedCubeAI;
    public bool isFinishedAI = false;

    //Other Components
    KarakterKontrol karakterKontrol;
    public GameManager gameManager;

    void Start()
    {
        for (int i = 0; i < targetsParent.transform.childCount; i++)
        {
            targets.Add(targetsParent.transform.GetChild(i).gameObject);
        }
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!haveTarget)
        {
            ChooseTarget();
        }

        if (
            haveTarget && ttt != null && ttt.tag != "Untagged" && ttt.tag != "Diz" + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>()
            .material.name.Substring(0, 1)
        )
        {
            haveTarget = false;
        }
        if (collectedCubeAI == 48)
        {
            // gameManager.finishPanelWin.SetActive(true);
            GameManager.instance.GameOver(false);
            isFinishedAI = true;
        }
    }

    void ChooseTarget()
    {
        int randomNumber = Random.Range(0, 3);

        if (randomNumber == 0 && cubes.Count >= 5)
        {
            ttt = null;
            // do
            // {
            //     randomRope = Random.Range(0, ropes.Length);
            // }
            // while(ropes[randomRope].tag != "Untagged");
            // target = ropes[randomRope];
            for (int i = 0; i < ropes.Count; i++)
            {
                if (
                    ropes[i].tag == "Untagged" || ropes[i].tag == "Diz" + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>()
                    .material.name.Substring(0, 1)
                )
                    {
                        ttt = ropes[i];
                    }
            }

            List<Transform> ropesNonActiveChild = new List<Transform>();

            foreach (Transform item in ttt)
            {
                if (
                    !item.GetComponent<MeshRenderer>().enabled || item.GetComponent<MeshRenderer>().enabled && item.gameObject.tag != "Diz" +
                     transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)
                    )
                        {
                            ropesNonActiveChild.Add(item);
                        }
            }

            targetTransform = cubes.Count > ropesNonActiveChild.Count ? ropesNonActiveChild[ropesNonActiveChild.Count - 1].position : ropesNonActiveChild[cubes.Count].position;
        }
        else
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            List<Vector3> ourColors = new List<Vector3>();
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag.StartsWith(transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)))
                {
                    ourColors.Add(hitColliders[i].transform.position);
                }
            }
            if (ourColors.Count > 0)
            {
                targetTransform = ourColors[0];
            }
            else
            {
                int random = Random.Range(0, targets.Count);
                float distance = 99999;
                GameObject nearTarget = null;
                foreach (var target in targets)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) < distance)
                    {
                        distance = Vector3.Distance(transform.position, target.transform.position);
                        nearTarget = target;
                    }
                }
                // targetTransform = targets[random].transform.position;
                targetTransform = nearTarget.transform.position;
            }
        }
        //if (isFinishedAI == false )
        //{
            agent.SetDestination(targetTransform);
        //}

        if (!animator.GetBool("running"))
        {
            animator.SetBool("running", true);
        }

        haveTarget = true;
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag.StartsWith(transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)))
        {
            target.transform.SetParent(ToplanacaklarAnaObjesi);
            Vector3 pos = prevObject.transform.localPosition;

            pos.y += 0.22f;
            pos.z = 0;
            pos.x = 0;

            target.transform.localRotation = new Quaternion(0, 0.7071068f, 0, 0.7071068f);

            target.transform.DOLocalMove(pos, 0.2f);
            prevObject = target.gameObject;
            cubes.Add(target.gameObject);

            targets.Remove(target.gameObject);
            target.tag = "Untagged";
            haveTarget = false;

            GenerateCubes.instance.GenerateCube((int)charachterEnum, this);
        }
        else if (
            target.gameObject.tag != "Diz" + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>()
            .material.name.Substring(0, 1) && target.gameObject.tag.StartsWith("Diz")
            )
                {
                    if (cubes.Count > 1)
                    {
                    GameObject obje = cubes[cubes.Count - 1];
                    cubes.RemoveAt(cubes.Count - 1);
                    Destroy(obje);

                    target.GetComponent<MeshRenderer>().material = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
                    target.GetComponent<MeshRenderer>().enabled = true;
                    //target.GetComponent<BoxCollider>().isTrigger = false;
                     //target.tag = tag;
                    target.tag = "Diz" + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1);
                    target.transform.parent.tag = "Diz" + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1);
                    targetT = target.transform.parent;
                    collectedCubeAI++;
                    }
                    else
                    {
                        prevObject = cubes[0].gameObject;
                         haveTarget = false;
                    }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
