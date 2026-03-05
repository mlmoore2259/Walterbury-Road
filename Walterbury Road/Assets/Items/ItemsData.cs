using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsData", menuName = "Scriptable Objects/ItemsData")]
public class ItemsData : ScriptableObject
{
    // Evidence Class
    [System.Serializable]
    public class ItemInfo
    {
        public int ID;
        public string name;
        public bool evidence;
        public string profileEntry;
        public string caseEntry;
        public string dialogue;

        // Constructor
        public ItemInfo(int Id, string EvidenceName, bool isEvidence, string profileEntryText, string caseEntryText, string dialogueText)
        {
            ID = Id;
            name = EvidenceName;
            evidence = isEvidence;
            profileEntry = profileEntryText;
            caseEntry = caseEntryText;
            dialogue = dialogueText;
        }
    }
    public ItemInfo[] items;



    // ID 1
    public ItemInfo evidence1Info = new ItemInfo(
        1,
        "Evidence1",
        true,
        "ProfileText",
        "CaseEntryText",
        "DialogueText"
        );


    // ID 2
    //ItemInfo evidence2Info = new ItemInfo();

    // ID 3
    //ItemInfo evidence3Info = new ItemInfo();

    // ID 4
    //ItemInfo evidence4Info = new ItemInfo();

    // ID 5
    //ItemInfo evidence5Info = new ItemInfo();

    // ID 6
    //ItemInfo evidence6Info = new ItemInfo();

    // Other Items

    private void Awake()
    {
        
        items = new ItemInfo[1];
        items[0] = evidence1Info;
    }

}
