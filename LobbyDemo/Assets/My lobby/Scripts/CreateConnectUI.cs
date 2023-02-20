using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateConnectUI : MonoBehaviour
{
    [SerializeField] Canvas mainMenuUI;
    
    void Start(){
        Hide();
    }



    public void OnClickBackButton(){
        Hide();
        mainMenuUI.GetComponent<MainMenuUI>().Show();
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    public void Show(){
        gameObject.SetActive(true);
    }
}
