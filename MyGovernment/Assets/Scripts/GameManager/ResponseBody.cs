using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseBody
{

    public string address;
    public Content content;
    public int status;

}

public class Content
{
    public string address;
    public Address_Detail address_detail;
    public Point point;
}
public class Address_Detail
{
    public string city;
    public int city_code;
    public string district;
    public string province;
    public string street;
    public string street_number;
    public Address_Detail(string city, int city_code, string district, string province, string street, string street_number)
    {
        this.city = city;
        this.city_code = city_code;
        this.district = district;
        this.province = province;
        this.street = street;
        this.street_number = street_number;
    }
}
public class Point
{
    public string x;
    public string y;
    public Point(string x, string y)
    {
        this.x = x;
        this.y = y;
    }
}
