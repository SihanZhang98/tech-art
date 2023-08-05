using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GetCurrentWeatherInfo : MonoBehaviour
{
    const string OpenWeatherAPIKey = "74685a051182bd277a7b5673528d68f5";
    const string URL_GetPublicIP= "https://api.ipify.org/";
    const string URL_GetGeographicData = "http://www.geoplugin.net/json.gp?ip=";
    const string URL_GetWeatherData = "http://api.openweathermap.org/data/2.5/weather";

    string publicIP;
    GeoPluginResponse geographicData;
    OpenWeatherResponse weatherData;

    public string CurrentWeather { get; private set; }
    ChangeWeather changeWeather;

    public TMP_Text weatherText;
    public TMP_Text coordText;

    public enum EPhase
    {
        NotStarted,
        GetPublicIP,
        GetGeographicData,
        GetWeatherData,

        Failed,
        Succeeded
    }

    public EPhase Phase { get; private set; } = EPhase.NotStarted;


    class GeoPluginResponse
    {

        [JsonProperty("geoplugin_latitude")] public string Latitude { get; set; }
        [JsonProperty("geoplugin_longitude")] public string Longitude { get; set; }
    }

    class OpenWeatherResponse
    {
        [JsonProperty("timezone")] public int Timezone { get; set; }    
        [JsonProperty("weather")] public List<OpenWeather_Condition> WeatherConditions { get; set; }

    }
    
     class OpenWeather_Condition
    {
        [JsonProperty("id")] public int ConditionID { get; set; }
        [JsonProperty("main")] public string Main { get; set; }
        [JsonProperty("description")] public string Description { get; set; }
        [JsonProperty("icon")] public string Icon { get; set; }
    }


    void Start()
    {
        changeWeather = this.GetComponent<ChangeWeather>();
        StartCoroutine(GetWeather_Phase1_PublicIP());
    }

    
    void Update()
    {
        
    }

    IEnumerator GetWeather_Phase1_PublicIP()
    {
        Phase = EPhase.GetPublicIP;
        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetPublicIP))
        {
            request.timeout = 5;
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.Success)
            {
                publicIP= request.downloadHandler.text.Trim();
                Debug.Log(publicIP);
                StartCoroutine(GetWeather_Phase2_GetGeographicInfo());
            }
            else
            {
                Phase = EPhase.Failed;
                Debug.LogError($"Failed to get public IP: { request.downloadHandler.text}");
            }

        }
        yield return null;
    }



    IEnumerator GetWeather_Phase2_GetGeographicInfo()
    {

        Phase = EPhase.GetGeographicData;
        using (UnityWebRequest request = UnityWebRequest.Get(URL_GetGeographicData + publicIP))
        {

            request.timeout = 3;
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                geographicData = JsonConvert.DeserializeObject<GeoPluginResponse>(request.downloadHandler.text);
                Debug.Log(geographicData.Latitude);
                StartCoroutine(GetWeather_Phase3_GetWeatherData());

            }
            else
            {
                Phase = EPhase.Failed;
                Debug.LogError($"Failed to get geo data: {request.downloadHandler.text}");
            }
        }

        yield return null;
    }

    IEnumerator GetWeather_Phase3_GetWeatherData()
    {
        Phase = EPhase.GetWeatherData;

        string weatherURL = URL_GetWeatherData;
        weatherURL += $"?lat={geographicData.Latitude}";
        weatherURL += $"&lon={geographicData.Longitude}";
        weatherURL += $"&appid={OpenWeatherAPIKey}";


        using (UnityWebRequest request = UnityWebRequest.Get(weatherURL))
        {

            request.timeout = 3;
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                weatherData = JsonConvert.DeserializeObject<OpenWeatherResponse>(request.downloadHandler.text);
                OpenWeather_Condition weatherconsitions = weatherData.WeatherConditions[0];
                CurrentWeather = weatherconsitions.Main;
                Debug.Log(CurrentWeather);

                coordText.text = $"Latitude: {geographicData.Latitude}, Longitude: {geographicData.Longitude}";
                weatherText.text = weatherconsitions.Main;

                //change weather
                changeWeather.HandleCurrentWeatherData(CurrentWeather);
            }
            else
            {
                Phase = EPhase.Failed;
                Debug.LogError($"Failed to get weather data: {request.downloadHandler.text}");
            }
            

        }

        yield return null;

    }
}
