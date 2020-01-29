using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class Saver : MonoBehaviour {

    PlayerInventory playerInventory;
    private void Start() {
        playerInventory = GameObject.Find("PlayerInventory").GetComponent<PlayerInventory>();
        Load();
    }

    private void OnApplicationQuit() {
        Save();
    }

    public void Save() {
        StreamWriter writer;
        try {
            writer = File.CreateText($"{Application.dataPath}/save.txt");
        } catch (FileNotFoundException) {
            return;
        }

        // Items
        writer.WriteLine(playerInventory.money);
        writer.WriteLine(playerInventory.baitAmount);
        writer.WriteLine(playerInventory.paracetamolAmount);
        writer.WriteLine(playerInventory.ibuprofenAmount);
        writer.WriteLine(playerInventory.morphineAmount);
        writer.WriteLine(playerInventory.hasBattlepass);

        // Carried monsters
        Monster primaryMonster = playerInventory.GetMonster().GetComponent<Monster>();
        Monster secondaryMonster = playerInventory.GetMonster(true).GetComponent<Monster>();

        writer.WriteLine(primaryMonster.monsterName);
        writer.WriteLine(primaryMonster.SaveToString());
        writer.WriteLine(secondaryMonster.monsterName);
        writer.WriteLine(secondaryMonster.SaveToString());

        // @TODO: PC monsters
        // @TODO: completed quests
        // @TODO: completed bosses

        writer.Close();
    }

    public void Load() {
        StreamReader reader;
        try {
            reader = File.OpenText($"{Application.dataPath}/save.txt");
        } catch (FileNotFoundException) {
            return;
        }

        // Items
        playerInventory.money = int.Parse(reader.ReadLine());
        playerInventory.baitAmount = int.Parse(reader.ReadLine());
        playerInventory.paracetamolAmount = int.Parse(reader.ReadLine());
        playerInventory.ibuprofenAmount = int.Parse(reader.ReadLine());
        playerInventory.morphineAmount = int.Parse(reader.ReadLine());
        playerInventory.hasBattlepass = bool.Parse(reader.ReadLine());

        // Carried monsters
        string primaryMonsterName = reader.ReadLine();
        playerInventory.LoadMonster(primaryMonsterName, reader.ReadLine());
        string secondaryMonsterName = reader.ReadLine();
        playerInventory.LoadMonster(secondaryMonsterName, reader.ReadLine(), true);

        // @TODO: PC monsters
        // @TODO: completed quests
        // @TODO: completed bosses

        reader.Close();
    }
}
