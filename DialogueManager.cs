using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private List<DialogueByKey> dialogues = new List<DialogueByKey>();
    public static Dictionary<string, GameObject> dialogueDataBase = new Dictionary<string, GameObject>();

    public static GameObject activeDialogue;

    void Awake()
    {
        Instance = this;
        InitializeDialogueDataBase();

        foreach (var dialogue in dialogueDataBase)
        {
            dialogue.Value.GetComponent<Dialogue_>().EndDialogue();
        }
    }

    private void InitializeDialogueDataBase()
    {
        dialogueDataBase.Clear();

        for (int i = 0; i < dialogues.Count; i++)
        {
            dialogueDataBase.Add(dialogues[i].key, dialogues[i].dialogueObj);
        }
    }

    public void StartDialogueByKey(string key)
    {
        if (!dialogueDataBase.ContainsKey(key))
        {
            Debug.LogError($"Key \"{key}\" not found in dialogueDataBase");
            return;
        }
        activeDialogue = dialogueDataBase[key];
        dialogueDataBase[key].GetComponent<Dialogue_>().StartDialogue();
    }
    public void EndDialogueByKey()
    {
        if (activeDialogue != null)
        {
            activeDialogue.SetActive(false);
            activeDialogue = null;
        }
    }
}
[System.Serializable]
public class DialogueByKey {
    public string key;
    public GameObject dialogueObj;
}

