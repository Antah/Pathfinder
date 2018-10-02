using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public float levelStartDelay = 2f;
	public static GameManager instance = null;
    public LevelGenerator boardScript;
    public AlgortihmType algortihmType;
    public Pathfinder pathfinderScript, previousScript;
	public int level;
    public bool settingsFocus = false;

    private Text levelText;
	private GameObject levelImage;
	private bool doingSetup;
	public Player player;
    public GameObject camera;

    void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

        boardScript = GetComponent<LevelGenerator>();

        UpdateAlgorithms();

        SceneManager.sceneLoaded += delegate (Scene scene, LoadSceneMode mode)
		{
            level++;
			InitGame();
		};
	}

    private void UpdateAlgorithms()
    {
        previousScript = pathfinderScript;
        if (algortihmType == AlgortihmType.AStar)
            pathfinderScript = GetComponent<AStar>();
        else
            pathfinderScript = GetComponent<Dijkstra>();
    }

    void InitGame(){
        UpdateAlgorithms();
        previousScript.ClearPath();
        camera = GameObject.Find("Main Camera");
        if(camera != null && camera.GetComponent<CameraController>() != null)
            camera.GetComponent<CameraController>().ResetPosition();

        player = GameObject.Find ("Player").GetComponent<Player> ();
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Level " + level;
		levelImage.SetActive(true);

        boardScript.SetupScene (level);
        pathfinderScript.CreatePath(boardScript.map);

        Invoke ("HideLevelImage", levelStartDelay);    
	}

	private void HideLevelImage(){
		levelImage.SetActive (false);
    }

    public void SetAlgorithm(AlgortihmType type)
    {
        algortihmType = type;
    }

	public void ExitToMenu(){
        SceneManager.LoadScene("MainMenu");
	}

    public void NextLevel(){
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

	public Player GetPlayer(){
		return player;
	}

    public void ToggleSettingsFocus()
    {
        if (instance.settingsFocus == false)
            instance.settingsFocus = true;
        else
            instance.settingsFocus = false;
    }

    public void NewStartAndFinish()
    {
        UpdateAlgorithms();
        previousScript.ClearPath();
        pathfinderScript.CreatePath(boardScript.map);
    }
}
