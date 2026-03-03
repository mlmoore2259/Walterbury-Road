using UnityEngine;

public class NotebookManager : MonoBehaviour
{
    [SerializeField] GameObject[] notebookPages;
    private int numPages = 5;
    public int currPage = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        notebookPages = new GameObject[numPages];
        InitializeNotebook();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeNotebook()
    {
        // Initialize the notebookPages with the children of the notebook game object
        for (int i = 0; i < numPages; i++)
        {
            notebookPages[i] = transform.GetChild(i).gameObject;
            if (i != 0)
            {
                notebookPages[i].SetActive(false);
            }
        }
    }

    public void SetPage(int prevPage)
    {
        notebookPages[prevPage].SetActive(false);
        notebookPages[currPage].SetActive(true);
    }
}
