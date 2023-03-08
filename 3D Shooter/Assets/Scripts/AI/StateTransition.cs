using System;
using UnityEngine.EventSystems;

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

        public static explicit operator StateTransition((Func<bool>, IState) tuple) => new(tuple.Item1, tuple.Item2);
    }
}