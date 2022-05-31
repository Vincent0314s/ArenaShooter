using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DialogueManager : Singleton<DialogueManager>
{
    [System.Serializable]
    public class ActorData {
        public MainActor actor;
        public Transform actorTransform;
    }

    public List<ActorData> actors = new List<ActorData>();

    public DialogueObject dialogueBox;
    private DialogueContext currentDialogueContext;

    private int dialogueIndex;

    public void SetDialogueContext(DialogueContext _dialogue) {
        currentDialogueContext = _dialogue;
        ShowDialogueBox();
    }

    private void ShowDialogueBox() {
        ResetDialogueIndex();
        dialogueBox.ShowDialogueBox(true);
        dialogueBox.ReadText(GetContextFromDialoge());
        SetDialoguePositionToAcotr();
    }

    private string GetContextFromDialoge() 
    {
        return currentDialogueContext.dialogus[dialogueIndex].context;
    }

    public void GoNextPage(InputAction.CallbackContext context) {
        if (currentDialogueContext == null)
            return;

        if (dialogueIndex >= currentDialogueContext.dialogus.Count - 1) { 
            dialogueBox.ShowDialogueBox(false);
            return;
        }
        if (!dialogueBox.CanGoNextPage())
            dialogueIndex += 1;

        SetDialoguePositionToAcotr();
        dialogueBox.ReadText(GetContextFromDialoge());
    }

    private void ResetDialogueIndex() {
        dialogueIndex = 0;
    }

    private void SetDialoguePositionToAcotr()
    {
        foreach (var speaker in actors)
        {
            if (speaker.actor.Equals(currentDialogueContext.dialogus[dialogueIndex].actor))
            {
                dialogueBox.SetPositionToActor(speaker.actorTransform);
            }
        }
    }
}
