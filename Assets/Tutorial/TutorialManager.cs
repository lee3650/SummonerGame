﻿using System.Collections;
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
    [SerializeField] GameEndPanel GameEndPanel;
    [SerializeField] Button NextWaveButton;

    const string tutorialFileName = "ttl";

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
            NextLevelEvent.OnNextLevel += OnNextLevel;
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
        if (sectionName == "capacity")
        {
            IncrementSection();
            GivePlayerItem(TrapGenerator);
        }
        else if (sectionName == "level")
        {
            if (CurrentLevelManager.OnLastWave())
            {
                IncrementSection();
                EndTutorial();
            }
        }
    }

    private void MousedOver()
    {
        if (sectionName == "income")
        {
            IncrementSection();
            GivePlayerItem(barracksPrefab);
        }
    }

    private void SummonsChanged()
    {
        print("Summons changed!");
        print("section name: " + sectionName);

        if (sectionName == "home tile")
        {
            IncrementSection();
            NextWaveButton.interactable = false;
        }

        else if (sectionName == "mason")
        {
            //so, this is the mason part. 
            if (BuiltGeqSummons(SummonType.WallGenerator, 1))
            {
                IncrementSection();
                //since this section is done we need to also 
                //give you the wall blueprint item. 
                GivePlayerItem(wallBlueprintPrefab);
            }
        }

        else if (sectionName == "walls")
        {
            if (BuiltGeqSummons(SummonType.Wall, 3))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "barracks")
        {
            if (BuiltGeqSummons(SummonType.Barracks, 1))
            {
                IncrementSection();
                GivePlayerItem(MeleeBlueprint);
            }
        }
        else if (sectionName == "gates")
        {
            if (BuiltGeqSummons(SummonType.Gate, 1))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "fix troops")
        {
            if (BuiltGeqSummons(SummonType.MeleeEntity, 3))
            {
                NextWaveButton.interactable = true;
                IncrementSection();
            }
        }
        else if (sectionName == "armory")
        {
            if (BuiltGeqSummons(SummonType.TrapGenerator, 1))
            {
                IncrementSection();
                GivePlayerItem(ArrowTrap);
            }
        }
        else if (sectionName == "ballista")
        {
            if (BuiltGeqSummons(SummonType.ArrowTrap, 1))
            {
                IncrementSection();
            }
        }
        else if (sectionName == "miner")
        {
            if (BuiltGeqSummons(SummonType.Miner, 1))
            {
                //well, this is duplicated information - it's assuming the home tile counts as a miner
                //I kind of need to separate charm type and 'real' type, right? 

                IncrementSection();
            }
        }
        else if (sectionName == "teardown gate")
        {
            if (BuiltEqSummons(SummonType.Gate, 0))
            {
                IncrementSection();
            }
        }

        else if (sectionName == "teardown melee")
        {
            if (BuiltGeqSummons(SummonType.MeleeEntity, 3))
            {
                IncrementSection();
                GivePlayerItem(Miner);
                PlayerMana.IncreaseMana(MinerSummon.GetCurrentMinerCost());
                MinerCostDisplay.SetActive(true);
            }
        }

        else if (sectionName == "rmelee")
        {
            if (BuiltEqSummons(SummonType.MeleeEntity, 2))
            {
                IncrementSection();
            }
        }

        else if (sectionName == "duplicate")
        {
            if (BuiltGeqSummons(SummonType.MeleeEntity, 4) && BuiltGeqSummons(SummonType.Wall, 4) && BuiltGeqSummons(SummonType.ArrowTrap, 2))
            {
                IncrementSection(); 
                //so, yeah we do have duplication here because each section knows what it has to do to setup the next section
                //what'd be better is if in IncrementSection we check the new section and then have each section give itself stuff 
                //but probably that's not worth it because the tutorial will not change much after this 
                GivePlayerItem(Miner);
                PlayerMana.IncreaseMana(MinerSummon.GetCurrentMinerCost());
                MinerCostDisplay.SetActive(true);
            }
        }
    }

    private void BlueprintsChanged()
    {
        if (sectionName == "troops")
        {
            if (BlueprintManager.GetBlueprintsOfTypes(new List<BlueprintType>() { BlueprintType.Melee }, false).Count >= 4)
            {
                IncrementSection();
                GivePlayerItem(GatePrefab);
            }
        }
    }

    private void GivePlayerItem(GameObject item)
    {
        PlayerInventory.TryToPickUpGameobject(Instantiate(item));
    }

    private void IncrementSection()
    {
        SectionAndSegment = new Vector2Int(SectionAndSegment.x + 1, 1);
    }

    private void OnNextLevel()
    {
        // we can safely assume we're in tutorial mode
        levelCounter++;
        if (levelCounter == 1)
        {
            NextWaveButton.interactable = false;
            //SectionAndSegment = new Vector2Int(1, 1);
            GivePlayerItem(WallGenerator);
        }
    }

    private void EndTutorial()
    {
        MainMenuScript.TutorialFinished();
        EndPanel.SetActive(true);
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
                }
            }

            //well, that's duplication
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2) && !Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.R))
            {
                SectionAndSegment = incrementSegment(SectionAndSegment, GetMaxSegment(SectionAndSegment.x)); //so, segment is under manual control, section is event controlled
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

    public void NextPage()
    {
        SectionAndSegment = incrementSegment(SectionAndSegment, GetMaxSegment(SectionAndSegment.x));
    }

    public void PrevPage()
    {
        SectionAndSegment = decrementSegment(SectionAndSegment);
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

    private Vector2Int decrementSegment(Vector2Int start)
    {
        start -= new Vector2Int(0, 1);
        if (start.y < 1)
        {
            start = new Vector2Int(start.x, 1);
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
