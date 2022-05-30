using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Context", menuName = "SO/Context")]
public class DialogueContext : ScriptableObject
{
    [System.Serializable]
    public struct Dialogue {
        public MainActor speaker;
        [TextArea(4,4)]
        public string context;
        public AudioClip voiceLine;
    }

    public List<Dialogue> dialogus;
}
