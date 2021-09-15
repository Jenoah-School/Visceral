using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandomText : MonoBehaviour
{
    private string currentText = "<i>Don't drink and drive kids</i> - Abraham Lincoln";
    [SerializeField] private List<string> textOptions = new List<string>();
    [SerializeField] private TMPro.TextMeshProUGUI randomLoadingText = null;

    private void Awake()
    {
        GetText();
    }

    public void GetText()
    {
        if (PlayerPrefs.HasKey("LoadingscreenText"))
        {
            currentText = PlayerPrefs.GetString("LoadingscreenText");
            randomLoadingText.text = currentText;
        }
        else
        {
            SetRandomText();
        }

    }

    public void SetRandomText()
    {
        List<string> selectableTextList = new List<string>(textOptions);
        selectableTextList.Remove(currentText);
        currentText = selectableTextList[Random.Range(0, selectableTextList.Count)];
        PlayerPrefs.SetString("LoadingscreenText", currentText);
        randomLoadingText.text = currentText;
    }
}
