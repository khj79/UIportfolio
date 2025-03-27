using UnityEngine;

public class InventoryInputTester : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            InventoryManager.Instance.AddRandomItem();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            InventoryManager.Instance.RemoveLastItem();

        if (Input.GetKeyDown(KeyCode.Alpha3))
            InventoryManager.Instance.Clear();
    }
}
