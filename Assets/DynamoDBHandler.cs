using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Threading;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime.Internal;
using System.Collections.Generic;
using Amazon.Util;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.CognitoIdentity;

public class DynamoDBHandler : MonoBehaviour {
    public static string QueryPK = "";
    public static string QueryTableName = "done_list";


    #region aws DynamoDB SDK
    private IAmazonDynamoDB _client;
    public Text resultText;
    void Start()
    {
        _client = Client;
    }

    // Domain 
    public string IdentityPoolId = "us-east-1:04ceb567-f209-4104-80fd-b17f27110ab0";
    public string CognitoPoolRegion = RegionEndpoint.USEast1.SystemName;
    public string DynamoRegion = RegionEndpoint.USEast1.SystemName;
    public bool LoadCheck = false;

    private RegionEndpoint _CognitoPoolRegion
    {
        get { return RegionEndpoint.GetBySystemName(CognitoPoolRegion); }
    }

    private RegionEndpoint _DynamoRegion
    {
        get { return RegionEndpoint.GetBySystemName(DynamoRegion); }
    }

    private static IAmazonDynamoDB _ddbClient;

    private AWSCredentials _credentials;

    private AWSCredentials Credentials
    {
        get
        {
            if (_credentials == null)
                _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoPoolRegion);
            return _credentials;
        }
    }

    protected IAmazonDynamoDB Client
    {
        get
        {
            if (_ddbClient == null)
            {
                _ddbClient = new AmazonDynamoDBClient(Credentials, _DynamoRegion);
            }

            return _ddbClient;
        }
    }

    public void DynamoDBQuery(Dictionary<string, AttributeValue> lastKeyEvaluated)
    {
        var request = new ScanRequest
        {
            TableName = "Index-sporden",

            ExclusiveStartKey = lastKeyEvaluated,
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                        {":val", new AttributeValue { S = QueryPK }}
                },
            FilterExpression = "user_email = :val",
        };

        _client.ScanAsync(request, (result) =>
        {
            foreach (Dictionary<string, AttributeValue> item
                     in result.Response.Items)
            {
                PrintItem(item);
                DataToList();
            }
            lastKeyEvaluated = result.Response.LastEvaluatedKey;
            if (lastKeyEvaluated != null && lastKeyEvaluated.Count != 0)
            {
                DynamoDBQuery(lastKeyEvaluated);
            }
        });
    }

    private void PrintItem(Dictionary<string, AttributeValue> attributeList)
    {
        foreach (var kvp in attributeList)
        {
            string attributeName = kvp.Key;
            AttributeValue value = kvp.Value;

            if (attributeName == "done_list")
            {
                resultText.text = (value.S == null ? "" : value.S);
                
            }

        }
    }

    #endregion

    public Text DynamoDB_Data;
    public List<string> timeData = new List<string>();

    

    public void DataToList() {
        // remove '[' and ']'
        DynamoDB_Data.text = DynamoDB_Data.text.Remove(0,1);
        DynamoDB_Data.text = DynamoDB_Data.text.Remove(DynamoDB_Data.text.Length-1,1);
        // process data to match PlayerData.timeData
        string[] temp;
        temp = DynamoDB_Data.text.Split(',');
        for (int i = 0; i < temp.Length; i++) {
            timeData.Add(temp[i]);
        }
        LoadCheck = true;
    }
    
    }

