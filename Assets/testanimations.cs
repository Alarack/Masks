using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testanimations : MonoBehaviour
{
    public Animator anim;
    bool moving;
    void Update(){
        if(Input.GetKeyDown(KeyCode.F)){
            moving = !moving;
        }
        anim.SetBool("moving", moving);
    }
}
