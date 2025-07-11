using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;


[System.Serializable]
public class EnemyStatusInfo
{
    [SerializeField] private int id;
    [SerializeField] private Enemy_Base_Status baseStatus;
    [SerializeField] private Enemy_Combat_Status combatStatus;
    [SerializeField] private Enemy_Movement_Status movementStatus;
    [SerializeField] private Enemy_Visualize visual;

    public int Id => id;
    public Enemy_Base_Status Base_Status => baseStatus;
    public Enemy_Combat_Status Combat_Status => combatStatus;
    public Enemy_Movement_Status Movement_Status => movementStatus;
    public Enemy_Visualize Visual => visual;
}

[System.Serializable]
public class Enemy_Visualize
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private AnimatorController anim;

    public Sprite Sprite => sprite;
    public AnimatorController Anim => anim;
}

[System.Serializable]
public class Enemy_Base_Status
{
    [SerializeField] private string name;
    [SerializeField] private int hp;
    [SerializeField] private int defense;
    [SerializeField] private int magicalDefense;

    public string Name => name;
    public int HP => hp;
    public int Defense => defense;
    public int MagicalDefense => magicalDefense;
}

[System.Serializable]
public class Enemy_Combat_Status
{
    [SerializeField] private int atk;

    [SerializeField] private float shortAttackRange;
    [SerializeField] private float longAttackRange;
    [SerializeField] private float mainAttCool;

    [SerializeField] private List<float> shortDisAttCool;
    [SerializeField] private List<float> longDisAttCool;

    public int Atk => atk;
    public float ShortAttackRange => shortAttackRange;
    public float LongAttackRange => longAttackRange;
    public float MainAttCool => mainAttCool;

    public List<float> ShortDisAttCool => shortDisAttCool;
    public List<float> LongDisAttCool => longDisAttCool;
}

[System.Serializable]
public class Enemy_Movement_Status
{
    [SerializeField] private int moveSpeed;
    [SerializeField] private int detectionRange;

    public int MoveSpeed => moveSpeed;
    public int DetectionRange => detectionRange;
}

[CreateAssetMenu(menuName = "EnemyStatus")]
public class EnemyStatusScriptableObject : ScriptableObject
{
    public EnemyStatusInfo[] status;

    public EnemyStatusInfo GetEnemyData(int id)
    {
        if (id < 0 || id > status.Length)
        {
            Debug.Log($"유효하지 않은 id값. ID : {id}");
            return null;
        }
        return status[id];
    }
}
