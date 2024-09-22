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

    // Хранит в себе диалог который проигрывается в текущий момент, если ни одного диалога сейчас не проигрывается - он равен null
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
        // Перед заполнением на всякий случай очищаем нашу базу данных
        dialogueDataBase.Clear();

        // Заполняем dialogueDataBase ключами и значениями которые мы укажем в листе dialogues
        for (int i = 0; i < dialogues.Count; i++)
        {
            dialogueDataBase.Add(dialogues[i].key, dialogues[i].dialogueObj);
        }
    }

    public void StartDialogueByKey(string key)
    {
        if (!dialogueDataBase.ContainsKey(key))
        {
            Debug.LogError($"диалога c ключом \"{key}\" нету в dialogueDataBase");
            return;
        }
        activeDialogue = dialogueDataBase[key];
        // запускаем с нашим ключом
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
