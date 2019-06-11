using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateTimeSystem : MonoBehaviour 
{
    public Dropdown _month;
    public Dropdown _day;
    public Dropdown _year;

    void Start () 
    {
        DateManager();
    }
	
    public void DateManager() 
    {
        //Month
        for (int i = 1; i < 13; i++)
        {
            if (i < 10)
                _month.options.Add(new Dropdown.OptionData("0" + i));
            else
                _month.options.Add(new Dropdown.OptionData(i.ToString()));
        }

        //Year
        _year.options.Add(new Dropdown.OptionData("2017"));
        _year.options.Add(new Dropdown.OptionData("2018"));
        _year.options.Add(new Dropdown.OptionData("2019"));

        for (int i = 1; i < 32; i++)
        {
            if (i < 10)
                _day.options.Add(new Dropdown.OptionData("0" + i));
            else
                _day.options.Add(new Dropdown.OptionData(i.ToString()));
        }
    }

    public void DayManager(Text month) 
    {
        switch (month.text)
        {
            case "01":
            case "03":
            case "05":
            case "07":
            case "08":
            case "10":
            case "12":
                Month(32);
                break;
            case "04":
            case "06":
            case "09":
            case "11":
                Month(31);
                break;
            case "02":
                Month(29);
                break;
        }
    }

    public void Month(int end)
    {
        _day.options.Clear();

        //sorry for being lazy
        for (int i = 1; i < end; i++)
        {
            if (i < 10)
                _day.options.Add(new Dropdown.OptionData("0" + i));
            else
                _day.options.Add(new Dropdown.OptionData(i.ToString()));
        }
    }
}
