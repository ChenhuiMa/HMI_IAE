using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ConfigTest : MonoBehaviour
{
    private string configPath;

    public Dictionary<string, Dictionary<string, string>> dic;

    // Use this for initialization
    void Awake()
    {
        //��ȡ�����ļ�(StreamingAssets)·��
        configPath = Path.Combine(Application.streamingAssetsPath, "Config.txt");
        if (dic == null)
        {
            dic = new Dictionary<string, Dictionary<string, string>>();
            LoadConfig();
        }
    }

    /// <summary>
    /// �������е�����
    /// </summary>
    private void LoadConfig()
    {
        string[] lines = null;
        if (File.Exists(configPath))
        {
            lines = File.ReadAllLines(configPath);
            BuildDic(lines);
        }
    }

    /// <summary>
    /// �������е�����
    /// </summary>
    /// <param name="lines"></param>
    void BuildDic(string[] lines)
    {
        string mainKey = null;//����
        string subKey = null;//�Ӽ�
        string subValue = null;//ֵ
        foreach (var item in lines)
        {
            string line = null;
            line = item.Trim();//ȥ���հ���
            if (!string.IsNullOrEmpty(line))
            {
                if (line.StartsWith("["))//ȡ����
                {
                    mainKey = line.Substring(1, line.IndexOf("]") - 1);
                    dic.Add(mainKey, new Dictionary<string, string>());
                }
                else//ȡ�Ӽ�
                {
                    var configValue = line.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    subKey = configValue[0].Trim();
                    subValue = configValue[1].Trim();
                    subValue = subValue.StartsWith("\"") ? subValue.Substring(1) : subValue;
                    dic[mainKey].Add(subKey, subValue);
                }
            }
        }
    }
}
