using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KarakterKontrol : MonoBehaviour
{
    public float turnSpeed, speed, lerpValue;

    public List<GameObject> cubes = new List<GameObject>();
    private Camera cam;
    private Animator animator;
    public LayerMask layer;
    public Transform ToplanacaklarAnaObjesi;
    public GameObject prevObject;
    private Touch touch;
    private float speedModifier;
    public int collectedCube = 0;
    public bool isFinishedChar = false;

    //Other Components
    KarakterAI karakterAI;
    GameManager gameManager;


    void Start()
    {
        speedModifier = 0.01f;
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        #region Singleton Movement

        if (Input.GetMouseButton(0) )
        {
            Movement();
        }
        else
        {
            if (animator.GetBool("running"))
            {
                animator.SetBool("running", false);
            }
        }
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount > 0 && !isFinishedChar && karakterAI.isFinishedAI == false)
        {
            touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(
                    transform.position.x + touch.deltaPosition.x * speedModifier,
                    transform.position.y,
                    transform.position.z+ touch.deltaPosition.y * speedModifier
                );
            }
        }
#endif
        #endregion

        if (collectedCube == 48)
        {
            // gameManager.finishPanelWin.SetActive(true);
            GameManager.instance.GameOver(true);
            isFinishedChar = true;
        }
    }

    private void Movement()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.transform.localPosition.z;

        Ray ray = cam.ScreenPointToRay(mousePos);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            Vector3 hitVec = hit.point;
            hitVec.y = transform.position.y;

            transform.position = Vector3.MoveTowards(transform.position, Vector3.Lerp(transform.position, hitVec, lerpValue), Time.deltaTime * speed);
            Vector3 newMowePoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newMowePoint - transform.position), turnSpeed * Time.deltaTime);
            if (!animator.GetBool("running"))
            {
                animator.SetBool("running", true);
            }
        }
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

            target.tag = "Untagged";

            GenerateCubes.instance.GenerateCube(1);

        }

        if (cubes.Count > 1 && target.gameObject.tag == "DizR" || cubes.Count > 1 && target.gameObject.tag != "Diz" + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1) && target.gameObject.tag.StartsWith("Diz"))
        {
            GameObject obje = cubes[cubes.Count - 1];
            cubes.RemoveAt(cubes.Count - 1);
            Destroy(obje);

            target.GetComponent<MeshRenderer>().material = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
            target.GetComponent<MeshRenderer>().enabled = true;
            //target.GetComponent<BoxCollider>().isTrigger = false;
            target.tag = "Diz" + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1);
            target.transform.parent.tag = "Diz" + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1);

            prevObject = cubes[cubes.Count - 1];

            collectedCube++;
        }
    }
}
