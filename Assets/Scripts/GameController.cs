using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameController : SingletonMonoBehaviour<GameController>
{

    public int currentLevel;
    public float waitTime;
    public int rocksDestroyed;
    public int rocksInLevel;

    public Coroutine levelCoroutine;

    public GameObject Rocks;
    public GameObject rock1;
    public GameObject rock2;
    public GameObject rock3;
    public GameObject rock4;
    public GameObject rock5;
    public GameObject spike;
    public int spikeCount;

    public SpriteRenderer background;
    public Sprite[] backgroundSprites;
    public GameObject player;
    public GameObject currentPlayer;
    public GameObject[] playerList;

    //public List<GameObject[]> LevelList = new List<GameObject[]>();
    [SerializeField ]public List<RockStats>[] LevelList= new List<RockStats>[40];
    public GameObject[] currentLevelRocks;
    public GameObject[] spawnPositions;

    public Vector2 playerSpawnPos = new Vector2(0f, -3.693279f);
    public UIView nextLevelView;
    public UIView gameOverView;
    public TMP_InputField levelInput;
    public Slider progressBar;

    //Nathan created this AudioController script;
    public AudioScript AudioScript;

    public override void Start()
    {
        base.Start();

        currentLevel = PlayerPrefs.GetInt("levelNumber", 0);
        FillLevels();
        MakeNewPlayer();

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DropSpikes(.5f, 4));

    }
    int levelDifficulty;
    public void FillLevels()
    {
        int DefaultHp = 100;

        for (int i = 0; i < 40; i++)
        {
            levelDifficulty = 51+20*i;
            int counter = 0;
            LevelList[i] = new List<RockStats>();
            string printstr = "filling level "+i;
            while (levelDifficulty > 50)
            {
                int old = levelDifficulty;
                RockStats temp = GenerateRock(Rocks, levelDifficulty, i);
                Debug.Log(temp.inihp + "\t" + temp.SplitCount);
                LevelList[i].Add(temp);
                //Debug.Log(LevelList[i][LevelList[i].Count - 1].inihp + "\t" + LevelList[i][LevelList[i].Count - 1].SplitCount);
                printstr += "\t " + (old - levelDifficulty);
                counter++;
                //Debug.Log(counter);
                if (counter > 50)
                {
                    break;
                }
            }
            Debug.Log(printstr);

        }

    }
    int[] score50 = new int[]{ 50, 75, 112};
    int[] score100 = new int[] { 100, 160, 230 };
    int[] score150 = new int[] { 150, 225, 337 };
    List<int[]> scores = new List<int[]>();
    
    /// <summary>
    /// create RockStats class with object to fill filled in
    /// </summary>
    /// <param name="rock"></param>
    /// <param name="difficulty"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    RockStats GenerateRock(GameObject rock, int difficulty, int level)
    {
        scores.Clear();
        scores.Add(score50); scores.Add(score100); scores.Add(score150);

        int splitCount = 0;
        if (level > 30) { splitCount = 2; }
        else if (level > 10) { splitCount = 1; }

        int[] RockLife = { 50, 100, 150 };
        int hp;

        if (level <= 5) { hp = 1; }
            else if(level <= 15) { hp = 2; }
            else { hp = 3; }

        int currMax = 0;
        int rockIndex = 0;
        int splitIndex = 0;
        //Debug.Log("curr diff: " + difficulty);
        List<int> validScores = new List<int>();
        for(int i = 0; i < hp; i++) //go through each array
        {
            for(int j = 0; j <= splitCount; j++) //go through each element up to split count
            {
                //Debug.Log("testing: " + scores[i][j].ToString());
                if (scores[i][j] < difficulty && scores[i][j]>currMax)
                {
                    currMax = scores[i][j];
                    //if (j == 2) { Debug.Log("reached here"); }
                    rockIndex = i;
                    splitIndex = j;
                }
            }
        }
        //Debug.Log(currMax+"\t"+RockLife[rockIndex]+"\t"+splitIndex);
        //rock.GetComponent<RockRefactored>().iniLife = RockLife[rockIndex];
        //rock.GetComponent<RockRefactored>().splitCount = splitIndex;
        //rock.GetComponent<RockRefactored>().waitTime = 5 * Mathf.Pow(1.5f, splitIndex);
        RockStats stats = new RockStats
        {
            obj = rock,
            inihp = RockLife[rockIndex],
            SplitCount = splitIndex,
            waitTimer = 5 * Mathf.Pow(1.5f, splitIndex)
        };

        levelDifficulty -= currMax;
        return stats;
    }

    public void SpawnRock(RockStats rock)
    {
        int temp = Random.Range(0, 6);
        Debug.Log(temp);
        Debug.Log(spawnPositions[temp].transform.position);
        var obj=Instantiate(rock.obj, spawnPositions[temp].transform.position, Quaternion.identity);
        Debug.Log("spawning with stats: " + rock.inihp + "\t" + rock.SplitCount);
        obj.GetComponent<RockRefactored>().iniLife = rock.inihp;
        obj.GetComponent<RockRefactored>().life = rock.inihp;
        obj.GetComponent<RockRefactored>().speed = 4;
        obj.GetComponent<RockRefactored>().splitCount = rock.SplitCount;
    }
    public IEnumerator DropSpikes(float dropFeq, int numSpikes)
    {
        while (spikeCount < numSpikes)
        {
            float xPos = Random.Range(-15, 16);
            Instantiate(spike, new Vector3(xPos, 5.5f, 0f), Quaternion.identity);
            spikeCount++;

            yield return new WaitForSeconds(dropFeq);
        }
        yield return new WaitForSeconds(8f);
    }
    public void ProgressBarMove()
    {
        progressBar.value = (float)rocksDestroyed / (float)rocksInLevel;
        Debug.Log("the value is: " + progressBar.value);
    }

    public void NextLevelCheck()
    {
        Debug.Log("rock destoryed");
        rocksDestroyed++;
        //ProgressBarMove();
        if (rocksDestroyed == rocksInLevel)
        {
            player.SetActive(false);
            currentLevel++;
            rocksInLevel = 0;
            rocksDestroyed = 0;
            //progressBar.value = 0;
            StopCoroutine(RunLevel(currentLevel));
            nextLevelView.Show();
        }

    }
    public void ChangBackground(int num)
    {
        background.sprite = backgroundSprites[num];
    }

    public void GameOver()
    {
        // currentLevel = 0;
        StopCoroutine(levelCoroutine);
        rocksInLevel = 0;
        rocksDestroyed = 0;
        //progressBar.value = 0;
        //progressBar.gameObject.SetActive(false);
        gameOverView.Show();
    }
    private GameObject[] eraseRocks;
    private GameObject[] eraseRocksText;

    public void EraseRocks()
    {
        eraseRocks = GameObject.FindGameObjectsWithTag("rock");
        eraseRocksText = GameObject.FindGameObjectsWithTag("life text");

        for (int i = 0; i< eraseRocks.Length; i++)
        {
            Destroy(eraseRocks[i]);
        }
        for (int i = 0; i < eraseRocksText.Length; i++)
        {
            Destroy(eraseRocksText[i]);
        }
    }


    public float GetWaitTimeRegular(GameObject rock)
    {
        if (GameObject.ReferenceEquals(rock, rock1))
        {
            waitTime = 15f;
        }
        if (GameObject.ReferenceEquals(rock, rock2))
        {
            waitTime = 4f;
        }
        if (GameObject.ReferenceEquals(rock, rock3))
        {
            waitTime = 8f;
        }
        if (GameObject.ReferenceEquals(rock, rock4))
        {
            waitTime = 15f;
        }
        if (GameObject.ReferenceEquals(rock, rock5))
        {
            waitTime = 8f;
        }

        return waitTime;

    }

    public int CalculateRockTotal(List<RockStats> list)
    {
        int rocksInLevel = 0;
        foreach (RockStats stat in list)
        {
            if (stat.SplitCount == 0) { rocksInLevel++; }
            else if(stat.SplitCount == 1) { rocksInLevel += 3; }
            else if(stat.SplitCount == 2) { rocksInLevel += 7; }
        }
        //for (int i = 0; i< currentLevelRocks.Length; i++)
        //{
        //    if (GameObject.ReferenceEquals(rocks[i], rock1))
        //    {
        //        rocksInLevel += 1;
        //    }
        //    if (GameObject.ReferenceEquals(rocks[i], rock2))
        //    {
        //        rocksInLevel += 1;
        //    }
        //    if (GameObject.ReferenceEquals(rocks[i], rock3))
        //    {
        //        rocksInLevel += 1;
        //    }
        //    if (GameObject.ReferenceEquals(rocks[i], rock4))
        //    {
        //        rocksInLevel += 3;
        //    }
        //    if (GameObject.ReferenceEquals(rocks[i], rock5))
        //    {
        //        rocksInLevel += 2;
        //    }
        //}
        return rocksInLevel;
    }

    public void StartLevel()
    {
        levelCoroutine =  StartCoroutine(RunLevel(currentLevel));
    }

    public void StartGame()
    {
        int temp = int.Parse(levelInput.text);
        if (temp <= 0)
        {
            levelCoroutine = StartCoroutine(RunLevel(0));
        }
        else
        {
            currentLevel = temp - 1;
            levelCoroutine = StartCoroutine(RunLevel(currentLevel));
        }
        //Nathan's code to activate the music set in the AudioController script;
        AudioScript = GameObject.Find("AudioController").GetComponent<AudioScript>();
        AudioScript.playMusic();

    }

    public void MakeNewPlayer()
    {
        player = Instantiate(player, playerSpawnPos, Quaternion.identity);
        player.gameObject.GetComponent<Animator>().enabled = false;

        //player.SetActive(false);
    }
    public void ChangePlayer(int num)
    {
        Destroy(player);
        player = Instantiate(playerList[num], playerSpawnPos, Quaternion.identity);
        player.gameObject.GetComponent<Animator>().enabled = false;
        //player.SetActive(false);

    }
    public int currHP;
    public int currSplitct;
    IEnumerator RunLevel(int level)
    {
        Debug.Log(level);
        yield return new WaitForSeconds(.5f);
        //progressBar.gameObject.SetActive(true);

        player.SetActive(true);
        player.gameObject.GetComponent<Animator>().enabled = true;
        PlayerController.fireReady = true;
        //convert the RockStats to CurrentLevelRocks
        //currentLevelRocks = LevelList[level].ToArray();
        rocksInLevel = CalculateRockTotal(LevelList[level]);

        for (int i = 0; i < LevelList[level].Count; i++)
        {
            currHP = LevelList[level][i].inihp;
            currSplitct = LevelList[level][i].SplitCount;
            SpawnRock(LevelList[level][i]); //spawn the gameobject from stats class
            //GetWaitTimeRegular(currentLevelRocks[i]);
            
            
            waitTime = LevelList[level][i].waitTimer;
            yield return new WaitForSeconds(waitTime);


        }
        yield return null;
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void QuitScene()
    {
        Application.Quit();
    }
}

public class RockStats
{
    public GameObject obj;
    public int inihp=0;
    public int SplitCount = 0;
    public float waitTimer = 0f;
}

