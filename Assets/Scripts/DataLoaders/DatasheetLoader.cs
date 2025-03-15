using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public static class DatasheetLoader
{
    private static readonly string folderPath = Path.Combine(Application.streamingAssetsPath, "Datasheets");

    /// <summary>
    /// 특정 데이터 클래스를 기반으로 CSV를 로드하여 객체 리스트로 변환
    /// </summary>
    public static List<T> LoadData<T>() where T : new()
    {
        Type type = typeof(T);

        // ✅ 빈 클래스는 CSV를 가질 수 없으므로 예외 처리
        if (IsEmptyClass(type))
        {
            Debug.Log($"⚠️ {type.Name} 클래스는 필드와 프로퍼티가 없으므로 로드하지 않습니다.");
            return new List<T>();
        }

        string fileName = $"{type.Name}.csv"; // ✅ 클래스명과 동일한 CSV 파일 찾기
        string path = Path.Combine(folderPath, fileName);

        if (!File.Exists(path))
        {
            string errorMessage = $"❌ [Critical] 필수 게임 데이터 {fileName}이(가) 없습니다! 게임 실행 불가!";
            Debug.LogError(errorMessage);
            throw new FileNotFoundException(errorMessage);
        }

        List<Dictionary<string, string>> csvData = CsvLoader.LoadCsv(fileName);
        if (csvData.Count == 0)
        {
            Debug.LogWarning($"[Warning] {fileName} 파일이 비어 있습니다.");
        }

        List<T> dataList = new List<T>();

        foreach (var row in csvData)
        {
            try
            {
                T instance = new T();
                foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (row.ContainsKey(field.Name))
                    {
                        object value = ConvertStringToType(row[field.Name], field.FieldType);
                        field.SetValue(instance, value);
                    }
                }
                dataList.Add(instance);
            }
            catch (Exception ex)
            {
                string errorMessage = $"❌ [Critical] {type.Name} 데이터를 파싱하는 중 오류 발생! (Row: {string.Join(", ", row.Values)})\n{ex.Message}";
                Debug.LogError(errorMessage);
                throw new Exception(errorMessage);
            }
        }

        return dataList;
    }

    /// <summary>
    /// 특정 클래스가 빈 클래스인지 확인 (필드와 프로퍼티가 없는 경우, 부모 필드는 무시)
    /// </summary>
    private static bool IsEmptyClass(Type type)
    {
        bool hasOwnFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length > 0;
        bool hasOwnProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Length > 0;

        return !hasOwnFields && !hasOwnProperties;
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

        if (type == typeof(Int64))
        {
            if (Int64.TryParse(value, out Int64 intValue))
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

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            Type elementType = type.GetGenericArguments()[0];

            if (elementType == typeof(Int32))
            {
                return CsvLoader.ParseIntList(value);
            }

            if (elementType.IsEnum)
            {
                MethodInfo parseMethod = typeof(CsvLoader).GetMethod("ParseStringList")
                    .MakeGenericMethod(elementType);
                return parseMethod.Invoke(null, new object[] { value });
            }
        }

        Debug.LogError($"변환할 수 없는 타입입니다. 프로퍼티가 필드로 세팅된 것 같습니다. : {type.Name} (값: {value})");
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
