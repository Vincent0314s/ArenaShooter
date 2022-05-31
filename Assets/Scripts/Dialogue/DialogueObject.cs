using UnityEngine;
using TMPro;

public class DialogueObject : MonoBehaviour
{

    public float headHeight = 4;

    [Header("Reference")]
    private SpriteRenderer dialogueBG;
    private TextMeshPro dialogueText;
    private TextMeshPro nextText;

    private Transform currentActor;
    private int pageCount = 1;

    private void Awake()
    {
        dialogueBG = transform.GetChild(0).GetComponent<SpriteRenderer>();
        dialogueText = transform.GetChild(1).GetComponent<TextMeshPro>();
        nextText = transform.GetChild(2).GetComponent<TextMeshPro>();
    }
    private void Start()
    {
        ShowDialogueBox(false);
    }

    private void Update()
    {
        if (currentActor != null) { 
            Vector3 headPosition = currentActor.position + Vector3.up * headHeight;
            transform.position = headPosition;
        }
    }

    public void ShowDialogueBox(bool _activate) {
        dialogueBG.enabled = _activate;
        dialogueText.enabled = _activate;
        nextText.enabled = _activate;
        ResetPageCount();
        if(!_activate)
            currentActor = null;
    }

    public void ReadText(string _context) {
        dialogueText.text = _context;
    }

    public bool CanGoNextPage() {
        if (dialogueText.textInfo.pageCount > 1 && pageCount < dialogueText.textInfo.pageCount)
        {
            pageCount += 1;
            dialogueText.pageToDisplay = pageCount;
            return true;
        }
        ResetPageCount();
        return false;
    }

    public void SetPositionToActor(Transform _actor)
    {
        currentActor = _actor;
    }

    private void ResetPageCount() {
        pageCount = 1;
        dialogueText.pageToDisplay = pageCount;
    }
}
