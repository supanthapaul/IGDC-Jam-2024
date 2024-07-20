using System;
using System.Collections.Generic;
using System.Linq;
using BehaviourTrees;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using Node = BehaviourTrees.Node;

[Serializable]
public class BehaviourTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits>
    {
    }

    private BehaviourTree _tree;

    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());
        
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/BehaviourTrees/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
        
        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PopulateView(_tree);
        AssetDatabase.SaveAssets();
    }

    public void PopulateView(BehaviourTree tree)
    {
        _tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if (tree.root == null)
        {
            tree.root = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }
        
        tree.nodes.ForEach(CreateNodeView);
        tree.nodes.ForEach(n =>
        {
            var children = tree.GetChildren(n);
            children.ForEach(c =>
            {
                var parentView = FindNodeView(n);
                var childView = FindNodeView(c);

                var edge = parentView.OutputPort.ConnectTo(childView.InputPort);
                AddElement(edge);
            });
        });
    }

    private NodeView FindNodeView(Node node)
    {
        if (node == null) return default;
        return GetNodeByGuid(node.guid) as NodeView;
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if(graphViewChange.elementsToRemove != null)
        {
            foreach (var element in graphViewChange.elementsToRemove)
            {
                if (element is NodeView nodeView)
                {
                    _tree.DeleteNode(nodeView.Node);
                }
                else if (element is Edge edge)
                {
                    if (edge.input.node is NodeView childView && edge.output.node is NodeView parentView)
                    {
                        _tree.RemoveChild(parentView.Node, childView.Node);
                    }
                }
            }
        }
        
        if(graphViewChange.edgesToCreate != null)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                if(edge.input.node is NodeView childView && edge.output.node is NodeView parentView)
                {
                    _tree.AddChild(parentView.Node, childView.Node);
                }
            }
        }
        
        if(graphViewChange.movedElements != null)
        {
            foreach (var element in graphViewChange.movedElements)
            {
                if (element is NodeView nodeView)
                {
                    nodeView.SortSiblings();
                }
            }
        }
        return graphViewChange;
    }


    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        var types = TypeCache.GetTypesDerivedFrom<TaskNode>();
        foreach (var type in types)
        {
            evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a)=>CreateNode(type));
        }

        var composites = TypeCache.GetTypesDerivedFrom<CompositeNode>();
        foreach (var type in composites)
        {
            evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a)=>CreateNode(type));
        }

        var decorator = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
        foreach (var type in decorator)
        {
            evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a)=>CreateNode(type));
        }
    }
    
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }
    private void CreateNode(Type type)
    {
        var node = _tree.CreateNode(type);
        CreateNodeView(node);
    }

    public void UpdateNodeStates()
    {
        foreach (var node in nodes)
        {
            if (node is NodeView view)
            {
                view.UpdateState();
            }
        }
    }
}