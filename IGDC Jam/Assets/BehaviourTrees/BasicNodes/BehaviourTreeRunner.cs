using UnityEngine;

namespace BehaviourTrees
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        [SerializeField]
        protected BehaviourTree behaviourTree;
        
        public BehaviourTree BehaviourTree => behaviourTree;

        protected virtual void Start()
        {
            behaviourTree = behaviourTree.Clone();
        }

        protected virtual void Update()
        {
            behaviourTree.Update();
        }
    }
}