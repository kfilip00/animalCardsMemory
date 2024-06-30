using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static Action<string> OnRevealed;
    GameObject token;
    public Text infoDisplay;
    public Button easyBtn;
    public Button mediumBtn;
    public Button hardBtn;
    MainToken tokenUp1 = null;
    MainToken tokenUp2 = null;
    List<int> faceIndexes =
        new List<int>{ 0, 1, 2, 3, 0, 1, 2, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11,12,13,14,15,16,17,18,19,20,21,22,23,24,25};
    public static System.Random rnd = new System.Random();
    private int shuffleNum = 0;
    float tokenScale = 4;
    float yStart = 2.5f;
    int numOfTokens = 8;
    float yChange = -5f;
    private int clickCount = 0;
    private int amountOfImagesPerColumn = 4;
    private float xPosBetween = 4;
    private bool isGameActive = true;
    private int timeCounter;
    private int pairsToGuess;
    public static int Difficulty= -1;
    [SerializeField] private GoToNextLevel goToNextLevel;

    void StartGame()
    {
        int startTokenCount = numOfTokens;
        pairsToGuess = startTokenCount/2;
        float xPosition = -6.2f;
        float yPosition = yStart;
        int row = 1;
        // The camera orthographicSize is 1/2 the height of the window
        float ortho = Camera.main.orthographicSize / 2.0f;
        for (int i = 1; i < startTokenCount + 1; i++)
        {
            shuffleNum = rnd.Next(0, (numOfTokens));
            var temp = Instantiate(token, new Vector3(
                xPosition, yPosition, 0),
                Quaternion.identity);
            temp.GetComponent<MainToken>().faceIndex = faceIndexes[shuffleNum];
            temp.transform.localScale = 
                new Vector3(ortho / tokenScale, ortho / tokenScale, 0);
            faceIndexes.Remove(faceIndexes[shuffleNum]);
            numOfTokens--;
            xPosition = xPosition + xPosBetween;
            if (i % amountOfImagesPerColumn < 1)
            {
                yPosition = yPosition + yChange;
                xPosition = -6.2f;
                row++;
            }
        }
        token.SetActive(false);
        StartCoroutine(CounterRoutine());
    }

    public void TokenDown(MainToken tempToken)
    {
        if (tokenUp1 == tempToken)
        {
            tokenUp1 = null;
        }
        else if (tokenUp2 == tempToken)
        {
            tokenUp2 = null;
        }
    }

    public bool TokenUp(MainToken tempToken)
    {
        bool flipCard = true;
        if (tokenUp1 == null)
        {
            tokenUp1 = tempToken;
        }
        else if (tokenUp2 == null)
        {
            tokenUp2 = tempToken;
        }
        else
        {
            flipCard = false;
        }
        return flipCard;
    }

    public void CheckTokens()
    {
        clickCount++;
        ShowInfo();
        if (tokenUp1 != null && tokenUp2 != null &&
            tokenUp1.faceIndex == tokenUp2.faceIndex)
        {
            OnRevealed?.Invoke(tokenUp1.GetComponent<SpriteRenderer>().sprite.name);
            tokenUp1.matched = true;
            tokenUp2.matched = true;
            tokenUp1 = null;
            tokenUp2 = null;
            pairsToGuess--;
        }
        else
        {
            AudioManager.Instance.PlayCardReveal();
        }

        if (pairsToGuess==0)
        {
            isGameActive = false;
            goToNextLevel.gameObject.SetActive(true);
        }
    }

    public void HardSetup()
    {
        Difficulty = 2;
        HideButtons();
        tokenScale = 12;
        yStart = 3.5f;
        numOfTokens = 36;
        yChange = -1.5f;
        amountOfImagesPerColumn = 6;
        xPosBetween = 2.5f;
        StartGame();
    }

    public void MediumSetup()
    {
        Difficulty = 1;
        HideButtons();
        tokenScale = 8;
        yStart = 3.1f;
        numOfTokens = 16;
        yChange = -2.2f;
        amountOfImagesPerColumn = 4;
        xPosBetween = 4;
        StartGame();
    }

    public void EasySetup()
    {
        Difficulty = 0;
        amountOfImagesPerColumn = 4;
        xPosBetween = 4;
        yStart = 2.2f;
        HideButtons();
        StartGame();
    }

    private void HideButtons()
    {
        easyBtn.gameObject.SetActive(false);
        mediumBtn.gameObject.SetActive(false);
        hardBtn.gameObject.SetActive(false);
        GameObject[] startImages = 
            GameObject.FindGameObjectsWithTag("startImage");
        foreach (GameObject item in startImages)
            Destroy(item);
    }

    private void Awake()
    {
        token = GameObject.Find("Token");
    }

    void OnEnable()
    {
        easyBtn.onClick.AddListener(() => EasySetup());
        mediumBtn.onClick.AddListener(() => MediumSetup());
        hardBtn.onClick.AddListener(() => HardSetup());
    }

    private IEnumerator CounterRoutine()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(1);
            ShowInfo();
            timeCounter++;
        }
    }

    private void ShowInfo()
    {
        infoDisplay.text = $"Clicks: {clickCount.ToString()}, Time: {timeCounter}s";
    }

    private void Start()
    {
        switch (Difficulty)
        {
            case 0: EasySetup();
                return;
            case 1: MediumSetup();
                return;
            case 2: HardSetup();
                return;
        }
    }
}
