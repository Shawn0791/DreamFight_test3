using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Black : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        GameManager.RegisterLBL(this);
    }

    public void SceneFadeOut()
    {
        anim.SetTrigger("Fade");
    }
}
