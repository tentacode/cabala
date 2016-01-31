using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientSpawner : MonoBehaviour
{
    public List<GameObject> ingredients;
    public float tick = 5;

    float lastTick;
    List<int> players;

	void Start ()
    {
        lastTick = 0;

        if (ingredients.Count != 3) {
            Debug.LogError("IngredientSpawner must have exactly 3 ingredients");
        }
	}
	
	void Update ()
    {
        if (Time.time > lastTick + tick) {
            SpawnIngredients();
            lastTick = Time.time;
        }
	}

    void SpawnIngredients()
    {
        int ingredientCount = GetIngredientCount();

        List<int> playerIndexs = GetPlayerIndexs();
        Randomize(playerIndexs);
        Randomize(ingredients);

        for (int i = 0; i < ingredientCount; i++) {
            SpawnIngredient(ingredients[i], playerIndexs[i]);
        }
    }

    void SpawnIngredient(GameObject ingredient, int playerIndex)
    {
        Transform transform = GetRandomSpawnPosition(playerIndex);

        Instantiate(ingredient, transform.position, transform.rotation);
    }

    Transform GetRandomSpawnPosition(int playerIndex)
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("IngredientSpawnPoint" + playerIndex);
        var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

        return spawnPoint.transform;
    }

    void Randomize(IList list)
    {
        for (int i = 0; i < list.Count; i++) {
            var temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    List<int> GetPlayerIndexs()
    {
        // pool of player id to know where to spawn the ingredients
        // randomize on each SpawnIngredients()
        int playerCount = GameSharedData.NumberOfPlayer;

        players = new List<int>();
        for (int i = 0; i < playerCount; i++) {
            players.Add(i + 1);
        }

        return players;
    }

    int GetIngredientCount()
    {
        var ingredientCount = GameSharedData.NumberOfPlayer - 1;
        if (ingredientCount < 0) {
            return 1;
        }

        return ingredientCount;
    }
}
