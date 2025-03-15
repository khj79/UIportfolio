using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CsvLoader
{
    private static readonly string folderPath = Path.Combine(Application.streamingAssetsPath, "Datasheets"); // ✅ CSV 파일 저장 경로

    /// <summary>
    /// CSV 파일을 읽어와 Dictionary<string, string> 형태로 반환 (PC 전용)
    /// </summary>
    public static List<Dictionary<string, string>> LoadCsv(string fileName)
    {
        string path = Path.Combine(folderPath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogError($"CSV 파일을 찾을 수 없습니다: {path}");
            return new List<Dictionary<string, string>>();
        }

        string[] lines = File.ReadAllLines(path);
        return ParseCsv(lines);
    }

    /// <summary>
    /// CSV 파일을 파싱하여 Dictionary 리스트로 변환 (따옴표 처리 포함)
    /// </summary>
    private static List<Dictionary<string, string>> ParseCsv(string[] lines)
    {
        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();

        if (lines.Length < 2) return dataList; // 데이터가 없는 경우 반환

        string[] headers = ParseCsvLine(lines[0]);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = ParseCsvLine(lines[i]);
            Dictionary<string, string> entry = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length; j++)
            {
                if (j < values.Length)
                    entry[headers[j].Trim()] = values[j].Trim();
            }
            dataList.Add(entry);
        }

        return dataList;
    }

    /// <summary>
    /// CSV 한 줄을 안전하게 파싱하는 함수 (쉼표 및 따옴표 처리 포함)
    /// </summary>
    private static string[] ParseCsvLine(string line)
    {
        List<string> fields = new List<string>();
        bool inQuotes = false;
        string field = "";

        for (int i = 0; i < line.Length; i++)
        {
            char current = line[i];

            if (current == '"')
            {
                inQuotes = !inQuotes; // 따옴표 안/밖 변경
            }
            else if (current == ',' && !inQuotes)
            {
                fields.Add(field.Trim());
                field = "";
            }
            else
            {
                field += current;
            }
        }

        fields.Add(field.Trim()); // 마지막 필드 추가
        return fields.ToArray();
    }

    /// <summary>
    /// "[1,2,3]" 형식의 문자열을 List<Int32>로 변환
    /// </summary>
    public static List<Int32> ParseIntList(string data)
    {
        List<Int32> list = new List<Int32>();

        if (string.IsNullOrEmpty(data) || data == "[]")
            return list;

        data = data.Trim('[', ']'); // 대괄호 제거
        string[] elements = data.Split(',');

        foreach (var elem in elements)
        {
            if (Int32.TryParse(elem.Trim(), out Int32 value))
            {
                list.Add(value);
            }
        }
        return list;
    }

    /// <summary>
    /// "[EnumValue1,EnumValue2]" 형식의 문자열을 List<T>로 변환 (Enum & 일반 문자열 지원)
    /// </summary>
    public static List<T> ParseStringList<T>(string data) where T : struct
    {
        List<T> list = new List<T>();

        if (string.IsNullOrEmpty(data) || data == "[]")
            return list;

        data = data.Trim('[', ']'); // 대괄호 제거
        string[] elements = data.Split(',');

        foreach (var elem in elements)
        {
            string trimmedValue = elem.Trim();
            
            // Enum이면 변환, 아니면 문자열 그대로 추가
            if (typeof(T).IsEnum)
            {
                if (Enum.TryParse(trimmedValue, out T enumValue))
                {
                    list.Add(enumValue);
                }
                else
                {
                    Debug.LogWarning($"⚠️ Enum 변환 실패: {trimmedValue} (Type: {typeof(T).Name})");
                }
            }
            else
            {
                list.Add((T)Convert.ChangeType(trimmedValue, typeof(T)));
            }
        }
        return list;
    }

    /// <summary>
    /// 문자열을 Int32로 변환 (빈 값은 기본값 0 반환)
    /// </summary>
    public static Int32 ParseInt(string data)
    {
        if (Int32.TryParse(data, out Int32 result))
            return result;
        return 0;
    }
}
