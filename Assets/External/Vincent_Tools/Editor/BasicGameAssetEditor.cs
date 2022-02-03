using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BasicGameAssetManager))]
public class BasicGameAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BasicGameAssetManager bga = (BasicGameAssetManager)target;
        if (GUILayout.Button("Refresh List")) {
            RefreshList();
        }
    }

    void RefreshList() {
        BasicGameAssetManager bga = (BasicGameAssetManager)target;
        int visualEffectLength = 0;
        int soundEffectLength = 0;
        int UISoundEffectLength = 0;

        visualEffectLength = Enum.GetNames(typeof(VisualEffect)).Length;
        soundEffectLength = Enum.GetNames(typeof(SoundEffect)).Length;
        UISoundEffectLength = Enum.GetNames(typeof(UI_SoundEffects)).Length;

        while (bga.visualEffects.Count < visualEffectLength)
        {
            bga.visualEffects.Add(new V_Effect());
        }
        for (int i = 0; i < bga.visualEffects.Count; i++)
        {
            VisualEffect ve = (VisualEffect)Enum.Parse(typeof(VisualEffect), Enum.GetNames(typeof(VisualEffect))[i]);
            bga.visualEffects[i].effect = ve;
        }

        while (bga.soundEffects.Count < soundEffectLength)
        {
            bga.soundEffects.Add(new S_Effect());
        }
        for (int i = 0; i < bga.soundEffects.Count; i++)
        {
            SoundEffect se = (SoundEffect)Enum.Parse(typeof(SoundEffect), Enum.GetNames(typeof(SoundEffect))[i]);
            bga.soundEffects[i].effect = se;
        }

        while (bga.UI_SoundClipArray.Count < UISoundEffectLength)
        {
            bga.UI_SoundClipArray.Add(new S_UI_Effect());
        }
        for (int i = 0; i < bga.UI_SoundClipArray.Count; i++)
        {
            UI_SoundEffects uise = (UI_SoundEffects)Enum.Parse(typeof(UI_SoundEffects), Enum.GetNames(typeof(UI_SoundEffects))[i]);
            bga.UI_SoundClipArray[i].effect = uise;
        }
    }
}
