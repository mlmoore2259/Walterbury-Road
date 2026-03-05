using UnityEngine;

public class NotebookManager : MonoBehaviour
{
    [SerializeField] public GameObject[] notebookPages;
    public int numPages = 5;
    public int currPage = 0;
    public int prevPage;
    public bool newPage = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        notebookPages = new GameObject[numPages];
        InitializeNotebook();
    }

    // Update is called once per frame
    void Update()
    {
        if (newPage)
        {
            SetPage();
        }
    }

    private void InitializeNotebook()
    {
        // Initialize the notebookPages with the children of the notebook game object
        for (int i = 0; i < numPages; i++)
        {
            notebookPages[i] = transform.GetChild(i).gameObject;
            // Set only the first page as active to start
            if (i != 0)
            {
                notebookPages[i].SetActive(false);
            }
        }
    }

    public void SetPage()
    {
        notebookPages[currPage].SetActive(true);
        notebookPages[prevPage].SetActive(false);
        newPage = false;
    }
}
