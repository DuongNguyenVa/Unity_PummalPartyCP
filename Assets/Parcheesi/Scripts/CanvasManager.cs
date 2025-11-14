using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    
    public ItemCanvasController pfPlayerCanvasItem;

    private Transform statCanvas;
    //private Transform inventoryCanvas;
    private Transform notiCanvas;

    Dictionary<PlayerControllerParchessi,ItemCanvasController > DictnStatCanvas = new Dictionary<PlayerControllerParchessi, ItemCanvasController>();
    Dictionary<int, string> DictnInventoryCanvas = new Dictionary<int, string>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        statCanvas = transform.Find("CanvasPlayerStat");
        //inventoryCanvas = transform.Find("CanvasInventory");
        notiCanvas = transform.Find("CanvasNoti");
        notiCanvas.GetComponentInChildren<TextMeshProUGUI>().text="";
    }
  
    public void InitStatCanvas(List<PlayerControllerParchessi> listPlayers)
    {
        foreach (PlayerControllerParchessi pl in listPlayers)
        {
            PlayerStatController playerSt = pl.GetComponent<PlayerStatController>(); 
            ItemCanvasController cv = Instantiate(pfPlayerCanvasItem, statCanvas.Find("Panel").transform);
            cv.nameText.GetComponent<TextMeshProUGUI>().text = pl.name;
            cv.UpdateUI(playerSt.maxKey,playerSt.maxHeal);
            DictnStatCanvas.Add(pl, cv);
        }
    }
    public void UpdatePlayerStat(PlayerControllerParchessi player, int key,int hp,float hpSlideVL,int gobIndex=-1)
    {
        ItemCanvasController icc = DictnStatCanvas[player];
        icc.UpdateUI(key, hp, hpSlideVL, gobIndex);
        
    }
    public void PostNoti(string content, float timeExist=0)
    {
        TextMeshProUGUI contenTex = notiCanvas.GetComponentInChildren<TextMeshProUGUI>();
        contenTex.text = content;
        contenTex.gameObject.SetActive(true);
        StartCoroutine(HideConten());

        IEnumerator HideConten()
        {
            yield return new WaitForSeconds(timeExist);
            contenTex.text = "";
        }
    }
}
