using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDiceEvent : MonoBehaviour
{
    private Vector3 startPos;
    private Transform deiceObjParent;

    public Transform deiceObj;
    private Rigidbody rig;
    public float force;
    public ParticleSystem vfx;

    private void Start()
    {
        rig = deiceObj.GetComponent<Rigidbody>();
        startPos = deiceObj.transform.localPosition;
        deiceObjParent = deiceObj.parent;
        deiceObj.gameObject.SetActive(false);
    }
    public void DiceFire()
    {
        vfx.Play();
        vfx.transform.parent = null;
        deiceObj.transform.parent = null;
        deiceObj.gameObject.SetActive(true);
        rig.AddForce(transform.forward * force, ForceMode.Force);
        Invoke(nameof(ResetPos), 2f);
    }
    void ResetPos()
    {
        deiceObj.transform.parent = deiceObjParent;
        deiceObj.gameObject.SetActive(false);
        deiceObj.transform.localPosition = startPos;
        deiceObj.transform.rotation = new Quaternion(0, 0, 0, 0);
        vfx.transform.parent = deiceObj.transform;
        vfx.transform.localPosition =new Vector3(0,0.01f,-0.01f);
    }
}
