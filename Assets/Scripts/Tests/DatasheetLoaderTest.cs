using System;
using System.Collections.Generic;
using UnityEngine;

public class DatasheetLoaderTest : MonoBehaviour
{
    private void Start()
    {
        TestLoadItemData<ItemEquipmentData>();
        TestLoadItemData<ItemMaterialData>();
        TestLoadEquipmentData();
        TestLoadMaterialData();
    }

    /// <summary>
    /// ItemData<T>.csv가 정상적으로 로드되는지 테스트 (Equipment, Material 구분)
    /// </summary>
    private void TestLoadItemData<T>() where T : class
    {
        Debug.Log($"===== ItemData<{typeof(T).Name}>.csv 로드 테스트 시작 =====");

        List<ItemData> items = DatasheetLoader.LoadData<ItemData>();

        if (items.Count > 0)
        {
            Debug.Log($"✅ ItemData<{typeof(T).Name}>.csv 로드 성공! 총 {items.Count}개의 아이템 로드됨.");
            for (int i = 0; i < items.Count; ++i)
            {
                Debug.Log($"Id={items[i].Id}, Name={items[i].ItemName}, Category={items[i].Category}");
            }
        }
        else
        {
            Debug.LogError($"❌ ItemData<{typeof(T).Name}>.csv 로드 실패! 파일이 없거나 비어 있음.");
        }

        Debug.Log($"===== ItemData<{typeof(T).Name}>.csv 로드 테스트 완료 =====");
    }

    /// <summary>
    /// ItemEquipmentData.csv가 정상적으로 로드되는지 테스트
    /// </summary>
    private void TestLoadEquipmentData()
    {
        Debug.Log("===== ItemEquipmentData.csv 로드 테스트 시작 =====");

        List<ItemEquipmentData> equipmentItems = DatasheetLoader.LoadData<ItemEquipmentData>();

        if (equipmentItems.Count > 0)
        {
            Debug.Log($"✅ ItemEquipmentData.csv 로드 성공! 총 {equipmentItems.Count}개의 장비 아이템 로드됨.");
            for (int i = 0; i < equipmentItems.Count; ++i)
            {
                Debug.Log($"예시 데이터: Id={equipmentItems[i].Id}, Level={equipmentItems[i].Level}, SealedOptionDataId={equipmentItems[i].SealedOptionDataId}");
            }
        }
        else
        {
            Debug.LogError("❌ ItemEquipmentData.csv 로드 실패! 파일이 없거나 비어 있음.");
        }

        Debug.Log("===== ItemEquipmentData.csv 로드 테스트 완료 =====");
    }

    /// <summary>
    /// ItemMaterialData.csv가 정상적으로 로드되는지 테스트
    /// </summary>
    private void TestLoadMaterialData()
    {
        Debug.Log("===== ItemMaterialData.csv 로드 테스트 시작 =====");

        List<ItemMaterialData> materialItems = DatasheetLoader.LoadData<ItemMaterialData>();

        if (materialItems.Count > 0)
        {
            Debug.Log($"✅ ItemMaterialData.csv 로드 성공! 총 {materialItems.Count}개의 재료 아이템 로드됨.");
            for (int i = 0; i < materialItems.Count; ++i)
            {
                Debug.Log($"예시 데이터: Id={materialItems[i].Id}, MaxStackCount={materialItems[0].MaxStackCount}");
            }
        }
        else
        {
            Debug.LogError("❌ ItemMaterialData.csv 로드 실패! 파일이 없거나 비어 있음.");
        }

        Debug.Log("===== ItemMaterialData.csv 로드 테스트 완료 =====");
    }
}
