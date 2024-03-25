using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    public Transform keyObj;

    private ParticleSystem vfx;
    private Transform target;
    private bool isFollowing;
    private void Start()
    {
        target = PlayerControllerParchessi.Instance.transform;

        vfx = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule main = vfx.main;
        main.startColor = Random.ColorHSV();
    }

    private void LateUpdate()
    {
        if (isFollowing)
        {
            //if (Vector3.Distance(transform.position, target.position) <= 0.1f) return;
            //{
            //    transform.position = Vector3.Lerp(transform.position, target.position, 5f * Time.deltaTime);
            //    if (keyObj.localPosition.magnitude <= 0.2f)
            //    {
            //        isFollowing = false;
            //        return;
            //    }
            //    keyObj.localPosition = Vector3.Lerp(keyObj.localPosition, Vector3.zero, 5f * Time.deltaTime);
            //}
            float d = Vector3.Distance(keyObj.transform.position, target.position);
            if (d >= 0.5f)
            {
                keyObj.position = Vector3.Lerp(keyObj.position, target.position, 5f * Time.deltaTime);
            }
            else BackToPublic();


        }
    }
    public void FollowTarget(Transform tg)
    {
        target = tg;
        isFollowing = true;
        PlayerStatManager.Instance.UpdateKey(1);
    }
    public void BackToPublic()
    {
        target = null;
        isFollowing = false;

        GetComponentInChildren<Collider>().enabled = false;
        gameObject.SetActive(false);
        SpawnKeyEx.Istaince.AddToListKeyVisualCanUse(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerControllerParchessi>(out PlayerControllerParchessi player))
        {
            FollowTarget(player.transform);
        };
    }

}
