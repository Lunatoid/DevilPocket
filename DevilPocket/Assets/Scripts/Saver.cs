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
        writer.WriteLine(primaryMonster.monsterName);
        writer.WriteLine(primaryMonster.SaveToString());


        GameObject secondaryMonsterGO = playerInventory.GetMonster(true);

        if (secondaryMonsterGO) {
            Monster secondaryMonster = secondaryMonsterGO.GetComponent<Monster>();

            writer.WriteLine(secondaryMonster.monsterName);
            writer.WriteLine(secondaryMonster.SaveToString());
        } else {
            writer.WriteLine("null");
        }

        // PC monsters
        writer.WriteLine(playerInventory.pcStorage.Count);

        foreach (PlayerInventory.PcEntry entry in playerInventory.pcStorage) {
            writer.WriteLine(entry.name);
            writer.WriteLine(entry.saveString);
        }

        // Quests
        writer.WriteLine(playerInventory.SaveQuestLedger());

        // Bosses
        foreach (bool b in playerInventory.beatenBosses) {
            writer.WriteLine(b);
        }

        writer.Close();
    }

    public void Load() {
        StreamReader reader;
        try {
            if (File.Exists($"{Application.dataPath}/save.txt")) {
                reader = File.OpenText($"{Application.dataPath}/save.txt");
            } else {
                return;
            }
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

        if (secondaryMonsterName != "null") {
            playerInventory.LoadMonster(secondaryMonsterName, reader.ReadLine(), true);
        }

        // PC monsters
        int count = int.Parse(reader.ReadLine());
        for (int i = 0; i < count; ++i) {
            PlayerInventory.PcEntry entry;
            entry.name = reader.ReadLine();
            entry.saveString = reader.ReadLine();

            playerInventory.pcStorage.Add(entry);
        }

        // Quests
        playerInventory.LoadQuestLedger(reader);

        // Bosses
        for (int i = 0; i < 4; ++i) {
            playerInventory.beatenBosses[i] = bool.Parse(reader.ReadLine());
        }

        reader.Close();
    }
}
