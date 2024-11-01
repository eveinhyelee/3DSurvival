using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.instance.Player.itemData = data;
        CharacterManager.instance.Player.addItem?.Invoke();
        Destroy(gameObject);

    }
}
