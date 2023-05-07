using UnityEditor;

[CustomEditor(typeof(PopupStack), true)]
public class UIPopupEditor : Editor
{
    BaseEditor AddEdit = new BaseEditor();
    BaseEditor RemoveEdit = new BaseEditor();
    PopupStack uiPopup;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        uiPopup = (PopupStack)target;
        if (uiPopup == null) return;

        Reset();

        AddEdit.Reset();
        RemoveEdit.Reset();

        AddEdit.Read(uiPopup.compStrAdd, uiPopup.methodStrAdd);

        RemoveEdit.Read(uiPopup.compStrRemove, uiPopup.methodStrRemove);


        AddEdit.InitEnable(uiPopup.AddObject);
        RemoveEdit.InitEnable(uiPopup.RemoveObject);

        GetCompMethodValue();
    }

    private void OnDisable()
    {
        if (uiPopup.AddObject == null) AddEdit.ResetStr();
        if (uiPopup.RemoveObject == null) RemoveEdit.ResetStr();

        AddEdit.Write(ref uiPopup.compStrAdd, ref uiPopup.methodStrAdd);
        RemoveEdit.Write(ref uiPopup.compStrRemove, ref uiPopup.methodStrRemove);
        uiPopup.SetDirty();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AddEdit.PrintCompList(uiPopup.AddObject);
        RemoveEdit.PrintCompList(uiPopup.RemoveObject);
        GetCompMethodValue();
    }

    void Reset()
    {
        AddEdit.Reset();
        RemoveEdit.Reset();
    }

    void GetCompMethodValue()
    {
        uiPopup.classTypeAdd = AddEdit.compType;
        uiPopup.methodInfoAdd = AddEdit.methodInfo;
        uiPopup.classTypeRemove = RemoveEdit.compType;
        uiPopup.methodInfoRemove = RemoveEdit.methodInfo;
    }
}
