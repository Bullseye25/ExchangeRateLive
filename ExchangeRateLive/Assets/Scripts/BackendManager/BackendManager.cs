using System.Collections;
using UnityEngine;
using System.Net;
using Newtonsoft.Json;
using System;

public class BackendManager : Singleton<BackendManager>
{
    private WebClient _client;
    private bool _isConnected = false;
    public GameObject _internetError;

    private void Start()
    {
        StartCoroutine(checkInternetConnection((isConnected) =>
        {
            _isConnected = isConnected;

            if (_isConnected == false)
            {
                _internetError.SetActive(true);
                return;
            }
            else
            {
                _client = new WebClient();
            }
        }));
    }

    // will create the list of currencies available
    public string[] GenerateCurrency() 
    {
        Rates rates = new Rates();

        var values = rates.GetType();

        string[] currencies = new string[values.GetProperties().Length];

        for (int i = 0; i < values.GetProperties().Length; i++)
        {
            var value = values.GetProperties().GetValue(i).ToString();
            var currency = value.Substring(value.Length - 3);

            currencies[i] = currency;
        }

        return currencies;
    }

    /// <summary>
    /// Checks the internet connection.
    /// </summary>
    /// <returns>The internet connection.</returns>
    /// <param name="action">Action.</param>
    private IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    public string GetLiveRate(string sourceCurrency,
                              string endCurrency)
    {
        if (_isConnected == true)
        {
            string url = "https://api.exchangeratesapi.io/latest?base=" + sourceCurrency + "&symbols=" + endCurrency;
            string jsonData;

            try 
            {
                jsonData = _client.DownloadString(url);
            }
            catch 
            {
                return "Result Not Available!";
            }

            var jData = JsonConvert.DeserializeObject<Exchange>(jsonData);
            var values = jData.rates.GetType();
            var currency = GenerateCurrency();

            for (int i = 0; i < currency.Length;i++)
            {
                if (currency[i] == endCurrency)
                {
                    return values.GetProperty(currency[i]).GetValue(jData.rates, null) + " " + currency[i];
                }
            }

            return "Result Not Available!";
        }
        else
        {
            _internetError.SetActive(true);

            return "invalid";
        }
    }


    public string GetLiveRate(float amount, string sourceCurrency, string endCurrency)
    {
        var result = GetLiveRate(sourceCurrency, endCurrency);

        if (result == "Result Not Available!" || result == "invalid")
            return result;
        else 
        {
            var res = result.Remove(result.Length - 4);
            float rate = float.Parse(res);
            var newResult = rate * amount;
            string fin = newResult + " " + endCurrency;

            return fin;
        }
    }

    public string GetRateOnDate(string day, string month, string year, string sourceCurrency, string endCurrency)
    {
        //Sorry for being lazy and copy pasting.. 
        if (_isConnected == true)
        {
            string url = "https://api.exchangeratesapi.io/" + year + "-" + month + "-" + day + "?base=" + sourceCurrency + "&symbols=" + endCurrency;
            string jsonData;

            try
            {
                jsonData = _client.DownloadString(url);
            }
            catch
            {
                return "Result Not Available!";
            }

            var jData = JsonConvert.DeserializeObject<Exchange>(jsonData);
            var values = jData.rates.GetType();
            var currency = GenerateCurrency();

            for (int i = 0; i < currency.Length; i++)
            {
                if (currency[i] == endCurrency)
                {
                    return values.GetProperty(currency[i]).GetValue(jData.rates, null) + " " + currency[i];
                }
            }

            return "Result Not Available!";
        }
        else
        {
            _internetError.SetActive(true);

            return "invalid";
        }
    }
}