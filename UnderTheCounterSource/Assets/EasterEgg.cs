using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    private static readonly int isBroken = Animator.StringToHash("isBroken");

    public void ToggleAnimation()
    {
        GetComponent<Animator>().SetBool(isBroken, !GetComponent<Animator>().GetBool(isBroken));
    }
}
