namespace BehaviourTrees
{
    /// <summary>
    /// Interface which handles all the Cross Node Communications
    /// </summary>
    public interface ITreeData
    {
        public void SetSharedData(string key, object value);
        public object GetSharedData(string key);
        public Node GetRootNode();
    }
}