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
    private float gameTime = 300;
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
            Instantiate(prop, new Vector3(Random.Range(-9, 10), 1.5f, Random.Range(-9, 10)), Quaternion.identity);
        
        return total;
    }

    private void CountTime()
    {
        gameTime -= Time.deltaTime * 5;
        textTime.text = gameTime.ToString("f0");
    }

    #endregion

    #region events
    private void Start()
    {
        countTotal = GenerateProps(props[0], 20);
    }
    private void Update()
    {
        CountTime();
    }
    #endregion

}
