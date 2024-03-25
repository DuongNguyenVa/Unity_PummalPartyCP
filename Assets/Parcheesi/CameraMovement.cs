using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;


    private bool isFollow;
    public CinemachineVirtualCamera normalCam;
    public CinemachineVirtualCamera overViewCam;
    public CinemachineVirtualCamera chestCam;
    //public CinemachineVirtualCamera overViewCam;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //cameras[1].enabled = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TurnOnOverView();
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (overViewCam.enabled)
        {
            overViewCam.transform.Translate(new Vector3(h, v, 0) *Time.deltaTime*50f);
        }
    }
    private void SwitcCamera()
    {

    }
    private void TurnOnOverView()
    {
        if (!overViewCam.enabled)
        {
            Vector3 pos = PlayerControllerParchessi.Instance.transform.position;
            overViewCam.transform.position = new Vector3(pos.x, overViewCam.transform.position.y, pos.z);
        }
        overViewCam.enabled = !overViewCam.enabled;
    }

    public void SetTagetForCamChest(Transform target)
    {
        chestCam.Follow = target;
        chestCam.LookAt = target;
        //Invoke(nameof(LookPlayer), 2f);
       
    }
    public void ToogleChestView()
    {       
        chestCam.enabled = !chestCam.enabled;
    }
    void LookPlayer()
    {
        Transform trans = PlayerControllerParchessi.Instance.transform;

        normalCam.Follow = trans;
        normalCam.LookAt = trans;
    }
}
