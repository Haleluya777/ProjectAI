using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovablePlatForm
{
    Vector2 momentum { get; set; }
    Vector2 deltaPos { get; set; } 
}
