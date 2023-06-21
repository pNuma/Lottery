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

    //���O���X�g�����
    void exGen(string ex)
    {
        int i = 0;
        string tmp="";
        while (true)
        {
            //���O����l��������Ώ������I��
            if (ex.Length == 0)
            {
                if (exList.Count == 0) exList.Add(-1);
                break;
            }

            //�J���}��؂�𔻒�
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

    //�o��������l�̃��X�g���쐬
    void listGen(int min, int max, List<int> exList)
    {

        for (int i = min; i <= max; i++)
        {
            //���O���X�g�ɒl(i)������Ώ������΂�
            if (exList.Contains(i)) continue;
            else list.Add(i);
        }

        //���X�g���V���b�t��
        list = list.OrderBy(a => Guid.NewGuid()).ToList();
    }

    void result()
    {
        //���X�g��1�Ԗڂ����ʂƂ��ďo��
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
