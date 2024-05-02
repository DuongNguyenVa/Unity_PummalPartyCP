using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;
    
    public ItemCanvasController pfPlayerCanvasItem;

    private Transform statCanvas;
    //private Transform inventoryCanvas;
    private Transform notiCanvas;
    private Transform notiOpenChest;
    public Button bntIgnoreChest;
    public Button bntOpenChest;

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

        notiOpenChest = transform.Find("CanvasOpenChestNoti");
        notiOpenChest.gameObject.SetActive(false);

        
    }
    private void OnEnable()
    {
        bntOpenChest.onClick.AddListener(GotGoblet);
        bntIgnoreChest.onClick.AddListener(IgnoreChest);
    }
    private void OnDisable()
    {
        bntOpenChest.onClick.RemoveListener(GotGoblet);
        bntIgnoreChest.onClick.RemoveListener(IgnoreChest);
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
    public void ToggleOpenChestNoti(bool isShow,bool canbeOpen=false)
    {
        Debug.Log(canbeOpen);
        notiOpenChest.gameObject.SetActive(isShow);
        bntOpenChest.interactable = canbeOpen;
    }
    public void GotGoblet()
    {
        notiOpenChest.gameObject.SetActive(false);

        PlayerControllerParchessi player = GameManagerParchessi.Instance.GetCurrentPlayerTurn();
        player.GetComponent<PlayerStatController>().UpdateStat(PlayerStatController.UpdateStatType.key, -40);
        //GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.DarkSpace);
        StepManager.Instance.AsSomeGetGoblet();
        player.FaceToST(Camera.main.transform.position);
        GameManagerParchessi.Instance.GetCurrentPlayerTurn().UpdateStat(PlayerStatController.UpdateStatType.gob, +1);
        if (player.GetComponent<PlayerStatController>().gobscount == GameManagerParchessi.Instance.soGameManager.gobletNumToWin)
        {
            player.Win();
            //GameManagerParchessi.Instance.StopAllCoroutines();
            GameManagerParchessi.Instance.SetState(GameManagerParchessi.StateGameParchessi.SomeOneWin);
            CanvasManager.Instance.PostNoti(player.name + " Win", 1000f);
        }
        else
        {
            player.DoDance();
            if (player.CheckNumSaving())
            {
                GameManagerParchessi.Instance.WaitEndEffect(5f, true, true);
                Debug.Log("Got goblet but still move");
            }
            else
            {
                GameManagerParchessi.Instance.WaitEndEffect(5f, true);
            }
        }
    }
    public void IgnoreChest()
    {
        notiOpenChest.gameObject.SetActive(false);
        GameManagerParchessi.Instance.WaitEndEffect(0f,false,true);

    }

}
