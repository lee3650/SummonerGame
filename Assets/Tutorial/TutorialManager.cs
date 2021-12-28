using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour, IWaveNotifier
{
    [SerializeField] StringDisplayPanel TutorialDisplayPanel;
    [SerializeField] PanelDisplayer TutorialPanelDisplayer;
    [SerializeField] NextLevelEvent NextLevelEvent;
    [SerializeField] Summoner Summoner;
    [SerializeField] BlueprintManager BlueprintManager;
    [SerializeField] Tooltip IncomeTooltip;
    [SerializeField] PlayerAttackState PlayerAttackState;
    [SerializeField] GameObject wallBlueprintPrefab;
    [SerializeField] PlayerInventory PlayerInventory;
    [SerializeField] GameObject barracksPrefab;
    [SerializeField] GameObject GatePrefab;
    [SerializeField] GameObject MeleeBlueprint;
    [SerializeField] SegmentToImageSequence[] Gifs;
    [SerializeField] VideoPlayer GifDisplayer;
    [SerializeField] GameObject TrapGenerator;
    [SerializeField] GameObject ArrowTrap;
    [SerializeField] GameObject Miner;
    [SerializeField] ManaManager PlayerMana;
    [SerializeField] GameObject WallGenerator;
    [SerializeField] CurrentLevelManager CurrentLevelManager;
    [SerializeField] ResetManager ResetManager;
    [SerializeField] GameObject MinerCostDisplay;
    [SerializeField] GameObject EndPanel;
    [SerializeField] GameObject IncomeAndBalanceIndicator;
    [SerializeField] GameEndPanel GameEndPanel;
    [SerializeField] Button NextWaveButton;

    const string tutorialFileName = "ttl";

    const int minSeg = 1; //the minimum segment is 1 because the 0th is the title of the section 

    string[][] tutorialText;
    int levelCounter = 0;

    private Vector2Int sectionAndSegment;

    private bool changeText = false;

    private void Start()
    {
        if (MainMenuScript.TutorialMode)
        {
            //convention: use \n to separate each section and use / to separate parts within a section - a segment
            string fileContents = File.ReadAllText(MainMenuScript.appendPath + tutorialFileName);
            tutorialText = ParseFileContents(fileContents);
            SectionAndSegment = new Vector2Int(0, 1);
            Summoner.SummonsChanged += SummonsChanged;
            IncomeTooltip.MousedOver += MousedOver;
            BlueprintManager.BlueprintsChanged += BlueprintsChanged;
            WaveSpawner.NotifyWhenWaveEnds(this);
            MinerCostDisplay.SetActive(false);
            GameEndPanel.SetExitMessage("Present Day");
        }
    }

    public void OnWaveEnds()
    {
        if (sectionName == "Next wave")
        {
            IncrementSection();
        }
        if (CurrentLevelManager.OnLastWave())
        {
            MainMenuScript.TutorialFinished();
            EndPanel.SetActive(true);
        }
    }

    private void MousedOver()
    {
        if (sectionName == "Mouseover")
        {
            IncrementSection();
        }
    }

    private void SummonsChanged()
    {
        print("Summons changed!");
        print("section name: " + sectionName);

        if (sectionName == "Intro")
        {
            IncrementSection();
        }
        else if (sectionName == "Masonry")
        {
            //so, this is the mason part. 
            if (BuiltGeqSummons(SummonType.WallGenerator, 1))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "Units")
        {
            if (BuiltGeqSummons(SummonType.Wall, 6))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "Barracks")
        {
            if (BuiltGeqSummons(SummonType.Barracks, 1))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "Melees")
        {
            if (BuiltGeqSummons(SummonType.MeleeEntity, 2))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "Armory")
        {
            if (BuiltGeqSummons(SummonType.TrapGenerator, 1))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "Ballista")
        {
            if (BuiltGeqSummons(SummonType.ArrowTrap, 1))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "Path")
        {
            if (BuiltGeqSummons(SummonType.Gate, 1))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "Miner")
        {
            if (BuiltGeqSummons(SummonType.Miner, 1))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "Duplication")
        {
            if (BuiltGeqSummons(SummonType.MeleeEntity, 4) && BuiltGeqSummons(SummonType.Wall, 7) && BuiltGeqSummons(SummonType.ArrowTrap, 2))
            {
                IncrementSection(); 
            }
        }
    }

    private void BlueprintsChanged()
    {
        if (sectionName == "Troubleshooting")
        {
            if (BlueprintManager.GetBlueprintsOfTypes(new List<BlueprintType> { BlueprintType.Melee}, false).Count >= 3)
            {
                IncrementSection();
            }
        }
    }

    private void GivePlayerItem(GameObject item)
    {
        PlayerInventory.TryToPickUpGameobject(Instantiate(item));
    }

    private void IncrementSection()
    {
        ExitSection();
        SectionAndSegment = new Vector2Int(SectionAndSegment.x + 1, 1);
        EnterSection();
    }

    private void ExitSection()
    {
        switch (sectionName)
        {
            case "Mouseover":
                IncomeAndBalanceIndicator.gameObject.SetActive(false);
                break;
        }
    }

    private void EnterSection()
    {
        print("Entered " + sectionName);

        switch (sectionName)
        {
            case "Intro":
                NextWaveButton.interactable = false;
                break;
            case "Masonry":
                GivePlayerItem(WallGenerator);
                break;
            case "Units":
                GivePlayerItem(wallBlueprintPrefab);
                break;
            case "Mouseover":
                IncomeAndBalanceIndicator.gameObject.SetActive(true);
                break;
            case "Barracks":
                GivePlayerItem(barracksPrefab);
                break;
            case "Melees":
                GivePlayerItem(MeleeBlueprint);
                break;
            case "Armory":
                GivePlayerItem(TrapGenerator);
                break;
            case "Ballista":
                GivePlayerItem(ArrowTrap);
                break;
            case "Next wave":
                NextWaveButton.interactable = true;
                break;
            case "Troubleshooting":
                NextWaveButton.interactable = false;
                break;
            case "Path":
                GivePlayerItem(GatePrefab);
                break;
            case "Duplication":
                break;
            case "Miner":
                MinerCostDisplay.SetActive(true);
                PlayerMana.IncreaseMana(20);
                GivePlayerItem(Miner);
                break;
            case "End":
                NextWaveButton.interactable = true;
                break;
        }
    }

    private void Update()
    {
        if (MainMenuScript.TutorialMode)
        {
            //I guess I use left click to increment the tutorial?
            if (changeText)
            {
                changeText = false;
                ShowTutorialText(SectionAndSegment);
                VideoClip gif = GetGifFromSection(GetMaxSegment(SectionAndSegment.x));
                if (gif != null)
                {
                    GifDisplayer.gameObject.SetActive(true);
                    GifDisplayer.clip = gif;
                    GifDisplayer.Play();
                } else
                {
                    GifDisplayer.gameObject.SetActive(false);
                }
            }

            //well, that's duplication
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || (Input.mouseScrollDelta.y > 0 && !Input.GetKey(KeyCode.LeftShift)))
            {
                SectionAndSegment = incrementSegment(SectionAndSegment, GetMaxSegment(SectionAndSegment.x)); //so, segment is under manual control, section is event controlled
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || (Input.mouseScrollDelta.y < 0 && !Input.GetKey(KeyCode.LeftShift)))
            {
                SectionAndSegment = decrementSegment(SectionAndSegment, minSeg); //so, segment is under manual control, section is event controlled
            }
        }
    }

    private string sectionName
    {
        get
        {
            return tutorialText[SectionAndSegment.x][0].Trim();
        }
    }

    private int GetMaxSegment(int section)
    {
        return tutorialText[section].Length - 1;
    }

    private VideoClip GetGifFromSection(int maxSegment)
    {
        foreach (SegmentToImageSequence seg in Gifs)
        {
            if ((sectionAndSegment.y == maxSegment && seg.label == sectionName)) //eventually we'll want to compare the label
            {
                return seg.Gif;
            }
        }
        return null;
    }

    public void EndTutorialIfActive()
    {
        if (MainMenuScript.TutorialMode)
        {
            MainMenuScript.TutorialFinished();
        }
    }

    public void NextPage()
    {
        SectionAndSegment = incrementSegment(SectionAndSegment, GetMaxSegment(SectionAndSegment.x));
    }

    public void PrevPage()
    {
        SectionAndSegment = decrementSegment(SectionAndSegment, minSeg);
    }

    private Vector2Int incrementSegment(Vector2Int start, int max)
    {
        start += new Vector2Int(0, 1);
        if (start.y > max)
        {
            start = new Vector2Int(start.x, max);
        }
        return start;
    }

    private Vector2Int decrementSegment(Vector2Int start, int min)
    {
        start -= new Vector2Int(0, 1);
        if (start.y < min)
        {
            start = new Vector2Int(start.x, min);
        }
        return start;
    }

    private void ShowTutorialText(Vector2Int secAndSeg)
    {
        TutorialPanelDisplayer.HideAllPanels();
        UIPanel p = TutorialPanelDisplayer.ShowPanel(TutorialDisplayPanel, tutorialText[secAndSeg.x][secAndSeg.y].Trim());
        p.GetComponent<TutorialPanel>().Initialize(this); //not super efficient to destroy and then recreate this every time lol
    }

    public string GetProgressText()
    {
        return GetCompletionText(SectionAndSegment);
    }
    
    private string GetCompletionText(Vector2Int secAndSeg)
    {
        return string.Format("({0}/{1})", secAndSeg.y, GetMaxSegment(secAndSeg.x));
    }

    private string[][] ParseFileContents(string text)
    {
        string[] sections = text.Split('\n');

        string[][] result = new string[sections.Length][];

        for (int i = 0; i < sections.Length; i++)
        {
            result[i] = sections[i].Split('/');
        }

        return result;
    }

    private bool BuiltGeqSummons(SummonType type, int num)
    {
        int total = GetNumberOfSummons(type);
        return total >= num;
    }

    private int GetNumberOfSummons(SummonType type)
    {
        int num = 0;

        foreach (Summon s in Summoner.GetSummons())
        {
            if (s.GetSummonType() == type)
            {
                num++;
            }
        }

        return num;
    }

    private bool BuiltEqSummons(SummonType type, int num)
    {
        int total = GetNumberOfSummons(type);
        return total == num;
    }

    private Vector2Int SectionAndSegment
    {
        get
        {
            return sectionAndSegment;
        }
        set
        {
            sectionAndSegment = value;
            changeText = true;
        }
    }
}
