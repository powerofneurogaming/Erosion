using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//How to access object's SpriteRenderer: https://answers.unity.com/questions/633998/how-to-access-an-objects-spriterenderer.html

//TODO: FIND OUT HOW TO CHECK FOR CHANGE WITHOUT USING UPDATE, CHECK IF GAME IS STARTED!

public class AudioScript : MonoBehaviour
{
    //This value is the volume as set by the PlayerPrefs with the volume float.
    private float volume;
    //Each AudioSource is a separate clip that may need to play at the same time
    public AudioSource GameMusic;

    private int gameState = 0;


    // The GamePlay Area SpriteRenderer has the name of the background.
    public SpriteRenderer GamePlayArea;

    // This string is equal to the name of the background currently active.
    public string bgString;

    public AudioSource[] blockSounds = new AudioSource[3];

    //DEPRECATED, Consolidated into blocksounds, may be deleted if preferred.
    //public AudioSource blockDropSounds;
    //public AudioSource blockGrabSounds;
    //public AudioSource blockDestroySounds;

    //"sounds" is an array of Audioclips.
    public AudioClip[] soundArray;

    // soundID is a sound clip. sourceID is the ID of the audiosource.
    public int soundID;
    public int sourceID;

    // Start is called before the first frame update
    void Start()
    {
        GamePlayArea = GameObject.Find("GamePlay Area").GetComponent<SpriteRenderer>();
        volume = PlayerPrefs.GetFloat("volume", 1);
        // The music is always playing.
        GameMusic = gameObject.AddComponent<AudioSource>();

        GameMusic.clip = Resources.Load("Sounds/Music/title") as AudioClip;
        GameMusic.Play();

        bgString = GamePlayArea.sprite.name;
        // These other sounds play only in specific circumstances.

        blockSounds[0] = gameObject.AddComponent<AudioSource>();
        blockSounds[1] = gameObject.AddComponent<AudioSource>();
        blockSounds[2] = gameObject.AddComponent<AudioSource>();
        blockSounds[0].loop = false;
        blockSounds[1].loop = false;
        blockSounds[2].loop = false;

        soundArray = Resources.LoadAll<AudioClip>("Sounds/Sounds") as AudioClip[];

        for (int i = 0; i < soundArray.Length; i++)
        {
            Debug.Log("Sound " + i + ": " + soundArray[i].name);
        }

        GameMusic.clip = Resources.Load("Sounds/Music/title") as AudioClip;
        GameMusic.loop = true;
        GameMusic.Play();

        //in the array blocksounds, change the audiosource volume to equal the 
        //volume set in PlayerPrefs
        foreach (AudioSource audio in blockSounds)
        {
            audio.volume = volume;
            Debug.Log("Current Volume: " + volume);
        }
        GameMusic.volume = volume;
        AudioListener.volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        //On game start, play chosen background's music. THis code commented out because the code to execute playMusic is now added to GameCOntroller.

        //If gamecontroller.checknextlevel
        /*
        if (GamePlayArea.sprite.name != bgString)
        {
            playMusic();
        }
        */
    }

    // Play the music based on the current background, and make the current string equal to the appropriate string of the current background.
    public void playMusic()
    {
        if (GamePlayArea.sprite.name == "bg1")
        {
            GameMusic.clip = Resources.Load("Sounds/Music/bgm1") as AudioClip;
            GameMusic.Play();
            bgString = GamePlayArea.sprite.name;
        }

        else if (GamePlayArea.sprite.name == "bg2")
        {
            GameMusic.clip = Resources.Load("Sounds/Music/bgm2") as AudioClip;
            GameMusic.Play();
            bgString = GamePlayArea.sprite.name;
        }

        else if (GamePlayArea.sprite.name == "bg3")
        {
            GameMusic.clip = Resources.Load("Sounds/Music/bgm3") as AudioClip;
            GameMusic.Play();
            bgString = GamePlayArea.sprite.name;
        }

    }

    //Play sound based on SoundID
    public void soundPlay(int sourceID, int soundID)
    {
        if (soundID == -1)
        {
            blockSounds[sourceID].Stop();
        }
        else if (blockSounds[sourceID].isPlaying == true)
        {
        }
        else
        {
            blockSounds[sourceID].clip = soundArray[soundID];
            blockSounds[sourceID].Play();
            Debug.Log("sourceID = " + sourceID + ", soundID = " + soundID);
        }
    }

    //Stop the music, and get the current state of gameplay upon hitting the submit button.
    public void setState(int stateSet)
    {
        gameState = stateSet;
        GameMusic.Stop();
        StartCoroutine(winLose(gameState));
    }

    public IEnumerator winLose(int gameState)
    {
        //If gameState == 1, the player made a mistake and lost the game.
        // Else if gameState == 2, the player submitted a correct build wall and won.

        if (gameState == 1)
        {
            GameMusic.clip = Resources.Load("Sounds/Music/GameLose") as AudioClip;
            GameMusic.loop = false;
            GameMusic.Play();
            Debug.Log("The player lost");
        }
        else if (gameState == 2)
        {
            GameMusic.clip = Resources.Load("Sounds/Music/GameWin") as AudioClip;
            GameMusic.loop = false;
            GameMusic.Play();
            Debug.Log("The player won!");
        }
        Debug.Log("Beginning Check");
        yield return new WaitForSeconds(GameMusic.clip.length);
        GameMusic.clip = Resources.Load("Sounds/Music/GameOverMusic") as AudioClip;
        GameMusic.loop = true;
        GameMusic.Play();
        Debug.Log("GameMisoc is " + GameMusic + ".");
    }
}
