using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField]
    private InventoryInputTester tester;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        DatasheetManager.Instance.Init();
        InventoryManager.Instance.Init();
    }

    private void OnApplicationQuit()
    {
        InventoryManager.Instance?.SaveInventory();
    }
}