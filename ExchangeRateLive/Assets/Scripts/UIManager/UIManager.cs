using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : Singleton<UIManager>
{
    private string _currencyA, _currencyB, _amount, _day, _month, _year;
    private BackendManager _backendManager;
    private Dropdown[] _dropdown = new Dropdown[2];
    public Text _liveRateText, _liveRateWithAmountText, _liveRateWithDate;

    void Start()
    {
        _backendManager = BackendManager.Instance;

        GetCurrencySlots();
    }

    private void GetCurrencySlots() 
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("currency");

        var currencies = _backendManager.GenerateCurrency();

        foreach(GameObject slot in slots) 
        {
            var currencyList = slot.GetComponent<Dropdown>();
            currencyList.options.Clear();
            currencyList.options.AddRange(from string currency in currencies
                                          let data = new Dropdown.OptionData { text = currency }
                                          select data);
        } 
    }

    public void CurrencyA(Text currency) 
    {
        _currencyA = currency.text;
    }

    public void CurrencyB(Text currency)
    {
        _currencyB = currency.text;
    }

    public void Amount(Text amount) 
    {
        _amount = amount.text;
    }

    public void OptionValueA(Dropdown dropdown) 
    {
        _dropdown[0] = dropdown;
    }

    public void OptionValueB(Dropdown dropdown)
    {
        _dropdown[1] = dropdown;
    }

    public void Day(Text day)
    {
        _day = day.text;
    }

    public void Month(Text month)
    {
        _month = month.text;
    }

    public void Year(Text year)
    {
        _year = year.text;
    }

    public void PerformCurrencyInverse() 
    {
        int value = _dropdown[0].value;

        _dropdown[0].value = _dropdown[1].value;

        _dropdown[1].value = value;
    }

    public void LiveRate()
    {
        _liveRateText.text = _backendManager.GetLiveRate(_currencyA, _currencyB);
    }

    public void LiveRateWithAmount() 
    {
        _liveRateWithAmountText.text = _backendManager.GetLiveRate(float.Parse(_amount), _currencyA, _currencyB);
    }

    public void LiveRateWithDate()
    {
        _liveRateWithDate.text = _backendManager.GetRateOnDate(_day, _month, _year, _currencyA, _currencyB);
    }
}
