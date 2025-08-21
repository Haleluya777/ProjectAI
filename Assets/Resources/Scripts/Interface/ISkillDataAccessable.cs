using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillDataAccessable
{
    List<Skill_Module.SkillData> AccessSkillData { get; set; }
}
