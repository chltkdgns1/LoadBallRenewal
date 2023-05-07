using System.IO;
using UnityEngine;

public class CsvIOManager : MonoBehaviour
{
    static public void ReadCsv(string fileName)
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/Assets/Language/" + fileName + ".csv");

        bool endOfFile = false;
        while (!endOfFile)
        {
            string data_String = sr.ReadLine();
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_String.Split(',');
            for (int i = 0; i < data_values.Length; i++)
            {
                Debug.Log("v: " + i.ToString() + " " + data_values[i].ToString());
            }
        }
    }
}
