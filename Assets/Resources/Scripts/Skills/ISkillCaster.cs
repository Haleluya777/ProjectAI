using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillCaster
{
    int TotalDmg { get; set; }
    string GetTag();

    // 스킬 사용자의 현재 위치
    void SetScale(int dir);
    Vector3 GetPosition();

    // 스킬 사용자의 현재 회전 (예: 투사체 방향 결정에 사용)
    Quaternion GetRotation();

    // 공격력 (스킬 데미지 계산에 사용)
    int GetAttackPower();

    // IDamageable 인터페이스 (예: 상태 이상 적용 시 대상이 필요한 경우)
    IDamageable GetDamageableComponent();

    // GameObject 자체를 넘겨야 하는 경우 (Instantiate 시 부모 설정 등)
    GameObject GetGameObject();

    //원하는 컴포넌트를 받아옴.
    T GetCom<T>();
    BoxCollider2D GetHitBox();
}
