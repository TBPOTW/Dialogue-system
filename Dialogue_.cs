using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue_ : MonoBehaviour
{
    [Header("System")]
    public GameObject dialogueBox; // Окно диалогов в Canvas
    public TextMeshProUGUI dialogueText; // Текст предложения в диалоговом окне 
    public string name_speaker; // Имя собеседника
    public string[] sentences; // Предложения, которые будут проигрываться в диалоге

    private int i; // Индекс предложения в диалоге 
    private bool isTypingSentence = false; // Печатается ли предложение в данный момент
    private bool isDialogueActive = false; // Есть ли активный диалог в данный момент

    private void Start()
    {
        i = 0; // Ставим индекс предложения равным 0-лю, чтобы печатать с 1-го предложения
        isTypingSentence = false;
    }
    // Функция для начала диалога
    public void StartDialogue() 
    {
        if (i + 1 > sentences.Length) 
        {
            EndDialogue();
            return;
        }

        dialogueBox.SetActive(true); // Активируем окно диалогов в Canvas 
        isDialogueActive = true; 
        if (!isTypingSentence) 
            StartCoroutine(TypeSentence(sentences[i])); 
        isTypingSentence = true;
    }

    private void Update()
    {
        if (isDialogueActive && !isTypingSentence)
        {
            // Условие написания нового предложения 
            if (Input.GetKeyDown(KeyCode.E))
                ContinueDialogue();
        }
    }
    // Функция для продолжения диалога
    public void ContinueDialogue()
    {
        // Если не выходим за пределы массива предложений в диалоге 
        if (i + 1 < sentences.Length)
        {
            i++;
            isTypingSentence = true;
            StartCoroutine(TypeSentence(sentences[i]));
        }
        // Если вышли за пределы - заканчиваем диалог
        else
        {
            EndDialogue();
        }
    }
    private int rnd;
    // Коорутина для последовательного написания предложения
    IEnumerator TypeSentence(string sentence)
    {
        rnd = Random.Range(0, DialogueManager.Instance.sounds.Count);
        if (name_speaker != "")
            dialogueText.text = name_speaker + ": ";
        else
            dialogueText.text = "";
        // Проходимся по предложению побуквенно
        foreach (char letter in sentence.ToCharArray())
        {
            // Добавляем 1 букву и ждем 0.07 секунд
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.07f);
            DialogueManager.Instance.playSound(DialogueManager.Instance.sounds[rnd], volume: 0.1f, p1: 1f, p2: 1f);
        }
        isTypingSentence = false;
    }
    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        isDialogueActive = false;
    }
}
