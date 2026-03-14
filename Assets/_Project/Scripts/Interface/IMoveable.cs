using UnityEngine;

public interface IMoveable
{
    float CurrentMoveSpeed { get; }
    void Move(Vector3 direction);
    void Rotate(Vector3 lookDirection);
    bool IsSprinting { get; set; }
}
