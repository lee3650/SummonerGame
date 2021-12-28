using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System;

public class ResearchManager : MonoBehaviour
{
    [SerializeField] ResearchOption[] ResearchOptions;
    [SerializeField] ResearchPanel ResearchPanel;
    [SerializeField] Research[] AllResearch;

    public int NumOfFirstResearches; //so, when something is unlocked, check if 
                                     //this is equal to GetNumUnlockedResearch()
                                     //if it is, then we show a letter
                                     //a different module constantly checks and shows these then

    private static Research[] staticResearchMap;

    private static ResearchSaveData[] ResearchSaveDatas;

    private Research[] ResearchMap;

    private static Research currentResearch; //we have to save this... hm. Okay. 

    private Research overflow = null;

    const string curresearchpath = "curResearch";
    const string researchdatapath = "progress";

    private float overflowRate = 0.2f;

    public bool Interactable
    {
        get;
        set;
    }

    public event Action<int> FinishedResearch = delegate { };

    public static Research CurrentResearch
    {
        get
        {
            return currentResearch;
        }
    }

    public static void ResetState()
    {
        currentResearch = null;
        staticResearchMap = null;
        ResearchSaveDatas = null;

        File.Delete(Application.persistentDataPath + curresearchpath);
        File.Delete(Application.persistentDataPath + researchdatapath);
    }

    private void Awake()
    {
        Interactable = true;
        overflow = null;

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

        staticResearchMap = ResearchMap;

        TryLoadCurrentResearch();
    }

    public int LastResearchIndex
    {
        get
        {
            int max = -1;

            for (int i = 0; i < ResearchMap.Length; i++)
            {
                max = Mathf.Max(ResearchMap[i].Index, max);
            }

            return max; 
        }
    }

    public int NumberOfUnlockedResearch()
    {
        int count = 0;

        for (int i = 0; i < ResearchMap.Length; i++)
        {
            if (ResearchMap[i].Unlocked)
            {
                count++;
            }
        }

        return count;
    }

    public bool AllResearchUnlocked()
    {
        foreach (Research r in ResearchMap)
        {
            if (!r.Unlocked)
            {
                return false;
            }
        }
        return true; 
    }

    public static List<GameObject> GetUnlockedItems()
    {
        List<GameObject> result = new List<GameObject>();

        if (staticResearchMap == null)
        {
            return result;
        }

        foreach (Research r in staticResearchMap)
        {
            if (r.Unlocked && r.Unlocks.Length != 0)
            {
                result.AddRange(r.Unlocks);
            }
        }

        return result; 
    }

    public static float GetCurrentResearchPercent()
    {
        if (CurrentResearch != null)
        {
            return CurrentResearch.Progress / CurrentResearch.XPReq;
        }
        return 0f;
    }

    public Research GainXP(float amount)
    {
        if (CurrentResearch != null)
        {
            CurrentResearch.Progress += amount;
            
            if (CurrentResearch.Progress >= CurrentResearch.XPReq)
            {
                CurrentResearch.Unlocked = true;
                Research temp = CurrentResearch;
                FinishedResearch(temp.Index);
                SetCurrentResearch(-1);
                return temp;
            }
        } 
        else
        {
            if (overflow == null)
            {
                TryChooseOverflow();
            }

            print("Overflow: " + overflow);

            if (overflow != null)
            {
                overflow.Progress += overflowRate * amount;

                if (overflow.Progress >= overflow.XPReq)
                {
                    if (overflow.PrereqUnlocked)
                    {
                        Research temp = overflow;
                        overflow.Unlocked = true;
                        FinishedResearch(temp.Index);
                        overflow = null;
                        return temp;
                    }
                    else
                    {
                        overflow.Progress -= overflowRate * amount;
                    }
                }
            }
        }

        return null;
    }

    public float GetResearchPercent(int index)
    {
        if (ResearchMap == null)
        {
            return 0f;
        }

        Research r = ResearchMap[index];

        return r.Progress / r.XPReq;
    }

    public static bool ResearchUnlocked (int index)
    {
        if (staticResearchMap == null)
        {
            return false;
        }
        if (index >= staticResearchMap.Length)
        {
            return false; 
        }
        return staticResearchMap[index].Unlocked;
    }

    public Research Overflow
    {
        get
        {
            return overflow;
        }
    }

