using System;
using System.Collections.Generic;
using UnityEngine;

public class CsvLoaderTest : MonoBehaviour
{
    private void Start()
    {
        TestCsvLoading();
        TestParseIntList();
        TestParseInt();
    }

    /// <summary>
    /// CSV 파일을 정상적으로 로드하는지 테스트
    /// </summary>
    private void TestCsvLoading()
    {
        Debug.Log("===== CSV 파일 로드 테스트 시작 =====");

        List<Dictionary<string, string>> data = CsvLoader.LoadCsv("ItemData.csv");

        if (data.Count > 0)
        {
            Debug.Log($"✅ CSV 로드 성공! 총 {data.Count}개의 데이터가 로드됨.");
            Debug.Log($"예시 데이터: Id={data[0]["Id"]}, Name={data[0]["ItemName"]}");
        }
        else
        {
            Debug.LogError("❌ CSV 로드 실패! 파일이 없거나 비어있음.");
        }

        Debug.Log("===== CSV 파일 로드 테스트 완료 =====");
    }

    /// <summary>
    /// "[1,2,3,4]" 형식의 문자열이 List<Int32>로 변환되는지 테스트
    /// </summary>
    private void TestParseIntList()
    {
        Debug.Log("===== List<Int32> 변환 테스트 시작 =====");

        string testData = "[10,20,30,40]";
        List<Int32> parsedList = CsvLoader.ParseIntList(testData);

        if (parsedList.Count == 4 && parsedList[0] == 10 && parsedList[1] == 20)
        {
            Debug.Log($"✅ List<Int32> 변환 성공! 데이터: {string.Join(",", parsedList)}");
        }
        else
        {
            Debug.LogError("❌ List<Int32> 변환 실패!");
        }

        Debug.Log("===== List<Int32> 변환 테스트 완료 =====");
    }

    /// <summary>
    /// 문자열이 Int32로 변환되는지 테스트
    /// </summary>
    private void TestParseInt()
    {
        Debug.Log("===== Int32 변환 테스트 시작 =====");

        string testInt = "12345";
        Int32 result = CsvLoader.ParseInt(testInt);

        if (result == 12345)
        {
            Debug.Log($"✅ Int32 변환 성공! 값: {result}");
        }
        else
        {
            Debug.LogError("❌ Int32 변환 실패!");
        }

        Debug.Log("===== Int32 변환 테스트 완료 =====");
    }
}
