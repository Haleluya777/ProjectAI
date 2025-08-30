using System.Collections;
using UnityEngine;

public interface ITriggerable
{
    BlackBoard GetBlackBoard();
    void Trigger();
}
