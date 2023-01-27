using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkService
{
    // URL-адрес для отправки запроса.
    private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Kemerovo,RU&mode=xml&APPID=0727b6aa4098d78a896ef7b6b7e6b4d8";

    private const string jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=Kemerovo,RU&APPID=0727b6aa4098d78a896ef7b6b7e6b4d8";

    private const string webImage = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";

    private const string localApi = "http://localhost/uia/api.php"; // http://127.0.0.1:8282/ws

    // Проверка ответа на наличие ошибок.
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
        // HTTP-запрос, отправленный путём создания
        // веб-объекта.
        WWW www = new WWW(url);

        // Пауза в процессе скачивания.
        yield return www;

        // Метод IsResponseValid() проверяет наличие
        // ошибок в HTTP-ответе.
        if (!IsResponcevalid(www))

        // Прерывание сопрограммы ва случае ошибки.
        yield break;

        // Делегат может быть вызван так же,
        // как и исходная функция.
        // С помощью метода callback() возвращает полученный
        // ответ.
        callback(www.text);
    }

    public IEnumerator GetWeatherJSON(Action<string> callback)
    {
        return CallAPI(jsonApi, callback);
    }

    public IEnumerator GetWeatherXML(Action<string> callback)
    {
        // Каскад ключевых слов yield в вызывающих
        // друг друга методах сопрограммы.
        return CallAPI(xmlApi, callback);
    }

    // Этот обратный вызов вместо строки
    // принимает объекты типа Texture2D.
    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        WWW www = new WWW(webImage);
        yield return www;
        callback(www.texture);
    }
}

/*Обратным.вызовом.(callback).называется.вызов.функции,.используемой.для.
обмена.данными.с.вызывающим.объектом..Объект.A.может.сообщить.объекту.B.об.одном.из.своих.
методов..Позднее.объект.B.может.вызвать.этот.метод.для.обмена.данными.с.объектом.A*/