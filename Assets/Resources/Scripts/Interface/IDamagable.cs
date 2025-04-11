using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    //인터페이스. 데미지 받는거 관련한거.
    //Dead메서드를 주석처리한 이유는 귀찮아서 나중에 하려고.
    //인터페이스 내부에서 선언한 모든 프로퍼티, 메서드는 상속받은 클래스 내부에서 다시 정의되어야 함.
    //그거 안하면 빨간줄 생김.
    
    public bool CanAction { get; set; }

    public void Damaged(int dmg, string attackType);
    public void StatusEffectProcess(float duration, string effectName);
    //public void Dead();
}
