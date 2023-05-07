using System;

public interface IComplete
{
    void OnComplete(Action<object[]> act, params object[] ob);
}

