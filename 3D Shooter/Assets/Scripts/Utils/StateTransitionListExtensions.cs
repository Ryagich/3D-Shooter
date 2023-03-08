using System;
using System.Collections.Generic;
using AI;

namespace Utils
{
    public static class StateTransitionListExtensions
    {
        public static void Add(this IList<StateTransition> source, Func<bool> trigger, IState nextState) =>
            source.Add(new StateTransition(trigger, nextState));
    }
}