using System;
using BehaviourTrees;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Node = BehaviourTrees.Node;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;
    public Node Node;
    public Port InputPort;
    public Port OutputPort;
    private Label _label;

    public NodeView(Node node) : base ("Assets/BehaviourTrees/Editor/NodeView.uxml")
    {
        Node = node;
        title = node.name;
        viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
        SetUpClasses();
        _label = this.Q<Label>("description");
    }

    private void SetUpClasses()
    {
        switch (Node)
        {
            case TaskNode:
                AddToClassList("task");
                break;
            case CompositeNode:
                AddToClassList("composite");
                break;
            case DecoratorNode:
                AddToClassList("decorator");
                break;
            case RootNode:
                AddToClassList("root");
                break;
        }
    }

    private void CreateInputPorts()
    {
        switch (Node)
        {
            case TaskNode:
            case CompositeNode:
            case DecoratorNode:
                InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                break;
            case RootNode:
                break;
        }

        if (InputPort == null) return;
        
        InputPort.portName = string.Empty;
        InputPort.style.flexDirection = FlexDirection.Column;
        inputContainer.Add(InputPort);
    }

    private void CreateOutputPorts()
    {
        switch (Node)
        {
            case TaskNode:
                break;
            case CompositeNode:
                OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
                break;
            case DecoratorNode:
            case RootNode:
                OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                break;
        }

        if (OutputPort == null) return;
        
        OutputPort.portName = string.Empty;
        OutputPort.style.flexDirection = FlexDirection.ColumnReverse;
        outputContainer.Add(OutputPort);
    }


    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(Node, "Behaviour Tree(Set Position)");
        Node.position = newPos.position;
        EditorUtility.SetDirty(Node);
    }

    public sealed override string title
    {
        get => base.title;
        set => base.title = value;
    }
    public override void OnSelected()
    {
        OnNodeSelected?.Invoke(this);
    }

    public void SortSiblings()
    {
        if(Node.parent is CompositeNode composite)
        {
            composite.Children.Sort(SortByHorizontalPosition);
        }
    }

    public void UpdateState()
    {
        if (!Application.isPlaying) return;
        _label.text = Node.NodeState.ToString();
        RemoveFromClassList("running");
        RemoveFromClassList("success");
        RemoveFromClassList("failure");
        switch (Node.NodeState)
        {
            case NodeState.Success:
                AddToClassList("success");
                break;
            case NodeState.Failure:
                AddToClassList("failure");
                break;
            case NodeState.Running:
                if(Node.Started)
                    AddToClassList("running");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private int SortByHorizontalPosition(Node a, Node b)
    {
        return a.position.x < b.position.x ? -1 : 1;
    }
}