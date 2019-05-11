using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCTRL : MonoBehaviour
{
    [Header("Assign In Inspector")]
    public EntitySTATS _ENT_STATS;
    public Rigidbody _RIG;
    public LayerMask GroundCheckMask;
    public bool isGrounded;

    void Update(){
        Movement();
        CheckInput();
        CheckGround();
    }
    void CheckGround(){
        RaycastHit hit;
        Vector3 dir = -transform.up;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.9f, transform.position.z);
        if(Physics.Raycast(origin, dir, out hit, 1f, GroundCheckMask)){
            if(hit.collider.gameObject.tag == "Ground"){
                isGrounded = true;
            }else{
                isGrounded = false;
            }
        }else{
            isGrounded = false;
        }
    }
    void CheckInput(){
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            _RIG.AddForce(new Vector3(0, _ENT_STATS.jumpStrength, 0), ForceMode.Impulse);
        }
    }
    void Movement(){
        float movX = Input.GetAxis("Horizontal") * _ENT_STATS.curSpeed;
        float velAbs = _RIG.velocity.x;
        velAbs = Mathf.Abs(velAbs);
        if(velAbs <= 10){
            _RIG.AddForce(new Vector3(movX, 0, 0));
        }
        
        if(Input.GetAxis("Horizontal") > 0.1){
            transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
        }else if(Input.GetAxis("Horizontal") < -0.1){
            transform.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);
        }
    }
}
