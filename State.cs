namespace MorphologicalAnalysis
{
    public class State
    {
        private bool _startState;
        private readonly bool _endState;
        private readonly string _name;
        private readonly string _pos;


        /**
         * <summary>First constructor of the {@link State} class which takes 3 parameters String name, boolean startState,
         * and boolean endState as input and initializes the private variables of the class also leaves pos as null.</summary>
         *
         * <param name="name">      String input.</param>
         * <param name="startState">boolean input.</param>
         * <param name="endState">  boolean input.</param>
         */
        public State(string name, bool startState, bool endState)
        {
            this._name = name;
            this._startState = startState;
            this._endState = endState;
            this._pos = null;
        }

        /**
         * <summary>Second constructor of the {@link State} class which takes 4 parameters as input; String name, boolean startState,
         * boolean endState, and String pos and initializes the private variables of the class.</summary>
         *
         * <param name="name">      String input.</param>
         * <param name="startState">boolean input.</param>
         * <param name="endState">  boolean input.</param>
         * <param name="pos">       String input.</param>
         */
        public State(string name, bool startState, bool endState, string pos)
        {
            this._name = name;
            this._startState = startState;
            this._endState = endState;
            this._pos = pos;
        }

        /**
         * <summary>Getter for the name.</summary>
         *
         * <returns>String name.</returns>
         */
        public string GetName()
        {
            return _name;
        }

        /**
         * <summary>Getter for the pos.</summary>
         *
         * <returns>String pos.</returns>
         */
        public string GetPos()
        {
            return _pos;
        }

        /**
         * <summary>The isEndState method returns endState's value.</summary>
         *
         * <returns>boolean endState.</returns>
         */
        public bool IsEndState()
        {
            return _endState;
        }

        /**
         * <summary>Overridden toString method which  returns the name.</summary>
         *
         * <returns>String name.</returns>
         */
        public override string ToString()
        {
            return _name;
        }
    }
}