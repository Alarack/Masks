using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LL.FSM {

    public class FSM {

        public FSMState CurrentState { get; private set; }
        private FSMState previousState;

        public Entity Owner { get; protected set; }


        public FSM(Entity owner)
        {
            this.Owner = owner;
        }


        public void ChangeState(FSMState newState)
        {
            if (Owner == null)
                return;

            if (CurrentState == newState)
                return;

            //Debug.Log("changing the sate on " + Owner.gameObject.name + " to " + newState.name);

            if(CurrentState != null)
                CurrentState.OnExit();

            previousState = CurrentState;
            CurrentState = newState;

            CurrentState.OnEnter();
        }

        public void ManagedUpdate()
        {
            if (CurrentState != null && Owner != null)
                CurrentState.OnUpdate();
        }

        public void ManagedFixedUpdate()
        {
            if (CurrentState != null && Owner != null)
                CurrentState.OnFixedUpdate();
        }

        public void SwapToPreviousState()
        {
            if (previousState == CurrentState)
                return;
            

            if(previousState != null && CurrentState != null)
            {
                //Debug.Log("Swapping to Previous: " + previousState.name);

                CurrentState.OnExit();
                CurrentState = previousState;
                CurrentState.OnEnter();
            }
        }


    }

}
