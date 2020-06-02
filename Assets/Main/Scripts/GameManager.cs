using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region preset
    [Header("道具")]
    public GameObject[] props;

    [Header("遊玩畫面")]
    public Text textCount;
    public Text textTime;

    [Header("結束畫面")]
    public CanvasGroup final;
    public Text textTitle;

    [Header("操控角色")]
    public Player player;

    /// <summary>
    /// 道具總數
    /// </summary>
    private int countTotal;

    /// <summary>
    /// 取得數量
    /// </summary>
    private int countProp;

    /// <summary>
    /// 遊戲時間
    /// </summary>
    private float gameTime = 500;
    #endregion

    #region method
    /// <summary>
    /// 生成道具
    /// </summary>
    /// <param name="prop">生成道具</param>
    /// <param name="count">生成數量(+RNG)</param>
    /// <returns>傳回生成幾顆</returns>
    private int GenerateProps(GameObject prop, int count)
    {
        int total = count + Random.Range(-5, 5);

        for (int i = 0; i < total; i++)
            Instantiate(prop, new Vector3(Random.Range(-9, 10), Random.Range(1f, 3f), Random.Range(-9, 10)), Quaternion.identity);
        
        return total;
    }

    private void CountTime()
    {
        if (final.alpha == 1) return;

        gameTime -= Time.deltaTime * 5;
        gameTime = Mathf.Clamp(gameTime, 0f, 9999f);

        textTime.text = gameTime.ToString("f0");

        Lose();
    }

    private void CountProp()
    {
        textCount.text = countProp + " /" + countTotal;
    }

    public void Eat(string prop)
    {
        switch (prop)
        {
            case "good":
                countProp++;
                Win();
                break;
            case "bad":
                gameTime -= 10;
                break;
        }
    }

    private void Win()
    {
        if (countProp == countTotal)
        {
            final.alpha = 1;
            final.interactable = true;
            final.blocksRaycasts = true;
            textTitle.text = "所有雞腿都吃光啦!";
            player.Win();
        }
    }

    private void Lose()
    {
        if (gameTime == 0f)
        {
            final.alpha = 1;
            final.interactable = true;
            final.blocksRaycasts = true;
            textTitle.text = "時間到!";
            player.Lose();
        }
            
    }

    public void Retry()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region events
    private void Start()
    {
        countTotal = GenerateProps(props[0], 10);
        GenerateProps(props[1], 10);
    }
    private void Update()
    {
        CountTime();
        CountProp();
    }
    #endregion

}
