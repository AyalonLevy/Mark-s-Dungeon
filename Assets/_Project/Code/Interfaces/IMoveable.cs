using UnityEngine;

public interface IMoveable
{
    Rigidbody RB { get; set; }
    bool IsFacingRight { get; set; }

    void Move(Vector3 velocity);
    void CheckForLeftOrRight(Vector3 velocity);
}
