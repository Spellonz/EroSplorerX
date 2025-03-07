using EroSplorerX.Data.AutoBlowApi.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using Serilog;

namespace EroSplorerX.Helpers;

public class AutoBlowHelper
{
    private const string BASE_URL = "https://us-east-1.autoblowapi.com/autoblow";
    private const string TOKEN_HEADER = "x-device-token";

    private static HttpClient GetClient(string deviceToken)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add(TOKEN_HEADER, deviceToken);
        return client;
    }

    /// <summary>
    /// Check if a device is connected and get the cluster it is connected to.
    /// This request can be performed on any cluster but for the best 
    /// performance you should use https://us-east-1.autoblowapi.com
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    public static async Task<ConnectedResponse?> Connected(string deviceToken)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/connected";

        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ConnectedResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Get additional information about the device such as the firmware/hardware version.
    /// </summary>
    /// <param name="deviceToken"></param>
    public static async Task<InfoResponse?> Info(string deviceToken)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/info";

        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<InfoResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Get the current state of the device
    /// </summary>
    /// <param name="deviceToken"></param>
    public static async Task<StateResponse?> State(string deviceToken)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/state";

        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Place the device in oscillate mode. This will cause the device to oscillate between 
    /// the minimum and maximum stroke points with the given speed.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="speed">Oscillator speed in percent</param>
    /// <param name="minY">Oscillator min point in percent</param>
    /// <param name="maxY">Oscillator max point in percent</param>
    public static async Task<StateResponse?> Oscillate(string deviceToken, int speed, int minY, int maxY)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/oscillate";

        try
        {
            var body = new { speed, minY, maxY };
            var jsonBody = JsonConvert.SerializeObject(body);
            var sContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, sContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Will start the oscillate mode with the current settings without having to send the settings again.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    public static async Task<StateResponse?> OscillateStart(string deviceToken)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/oscillate/start";

        try
        {
            var response = await client.PutAsync(url, null);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Will stop the oscillate mode.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    public static async Task<StateResponse?> OscillateStop(string deviceToken)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/oscillate/stop";

        try
        {
            var response = await client.PutAsync(url, null);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Will start playing the loaded sync script from the specified time. 
    /// If there is no sync script loaded it will return an error.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="startTimeMs">At what time to start playing the sync script from</param>
    public static async Task<StateResponse?> SyncScriptStart(string deviceToken, double startTimeMs)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/sync-script/start";

        try
        {
            var body = new { startTimeMs };
            var jsonBody = JsonConvert.SerializeObject(body);
            var sContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, sContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Will stop playing the sync script.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    public static async Task<StateResponse?> SyncScriptStop(string deviceToken)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/sync-script/stop";

        try
        {
            var response = await client.PutAsync(url, null);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Will load a sync script from a token.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="scriptToken">The sync script token you want to load on the device</param>
    public static async Task<StateResponse?> SyncScriptLoadToken(string deviceToken, string scriptToken)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/sync-script/load-token";

        try
        {
            var body = new { scriptToken };
            var jsonBody = JsonConvert.SerializeObject(body);
            var sContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, sContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Will upload a sync script from a file. 
    /// The script will be converted to a binary format that the autoblow can play and it will be 
    /// stored on our servers for 48 hours. Each uploaded script will be assigned a token that can 
    /// be used to load the script on the device or to check if the script is loaded.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="file">The funscript file you want to upload</param>
    public static async Task<StateResponse?> SyncScriptUploadFunscript(string deviceToken, byte[] file)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/sync-script/upload-funscript";

        try
        {
            var multipartContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(file);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            multipartContent.Add(fileContent, "file", "funscript.funscript");

            var response = await client.PutAsync(url, multipartContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Will upload a sync script from a csv file. 
    /// The script will be converted to a binary format that the autoblow can play and it will be 
    /// stored on our servers for 48 hours. Each uploaded script will be assigned a token that can 
    /// be used to load the script on the device or to check if the script is loaded.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="file">The csv file you want to upload</param>
    public static async Task<StateResponse?> SyncScriptUploadCsv(string deviceToken, byte[] file)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/sync-script/upload-csv-file";

        try
        {
            var multipartContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(file);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            multipartContent.Add(fileContent, "file", "funscript.csv");

            var response = await client.PutAsync(url, multipartContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Will upload a sync script from a url. 
    /// The script will be converted to a binary format that the autoblow can play and it will be 
    /// stored on our servers for 48 hours. Each uploaded script will be assigned a token that can 
    /// be used to load the script on the device or to check if the script is loaded.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="url">The URL that hosts the funscript you want to upload onto the device</param>
    public static async Task<StateResponse?> SyncScriptUploadFunscriptUrl(string deviceToken, string url)
    {
        var client = GetClient(deviceToken);
        var httpUrl = $"{BASE_URL}/sync-script/upload-funscript-url";

        try
        {
            var body = new { url };
            var jsonBody = JsonConvert.SerializeObject(body);
            var sContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(httpUrl, sContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", httpUrl);
            return null;
        }
    }

    /// <summary>
    /// Will upload a sync script from a csv url. 
    /// The script will be converted to a binary format that the autoblow can play and it will be 
    /// stored on our servers for 48 hours. Each uploaded script will be assigned a token that can 
    /// be used to load the script on the device or to check if the script is loaded.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="url">The URL that hosts the csv script you want to upload onto the device</param>
    public static async Task<StateResponse?> SyncScriptUploadCsvUrl(string deviceToken, string url)
    {
        var client = GetClient(deviceToken);
        var httpUrl = $"{BASE_URL}/sync-script/upload-csv-url";

        try
        {
            var body = new { url };
            var jsonBody = JsonConvert.SerializeObject(body);
            var sContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(httpUrl, sContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", httpUrl);
            return null;
        }
    }

    /// <summary>
    /// Add an offset to sync script for latency compensation. 
    /// The offset is in milliseconds and can be positive or negative.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="offsetTimeMs">syncScriptCurrentTime, the offset can be positive or negative</param>
    public static async Task<StateResponse?> SyncScriptOffset(string deviceToken, int offsetTimeMs)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/sync-script/offset";

        try
        {
            var body = new { offsetTimeMs };
            var jsonBody = JsonConvert.SerializeObject(body);
            var sContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, sContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Set the script to play in a loop
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="loop">If set to true it will restart the script after it finishes playing it</param>
    public static async Task<StateResponse?> SyncScriptLoop(string deviceToken, bool loop)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/sync-script/loop";

        try
        {
            var body = new { loop };
            var jsonBody = JsonConvert.SerializeObject(body);
            var sContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, sContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Play one of the 10 loaded local scripts on the device.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="localScriptIndex">Local script playing index</param>
    /// <param name="speed">Local script speed playing index</param>
    public static async Task<StateResponse?> LocalScript(string deviceToken, int localScriptIndex, int speed)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/local-script";

        try
        {
            var body = new { localScriptIndex, speed };
            var jsonBody = JsonConvert.SerializeObject(body);
            var sContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, sContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Play the already selected local script on the device.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    public static async Task<StateResponse?> LocalScriptStart(string deviceToken)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/local-script/start";

        try
        {
            var response = await client.PutAsync(url, null);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Stop playing the local script on the device.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    public static async Task<StateResponse?> LocalScriptStop(string deviceToken)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/local-script/stop";

        try
        {
            var response = await client.PutAsync(url, null);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }

    /// <summary>
    /// Move the stroker to a specific stroke point with a specific speed.
    /// </summary>
    /// <param name="deviceToken">The autoblow device token</param>
    /// <param name="position">Position in percent</param>
    /// <param name="speed">Speed in percent</param>
    public static async Task<GoToResponse?> GoTo(string deviceToken, int position, int speed)
    {
        var client = GetClient(deviceToken);
        var url = $"{BASE_URL}/goto";

        try
        {
            var body = new { position, speed };
            var jsonBody = JsonConvert.SerializeObject(body);
            var sContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, sContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GoToResponse>(content);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error connecting to AutoBlow API at {url}", url);
            return null;
        }
    }
}
