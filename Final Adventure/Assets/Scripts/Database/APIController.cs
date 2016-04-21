using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Assets.Scripts.Model;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

public class APIController : MonoBehaviour
{

    private const string URL = "http://localhost:8070/api";
    private const string TURN = "Turn";

    private DTOTurn dtoTurn = new DTOTurn();

    public void SetData(int surroundingAllyCount, int surroundingOppositionCount, int totalAllyCount, int totalOppositionCount, string job, float healthPercent, float manaPercent)
    {
        dtoTurn.SurroundingAllyCount = surroundingAllyCount;
        dtoTurn.SurroundingOppositionCount = surroundingOppositionCount;
        dtoTurn.TotalAllyCount = totalAllyCount;
        dtoTurn.TotalOppositionCount = totalOppositionCount;
        dtoTurn.Job = job;
        dtoTurn.HealthPercent = healthPercent;
        dtoTurn.ManaPercent = manaPercent;
    }

    public void SetAction(string action)
    {
        dtoTurn.Action = action;
    }

    public void SendData(bool move)
    {
        var client = new RestClient(URL);

        dtoTurn.Move = move ? 1 : 0;

        var request = new RestRequest(TURN, Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddBody(dtoTurn);

        client.ExecuteAsync(request, response => {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //Added to the database
                //print(response.Content);
            }
        });
    }

    public void GetData()
    {
        var client = new RestClient(URL);
        var request = new RestRequest(TURN, Method.GET);

        client.ExecuteAsync<List<DTOTurn>>(request, response => {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                print(response.Content);
                JsonDeserializer JSON = new JsonDeserializer();

                List<DTOTurn> myObject =
                    JSON.Deserialize<List<DTOTurn>>(response);
                print(myObject);
            }
        });
    }
}
