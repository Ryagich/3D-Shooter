using System.Collections.Generic;

namespace AI
{
    public interface IState
    {
        void OnEnter();
        void FixedUpdate();
        IList<StateTransition> Transitions { get; }
    }
}