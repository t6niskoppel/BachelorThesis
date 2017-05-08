using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class IdControl : MonoBehaviour {

    
    private string savePath;
    private int persistentId;
    public SubjectDataController sdc;

    void Awake()
    {
        savePath = Application.dataPath + "/ExperimentData";

        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        if (File.Exists(savePath + "/id_data.dat"))
        {
            LoadNextId();
        }
        else
        {
            persistentId = 1;
            SaveCurrentId();
        }
        sdc.subjectId = persistentId;
    }

    public void SaveCurrentId()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath + "/id_data.dat");
        IdData data = new IdData();
        data.persistentId = persistentId;

        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadNextId()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Open(savePath + "/id_data.dat", FileMode.Open))
        {
            IdData data = (IdData)bf.Deserialize(file);
            persistentId = data.persistentId + 1;
        }
        SaveCurrentId();
    }
}

[Serializable]
class IdData
{
    public int persistentId;
}
