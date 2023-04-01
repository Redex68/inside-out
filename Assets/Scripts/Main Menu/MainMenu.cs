using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using TMPro;

using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject fader;
    [Space]
    [SerializeField] Button PlayButton;
    [SerializeField] Button QuizButton;
    [SerializeField] Button SettingsButton;
    [SerializeField] Button CreditsButton;
    [SerializeField] Button ExitButton;
    [SerializeField] Button BackButton;
    [Space]
    [SerializeField] TMP_Text Title;
    [SerializeField] TMP_Text Credits;
    [Space]
    [SerializeField] GameObject Volume; 
    
    public static MainMenu Instance;

    void Awake()
    {
        if(Instance != null) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayButton.onClick.AddListener      (() => StartCoroutine(onPlay()));
        QuizButton.onClick.AddListener      (() => StartCoroutine(onQuiz()));
        SettingsButton.onClick.AddListener  (() => onSettings());
        CreditsButton.onClick.AddListener   (() => onCredits());
        ExitButton.onClick.AddListener      (() => onExit());
        BackButton.onClick.AddListener      (() => onBack());

        Volume.GetComponentInChildren<Slider>().value = AudioListener.volume;
        Volume.GetComponentInChildren<Slider>().onValueChanged.AddListener((call) => AudioListener.volume = call);

        PersistentData pd = Serializer.Load();
        if(pd == null || !pd.storyCompleted) QuizButton.interactable = false;
        
        StartCoroutine(lateStart());
    }

    IEnumerator lateStart()
    {
        yield return new WaitForSeconds(1.0f);

        Fader.Instance.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator onPlay() 
    {
        float fadeIn = 0.0f;
        float fadeTime = 1.0f;

        Fader.Instance.gameObject.SetActive(true);
        while(fadeIn < fadeTime)
        {
            fadeIn += Time.deltaTime;
            Fader.Instance.setTransparency(fadeIn / fadeTime);
            yield return new WaitForEndOfFrame();
        }

        EditorSceneManager.LoadScene("Sandbox");
    }

    IEnumerator onQuiz() 
    {
        float fadeIn = 0.0f;
        float fadeTime = 1.0f;

        Fader.Instance.gameObject.SetActive(true);
        while(fadeIn < fadeTime)
        {
            fadeIn += Time.deltaTime;
            Fader.Instance.setTransparency(fadeIn / fadeTime);
            yield return new WaitForEndOfFrame();
        }

        EditorSceneManager.LoadScene("Quiz");
    }

    void onSettings() 
    {
        Title.text = "Settings";

        PlayButton.gameObject.SetActive(false);
        QuizButton.gameObject.SetActive(false);
        SettingsButton.gameObject.SetActive(false);
        CreditsButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(true);
        Credits.gameObject.SetActive(false);
        Volume.SetActive(true);

    }

    void onCredits() 
    {
        Title.text = "Credits";

        PlayButton.gameObject.SetActive(false);
        QuizButton.gameObject.SetActive(false);
        SettingsButton.gameObject.SetActive(false);
        CreditsButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(true);
        Credits.gameObject.SetActive(true);
        Volume.SetActive(false);

    }

    void onBack() 
    {
        Title.text = "InsideOut";

        PlayButton.gameObject.SetActive(true);
        QuizButton.gameObject.SetActive(true);
        SettingsButton.gameObject.SetActive(true);
        CreditsButton.gameObject.SetActive(true);
        ExitButton.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(false);
        Credits.gameObject.SetActive(false);
        Volume.SetActive(false);

    }

    void onExit() 
    {
        Application.Quit();
    }
}
