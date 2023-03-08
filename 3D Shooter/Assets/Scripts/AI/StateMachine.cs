using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace AI
{
    public class StateMachine
    {
        public UnityEvent<IState> OnStateChanged { get; } = new UnityEvent<IState>();
        public IState CurrentState { get; private set; }
        private List<StateTransition> _fromAny;

        public void UpdateStates()
        {
            var transition = _fromAny.FirstOrDefault(x => x.Trigger());

            if (transition == null)
                return;

            CurrentState = transition.NextState;
        }

        public void AddFromAnyTransition(Func<bool> trigger, IState nextState)
        {
            _fromAny.Add(new StateTransition(trigger, nextState));
        }
    }
}