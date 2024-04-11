using UnityEngine.UI;

public Button button_plus;
public Image ExpGuage;

private float exp;
private float exp_max;
private float exp_delta;

void Set_EXP()
    {
        exp = _value;
        if (exp<=0)
        {
            exp = 0;
        }
        else 
        {
            if (exp > exp_max)
            {
                hp = hp_max; //최댓값으로맞추기
            }
        }
        ExpGuage.fillAmount = exp/exp_max;

    }
    void Change_EXP()
    {
        hp += _value;
        Set_EXP(exp);
    }
    void Function_Button_Plus()
    {
        
    }
    


