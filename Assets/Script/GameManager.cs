using UnityEngine;
using UnityEngine.UI;


public class Gamemanager : MonoBehaviour
{
    public string name;
    
    public int level = 1;
    public int exp = 0;
    public int cash = 0;
    public int energy = 0;
    public int repute = 0;
    public int follower = 0;

    void Start()
    {
        Debug.Log(exp);
        Debug.Log(energy);
        Debug.Log(repute);
    }
        public void PlusButton()
    {
        exp += 10;
        energy -= 10;
        repute +=5;

    Debug.Log(exp);
    Debug.Log(energy);
    Debug.Log(repute);

    }

}