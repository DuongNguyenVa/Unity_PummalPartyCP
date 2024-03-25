using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    private Vector3 startPos;
    private Rigidbody rig;
    public float force;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        startPos = transform.localPosition;
    }
    private void OnEnable()
    {
        DiceFire();
    }
    public void DiceFire()
    {
        rig.AddForce(transform.forward* force, ForceMode.Force);

        Invoke("ResetPos", 2f);
    }
    void ResetPos()
    {
        transform.localPosition = startPos;
        transform.rotation =new Quaternion(0,0,0,0);
       
        gameObject.SetActive(false);
    }
}
