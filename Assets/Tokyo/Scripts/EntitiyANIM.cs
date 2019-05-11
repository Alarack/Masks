using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiyANIM : MonoBehaviour
{
    [Header("Assign In Inspector")]
    public EntityCTRL _ENT_CTRL;
    public EntitySTATS _ENT_STATS;
    public Animator ANIM_CTRL;

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            Attack();
        }
        if(_ENT_CTRL.isGrounded){
            ANIM_CTRL.SetBool("jumping", false);
            GroundedMovement();
        }else{
            ANIM_CTRL.SetBool("moving", false);
            ANIM_CTRL.SetBool("sliding", false);
            ANIM_CTRL.SetBool("jumping", true);
            JumpVelocityAnimation();
        }

        float velAbs = _ENT_CTRL._RIG.velocity.x;
        velAbs = Mathf.Abs(velAbs);
        if(ANIM_CTRL.GetBool("moving") == false && velAbs > 2){
            ANIM_CTRL.SetBool("sliding", true);
        }else if(ANIM_CTRL.GetBool("moving") == false && velAbs <= 2){
            ANIM_CTRL.SetBool("sliding", false);
        }
    }
    void GroundedMovement(){
        float inputAbs = Input.GetAxis("Horizontal");
        inputAbs = Mathf.Abs(inputAbs);
        if(inputAbs >= 0.1f){
            ANIM_CTRL.SetBool("moving", true);
        }else if(inputAbs == 0f){
            ANIM_CTRL.SetBool("moving", false);
        }
    }
    void JumpVelocityAnimation(){
        float velY = _ENT_CTRL._RIG.velocity.y;
        ANIM_CTRL.SetFloat("VelocityY", velY);
    }
    void Attack(){
        if(_ENT_STATS.curAttackCombo <= _ENT_STATS.attackCombos){
            _ENT_STATS.curAttackCombo ++;
            if(_ENT_STATS.curAttackCombo > _ENT_STATS.attackCombos){
                _ENT_STATS.curAttackCombo = 0;
            }
        }
        if(_ENT_STATS.curAttackCombo == 1){
            ANIM_CTRL.SetTrigger("Attack1");
        }else if(_ENT_STATS.curAttackCombo == 2){
            ANIM_CTRL.SetTrigger("Attack2");
        }else if(_ENT_STATS.curAttackCombo == 3){
            ANIM_CTRL.SetTrigger("Attack3");
        }
    }
}
