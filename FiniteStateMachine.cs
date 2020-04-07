using System.Collections.Generic;

namespace MorphologicalAnalysis
{
    public class FiniteStateMachine
    {
        private List<State> states;
        private Dictionary<State, List<Transition>> transitions;

        /**
         * <summary>The isValidTransition loops through states ArrayList and checks transitions between states. If the actual transition
         * equals to the given transition input, method returns true otherwise returns false.</summary>
         *
         * <param name="transition">is used to compare with the actual transition of a state.</param>
         * <returns>true when the actual transition equals to the transition input, false otherwise.</returns>
         */
        public bool IsValidTransition(string transition)
        {
            foreach (var state in transitions.Keys) {
                foreach (var transition1 in transitions[state]) {
                    if (transition1.ToString() != null && transition1.ToString().Equals(transition))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /**
         * <summary>The getState method is used to loop through the states {@link ArrayList} and return the state whose name equal
         * to the given input name.</summary>
         *
         * <param name="name">is used to compare with the state's actual name.</param>
         * <returns>state if found any, null otherwise.</returns>
         */
        public State GetState(string name)
        {
            foreach (var state in states) {
                if (state.GetName() == name)
                {
                    return state;
                }
            }
            return null;
        }

        /**
         * <summary>The addTransition method creates a new {@link Transition} with given input parameters and adds the transition to
         * transitions {@link ArrayList}.</summary>
         *
         * <param name="toState"> State type input indicating the next state.</param>
         * <param name="with">    string input indicating with what the transition will be made.</param>
         * <param name="withName">string input.</param>
         */
        public void AddTransition(State fromState, State toState, string with, string withName)
        {
            List<Transition> transitionList;
            var newTransition = new Transition(toState, with, withName);
            if (transitions.ContainsKey(fromState))
            {
                transitionList = transitions[fromState];
            }
            else
            {
                transitionList = new List<Transition>();
            }

            transitionList.Add(newTransition);
            transitions[fromState] = transitionList;
        }

        /**
         * <summary>Another addTransition method which takes additional argument; toPos and. It creates a new {@link Transition}
         * with given input parameters and adds the transition to transitions {@link ArrayList}.</summary>
         *
         * <param name="toState"> State type input indicating the next state.</param>
         * <param name="with">    string input indicating with what the transition will be made.</param>
         * <param name="withName">string input.</param>
         * <param name="toPos">   string input.</param>
         */
        public void AddTransition(State fromState, State toState, string with, string withName, string toPos)
        {
            List<Transition> transitionList;
            var newTransition = new Transition(toState, with, withName, toPos);
            if (transitions.ContainsKey(fromState))
            {
                transitionList = transitions[fromState];
            }
            else
            {
                transitionList = new List<Transition>();
            }

            transitionList.Add(newTransition);
            transitions[fromState] = transitionList;
        }

        /**
         * <summary>The getTransitions method returns the transitions at the given state.</summary>
         *
         * <param name="state">State input.</param>
         * <returns>transitions at given state.</returns>
         */
        public List<Transition> GetTransitions(State state)
        {
            if (transitions.ContainsKey(state))
            {
                return transitions[state];
            }

            return new List<Transition>();
        }
    }
}