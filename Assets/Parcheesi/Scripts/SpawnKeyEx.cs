using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKeyEx : MonoBehaviour
{
    public static SpawnKeyEx Istaince;
    private void Awake()
    {
        Istaince = this;
    }
    private string keysOwer;

    private List<GameObject> keyDropItems = new List<GameObject>();
    public List<GameObject> keyVisualCanUse = new List<GameObject>();
    private int nextIndexKeyused;

    public int keysCount;
    public GameObject pfKeyDropItem;
    public int keysDropTets = 1;
    public int maxKeydrop = 15;
    private void Start()
    {
        for (int i = 0; i < maxKeydrop; i++)
        {
            InstaintiteNewKeyIteam();
        }
    }
    public void SpawnKeys()
    {
        foreach (GameObject item in keyDropItems)
        {
            item.SetActive(true);
            item.GetComponentInChildren<Animation>().Play("keydrop");
        }
        //fbKeyObj.GetComponent<Rigidbody>().AddForce(Vector3.up)
    }
    public void SpawnIntKeys(int kc) //16
    {
        int indexElse = maxKeydrop - nextIndexKeyused;
        if (kc - indexElse > 0)
        {
            for (int i = 0; i < kc - indexElse; i++)
            {
                InstaintiteNewKeyIteam();
            }
            maxKeydrop += kc - indexElse;
        }

        for (int i = nextIndexKeyused; i < maxKeydrop; i++)
        {
            keyDropItems[i].SetActive(true);
            keyDropItems[i].GetComponentInChildren<Animation>().Play("keydrop");
            nextIndexKeyused++;
        }
    }

    public void SpawnIntKeysUsed(int kc, Transform owerTrans=null) //16
    {
        int indexElse =kc- keyVisualCanUse.Count;
        if (indexElse > 0)
        {
            for (int i = 0; i < indexElse; i++)
            {
                InstaintiteNewKeyIteam();
            }
            maxKeydrop += indexElse;
        }
        int kcs=0;
        for (int i = 0; i < kc; i++)
        {
            if (owerTrans)
            keyVisualCanUse[i].transform.position = owerTrans.position;
            
            keyVisualCanUse[i].SetActive(true);
            keyVisualCanUse[i].GetComponentInChildren<Animation>().Play("keydrop");
            kcs++;
        }
        keyVisualCanUse.RemoveRange(0, kcs);

    }
    private void Update()
    {
      
    }
    private void InstaintiteNewKeyIteam()
    {
        GameObject keyDropItem = Instantiate(pfKeyDropItem, transform);
        keyDropItem.transform.rotation = Quaternion.Euler(Random.Range(0, 10), Random.Range(0, 360), Random.Range(0, 10));
        keyDropItems.Add(keyDropItem);
        keyVisualCanUse.Add(keyDropItem);
        keyDropItem.SetActive(false);


    }
    public void AddToListKeyVisualCanUse(GameObject goj)
    {
        keyVisualCanUse.Add(goj);
    }
}
