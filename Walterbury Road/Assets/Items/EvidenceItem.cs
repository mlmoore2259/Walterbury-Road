using UnityEngine;

public class EvidenceItem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public string evidenceName;
    [SerializeField] public bool found = false;
    [SerializeField] PlayerGameInfo playerGameInfo;

    void Start()
    {
        
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
            Destroy(gameObject);
        }
    }
}
