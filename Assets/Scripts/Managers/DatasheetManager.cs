using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DatasheetManager
{
    public static DatasheetManager Instance { get; private set; } = new DatasheetManager();

    private Dictionary<Type, Dictionary<int, GameData>> dataDictionaries;

    public void Init()
    {
        dataDictionaries = new Dictionary<Type, Dictionary<int, GameData>>();
        LoadAllDatasheets();
    }

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
        }

        InitializeAllReferences();
    }

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
            }
        }
    }

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

            var convertedDictionary = dictionary
                .Where(kvp => kvp.Value is T)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value as T);

            return convertedDictionary;
        }

        Debug.LogError($"❌ GetDataTable<{typeof(T).Name}> - {typeof(T).Name}이(가) dataDictionaries에 없음!");
        return null;
    }
}