    private void TryChooseOverflow()
    {
        overflow = null;

        Research[] researches = new Research[ResearchMap.Length];

        for (int i = 0; i < researches.Length; i++)
        {
            researches[i] = ResearchMap[i];
        }

        researches = (Research[])ListRandomSort<Research>.SortListRandomly(researches);

        for (int i = 0; i < researches.Length; i++)
        {
            overflow = researches[i];
            if (!overflow.Unlocked && !ResearchDescendedCurrent(overflow) && IsResearchVisible(overflow))
            {
                return;
            }
        }

        for (int i = 0; i < researches.Length; i++)
        {
            overflow = researches[i];
            if (!overflow.Unlocked && IsResearchVisible(overflow))
            {
                return;
            }
        }

        //otherwise... they're all unlocked.
        overflow = null;
    }

    private bool IsResearchVisible(Research research)
    {
        if (research.Index == LastResearchIndex)
        {
            return NumberOfUnlockedResearch() == LastResearchIndex; 
        }
        if (research.Index >= NumOfFirstResearches)
        {
            return NumberOfUnlockedResearch() >= NumOfFirstResearches;
        }
        return true; 
    }

    private bool ResearchDescendedCurrent(Research research)
    {
        if (CurrentResearch == null)
        {
            return false;
        }

        Research cur = research;

        while (cur != null)
        {
            if (cur.Index == CurrentResearch.Index)
            {
                return true;
            }
            cur = cur.Prereq;
        }

        return false;
    }


    private void TryLoadCurrentResearch()
    {
        if (CurrentResearch != null)
        {
            return;
        }

        string curResearch = Application.persistentDataPath + curresearchpath;

        if (File.Exists(curResearch))
        {
            string research = File.ReadAllText(curResearch);
            print("research string: " + research);
            try
            {
                int r_ind = int.Parse(research.Trim());
                SetCurrentResearch(r_ind);
            }
            catch
            {
                SetCurrentResearch(-1);
            }
        }
    }

    private void LoadResearchSaveDatas()
    {
        string path = Application.persistentDataPath + researchdatapath;

        if (File.Exists(path))
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ResearchSaveData[]));
                using (Stream reader = new FileStream(path, FileMode.Open))
                {
                    ResearchSaveDatas = (ResearchSaveData[])serializer.Deserialize(reader);

                    if (ResearchSaveDatas.Length < AllResearch.Length)
                    {
                        ResearchSaveData[] temp = ResearchSaveDatas;
                        ResearchSaveDatas = new ResearchSaveData[AllResearch.Length];

                        for (int i = 0; i < ResearchSaveDatas.Length; i++)
                        {
                            if (i < temp.Length)
                            {
                                ResearchSaveDatas[i] = temp[i];
                            } else
                            {
                                ResearchSaveDatas[i] = new ResearchSaveData(i);
                            }
                        }
                    }
                }
            }
            catch
            {
                print("Remove this later: resetting save if you can't deserialize");
                InitializeResearchSaveData(); //probably want to remove that
            }
        }
        else
        {
            InitializeResearchSaveData();
        }
    }

    private void InitializeResearchSaveData()
    {
        ResearchSaveDatas = new ResearchSaveData[ResearchOptions.Length];

        for (int i = 0; i < ResearchSaveDatas.Length; i++)
        {
            ResearchSaveDatas[i] = new ResearchSaveData(i);
        }
    }

    public void WriteResearchSaveDatas()
    {
        if (ResearchSaveDatas == null)
        {
            return;
        }

        TextWriter writer = new StreamWriter(Application.persistentDataPath + researchdatapath);
        XmlSerializer serializer = new XmlSerializer(typeof(ResearchSaveData[]));

        serializer.Serialize(writer, ResearchSaveDatas);
        writer.Close();
    }


    public void SaveResearchData()
    {
        WriteResearchSaveDatas();
    }

    public void SetCurrentResearch(int index)
    {
        if (!Interactable)
        {
            return;
        }

        if (index < 0)
        {
            currentResearch = null;
        } else
        {
            currentResearch = ResearchMap[index];
        }

        File.WriteAllText(Application.persistentDataPath + curresearchpath, "" + index);
    }

    public string GetResearchName(int index)
    {
        return ResearchMap[index].name;
    }

    public void ShowResearchPanel(int index)
    {
        if (!Interactable)
        {
            return;
        }

        ResearchPanel.Show(ResearchMap[index], false);
    }
}
