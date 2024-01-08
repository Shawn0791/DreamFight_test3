using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InterFaces
{
    void GetHit(float damage);
    void GetHitBack(float damage, Vector3 dir, float force);
}
