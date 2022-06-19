using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using DG.Tweening;

public class CollectCube : MonoBehaviour
{
    public Transform ToplanacaklarAnaObjesi;
    public GameObject prevObject;
    public List<GameObject> cubes = new List<GameObject>();

    private void OnTriggerEnter(Collider target)
    {
        if (target.gameObject.tag.StartsWith(transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)))
        {
            toplama1(target);
        }
        if (cubes.Count > 1 && target.gameObject.tag == "DizR" || cubes.Count > 1 && target.gameObject.tag != "Diz" + transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1) && target.gameObject.tag.StartsWith("Diz"))
        {
            toplama2(target);
        }
    }

    [Rpc]
    public void toplama1(Collider target)
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

    [Rpc]
    public void toplama2(Collider target)
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

        //collectedCube++;
    }
}
