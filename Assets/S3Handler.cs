using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.IO;
using System;
using Amazon.S3.Util;
using System.Collections.Generic;
using Amazon.CognitoIdentity;
using Amazon;

public class S3Handler : MonoBehaviour {
    public string IdentityPoolId = "us-east-1:04ceb567-f209-4104-80fd-b17f27110ab0";
    public string CognitoIdentityRegion = RegionEndpoint.USEast1.SystemName;
    private RegionEndpoint _CognitoIdentityRegion
    {
        get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
    }
    public string S3Region = RegionEndpoint.USEast1.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }
    public string S3BucketName = "spordena15102b183ff4128b4ef6d0604708374-sporden";
    public static string DataFile = "playerprefs.json";


    private void Awake()
    {
        // Attach AWS S3 SDK
        UnityInitializer.AttachToGameObject(this.gameObject);
        // Fixes: "InvalidOperationException: Cannot override system-specified headers"
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
    }

    #region private members

    private IAmazonS3 _s3Client;
    private AWSCredentials _credentials;

    private AWSCredentials Credentials
    {
        get
        {
            if (_credentials == null)
                _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
            return _credentials;
        }
    }

    private IAmazonS3 Client
    {
        get
        {
            if (_s3Client == null)
            {
                _s3Client = new AmazonS3Client(Credentials, _S3Region);
            }
            //test comment
            return _s3Client;
        }
    }

    #endregion

    #region helper methods

    private string GetFileHelper()
    {
        var fileName = DataFile;

        if (!File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName))
        {
            var streamReader = File.CreateText(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName);
            streamReader.WriteLine("This is a sample s3 file uploaded from unity s3 sample");
            streamReader.Close();
        }
        return fileName;
    }

    private string GetPostPolicy(string bucketName, string key, string contentType)
    {
        bucketName = bucketName.Trim();

        key = key.Trim();
        // uploadFileName cannot start with /
        if (!string.IsNullOrEmpty(key) && key[0] == '/')
        {
            throw new ArgumentException("uploadFileName cannot start with / ");
        }

        contentType = contentType.Trim();

        if (string.IsNullOrEmpty(bucketName))
        {
            throw new ArgumentException("bucketName cannot be null or empty. It's required to build post policy");
        }
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("uploadFileName cannot be null or empty. It's required to build post policy");
        }
        if (string.IsNullOrEmpty(contentType))
        {
            throw new ArgumentException("contentType cannot be null or empty. It's required to build post policy");
        }

        string policyString = null;
        int position = key.LastIndexOf('/');
        if (position == -1)
        {
            policyString = "{\"expiration\": \"" + DateTime.UtcNow.AddHours(24).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\",\"conditions\": [{\"bucket\": \"" +
                bucketName + "\"},[\"starts-with\", \"$key\", \"" + "\"],{\"acl\": \"private\"},[\"eq\", \"$Content-Type\", " + "\"" + contentType + "\"" + "]]}";
        }
        else
        {
            policyString = "{\"expiration\": \"" + DateTime.UtcNow.AddHours(24).ToString("yyyy-MM-ddTHH:mm:ssZ") + "\",\"conditions\": [{\"bucket\": \"" +
                bucketName + "\"},[\"starts-with\", \"$key\", \"" + key.Substring(0, position) + "/\"],{\"acl\": \"private\"},[\"eq\", \"$Content-Type\", " + "\"" + contentType + "\"" + "]]}";
        }

        return policyString;
    }

    #endregion

    public void PostObject()
    {

        var stream = new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + DataFile, FileMode.Open, FileAccess.Read, FileShare.Read);

        // Changed from the sample to include region to fix:
        // HttpErrorResponseException 
        var request = new PostObjectRequest()
        {
            Bucket = S3BucketName,
            Key = DataFile,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                //ResultText.text += string.Format("\nobject {0} posted to bucket {1}", responseObj.Request.Key, responseObj.Request.Bucket);
            }
            else
            {
                //ResultText.text += "\nException while posting the result object";
                // Changed from sample so we can see actual error and not null pointer exception
                //ResultText.text += string.Format("\n receieved error {0}", responseObj.Exception.ToString());
            }
        });

        Debug.Log("上傳完畢。");
    }
    
    public void GetObject()
    {
        //ResultText.text = string.Format("fetching {0} from bucket {1}", SampleFileName, S3BucketName);
        
        Client.GetObjectAsync(S3BucketName, DataFile, (responseObj) =>
        {
            string data = null;
            var response = responseObj.Response;
            if (response.ResponseStream != null)
            {
                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    data = reader.ReadToEnd();
                }

                //ResultText.text += "\n";
                //ResultText.text += data;
               
                StreamWriter file = new StreamWriter(Application.persistentDataPath + "/playerprefs.json");
                file.Write(data);
                file.Close();
                Checker = true;
                Debug.Log("下載完畢。");
            }
        });
    }


    public void PostObjectReNameAccount()
    {
        DataFile = GetUserAccount.NowUser + ".json";
        var stream = new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + GetUserAccount.NowUser +".json", FileMode.Open, FileAccess.Read, FileShare.Read);

        // Changed from the sample to include region to fix:
        // HttpErrorResponseException 
        var request = new PostObjectRequest()
        {
            Bucket = S3BucketName,
            Key = DataFile,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region
        };

        Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                //ResultText.text += string.Format("\nobject {0} posted to bucket {1}", responseObj.Request.Key, responseObj.Request.Bucket);
            }
            else
            {
                //ResultText.text += "\nException while posting the result object";
                // Changed from sample so we can see actual error and not null pointer exception
                //ResultText.text += string.Format("\n receieved error {0}", responseObj.Exception.ToString());
            }
        });

        Debug.Log(GetUserAccount.NowUser + ".json " + "上傳完畢。");
    }

    public static bool Checker;
    public void GetNowUserObject()
    {
        Checker = false;
        string fileName = GetUserAccount.NowUser;

        Debug.Log(fileName + ".json  下載開始。");

        Client.GetObjectAsync(S3BucketName, fileName + ".json", (responseObj) =>
        {
            string data = null;
            var response = responseObj.Response;
            if (response.ResponseStream != null)
            {
                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    data = reader.ReadToEnd();
                }

                //ResultText.text += "\n";
                //ResultText.text += data;

                StreamWriter file = new StreamWriter(Application.persistentDataPath + "/" + fileName + ".json");
                file.Write(data);
                file.Close();
                Debug.Log(fileName + ".json  下載完畢。");
                Checker = true;
            }
        });
    }

    public void GetFriendsObject()
    {
        Checker = false;
        string fileName = UIFriendList.myFriendAccount;

        Debug.Log(fileName + ".json  下載開始。");

        Client.GetObjectAsync(S3BucketName, fileName + ".json", (responseObj) =>
        {
            string data = null;
            var response = responseObj.Response;
            if (response.ResponseStream != null)
            {
                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    data = reader.ReadToEnd();
                }

                //ResultText.text += "\n";
                //ResultText.text += data;

                StreamWriter file = new StreamWriter(Application.persistentDataPath + "/" +fileName + ".json");
                file.Write(data);
                file.Close();
                Debug.Log(fileName + ".json  下載完畢。");
                Checker = true;
            }
        });
    }
    
    
}
