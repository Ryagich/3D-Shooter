using System.Collections.Generic;

namespace AI
{
    public interface IState
    {
        void OnEnter();
        void FixedUpdate();
        void OnExit();
        IList<StateTransition> Transitions { get; }
    }
}