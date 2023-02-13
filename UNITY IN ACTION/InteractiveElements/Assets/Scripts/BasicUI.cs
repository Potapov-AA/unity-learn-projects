using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUI : MonoBehaviour
{
    void OnGUI(){
        int posX = 10;
        int posY = 10;
        int width = 100;
        int heigth = 30;
        int buffer = 10;

        List<string> itemList = Managers.Inventory.GetItemList();
        if(itemList.Count == 0){
            GUI.Box(new Rect(posX, posY, width, heigth), "No Items");
        }
        foreach(string item in itemList){
            int count = Managers.Inventory.GetItemCount(item);
            Texture2D image = Resources.Load<Texture2D>("Icons/"+item);
            GUI.Box(new Rect(posX, posY, width, heigth), new GUIContent("("+count+")", image));
            posX += width + buffer;
        }

        string equipped = Managers.Inventory.equippedItem;
        if(equipped != null){
            posX = Screen.width - (width+buffer);
            Texture2D image = Resources.Load("Icons/"+equipped) as Texture2D;
            GUI.Box(new Rect(posX, posY, width, heigth), new GUIContent("Equipped", image));
        }

        posX = 10;
        posY += heigth+buffer;
        
        foreach(string item in itemList){
            if(GUI.Button(new Rect(posX, posY, width, heigth), "Equip "+item)){
                Managers.Inventory.EquipItem(item);
            }

            if(item == "health"){
                if(GUI.Button(new Rect(posX, posY + heigth +buffer, width, heigth), "Use Health")){
                    Managers.Inventory.ConsumeItem("health");
                    Managers.Player.ChangeHealth(25);
                }
            }
            posX += width+buffer;
        }
    }
}
