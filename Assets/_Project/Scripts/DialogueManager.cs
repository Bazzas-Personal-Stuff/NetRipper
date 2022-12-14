using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TMP_Text dialogueOutputField;
    public ScrollRect dialogueScrollRect;
    public TextAsset[] textAssets;
    public float timePerCharacter = 0.1f;
    public Color usernameColor;
    public Color transmissionMessageColor;
    public Queue<TextAsset> disconnectQueue = new();

    public IEnumerator currentDialogue;

    public void OnDisconnect() {
        if(disconnectQueue.TryDequeue(out TextAsset asset)) {
            RunDialogue(asset);
        }
    }

    public void QueueDisconnectDialogue(TextAsset dialogue) {
        disconnectQueue.Enqueue(dialogue);
    }
    
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    private IEnumerator Start() {
        LevelManager.instance.onDisconnect.AddListener(OnDisconnect);
        yield return new WaitForSeconds(7);
        RunDialogue(0);
    }

    public void RunDialogue(int idx)
    {
        if (idx >= textAssets.Length || idx < 0) return;
        RunDialogue(textAssets[idx]);
    }
    public void RunDialogue(TextAsset textAsset) {
        if (currentDialogue != null) {
            StopCoroutine(currentDialogue);
        }
        
        currentDialogue = DialogueCoroutine(textAsset);
        StartCoroutine(currentDialogue);
    }

    private IEnumerator DialogueCoroutine(TextAsset textAsset, bool clear = true)
    {
        if (clear) {
            dialogueOutputField.text = "";
        }

        PrintDialogue($"<color=#{ColorUtility.ToHtmlStringRGB(transmissionMessageColor)}>[Incoming message]</color>");
        string[] lines = textAsset.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
        float lastWaitTime = 0;
        for(int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            float thisWaitTime = line.Length * timePerCharacter;
            float waitTime = Mathf.Max(thisWaitTime, lastWaitTime);
            lastWaitTime = thisWaitTime;
            
            string[] splitLine = line.Split(' ');
            if (splitLine[0].EndsWith('>'))
            {
                splitLine[0] = $"<color=#{ColorUtility.ToHtmlStringRGB(usernameColor)}>{splitLine[0]}</color>";
            }

            line = String.Join(' ', splitLine);


            yield return new WaitForSeconds(waitTime);  // Wait before printing, the other character is typing
            PrintDialogue(line);
        }

        yield return new WaitForSeconds(2f);
        PrintDialogue($"<color=#{ColorUtility.ToHtmlStringRGB(transmissionMessageColor)}>[Transmission ended]</color>");
    }

    private IEnumerator PrintDialogueInternal(string message)
    {
        AudioManager.instance.PlayMessageSound();
        dialogueOutputField.text += '\n' + message;
        yield return 0;
        dialogueScrollRect.normalizedPosition = Vector2.zero;
    }

    public static void PrintDialogue(string message) {
        instance.StartCoroutine(instance.PrintDialogueInternal(message));
    }

}
