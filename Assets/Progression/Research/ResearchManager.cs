using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class ResearchManager : MonoBehaviour
{
    [SerializeField] ResearchOption[] ResearchOptions;
    [SerializeField] ResearchPanel ResearchPanel;
    [SerializeField] Research[] AllResearch;

    private static ResearchSaveData[] ResearchSaveDatas;

    private Research[] ResearchMap;


    private static Research CurrentResearch; //we have to save this... hm. Okay. 

    private void Awake()
    {
        foreach (ResearchOption ro in ResearchOptions)
        {
            ro.Init(this);
        }

        ResearchMap = new Research[ResearchOptions.Length];

        foreach (Research r in AllResearch)
        {
            ResearchMap[r.Index] = r;
        }

        if (ResearchSaveDatas == null)
        {
            LoadResearchSaveDatas();
        }

        for (int i = 0; i < ResearchSaveDatas.Length; i++)
        {
            ResearchSaveData save = ResearchSaveDatas[i];
            ResearchMap[save.Index].ResearchSaveData = save;
        }
    }

    private void LoadResearchSaveDatas()
    {
        string path = Application.persistentDataPath + "progress";

        if (File.Exists(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ResearchSaveData[]));
            using (Stream reader = new FileStream(path, FileMode.Open))
            {
                ResearchSaveDatas = (ResearchSaveData[])serializer.Deserialize(reader);
            }
        } else
        {
            ResearchSaveDatas = new ResearchSaveData[ResearchOptions.Length];

            for (int i = 0; i < ResearchSaveDatas.Length; i++)
            {
                ResearchSaveDatas[i] = new ResearchSaveData(i);
            }
        }
    }

    private void WriteResearchSaveDatas()
    {
        throw new System.NotImplementedException();
    }

    public void SetCurrentResearch(int index)
    {
        throw new System.NotImplementedException();
    }

    public void ShowResearchPanel(int index)
    {
        ResearchPanel.Show(ResearchMap[index]);
    }
}
