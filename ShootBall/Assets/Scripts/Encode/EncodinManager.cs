using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncryString
{
    string[] parts;
    string[] tempParts;
    const int encrySize = 4;

    public EncryString()
    {
        parts = new string[encrySize];
        tempParts = new string[encrySize];
    }

    public EncryString(string value) : this()
    {
        SetString(value);
    }

    public string value
    {
        get { return GetString(); }
        set { SetString(value); }
    }

    public void SetString(string value)
    {
        Clear();

        if (value == null)
            return;

        int splitSize = value.Length / encrySize + (value.Length % encrySize == 0 ? 0 : 1);

        string temp = "";
        int index = 0;
        for (int i = 0, k = 0; i < value.Length; i++, k++)
        {
            if (splitSize == k)
            {
                parts[index++] = temp;
                temp = "";
                k = 0;
            }
            temp += value[i];
        }

        parts[index] = temp;

        for (int i = 0; i < encrySize; i++)
        {
            tempParts[i] = parts[encrySize - 1 - i];
        }
    }

    public string GetString()
    {
        string str1 = "";
        for (int i = 0; i < encrySize; i++)
        {
            str1 += parts[i];
        }

        string str2 = "";
        for (int i = 0; i < encrySize; i++)
        {
            str2 += tempParts[encrySize - 1 - i];
        }

        if (str1 != str2)
        {
            UtilManager.Quit();
            return null;
        }

        return str1;
    }

    void Clear()
    {
        for (int i = 0; i < 4; i++)
        {
            parts[i] = tempParts[i] = "";
        }
    }
}

public class EncryFloat
{
    string[] parts;
    string[] tempParts;
    const int encrySize = 4;

    public EncryFloat()
    {
        parts = new string[encrySize];
        tempParts = new string[encrySize];
    }

    public EncryFloat(float value) : this()
    {
        SetFloat(value);
    }

    public float value
    {
        get { return GetFloat(); }
        set { SetFloat(value); }
    }

    public void SetFloat(float value)
    {
        Clear();

        string floatStr = value.ToString("F8");

        int splitSize = floatStr.Length / encrySize + (floatStr.Length % encrySize == 0 ? 0 : 1);

        string temp = "";
        int index = 0;
        for (int i = 0, k = 0; i < floatStr.Length; i++, k++)
        {
            if(splitSize == k)
            {
                parts[index++] = temp;
                temp = "";
                k = 0;
            }
            temp += floatStr[i];
        }

        parts[index] = temp;

        for(int i = 0; i < encrySize; i++)
        {
            tempParts[i] = parts[encrySize - 1 - i];
        }
    }

    public float GetFloat()
    {
        string float1 = "";
        for(int i = 0; i < encrySize; i++)
        {
            float1 += parts[i];
        }

        string float2 = "";
        for (int i = 0; i < encrySize; i++)
        {
            float2 += tempParts[encrySize - 1 - i];
        }

        if(float1 != float2)
        {
            UtilManager.Quit();
            return -1;
        }

        return float.Parse(float1);
    }

    void Clear()
    {
        for (int i = 0; i < 4; i++)
        {
            parts[i] = tempParts[i] = "";
        }
    }
}

public class EncryNumber
{
    long[] parts;
    long[] tempParts;
    const int encrySize = 4;
    const int splitSize = 16;
    public EncryNumber()
    {
        parts = new long[encrySize];
        tempParts = new long[encrySize];
    }

    public EncryNumber(long value) : this()
    {
        SetNumber(value);
    }

    public long value
    {
        get { return GetNumber(); }
        set { SetNumber(value); }
    }

    public void SetNumber(long value)
    {
        Clear();
        int index = 0;
        long number = 0;
        for (int i = 0, k = 0; i < 64; i++, k++)
        {
            if (k == splitSize)
            {
                parts[index] = (number >> (splitSize * index));
                index++;
                number = 0;
                k = 0;
            }

            int seat = (1 << i);
            number += (seat & value);
            k %= splitSize;
        }

        parts[index] = (number >> (splitSize * 3));

        for (int i = 0; i < encrySize; i++)
        {
            tempParts[i] = -parts[i];
        }
    }

    public long GetNumber()
    {
        long number1 = 0;
        for (int i = 0; i < encrySize; i++)
        {
            number1 += (parts[i] << (splitSize * i));
        }

        long number2 = 0;
        for (int i = 0; i < encrySize; i++)
        {
            number2 += (-tempParts[i] << (splitSize * i));
        }

        if (number1 != number2)
        {
            UtilManager.Quit();
            return -1;
        }
        return number1;
    }

    void Clear()
    {
        for (int i = 0; i < 4; i++)
        {
            parts[i] = tempParts[i] = 0;
        }
    }
}

public class EncryBool
{
    long[] parts;
    long[] tempParts;
    const int encrySize = 4;

    public EncryBool()
    {
        parts = new long[encrySize];
        tempParts = new long[encrySize];
    }

    public EncryBool(bool value) : this()
    {
        SetBool(value);
    }

    public bool value
    {
        get { return GetBool(); }
        set { SetBool(value); }
    }

    public void SetBool(bool value)
    {
        Clear();

        System.Random rand = new System.Random();
        int n1 = rand.Next(0, 10000);
        int n2 = rand.Next(0, 10000);

        parts[0] = parts[2] = n1;
        parts[1] = parts[3] = n2;

        if (value == true)
        {
            parts[2]++;
            parts[3]++;
        }

        for (int i = 0; i < encrySize; i++)
        {
            tempParts[i] = -parts[i];
        }
    }

    public bool GetBool()
    {
        if (parts[0] == parts[2] && parts[1] == parts[3]) 
            return false;

        if (parts[2] - parts[0] == parts[3] - parts[1])
            return true;

        UtilManager.Quit();
        return false;
    }

    void Clear()
    {
        for (int i = 0; i < 4; i++)
        {
            parts[i] = tempParts[i] = 0;
        }
    }
}

