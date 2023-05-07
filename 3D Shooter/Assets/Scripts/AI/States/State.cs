using System.Collections.Generic;

namespace AI.States
{
    public class State : IState
    {
        public virtual void OnEnter()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }

        public IList<StateTransition> Transitions { get; } = new List<StateTransition>();
    }
}