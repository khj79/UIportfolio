using System;
using UnityEngine;
using Game.Enums;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [Header("Controller")]
    [SerializeField] private SealedOptionViewController sealedOptionViewController;

    [Header("Test")]
    [SerializeField] private InputTester tester;
    [SerializeField] private CharacterClass currentClass = CharacterClass.Asura;
    public CharacterClass CurrentClass => currentClass;

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
        
        sealedOptionViewController.Init();
    }

    private void OnApplicationQuit()
    {
        InventoryManager.Instance?.SaveInventory();
    }
}