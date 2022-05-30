using UnityEngine.InputSystem;

public class DialogueManager : Singleton<DialogueManager>
{
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

        dialogueBox.ReadText(GetContextFromDialoge());
    }

    private void ResetDialogueIndex() {
        dialogueIndex = 0;
    }

}
