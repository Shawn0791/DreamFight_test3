using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillInventory",menuName = "Skill/New SkillInventory")]
public class SkillInventory : ScriptableObject
{
    public List<SkillData> skillList = new List<SkillData>();
}
