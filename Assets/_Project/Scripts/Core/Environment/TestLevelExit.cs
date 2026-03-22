using UnityEngine;

public class TestLevelExit : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => "Press E to Exit Dungeon";

    public void Interact(GameObject user)
    {
        Debug.Log("<color=green>SUCCESS: Mark and Churro escaped the floor!</color>");
    }
}
