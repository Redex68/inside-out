using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

public static class Serializer
{
    //Completed story mode?
    public static void Save(PersistentData data)
    {
        BinaryFormatter fmt = new BinaryFormatter();
        string path = Application.persistentDataPath + "/InsideOut.savedata";
        FileStream fs = new FileStream(path, FileMode.Create);

        fmt.Serialize(fs, data);
        fs.Close();
    }

    public static PersistentData Load()
    {
        string path = Application.persistentDataPath + "/InsideOut.savedata";
        if(File.Exists(path))
        {
            BinaryFormatter fmt = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            PersistentData pd = fmt.Deserialize(fs) as PersistentData;
            fs.Close();
            return pd;
        }
        else Debug.LogError("No save file found!");

        return null;
    }
}
