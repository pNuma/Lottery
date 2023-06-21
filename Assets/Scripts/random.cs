using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class random : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    public TMP_InputField minInputField;
    public TMP_InputField maxInputField;
    public TMP_InputField exInputField;
    public TextMeshProUGUI resultText;

    public int min=0, max=1;
    public string ex;
    private List<int> list = new List<int>();
    private List<int> exList = new List<int>();

    //除外リストを作る
    void exGen(string ex)
    {
        int i = 0;
        string tmp="";
        while (true)
        {
            //除外する値が無ければ処理を終了
            if (ex.Length == 0)
            {
                if (exList.Count == 0) exList.Add(-1);
                break;
            }

            //カンマ区切りを判定
            if(ex[i] == ',')
            {
                exList.Add(int.Parse(tmp));                
                tmp = "";  
            }
            else
            {
                tmp = tmp + ex[i];
            }

            i++;

            if (i >= ex.Length)
            {
                exList.Add(int.Parse(tmp));                
                break;
            }
        }
    }

    //出現しうる値のリストを作成
    void listGen(int min, int max, List<int> exList)
    {

        for (int i = min; i <= max; i++)
        {
            //除外リストに値(i)があれば処理を飛ばす
            if (exList.Contains(i)) continue;
            else list.Add(i);
        }

        //リストをシャッフル
        list = list.OrderBy(a => Guid.NewGuid()).ToList();
    }

    void result()
    {
        //リストの1番目を結果として出力
        resultText.text = list[0].ToString();
    }

    public void OnClick()
    {
        if(minInputField.text.Length == 0|| maxInputField.text.Length == 0)
        {
            resultText.text = "Please enter a value";
            resultText.color = Color.red;
            return;
        }

        resultText.color = Color.white;
        min = int.Parse(minInputField.text);
        max = int.Parse(maxInputField.text);
        ex = exInputField.text;

        exGen(ex);
        listGen(min, max, exList);
        result();

        exList.Clear();
        list.Clear();
    }


    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        minInputField = GameObject.Find("MinInput").GetComponent<TMP_InputField>();
        maxInputField = GameObject.Find("MaxInput").GetComponent<TMP_InputField>();
        exInputField = GameObject.Find("ExInput").GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
