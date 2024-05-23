using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Game : MonoBehaviour
{

    public void OnClick()
    {
       
    }

    private Dictionary<int, int> bonusCome; //Словарь для хранения значений пассивного дохода
    //Clicker
    [SerializeField] double Score;
    public Text ScoreText;
    private float ClickScore = 1;
    public int[] PassiveAmountBuild;
    public int[] ClickScores; // array to store ClickScore for each item
    


    //Shop and upgrades
    public GameObject ToolsPan;
    public GameObject BuildPan;
    public float[] CostInt;
    public Text[] CostText;
    private bool[] hasBeenPurchased;
    private float[] clickIncrease; //Процентик кликов для улучшений
    private float[] costIncrease; //Процентик стоимости для улучшений
    private int[] upgradeSinseChange; //Кол-во улучшений для уменьшения процента за клик
    private int[] upgradeBeforeChange; //Кол-во улучшений для сброса
    private float[] decreaseClick; //Уменьшение процента кликов
    public Text[] LevelUp;
    public int maxLevel = 50;
    public Button[] buyToolButton;


    public void OnClickButton()
    {
        Score += ClickScore;
    }


    //tools shop





    // Start is called before the first frame update
    void Start()
    {
       
        // Убедитесь, что массивы инициализированы в инспекторе

            int numberOfItems = ClickScores.Length;
            hasBeenPurchased = new bool[numberOfItems];
            clickIncrease = new float[numberOfItems];
            decreaseClick = new float[numberOfItems];
            costIncrease = new float[numberOfItems];
            upgradeSinseChange = new int[numberOfItems];
            upgradeBeforeChange = new int[numberOfItems];
            PassiveAmountBuild = new int[numberOfItems];

            bonusCome = new Dictionary<int, int>();
            bonusCome.Add(6, 2);
            bonusCome.Add(7, 5);
            bonusCome.Add(8, 10);

            for (int i = 0; i < numberOfItems; i++)
            {
                upgradeBeforeChange[i] = 30;
                upgradeSinseChange[i] = 0;
                costIncrease[i] = 0.23f;
                clickIncrease[i] = 0.12f;
                decreaseClick[i] = 0.10f;

                
            }

            StartCoroutine(BuildShop());
        }

    // Update is called once per frame
    void Update()

        //Clicker
    {
        ScoreText.text = FormatScore((double)Score) + "$";
        
    }
    
    //Shop and upgrades open pan
    public void ShowAndHideToolsPan()
    {
        ToolsPan.SetActive(!ToolsPan.activeSelf);
    }

    public void ShowAndHideBuildPan()
    {
        BuildPan.SetActive(!BuildPan.activeSelf);
    }

    //Amount
    public void OnCLickBuyTool(int itemIndex)
    {
        if (Score >= CostInt[itemIndex])
        {
            //Main amount

                // Вычитаем стоимость из общего счёта
            Score -= CostInt[itemIndex];
        
                // Обновляем стоимость и прирост за клик для текущего улучшения
            CostInt[itemIndex] += Mathf.RoundToInt(CostInt[itemIndex] * costIncrease[itemIndex]);
            ClickScores[itemIndex] += Mathf.RoundToInt(ClickScores[itemIndex] * clickIncrease[itemIndex]);

            upgradeSinseChange[itemIndex]++;

            if(upgradeSinseChange[itemIndex] >= upgradeBeforeChange[itemIndex])
            {
                clickIncrease[itemIndex] -= decreaseClick[itemIndex];
                upgradeSinseChange[itemIndex] = 0;
            }
            
            /*CostInt[itemIndex] *= 2;
            ClickScores[itemIndex] *= 2;*/

                //Флаг покупки товаров
                hasBeenPurchased[itemIndex] = true;

                // Пересчитываем общий ClickScore
                int totalClickScore = 0;
                for (int i = 0; i < ClickScores.Length; i++)
                {
                    if (hasBeenPurchased[i])
                    {
                    totalClickScore += ClickScores[i]; 
                    }
                }
        
                // Обновляем текущий ClickScore
            ClickScore = totalClickScore;

                // Обновляем текст стоимости для текущего улучшения
            CostText[itemIndex].text = CostInt[itemIndex] + "$";
            // increase level by 1

            int currentLevel = int.Parse(LevelUp[itemIndex].text.Split(':')[1].Trim());
            currentLevel++;

            if (currentLevel > maxLevel)
            {
                currentLevel = maxLevel;
            }

            LevelUp[itemIndex].text = "Level: " + (currentLevel == maxLevel ? "MAX" : currentLevel.ToString());

            // disable button if max level is reached

            if (currentLevel == maxLevel)
            {
                buyToolButton[itemIndex].interactable = false;
            }

            // hide the cost when the current level is maxLevel

            if (currentLevel == maxLevel)
            {
                CostText[itemIndex].text = "";
            }
            else
            {
                CostText[itemIndex].text = FormatPrice(CostInt[itemIndex]);
            }
        }
    }
    public void OnCLickBuyBuild(int itemIndex)
    {
        
        if (itemIndex < 0 || itemIndex >= CostInt.Length || itemIndex >= PassiveAmountBuild.Length || itemIndex >= costIncrease.Length)
    {
        Debug.LogError($"itemIndex {itemIndex} is out of bounds");
        return;
    }

        if (Score >= CostInt[itemIndex])
        {
            

            //Main amount

                // Вычитаем стоимость из общего счёта
            Score -= CostInt[itemIndex];
        
                // Обновляем стоимость и прирост пассива для текущего улучшения
            CostInt[itemIndex] += Mathf.RoundToInt(CostInt[itemIndex] * costIncrease[itemIndex]);
                // Получаем значение пассивного дохода для данной постройки из словаря
            int bonusIncome = 0;
            if (bonusCome.ContainsKey(itemIndex))
            {
                bonusIncome = bonusCome[itemIndex];
            }

            // Увеличиваем уровень пассивного дохода для данной постройки на значение из словаря
            PassiveAmountBuild[itemIndex] += bonusIncome;

            // Обновляем стоимость для следующего уровня
            CostInt[itemIndex] += Mathf.RoundToInt(CostInt[itemIndex] * costIncrease[itemIndex]);
            

             // Обновляем текст стоимости для текущего улучшения
            CostText[itemIndex].text = CostInt[itemIndex] + "$";



            // increase level by 1

            int currentLevel = int.Parse(LevelUp[itemIndex].text.Split(':')[1].Trim());
            currentLevel++;

            if (currentLevel > maxLevel)
            {
                currentLevel = maxLevel;
            }

            LevelUp[itemIndex].text = "Level: " + (currentLevel == maxLevel ? "MAX" : currentLevel.ToString());

            
           
            // disable button if max level is reached

            if (currentLevel == maxLevel)
            {
                buyToolButton[itemIndex].interactable = false;
            }

            // hide the cost when the current level is maxLevel

            if (currentLevel == maxLevel)
            {
                CostText[itemIndex].text = "";
            }
            else
            {
                CostText[itemIndex].text = FormatPrice(CostInt[itemIndex]);
            }

        }

    }


    /*public void OnCLickBuyBuild(int itemIndex)
    {
        if (Score >= CostInt[itemIndex])
        {
            Score -= CostInt[itemIndex];
            CostInt[itemIndex] *= 2;
            PassiveAmountBuild[itemIndex] += 2; 
            CostText[itemIndex].text = CostInt[itemIndex] + "$";
        }
    }*/

    IEnumerator BuildShop()
    {
        while (true)
        {
            for (int i = 0; i < PassiveAmountBuild.Length; i++)
            {
                Score += PassiveAmountBuild[i];
            }
            yield return new WaitForSeconds(1);
        }

    }
    //suffix

    public string FormatScore(double score)
{
    string[] suffixes = { "", "K", "M", "B", "T", "Q", "S","O"};
    int index = 0;
    double divisor = 1.0;

    while (score >= 1000 && index < suffixes.Length - 1)
    {
        score /= 1000.0;
        index++;
        divisor *= 1000.0;
    }

    return $"{score:f1}{suffixes[index]}";
}


    public string FormatPrice(double price)
{
    string[] suffixes = { "", "K", "M", "B", "T", "Q", "S","O" };
    int index = 0;
    double divisor = 1.0;

    while (price >= 1000 && index < suffixes.Length - 1)
    {
        price /= 1000.0;
        index++;
        divisor *= 1000.0;
    }

    string formattedPrice = $"{price:f1}{suffixes[index]}";

    // Добавляем знак валюты только если не достигнут максимальный уровень
    if (LevelUp[0].text != "Level: MAX")
    {
        formattedPrice += " $";
    }

    return formattedPrice;
}



}
   


