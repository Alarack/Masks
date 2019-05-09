using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInput 
{
    public enum GameButtonType
    {
        None,
        PrimaryAttack,
        SecondaryAttack,
        SpecialAttack,
        Jump,
        Dash,

    }



    #region DIRECTIONS

    public static bool Left
    {
        get { return Input.GetAxis("Horizontal") == -1f; }
    }

    public static bool Right
    {
        get { return Input.GetAxis("Horizontal") == 1f; }
    }

    public static bool Up
    {
        get { return Input.GetAxis("Vertical") == 1f; }
    }

    public static bool Down
    {
        get { return Input.GetAxis("Vertical") == -1f; }
    }

    public static float Horizontal
    {
        get { return Input.GetAxis("Horizontal"); }
    }

    public static float Vertical
    {
        get { return Input.GetAxis("Vertical"); }
    }

    #endregion


    #region BUTTONS

    public static bool Jump
    {
        get { return Input.GetButtonDown("Jump"); }
    }

    public static bool JumpHeld
    {
        get { return Input.GetButton("Jump"); }
    }

    public static bool Dash
    {
        get { return Input.GetButtonDown("Dash"); }
    }

    public static bool Fire1
    {
        get { return Input.GetButtonDown("Fire1"); }
    }

    public static bool Fire2
    {
        get { return Input.GetButtonDown("Fire2"); }
    }

    #endregion


}
