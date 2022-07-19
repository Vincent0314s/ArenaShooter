using UnityEngine;
using Utils;

public class DialogueTrigger : MonoBehaviour,IDialogueTrigger
{
    [SerializeField] private DialogueContext dialogue;

    public void BeginDialogue() {
        if (dialogue == null) {
            this.LogError("Empty Dialogue");
            return;
        }

        DialogueManager.Instance.SetDialogueContext(dialogue);
    }
}
