using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Assets.Scripts.Model;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

public static class APIController
{

    private const string URL = "http://178.62.63.37:8060/api";
    private const string TURN = "Turn";

    public static List<DTOTurn> TurnList = new List<DTOTurn>(); 

    private static DTOTurn dtoTurn = new DTOTurn();

    public static void SetData(int surroundingAllyCount, int surroundingOppositionCount, int totalAllyCount, int totalOppositionCount, string job, float healthPercent, float manaPercent)
    {
        dtoTurn = new DTOTurn();
        dtoTurn.SurroundingAllyCount = surroundingAllyCount;
        dtoTurn.SurroundingOppositionCount = surroundingOppositionCount;
        dtoTurn.TotalAllyCount = totalAllyCount;
        dtoTurn.TotalOppositionCount = totalOppositionCount;
        dtoTurn.Job = job;
        dtoTurn.HealthPercent = healthPercent;
        dtoTurn.ManaPercent = manaPercent;
    }

    public static void SetAction(string action)
    {
        dtoTurn.Action = action;
    }

    public static void SendData(bool move)
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

    public static void GetData()
    {
        var client = new RestClient(URL);
        var request = new RestRequest(TURN, Method.GET);
        request.AddHeader("SurroundingAllyCount", dtoTurn.SurroundingAllyCount.ToString());
        request.AddHeader("SurroundingOppositionCount", dtoTurn.SurroundingOppositionCount.ToString());
        request.AddHeader("TotalAllyCount", dtoTurn.TotalAllyCount.ToString());
        request.AddHeader("TotalOppositionCount", dtoTurn.TotalOppositionCount.ToString());
        request.AddHeader("Job", dtoTurn.Job);
        request.AddHeader("HealthPercent", dtoTurn.HealthPercent.ToString());
        request.AddHeader("ManaPercent", dtoTurn.ManaPercent.ToString());

        client.ExecuteAsync<List<DTOTurn>>(request, response => {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JsonDeserializer JSON = new JsonDeserializer();

                TurnList = JSON.Deserialize<List<DTOTurn>>(response);
            }
        });
    }
}
