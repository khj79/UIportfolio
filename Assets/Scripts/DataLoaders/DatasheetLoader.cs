using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class DatasheetLoader
{
    private static readonly string folderPath = Path.Combine(Application.streamingAssetsPath, "Datasheets");

    /// <summary>
    /// 특정 데이터 클래스를 기반으로 CSV를 로드하여 객체 리스트로 변환
    /// </summary>
    public static List<T> LoadData<T>() where T : new()
    {
        string fileName = $"{typeof(T).Name}.csv"; // ✅ 클래스명과 동일한 CSV 파일 찾기
        string path = Path.Combine(folderPath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogError($"CSV 파일을 찾을 수 없습니다: {path}");
            return new List<T>();
        }

        List<Dictionary<string, string>> csvData = CsvLoader.LoadCsv(fileName);
        List<T> dataList = new List<T>();

        foreach (var row in csvData)
        {
            T instance = new T();
            foreach (FieldInfo field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (row.ContainsKey(field.Name))
                {
                    object value = ConvertStringToType(row[field.Name], field.FieldType);
                    field.SetValue(instance, value);
                }
            }
            dataList.Add(instance);
        }

        return dataList;
    }

    /// <summary>
    /// 문자열 값을 적절한 타입으로 변환
    /// </summary>
    private static object ConvertStringToType(string value, Type type)
    {
        if (string.IsNullOrEmpty(value)) return GetDefaultValue(type);

        if (type == typeof(Int32))
        {
            if (Int32.TryParse(value, out Int32 intValue))
                return intValue;
            return 0;
        }

        if (type == typeof(string))
        {
            return value;
        }

        if (type.IsEnum)
        {
            return Enum.Parse(type, value);
        }

        if (type == typeof(List<Int32>))
        {
            return CsvLoader.ParseIntList(value);
        }

        Debug.LogWarning($"⚠️ 변환할 수 없는 타입: {type.Name} (값: {value})");
        return GetDefaultValue(type);
    }

    /// <summary>
    /// 기본값 반환
    /// </summary>
    private static object GetDefaultValue(Type type)
    {
        if (type.IsValueType) return Activator.CreateInstance(type);
        return null;
    }
}