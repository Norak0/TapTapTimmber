using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveIncomeManager : MonoBehaviour
{
    
   // Начальный процент пассивного дохода
    public float initialPassiveIncomePercentage = 5;

    // Коэффициент роста пассивного дохода
    public float growthFactor = 1.1f;

    // Уровень, с которого начинается нелинейный рост пассивного дохода
    public int nonlinearLevel = 5;

    // Функция для расчета пассивного дохода на основе текущего уровня
    public float CalculatePassiveIncome(int currentLevel)
    {
        if (currentLevel < nonlinearLevel)
        {
            // Линейный рост до nonlinearLevel уровня
            return initialPassiveIncomePercentage * currentLevel;
        }
        else
        {
            // Нелинейный рост после nonlinearLevel уровня
            return initialPassiveIncomePercentage * Mathf.Pow(growthFactor, currentLevel - nonlinearLevel + 1);
        }
    }
}
