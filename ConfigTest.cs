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
        //读取配置文件(StreamingAssets)路径
        configPath = Path.Combine(Application.streamingAssetsPath, "Config.txt");
        if (dic == null)
        {
            dic = new Dictionary<string, Dictionary<string, string>>();
            LoadConfig();
        }
    }

    /// <summary>
    /// 处理所有的数据
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
    /// 处理所有的数据
    /// </summary>
    /// <param name="lines"></param>
    void BuildDic(string[] lines)
    {
        string mainKey = null;//主键
        string subKey = null;//子键
        string subValue = null;//值
        foreach (var item in lines)
        {
            string line = null;
            line = item.Trim();//去除空白行
            if (!string.IsNullOrEmpty(line))
            {
                if (line.StartsWith("["))//取主键
                {
                    mainKey = line.Substring(1, line.IndexOf("]") - 1);
                    dic.Add(mainKey, new Dictionary<string, string>());
                }
                else//取子键
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
