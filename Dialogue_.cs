using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class Dialogue_ : SoundManager
{
    [Header("System")]
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public string name_speaker;
    public string[] sentences;
    [Header("Boolean")]
    public bool isAddNewItem = false;
    public bool isNeighbourFromDay3 = false;

    int i = 0;
    bool isTypingSentence = false;
    bool IsNewItemGiven = false;
    public GameObject neighbour;

    private void Start()
    {
        i = 0;
        isTypingSentence = false;
    }
    public void StartDialogue()
    {
        dialogueBox.SetActive(true);
        playerCamRotate.Instance.enabled = false;
        playerControl.Instance.enabled = false;
        if (i + 1 <= sentences.Length)
        {
            if (!isTypingSentence)
            {
                StartCoroutine(TypeSentence(sentences[i]));
                isTypingSentence = true;
                playSound(sounds[0], volume: 0.5f, p1: 1f, p2: 1f);
                i++;
            }
        }
        else
        {
            if (isNeighbourFromDay3)
            {
                neighbour.GetComponent<NPC>().isWalkingContinued = true;
            }
            EndDialogue();
            i = 0;
        }
    }
    IEnumerator TypeSentence(string sentence)
    {
        if (name_speaker != "")
            dialogueText.text = name_speaker + ": ";
        else
            dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        isTypingSentence = false;
    }
    public void EndDialogue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCamRotate.Instance.transform.gameObject.GetComponent<playerCamRotate>().enabled = true;
        playerControl.Instance.enabled = true;
        if (gameObject.tag == "NPC" && gameObject.GetComponent<NPC>().isWalking)
            Invoke("continueWalk", 1f); 
        dialogueBox.SetActive(false);
        if (isAddNewItem && !IsNewItemGiven)
        {
            Main.instance.panels[2].gameObject.SetActive(true);
            IsNewItemGiven = true;
        }
    }
    void continueWalk()
    {
        gameObject.GetComponent<NPC>().isWalkingContinued = true;
    }
}
