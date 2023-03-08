using System.Linq;
using AI.States;
using UnityEngine.Events;

namespace AI
{
    public class StateMachine
    {
        public IState Entry { get; }
        public IState AnyState { get; }
        public IState Exit { get; }

        public IState CurrentState { get; private set; }

        public UnityEvent<IState> OnStateChanged { get; } = new();

        public StateMachine()
        {
            Entry = new State();
            AnyState = new State();
            Exit = new State();

            CurrentState = Entry;
        }


        public void UpdateStates()
        {
            var transition = AnyState.Transitions
                .Concat(CurrentState.Transitions)
                .FirstOrDefault(x => x.Trigger());

            if (transition == null)
                return;

            SetState(transition.NextState);

            if (CurrentState == Exit)
                SetState(Entry);
        }

        private void SetState(IState state)
        {
            CurrentState.OnExit();
            CurrentState = state;
            CurrentState.OnEnter();
        }
    }
}