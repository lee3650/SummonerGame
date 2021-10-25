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
            string fileContents = File.ReadAllText(tutorialFileName);
            tutorialText = ParseFileContents(fileContents);
            NextLevelEvent.OnNextLevel += OnNextLevel;
            SectionAndSegment = new Vector2Int();
            Summoner.SummonsChanged += SummonsChanged;
            IncomeTooltip.MousedOver += MousedOver;
            BlueprintManager.BlueprintsChanged += BlueprintsChanged;
            WaveSpawner.NotifyWhenWaveEnds(this);
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
            SectionAndSegment = new Vector2Int(1, 0);
        } else
        {
            //finish tutorial
            EndTutorial();
        }
    }

    private void EndTutorial()
    {
        //we'll have to kick you out somehow - change scenes, I guess. 
        //so, we have to get you to level 2, actually - you need to get your default stuff (level 1) and a letter (level 2). 
        //Then we'll be kind of 'setup' for the standard progression

        MainMenuScript.TutorialFinished();
        ExperienceManager.GainXP(ExperienceManager.FirstTwoLevelXP);
        SceneManager.LoadScene("ProgressionMenu");
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
                ImageSequence gif = GetGifFromSection(SectionAndSegment);
                if (gif != null)
                {
                    GifDisplayer.PlayGif(gif);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                SectionAndSegment = incrementSegment(SectionAndSegment, tutorialText[SectionAndSegment.x].Length - 1); //so, segment is under manual control, section is event controlled
            } else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                SectionAndSegment = decrementSegment(SectionAndSegment, tutorialText[SectionAndSegment.x].Length - 1);
            }
        }
    }

    private ImageSequence GetGifFromSection(Vector2Int sectionAndSegment)
    {
        foreach (SegmentToImageSequence seg in Gifs)
        {
            if (seg.SecAndSeg == sectionAndSegment)
            {
                return seg.Gif;
            }
        }
        return null;
    }

    private Vector2Int incrementSegment(Vector2Int start, int max)
    {
        start += new Vector2Int(0, 1);
        if (start.y > max)
        {
            start = new Vector2Int(start.x, 0);
        }
        return start;
    }

    private Vector2Int decrementSegment(Vector2Int start, int max)
    {
        start -= new Vector2Int(0, 1);
        if (start.y < 0)
        {
            start = new Vector2Int(start.x, max);
        }
        return start;
    }

    private void ShowTutorialText(Vector2Int secAndSeg)
    {
        TutorialPanelDisplayer.HideAllPanels();
        TutorialPanelDisplayer.ShowPanel(TutorialDisplayPanel, tutorialText[secAndSeg.x][secAndSeg.y]);
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
