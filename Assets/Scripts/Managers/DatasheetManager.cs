using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DatasheetManager : MonoBehaviour
{
    public static DatasheetManager Instance { get; private set; }

    public Dictionary<Type, Dictionary<int, GameData>> dataDictionaries;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 모든 게임 데이터를 로드하고 자동으로 매핑
    /// </summary>
    private void Init()
    {
        Debug.Log("===== 게임 데이터 로딩 시작 =====");

        dataDictionaries = new Dictionary<Type, Dictionary<int, GameData>>();

        // 1️⃣ Reflection을 사용하여 모든 게임 데이터 클래스 로드
        LoadAllDatasheets();

        Debug.Log("===== 게임 데이터 로딩 완료 =====");
    }

    /// <summary>
    /// GameData를 상속받은 모든 클래스 자동 검색 및 로드
    /// </summary>
    private void LoadAllDatasheets()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        IEnumerable<Type> dataTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(GameData)));

        foreach (Type dataType in dataTypes)
        {
            MethodInfo method = typeof(DatasheetLoader).GetMethod("LoadData").MakeGenericMethod(dataType);
            object result = method.Invoke(null, null);

            Dictionary<int, GameData> dataDictionary = new Dictionary<int, GameData>();

            foreach (GameData data in (IEnumerable<GameData>)result)
            {
                dataDictionary[data.Id] = data;
            }

            dataDictionaries[dataType] = dataDictionary;
            Debug.Log($"✅ {dataType.Name} 로드 완료: {dataDictionary.Count}개");
        }

        // ✅ 모든 데이터 로드 후 InitializeReferences() 실행
        InitializeAllReferences();
    }

    /// <summary>
    /// 모든 GameData의 InitializeReferences()를 실행
    /// </summary>
    private void InitializeAllReferences()
    {
        foreach (var dataDictionary in dataDictionaries)
        {
            Type dataType = dataDictionary.Key;
            MethodInfo initMethod = dataType.GetMethod("InitializeReferences", BindingFlags.Public | BindingFlags.Instance);

            if (initMethod != null)
            {
                foreach (GameData instance in dataDictionary.Value.Values)
                {
                    initMethod.Invoke(instance, null);
                }
                Debug.Log($"🔄 {dataType.Name}.InitializeReferences() 실행 완료");
            }
        }
    }


    /// <summary>
    /// 특정 ID의 데이터 반환
    /// </summary>
    public T GetData<T>(int id) where T : GameData
    {
        if (dataDictionaries.TryGetValue(typeof(T), out var dictionary))
        {
            if (dictionary.TryGetValue(id, out var data))
            {
                return data as T;
            }
        }
        return null;
    }

    public Dictionary<int, T> GetDataTable<T>() where T : GameData
    {
        if (dataDictionaries.TryGetValue(typeof(T), out var dictionary))
        {
            if (dictionary is Dictionary<int, T> typedDictionary)
            {
                return typedDictionary;
            }

            // ✅ Dictionary<int, GameData> → Dictionary<int, T>로 변환
            var convertedDictionary = dictionary
                .Where(kvp => kvp.Value is T)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value as T);
            
            return convertedDictionary;
        }

        Debug.LogError($"❌ GetDataTable<{typeof(T).Name}> - {typeof(T).Name}이(가) dataDictionaries에 없음!");
        return null;
    }
}