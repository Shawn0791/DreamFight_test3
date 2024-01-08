using UnityEngine;

[CreateAssetMenu(fileName ="New Skill",menuName ="Skill/New Skill")]
public class SkillData : ScriptableObject
{
    public int skillID;
    public Sprite skillSprite;
    public Sprite weaponImage;
    public string skillName;
    [TextArea(1,2)]
    public string mpCons;
    public float creatMp;
    public float reloadMp;
    public float magazineNum;

    public float skillExp;
    public float unlockExp;
    [TextArea(1, 4)]
    public string headTaking;
    [TextArea(1, 4)]
    public string skillAttribute;

    public bool isUnlocked;
    public SkillData[] preSkills;

    [Header("Debug")]
    public bool canUnlocked;
    public bool isNew;
    public bool canEquip;
}