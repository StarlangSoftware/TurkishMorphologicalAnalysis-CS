using System.Collections.Generic;
using System.Xml;

namespace MorphologicalAnalysis
{
    public class FiniteStateMachine
    {
        private readonly List<State> _states;
        private readonly Dictionary<State, List<Transition>> _transitions;

        /**
         * <summary>Constructor reads the finite state machine in the given input file. It has a NodeList which holds the states
         * of the nodes and there are 4 different type of nodes; stateNode, root Node, transitionNode and withNode.
         * Also there are two states; state that a node currently in and state that a node will be in.
         * <p/>
         * DOMParser is used to parse the given file. Firstly it gets the document to parse, then gets its elements by the
         * tag names. For instance, it gets states by the tag name 'state' and puts them into an ArrayList called stateList.
         * Secondly, it traverses this stateList and gets each Node's attributes. There are three attributes; name, start,
         * and end which will be named as states. If a node is in a startState it is tagged as 'yes', otherwise 'no'.
         * Also, if a node is in a startState, additional attribute will be fetched; originalPos that represents its original
         * part of speech.
         * <p/>
         * At the last step, by starting rootNode's first child, it gets all the transitionNodes and next states called toState,
         * then continue with the nextSiblings. Also, if there is no possible toState, it prints this case and the causative states.</summary>
         *
         * <param name="fileName">the resource file to read the finite state machine. Only files in resources folder are supported.</param>
         */
        public FiniteStateMachine(string fileName)
        {
            string stateName;
            var assembly = typeof(FiniteStateMachine).Assembly;
            var stream = assembly.GetManifestResourceStream("MorphologicalAnalysis." + fileName);
            var doc = new XmlDocument();
            doc.Load(stream);
            _transitions = new Dictionary<State, List<Transition>>();
            _states = new List<State>();
            foreach (XmlNode stateNode in doc.DocumentElement.ChildNodes)
            {
                stateName = stateNode.Attributes["name"].Value;
                var startState = stateNode.Attributes["start"].Value == "yes";
                var endState = stateNode.Attributes["end"].Value == "yes";
                if (startState)
                {
                    var originalPos = stateNode.Attributes["originalpos"].Value;
                    _states.Add(new State(stateName, true, endState, originalPos));
                }
                else
                {
                    _states.Add(new State(stateName, false, endState));
                }
            }

            foreach (XmlNode stateNode in doc.DocumentElement.ChildNodes)
            {
                stateName = stateNode.Attributes["name"].Value;
                var state = GetState(stateName);
                foreach (XmlNode transitionNode in stateNode.ChildNodes)
                {
                    stateName = transitionNode.Attributes["name"].Value;
                    string withName;
                    if (transitionNode.Attributes["transitionname"] != null)
                    {
                        withName = transitionNode.Attributes["transitionname"].Value;
                    }
                    else
                    {
                        withName = null;
                    }

                    string rootToPos;
                    if (transitionNode.Attributes["topos"] != null)
                    {
                        rootToPos = transitionNode.Attributes["topos"].Value;
                    }
                    else
                    {
                        rootToPos = null;
                    }

                    var toState = GetState(stateName);
                    if (toState != null)
                    {
                        foreach (XmlNode withNode in transitionNode.ChildNodes)
                        {
                            string toPos;
                            if (withNode.Attributes["name"] != null)
                            {
                                withName = withNode.Attributes["name"].Value;
                                if (withNode.Attributes["topos"] != null)
                                {
                                    toPos = withNode.Attributes["topos"].Value;
                                }
                                else
                                {
                                    toPos = null;
                                }
                            }
                            else
                            {
                                toPos = null;
                            }

                            if (toPos == null)
                            {
                                if (rootToPos == null)
                                {
                                    AddTransition(state, toState, withNode.InnerText, withName);
                                }
                                else
                                {
                                    AddTransition(state, toState, withNode.InnerText, withName, rootToPos);
                                }
                            }
                            else
                            {
                                AddTransition(state, toState, withNode.InnerText, withName, toPos);
                            }
                        }
                    }
                }
            }
        }

        /**
         * <summary>The isValidTransition loops through states ArrayList and checks transitions between states. If the actual transition
         * equals to the given transition input, method returns true otherwise returns false.</summary>
         *
         * <param name="transition">is used to compare with the actual transition of a state.</param>
         * <returns>true when the actual transition equals to the transition input, false otherwise.</returns>
         */
        public bool IsValidTransition(string transition)
        {
            foreach (var state in _transitions.Keys)
            {
                foreach (var transition1 in _transitions[state])
                {
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
            foreach (var state in _states)
            {
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
         * <param name="fromState"> State type input indicating the from state.</param>
         * <param name="toState"> State type input indicating the next state.</param>
         * <param name="with">    string input indicating with what the transition will be made.</param>
         * <param name="withName">string input.</param>
         */
        public void AddTransition(State fromState, State toState, string with, string withName)
        {
            List<Transition> transitionList;
            var newTransition = new Transition(toState, with, withName);
            if (_transitions.ContainsKey(fromState))
            {
                transitionList = _transitions[fromState];
            }
            else
            {
                transitionList = new List<Transition>();
            }

            transitionList.Add(newTransition);
            _transitions[fromState] = transitionList;
        }

        /**
         * <summary>Another addTransition method which takes additional argument; toPos and. It creates a new {@link Transition}
         * with given input parameters and adds the transition to transitions {@link ArrayList}.</summary>
         *
         * <param name="fromState"> State type input indicating the from state.</param>
         * <param name="toState"> State type input indicating the next state.</param>
         * <param name="with">    string input indicating with what the transition will be made.</param>
         * <param name="withName">string input.</param>
         * <param name="toPos">   string input.</param>
         */
        public void AddTransition(State fromState, State toState, string with, string withName, string toPos)
        {
            List<Transition> transitionList;
            var newTransition = new Transition(toState, with, withName, toPos);
            if (_transitions.ContainsKey(fromState))
            {
                transitionList = _transitions[fromState];
            }
            else
            {
                transitionList = new List<Transition>();
            }

            transitionList.Add(newTransition);
            _transitions[fromState] = transitionList;
        }

        /**
         * <summary>The getTransitions method returns the transitions at the given state.</summary>
         *
         * <param name="state">State input.</param>
         * <returns>transitions at given state.</returns>
         */
        public List<Transition> GetTransitions(State state)
        {
            if (_transitions.ContainsKey(state))
            {
                return _transitions[state];
            }

            return new List<Transition>();
        }
    }
}