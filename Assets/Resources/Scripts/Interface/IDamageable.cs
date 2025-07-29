
public interface IDamageable
{
    int CurrentHp { get; }
    float Scale{ get; }
    bool CanAction { get; set; }

    void Damaged(int damage, string damageType);
    void StatusEffectProcess(float duration, string statuseffectName);
    void Dead();
    bool IsDead { get; }
}