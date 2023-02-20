using UnityEngine;


public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Canvas createConnectUI;


    public void OnClickGameButton(){
        Hide();
        createConnectUI.GetComponent<CreateConnectUI>().Show();
    }

    public void OnClickSettingButton(){
        Debug.Log("Нажата кнопка настройки...");
    }

    public void OnClickExitButton(){
        Application.Quit();
    }


    public void Hide(){
        gameObject.SetActive(false);
    }

    public void Show(){
        gameObject.SetActive(true);
    }
}
