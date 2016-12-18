using UnityEngine;

public struct HitResult {

    public GameObject _hitTarget;
    public float _resultHP;

    public HitResult(GameObject hitTarget, float resultHP)
    {
        _hitTarget = hitTarget;
        _resultHP = resultHP;
    }
}
