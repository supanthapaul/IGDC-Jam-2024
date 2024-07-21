#if UNITY_EDITOR
using Dialogue;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueSO))]
public class DialogueSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Draws the default inspector

        DialogueSO dialogueSO = (DialogueSO)target;

        if (GUILayout.Button("Set Duration from AudioClip"))
        {
            SetDuration(dialogueSO);
        }
    }

    private void SetDuration(DialogueSO dialogueSO)
    {
        if (dialogueSO.audio != null)
        {
            dialogueSO.duration = dialogueSO.audio.length;
            EditorUtility.SetDirty(dialogueSO); // Marks the object as dirty to ensure the change is saved
        }
        else
        {
            Debug.LogWarning("AudioClip is null. Cannot set duration.");
        }
    }
}
#endif