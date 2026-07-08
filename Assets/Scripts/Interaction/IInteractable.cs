using UnityEngine;

// Interface dasar buat semua objek yang bisa diinteraksi lewat raycast.
public interface IInteractable
{
    void Interact(GameObject interactor);
    string GetPrompt();
}
