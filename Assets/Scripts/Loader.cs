using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {
	public AudioClip doorSound;
	public GameManager gameManager;
	public SoundManager soundManager;
    public UISettings caUI;

    public GameObject boardManager;

    void Awake () {
		if (GameManager.instance == null)
			Instantiate (gameManager);
		if (SoundManager.instance == null)
			Instantiate (soundManager);
    }

	public void EnterGame(){
		GameManager.instance.level = 0;
		SceneManager.LoadScene ("Game");
    }

	public void ExitGame(){
		Application.Quit ();
	}
}
