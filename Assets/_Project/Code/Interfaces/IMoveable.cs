using UnityEngine;

public interface IMoveable
{
    Rigidbody RB { get; set; }
    bool IsFacingRight { get; set; }

    void Move(Vector3 linearVelocity, Vector3 angularVelocity);
    void HandleFlip(Vector3 velocity);
}
