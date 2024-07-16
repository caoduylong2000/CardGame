using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using System.Linq;
using Unity.Services.Authentication;

public class LeaderboardScene : MonoBehaviour
{
    [SerializeField] private GameObject recordObject;
    [SerializeField] private Transform transformParent;

    List<RecordData> recordList = new List<RecordData>();

    private int totalRecord;
    private int limitRecord;

    async void Start()
    {
        var data = await GameManager.Instance.Leaderboard.GetScores();

        //Debug.Log(data);

        ExtractData(data);
    }

    public void ExtractData(string jsonData)
    {
        // Chuyển đổi chuỗi JSON thành một đối tượng JObject
        JObject json = JObject.Parse(jsonData);

        totalRecord = json["total"].Value<int>();
        limitRecord = json["limit"].Value<int>();

        int recordCount;

        if (totalRecord > limitRecord)
            recordCount = limitRecord;
        else
            recordCount = totalRecord;

        // Trích xuất từng cặp key-value từ đối tượng JObject
        foreach (JToken result in json["results"])
        {
            JObject metadata = JObject.Parse(result["metadata"].ToString());

            RecordData recordData = new RecordData();

            //Lấy giá trị của mỗi key
            var score = result["score"].Value<int>();
            recordData.playerName = metadata["PlayerName"].ToString();
            recordData.stage = metadata["CurrentStage"].Value<int>();
            recordData.gold = metadata["Gold"].Value<int>();

            recordList.Add(recordData);
        }

        recordList = recordList.OrderByDescending(record => record.score)
                           .ThenByDescending(record => record.stage)
                           .ThenByDescending(record => record.gold)
                           .ToList();

        foreach (RecordData recordData in recordList)
        {
            GameObject prefabToClone = recordObject;

            prefabToClone.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = recordData.playerName;

            var stageColor = "#0077B0";
            var goldColor = "#FFED37";

            prefabToClone.transform.Find("Result").GetComponent<TextMeshProUGUI>().text =
                $" <color={stageColor}>{recordData.stage} Year(s)</color> / <color={goldColor}>{recordData.gold} G</color>";

            Instantiate(prefabToClone, transformParent);
        }
    }
}

public class RecordData
{
    public int score { get; set; }
    public string playerName { get; set; }
    public int stage { get; set; }
    public int gold { get; set; }
}
