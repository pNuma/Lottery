using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class Random : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    public TMP_InputField minInputField;
    public TMP_InputField maxInputField;
    public TMP_InputField exInputField;
    public TMP_InputField volInputField;
    public TextMeshProUGUI resultText;

    public int min=0, max=1,vol=1;
    public string ex;
    private List<int> list = new List<int>();
    private List<int> exList = new List<int>();
    private List<int> resultList = new List<int>();

    private int upLimit = 9999;
    private int lowLimit = -9999;

    //除外リストを作る
    void ExGen(string ex)
    {
        int i = 0;
        string tmp="";
        while (true)
        {
            //除外する値が無ければ処理を終了
            if (ex.Length == 0)
            {
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
                tmp += ex[i];
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
    void ListGen(int min, int max, List<int> exList)
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

    void Result()
    {
        if (vol == 1)
        {
            if (list.Max() >= 1000 || list.Min() <= -999) resultText.fontSize = 72;
            else resultText.fontSize = 140;

            resultText.text = list[0].ToString();
            return;
        }

        //リストのi番目までを結果として出力
        string tmp="";
        if(vol>list.Count) vol = list.Count;

        
        for(int i = 0; i < vol; i++)
        {
            resultList.Add(list[i]);
        }

        resultList.Sort();

        for (int i = 0; i < vol; i++)
        {
            if (i == 0) tmp += resultList[i].ToString();
            else tmp += "\n" + resultList[i].ToString();
        }

        if (list.Max() >= 1000 || list.Min() <= -999)  resultText.fontSize = 72;
        else resultText.fontSize=140;
        resultText.text = tmp;
    }

    public void OnClick()
    {
        resultText.fontSize = 50;
        if (minInputField.text.Length == 0|| maxInputField.text.Length == 0)
        {
            resultText.text = "Please enter a value";
            resultText.color = Color.red;
            return;
        }


        resultText.color = Color.white;
        min = int.Parse(minInputField.text);
        max = int.Parse(maxInputField.text);
        ex = exInputField.text;

        if (volInputField.text.Length == 0)
        {
            vol = 1;            
        }
        else vol = int.Parse(volInputField.text);


        if (min > upLimit || min < lowLimit || max > upLimit || max < lowLimit || vol > 100 || vol < 1)
        {
            resultText.fontSize = 20;
            resultText.text = "value range:\n" +
                "-9999 <= min,max <= 9999 \n" +
                "1 <= vol <= 99";
            resultText.color = Color.red;
            return;
        }



        ExGen(ex);
        ListGen(min, max, exList);
        Result();

        exList.Clear();
        list.Clear();
        resultList.Clear();
    }


    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        minInputField = GameObject.Find("MinInput").GetComponent<TMP_InputField>();
        maxInputField = GameObject.Find("MaxInput").GetComponent<TMP_InputField>();
        exInputField = GameObject.Find("ExInput").GetComponent<TMP_InputField>();
        volInputField = GameObject.Find("VolInput").GetComponent<TMP_InputField>();

        resultText.fontSize = 50;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
