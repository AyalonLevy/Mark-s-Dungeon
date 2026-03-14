using UnityEngine;

public class Stairs : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In the stairs");
        //TODO: Add more scenes
        //GameManager.Instance.LoadNextLevel();
    }
}
