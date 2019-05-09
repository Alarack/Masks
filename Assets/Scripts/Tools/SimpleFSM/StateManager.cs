using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LL.FSM {

    public class StateManager : MonoBehaviour {

        public List<FSMState> states = new List<FSMState>();

        public Entity Owner { get; protected set; }

        public FSM stateMachine;

        public void Initialize(Entity owner, FSM stateMachine)
        {
            this.Owner = owner;
            this.stateMachine = stateMachine;
            InitStates();
        }

        protected virtual void Update()
        {
            if (stateMachine != null && Owner != null)
                stateMachine.ManagedUpdate();

            //UpdateStates();
        }

        protected virtual void FixedUpdate()
        {
            if (stateMachine != null && Owner != null)
                stateMachine.ManagedFixedUpdate();
        }

        private void InitStates()
        {
            int count = states.Count;
            for (int i = 0; i < count; i++)
            {
                states[i].Initialize(Owner);
            }
        }

        public FSMState GetState(string name)
        {
            int count = states.Count;
            for (int i = 0; i < count; i++)
            {
                if (states[i].name == name)
                    return states[i];
            }

            return null;
        }

        public void UnregisterEventListeners()
        {
            int count = states.Count;
            for (int i = 0; i < count; i++)
            {
                states[i].UnregisterAllEvents();
            }
        }




    }

}
