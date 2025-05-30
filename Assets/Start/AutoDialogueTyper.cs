using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AutoDialogueTyper : MonoBehaviour
{
    [Header("Text Components")]
    public TextMeshProUGUI dialogueText;

    [Header("Dialogue Settings")]
    [TextArea(3, 10)]
    public string[] dialogueLines;
    public float delayBeforeStart = 5f;
    public float typeSpeed = 0.12f;
    public float delayBetweenLines = 2f;

    [Header("Typing Sound")]
    public AudioSource typingSound; // 可選打字聲音

    [Header("Cursor Settings")]
    public bool showBlinkingCursor = true;
    public string cursorChar = "|";
    public float cursorBlinkSpeed = 0.5f;

    private bool isTyping = false;
    private bool cursorVisible = false;

    void Start()
    {
        StartCoroutine(DialogueSequence());
    }

    IEnumerator DialogueSequence()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        foreach (string line in dialogueLines)
        {
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(delayBetweenLines);
        }

        dialogueText.text = ""; // 清除文字或保留最後一句可依需求修改
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i < line.Length; i++)
        {
            char currentChar = line[i];
            dialogueText.text = line.Substring(0, i + 1) + (showBlinkingCursor ? cursorChar : "");

            // 檢查是否為標點符號
            if (typingSound != null && !IsPunctuation(currentChar))
                typingSound.Play();

            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;

        if (showBlinkingCursor)
            StartCoroutine(BlinkCursor());
    }

    bool IsPunctuation(char c)
    {
        // 可依需要擴充這個字元集
        return char.IsPunctuation(c) || 
            c == '，' || c == '。' || c == '！' || c == '…' || 
            c == '？' || c == '；' || c == '、' || 
            c == '"' || c == '」' || c == '『' || c == '』';
    }



    IEnumerator BlinkCursor()
    {
        cursorVisible = true;
        while (!isTyping)
        {
            dialogueText.text = dialogueText.text.EndsWith(cursorChar) ?
                dialogueText.text.Substring(0, dialogueText.text.Length - 1) :
                dialogueText.text + cursorChar;

            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }
}
