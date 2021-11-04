using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

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
    [SerializeField] GifDisplayer GifDisplayer;
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
    [SerializeField] GameObject NextWaveButton;

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
            SectionAndSegment = new Vector2Int();
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
        //okay well here's one way to do this: we define constant strings (that function as labels - or... we could parse labels). 
        //Then we put them in order in an array/list. Then we call index of for that variable name? We'd have to duplicate the labels if we parsed them
        //but we wouldn't have to update anything else... hm. 
        //Well, okay... if we add more mechanics we may need to do like, a second tutorial or something as well, right? 
        
        if (SectionAndSegment.x == 9)
        {
            IncrementSection();
            GivePlayerItem(TrapGenerator);
        }
        else if (SectionAndSegment.x == 13)
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
        if (SectionAndSegment.x == 4)
        {
            IncrementSection();
            GivePlayerItem(barracksPrefab);
        }
    }

    private void SummonsChanged()
    {
        print("Summons changed!");
        //so, this is unfortunate because there has to be duplication between this and the actual tutorial text
        //and also this is a really disgusting way to do this - I honestly don't see a better way though. 
        //maybe each section could expose it's... own tutorial text somehow? 
        //this is why next time I'm going to do it all at the same time. 

        if (SectionAndSegment.x == 1)
        {
            IncrementSection();
        }

        else if (SectionAndSegment.x == 2)
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
        else if (SectionAndSegment.x == 3)
        {
            if (BuiltGeqSummons(SummonType.Wall, 3))
            {
                IncrementSection();
            }
        }
        else if (SectionAndSegment.x == 5)
        {
            if (BuiltGeqSummons(SummonType.Barracks, 1))
            {
                IncrementSection();
                GivePlayerItem(MeleeBlueprint);
            }
        }
        else if (SectionAndSegment.x == 7)
        {
            if (BuiltGeqSummons(SummonType.Gate, 1))
            {
                IncrementSection();
            }
        }
        else if (SectionAndSegment.x == 8)
        {
            if (BuiltGeqSummons(SummonType.MeleeEntity, 4))
            {
                NextWaveButton.SetActive(true);
                IncrementSection();
            }
        }
        else if (SectionAndSegment.x == 10)
        {
            if (BuiltGeqSummons(SummonType.TrapGenerator, 1))
            {
                IncrementSection();
                GivePlayerItem(ArrowTrap);
            }
        }
        else if (SectionAndSegment.x == 11)
        {
            if (BuiltGeqSummons(SummonType.ArrowTrap, 2))
            {
                IncrementSection();
                GivePlayerItem(Miner);
                PlayerMana.IncreaseMana(MinerSummon.GetCurrentMinerCost());
                MinerCostDisplay.SetActive(true);
            }
        }
        else if (SectionAndSegment.x == 12)
        {
            if (BuiltGeqSummons(SummonType.Miner, 2))
            {
                //well, this is duplicated information - it's assuming the home tile counts as a miner
                //I kind of need to separate charm type and 'real' type, right? 

                IncrementSection();
            }
        }
    }

    private void BlueprintsChanged()
    {
        if (SectionAndSegment.x == 6)
        {
            if (BlueprintManager.GetBlueprintsOfTypes(new List<BlueprintType>() { BlueprintType.Melee }).Count >= 4)
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
        SectionAndSegment = new Vector2Int(SectionAndSegment.x + 1, 0);
    }

    private void OnNextLevel()
    {
        // we can safely assume we're in tutorial mode
        levelCounter++;
        if (levelCounter == 1)
        {
            NextWaveButton.SetActive(false);
            SectionAndSegment = new Vector2Int(1, 0);
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
                ImageSequence gif = GetGifFromSection(SectionAndSegment, GetMaxSegment(SectionAndSegment.x));
                if (gif != null)
                {
                    GifDisplayer.PlayGif(gif);
                }
            }

            //well, that's duplication
            if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(2) && !Input.GetKeyDown(KeyCode.Escape) && !Input.GetKeyDown(KeyCode.R))
            {
                SectionAndSegment = incrementSegment(SectionAndSegment, GetMaxSegment(SectionAndSegment.x)); //so, segment is under manual control, section is event controlled
            }
        }
    }

    private int GetMaxSegment(int section)
    {
        return tutorialText[section].Length - 1;
    }

    private ImageSequence GetGifFromSection(Vector2Int sectionAndSegment, int maxSegment)
    {
        foreach (SegmentToImageSequence seg in Gifs)
        {
            if (seg.SecAndSeg == sectionAndSegment || (seg.SecAndSeg.y == -1 && sectionAndSegment.y == maxSegment && seg.SecAndSeg.x == sectionAndSegment.x)) //eventually we'll want to compare the label
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
        if (start.y < 0)
        {
            start = new Vector2Int(start.x, 0);
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
        return string.Format("({0}/{1})", secAndSeg.y + 1, GetMaxSegment(secAndSeg.x) + 1);
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
        foreach (Summon s in Summoner.GetSummons())
        {
            if (s.GetSummonType() == type)
            {
                num--;
                if (num <= 0)
                {
                    return true;
                }
            }
        }
        return false; 
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
