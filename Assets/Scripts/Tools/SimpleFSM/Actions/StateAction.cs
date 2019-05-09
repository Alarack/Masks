using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LL.FSM {

    [CreateAssetMenu(menuName = "LL/FSM/Actions/Action Template")]
    public class StateAction : ScriptableObject {

        public enum ActionType {
            PlayerMove,
            PlayerJump,
            PlayerDash,
            MobWander,
            MobTurnAtLedge,
            MobTurnAtWall,
            MobFaceTarget,
            MobMoveNormal,
            AbilityHolder
        }
       

        public bool runUpdate;
        public ActionType actionType;

        [Header("Abilities")]
        public List<AbilityData> abilities = new List<AbilityData>();

        public virtual BaseStateAction CreateActionClass(Entity owner)
        {
            BaseStateAction action = null;

            switch (actionType)
            {
                case ActionType.PlayerMove:
                    action = new PlayerMoveAction(owner, runUpdate);
                    break;
                case ActionType.PlayerJump:
                    action = new PlayerJumpAction(owner, runUpdate);
                    break;
                case ActionType.PlayerDash:
                    action = new PlayerDashAction(owner, runUpdate);
                    break;
                case ActionType.MobWander:
                    action = new WanderAction(owner, runUpdate);
                    break;
                case ActionType.MobTurnAtLedge:
                    action = new TurnAtLedgeAction(owner, runUpdate);
                    break;
                case ActionType.MobTurnAtWall:
                    action = new TurnAtWallAction(owner, runUpdate);
                    break;
                case ActionType.MobFaceTarget:
                    action = new ChaseTargetAction(owner, runUpdate);
                    break;
                case ActionType.MobMoveNormal:
                    action = new MoveAction(owner, runUpdate);
                    break;

                default:
                    action = new BaseStateAction(owner, runUpdate);
                    break;

            }

            action.PopulateAbilities(abilities);

            return action;
        }





    }

}
