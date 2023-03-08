using System;

namespace AI
{
    public class StateTransition
    {
        public Func<bool> Trigger { get; }
        public IState NextState { get; }

        public StateTransition(Func<bool> trigger, IState nextState)
        {
            Trigger = trigger;
            NextState = nextState;
        }
    }
}