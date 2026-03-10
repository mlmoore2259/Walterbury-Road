using UnityEngine;

public class PlayerGameInfo : MonoBehaviour
{
    public int evidenceCollected = 0;
    public int health = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Evidence: " + evidenceCollected);
    }
}
