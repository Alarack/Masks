using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LL.FSM {

    [CreateAssetMenu(menuName ="LL/FSM/State")]
    public class FSMState : ScriptableObject {

        public enum StateActionType {
            Enter,
            Exit,
            Update,
            Fixed
        }

        public List<StateAction> onEnterActions = new List<StateAction>();
        public List<StateAction> onExitActions = new List<StateAction>();
        public List<StateAction> onUpdateActions = new List<StateAction>();
        public List<StateAction> onFixedUpdateActions = new List<StateAction>();

        protected List<BaseStateAction> _OnEnterActions = new List<BaseStateAction>();
        protected List<BaseStateAction> _OnExitActions = new List<BaseStateAction>();
        protected List<BaseStateAction> _OnUpdateActions = new List<BaseStateAction>();
        protected List<BaseStateAction> _OnFixedUpdateActions = new List<BaseStateAction>();

        public virtual void Initialize(Entity owner)
        {
            InitActions(owner, onEnterActions, StateActionType.Enter);
            InitActions(owner, onExitActions, StateActionType.Exit);
            InitActions(owner, onUpdateActions, StateActionType.Update);
            InitActions(owner, onFixedUpdateActions, StateActionType.Fixed);
        }

        public virtual void ManagedUpdate()
        {
            RunSeperateActionUpdate(_OnEnterActions);
            RunSeperateActionUpdate(_OnExitActions);
            RunSeperateActionUpdate(_OnFixedUpdateActions);
            RunSeperateActionUpdate(_OnUpdateActions);
        }

        public virtual void OnEnter()
        {
            RegisterAllEvents();
            ExecutActions(_OnEnterActions);
        }
        public virtual void OnExit()
        {
            UnregisterAllEvents();
            ExecutActions(_OnExitActions);
        }
        public virtual void OnUpdate()
        {
            ExecutActions(_OnUpdateActions);
            ManagedUpdate();
        }
        public virtual void OnFixedUpdate()
        {
            ExecutActions(_OnFixedUpdateActions);
        }

        protected void ExecutActions(List<BaseStateAction> actions)
        {
            int count = actions.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if(actions[i].owner == null)
                {
                    actions.RemoveAt(i);
                    continue;
                }
                actions[i].Execute();
            }
        }

        private void RegisterAllEvents()
        {
            RegisterEventListeners(_OnEnterActions);
            RegisterEventListeners(_OnExitActions);
            RegisterEventListeners(_OnUpdateActions);
            RegisterEventListeners(_OnFixedUpdateActions);
        }

        public void UnregisterAllEvents()
        {
            UnregisterEventListeners(_OnEnterActions);
            UnregisterEventListeners(_OnExitActions);
            UnregisterEventListeners(_OnUpdateActions);
            UnregisterEventListeners(_OnFixedUpdateActions);
        }

        private void RegisterEventListeners(List<BaseStateAction> actions)
        {
            int count = actions.Count;
            for (int i = 0; i < count; i++)
            {
                actions[i].RegisterEvents();
            }
        }

        private void UnregisterEventListeners(List<BaseStateAction> actions)
        {
            int count = actions.Count;
            for (int i = 0; i < count; i++)
            {
                actions[i].UnregisterEvents();
            }
        }

        private void InitActions(Entity owner, List<StateAction> actions, StateActionType type)
        {
            int count = actions.Count;
            for (int i = 0; i < count; i++)
            {
                BaseStateAction newAction = actions[i].CreateActionClass(owner);
                PopualteActions(newAction, type);
            }
        }

        private void PopualteActions(BaseStateAction action, StateActionType type)
        {
            switch (type)
            {
                case StateActionType.Enter:
                    _OnEnterActions.Add(action);
                    break;
                case StateActionType.Exit:
                    _OnExitActions.Add(action);
                    break;
                case StateActionType.Update:
                    _OnUpdateActions.Add(action);
                    break;
                case StateActionType.Fixed:
                    _OnFixedUpdateActions.Add(action);
                    break;

            }
        }

        private void RunSeperateActionUpdate(List<BaseStateAction> actions)
        {
            int count = actions.Count;
            for (int i = 0; i < count; i++)
            {
                if (actions[i].RunUpdate)
                {
                    actions[i].ManagedUpdate();
                }
            }
        }

        //private List<Ability> GetAbilities(List<BaseStateAction> actions)
        //{
        //    List<Ability> results = new List<Ability>();

        //    int count = actions.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        results.AddRange(actions[i].abilities);
        //    }


        //    return results;
        //}

    }

}
