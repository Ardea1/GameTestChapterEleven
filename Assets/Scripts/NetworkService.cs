using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkService
{
    // URL-����� ��� �������� �������.
    private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Kemerovo,RU&mode=xml&APPID=0727b6aa4098d78a896ef7b6b7e6b4d8";

    private const string jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=Kemerovo,RU&APPID=0727b6aa4098d78a896ef7b6b7e6b4d8";

    private const string webImage = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";

    private const string localApi = "http://localhost/uia/api.php"; // http://127.0.0.1:8282/ws

    // �������� ������ �� ������� ������.
    private bool IsResponcevalid(WWW www)
    {
        if (www.error != null)
        {
            Debug.Log("Bad connection");
            return false;
        }
        else if (string.IsNullOrEmpty(www.text))
        {
            Debug.Log("Bad data");
            return false;
        }
        else
        {
            return true;
        }
    }

    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        // HTTP-������, ������������ ���� ��������
        // ���-�������.
        WWW www = new WWW(url);

        // ����� � �������� ����������.
        yield return www;

        // ����� IsResponseValid() ��������� �������
        // ������ � HTTP-������.
        if (!IsResponcevalid(www))

        // ���������� ����������� �� ������ ������.
        yield break;

        // ������� ����� ���� ������ ��� ��,
        // ��� � �������� �������.
        // � ������� ������ callback() ���������� ����������
        // �����.
        callback(www.text);
    }

    public IEnumerator GetWeatherJSON(Action<string> callback)
    {
        return CallAPI(jsonApi, callback);
    }

    public IEnumerator GetWeatherXML(Action<string> callback)
    {
        // ������ �������� ���� yield � ����������
        // ���� ����� ������� �����������.
        return CallAPI(xmlApi, callback);
    }

    // ���� �������� ����� ������ ������
    // ��������� ������� ���� Texture2D.
    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        WWW www = new WWW(webImage);
        yield return www;
        callback(www.texture);
    }
}

/*��������.�������.(callback).����������.�����.�������,.������������.���.
������.�������.�.����������.��������..������.A.�����.��������.�������.B.��.�����.��.�����.
�������..�������.������.B.�����.�������.����.�����.���.������.�������.�.��������.A*/