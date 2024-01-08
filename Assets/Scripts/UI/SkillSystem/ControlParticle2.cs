using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlParticle2 : MonoBehaviour
{
    public SkillManager skillManager;
    public SkillButton[] skillBottons;
    void Update()
    {
        if (skillManager.activeSkill != null)
        {
            Vector3 skill = (skillBottons[skillManager.activeSkill.skillID - 1].transform.position);
            transform.position = skill;
        }
    }
}
