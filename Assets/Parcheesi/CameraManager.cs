using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public enum ForcusType { player, overview, eventGame}

    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera overViewCam;
    public CinemachineVirtualCamera eventCam;

    public Transform pfPlayerCame;

    //public List<CinemachineVirtualCamera> listCamera=new List<CinemachineVirtualCamera>();

    private ForcusType focusType;
    private CinemachineVirtualCamera currentActiceCam;
    //public CinemachineVirtualCamera overViewCam;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
            overViewCam.transform.GetComponentInChildren<Canvas>().enabled=false;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TurnOnOverView();
        }
       

        if (overViewCam.enabled)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            //overViewCam.transform.Translate(new Vector3(h, v, 0) *Time.deltaTime*50f);
            overViewCam.transform.localPosition += new Vector3(h, 0, v) * Time.deltaTime * 50f;
        }
    }
    
    private void TurnOnOverView()
    {

        if (!overViewCam.enabled)
        {
            overViewCam.transform.GetComponentInChildren<Canvas>().enabled = true;

            Vector3 pos = GameManagerParchessi.Instance.GetCurrentPlayerTurn().transform.position;
            overViewCam.transform.position = new Vector3(pos.x, overViewCam.transform.position.y, pos.z-6f);
        }
        else
            overViewCam.transform.GetComponentInChildren<Canvas>().enabled = false;

        overViewCam.enabled = !overViewCam.enabled;

    }

    public void ToogleChestView()
    {       
        eventCam.enabled = !eventCam.enabled;
    }
    
    public void FocusPlayer(Transform target)
    {
        playerCam.Follow = target;
        playerCam.LookAt = target;

        playerCam.enabled=true;
        overViewCam.enabled = false;
        eventCam.enabled = false;

        //currentActiceCam = playerCam;
        focusType = ForcusType.player;
    }
    public void OverView()
    {
        playerCam.enabled = false;
        overViewCam.enabled = true;
        eventCam.enabled = false;

        //currentActiceCam = overViewCam;
        focusType = ForcusType.overview;

    }
    public void FocusEvent()
    {
        playerCam.enabled = false;
        overViewCam.enabled = false;
        eventCam.enabled = true;

        //currentActiceCam = eventCam;
        focusType = ForcusType.eventGame;

    }
    public ForcusType GetCurrentCamFocus()
    {
        return focusType;
    }
    public void SetTargetForEventCam(Transform target)
    {
        eventCam.Follow = target;
        eventCam.LookAt = target;
    }
}
