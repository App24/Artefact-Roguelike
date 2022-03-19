using System.Collections.Generic;

namespace Artefact.States
{
    internal static class StateMachine
    {
        private static Stack<State> states = new Stack<State>();
        private static State newState;
        private static bool isAdd, isRemove, isReplace;

        public static State ActiveState { get { if (IsEmpty) return null; return states.Peek(); } }
        public static bool IsEmpty { get { return states.Count <= 0; } }

        /// <summary>
        /// Add a state to the queue to be added in the next game loop
        /// </summary>
        /// <param name="newState">New state to be added</param>
        /// <param name="replace">Replace the current state</param>
        public static void AddState(State newState, bool replace = true)
        {
            StateMachine.newState = newState;
            isAdd = true;
            isReplace = replace;
        }

        /// <summary>
        /// Remove the current state
        /// </summary>
        public static void RemoveState()
        {
            isRemove = true;
        }

        /// <summary>
        /// Process state changes, such as adding, removing and replacing
        /// </summary>
        public static void ProcessStateChanges()
        {
            if (isRemove && !IsEmpty)
            {
                ActiveState.Remove();

                states.Pop();

                if (!IsEmpty)
                {
                    ActiveState.Resume();
                }

                isRemove = false;
            }

            if (isAdd)
            {
                if (!IsEmpty)
                {
                    if (isReplace)
                    {
                        ActiveState.Remove();
                        states.Pop();
                    }
                    else
                    {
                        ActiveState.Pause();
                    }
                }

                states.Push(newState);
                ActiveState.Init();
                isAdd = false;
            }
        }

        /// <summary>
        /// Remove all states
        /// </summary>
        public static void CleanStates()
        {
            for (int i = 0; i < states.Count; i++)
            {
                RemoveState();
                ProcessStateChanges();
            }
        }
    }
}
