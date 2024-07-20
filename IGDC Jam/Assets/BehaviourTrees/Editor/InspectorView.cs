using UnityEditor;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    private Editor editor;
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits>{}
    public InspectorView()
    {
        
    }

    public void UpdateSelection(NodeView view)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);
        
        editor = Editor.CreateEditor(view.Node);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            if(editor !=null && editor.target)
                editor.OnInspectorGUI();
        });
        Add(container);
    }
}
