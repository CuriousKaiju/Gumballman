using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StorageExample : MonoBehaviour
{
    private const string DirectoryPath = "D:\\Uniti Builds\\Gumballman"; //Рассположение файла
    private const string FileName = "PlayerStorage"; //Имя файла

    private GameStorage _gameStorage;
    void Start()
    {
        
    }
    void Update()
    {
        
       
    }

    private void Save(string storageRaw)
    {
        if (!Directory.Exists(DirectoryPath)) //Проверяем, существует ли папка
        {
            Directory.CreateDirectory(DirectoryPath);
        }

        var fullFileName = $"{DirectoryPath}\\{FileName}.json";

        using (FileStream fileStream = new FileStream(fullFileName, FileMode.OpenOrCreate)) //Создаем файл по маршруту
        {
            byte[] array = System.Text.Encoding.Default.GetBytes(storageRaw); //Преобразовали storageRaw в массив байтов
            fileStream.Write(array, 0, array.Length);
            Debug.Log("Storage was Saved");
        }
    }   
    private string Load()
    {

        var fullFileName = $"{DirectoryPath}\\{FileName}.json";
        if (!File.Exists(fullFileName))
        {
            Debug.Log("File is not Exist");
            return null;
        }

        string result = string.Empty;

        using (FileStream fileStream = File.OpenRead(fullFileName))
        {
            byte[] array = new byte[fileStream.Length];
            fileStream.Read(array, 0, array.Length);
            result = System.Text.Encoding.Default.GetString(array);
            Debug.Log("WasLoaded");
        }
        return result;
    }
    private GameStorage GetGameStorage()
    {
        return new GameStorage()
        {
            LiveValue = Random.Range(1, 10),
            CoinValue = Random.Range(10, 1000),
            PlayerPosition = Random.insideUnitCircle
        };
    }
    [ContextMenu("Load")]
    private void TestLoadData()
    {
        var dataStorage = Load();
        if(string.IsNullOrEmpty(dataStorage))
        {
            return;
        }
        Debug.Log(dataStorage);
        _gameStorage = JsonUtility.FromJson<GameStorage>(dataStorage);
        Debug.Log(_gameStorage.LiveValue + "   " + _gameStorage.CoinValue + "   " + _gameStorage.PlayerPosition);
    }
    [ContextMenu("Save")]
    private void TestSaveData()
    {
        _gameStorage = GetGameStorage();
        var dataStorageRaw = JsonUtility.ToJson(_gameStorage, true);
        Save(dataStorageRaw);
    }

    [System.Serializable]
    public class GameStorage
    {
        public int LiveValue;
        public int CoinValue;
        public Vector3 PlayerPosition;
    }

}
