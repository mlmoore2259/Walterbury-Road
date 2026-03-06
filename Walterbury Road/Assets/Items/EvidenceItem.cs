using TMPro;
using UnityEngine;

public class EvidenceItem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public int ID;
    [SerializeField] public bool found = false;
    [SerializeField] PlayerGameInfo playerGameInfo;
    public NotebookManager notebookManager;
    public GameObject[] notebookPages;
    private ItemsData itemsData;

    void Start()
    {
        //playerGameInfo = GameObject.Find("PlayerGameInfo").GetComponent<PlayerGameInfo>();
        notebookManager = GameObject.Find("PlayerUICanvas").transform.GetChild(0).GetComponent<NotebookManager>();
        itemsData = ScriptableObject.CreateInstance<ItemsData>();
    }

    // Update is called once per frame
    void Update()
    {
        OnFound();
    }

    public void OnFound()
    {
        if (found)
        {
            playerGameInfo.evidenceCollected++;
            UpdateNotebook();
            Destroy(gameObject);
        }
    }

    private void UpdateNotebook()
    {
        notebookManager.notebookPages[ID].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemsData.items[ID - 1].profileEntry;
    }
}
