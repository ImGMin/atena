using UnityEngine;
using TMPro;

public class PlottingManager : MonoBehaviour
{
    private static PlottingManager instance;
    public static PlottingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlottingManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("PlottingManager");
                    instance = go.AddComponent<PlottingManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public TMP_Text energyText;
    public TMP_Text reputationText;
    public TMP_Text currencyText;

    public void UpdateEnergyUI(int energy)
    {
        if (energyText != null)
            energyText.text = $"Energy: {energy}";
    }

    public void UpdateReputationUI(int reputation)
    {
        if (reputationText != null)
            reputationText.text = $"Reputation: {reputation}";
    }

    public void UpdateCurrencyUI(int currency)
    {
        if (currencyText != null)
            currencyText.text = $"Currency: {currency}";
    }
}
