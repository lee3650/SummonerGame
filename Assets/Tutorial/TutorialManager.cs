using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TutorialManager : MonoBehaviour
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
        }
    }

    private void MousedOver()
    {
        if (SectionAndSegment.x == 4)
        {
            SectionAndSegment = new Vector2Int(5, 0);
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
            SectionAndSegment = new Vector2Int(2, 0);
        }

        if (SectionAndSegment.x == 2)
        {
            //so, this is the mason part. 
            if (BuiltGeqSummons(SummonType.WallGenerator, 1))
            {
                SectionAndSegment = new Vector2Int(3, 0);
                //since this section is done we need to also 
                //give you the wall blueprint item. 
                GivePlayerItem(wallBlueprintPrefab);
            }
        }

        if (SectionAndSegment.x == 3)
        {
            if (BuiltGeqSummons(SummonType.Wall, 3))
            {
                SectionAndSegment = new Vector2Int(4, 0);
            }
        }

        //4 is handled in MousedOver
        
        if (SectionAndSegment.x == 5)
        {
            if (BuiltGeqSummons(SummonType.Barracks, 1))
            {
                SectionAndSegment = new Vector2Int(6, 0);
                GivePlayerItem(MeleeBlueprint);
            }
        }
    }

    private void BlueprintsChanged()
    {
        if (SectionAndSegment.x == 6)
        {
            if (BlueprintManager.GetBlueprintsOfTypes(new List<BlueprintType>() { BlueprintType.Melee }).Count >= 4)
            {
                SectionAndSegment = new Vector2Int(7, 0);
                GivePlayerItem(GatePrefab);
            }
        }
    }

    private void GivePlayerItem(GameObject item)
    {
        PlayerInventory.TryToPickUpGameobject(Instantiate(item));
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

            if (Input.GetMouseButtonDown(0) && !PlayerAttackState.AttackedThisFrame())
            {
                SectionAndSegment = incrementSegment(SectionAndSegment, tutorialText[SectionAndSegment.x].Length - 1); //okay. So, segment should be click based but section should be event based. 
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
