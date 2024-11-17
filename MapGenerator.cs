using System;

namespace MyGame;

public class MapGenerator
{
    private const int MapWidth = 32;
    private const int MapHeight = 18;

    private string[,] _gameMap = new string[MapWidth, MapHeight];
    private Random _random = new Random();

    public MapGenerator()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        // Начнем с базового слоя травы
        for (int x = 0; x < MapWidth; x++)
            for (int y = 0; y < MapHeight; y++)
                _gameMap[x, y] = "grass";

        // Генерируем несколько зон воды
        GenerateZones("water", 3, 7);

        // Генерируем зоны леса
        GenerateZones("forest", 5, 8);

        // Генерируем зоны гор
        GenerateZones("mountain", 2, 8);

        // Переходные зоны с песком или каменистой землей
        GenerateZones("rocky", 5, 7);
    }

    private void GenerateZones(string tileType, int zoneCount, int maxZoneSize)
    {
        for (int i = 0; i < zoneCount; i++)
        {
            int startX = _random.Next(0, MapWidth);
            int startY = _random.Next(0, MapHeight);

            for (int j = 0; j < maxZoneSize; j++)
            {
                int offsetX = _random.Next(-1, 2);
                int offsetY = _random.Next(-1, 2);
                int x = Math.Clamp(startX + offsetX, 0, MapWidth - 1);
                int y = Math.Clamp(startY + offsetY, 0, MapHeight - 1);
                _gameMap[x, y] = tileType;
            }
        }
    }

    public bool IsTileWalkable(int x, int y)
    {
        //int _x = System.Convert.ToInt32(x);
        //int _y = System.Convert.ToInt32(y);

        System.Console.WriteLine("IsTW?");
        System.Console.WriteLine("_x = " + x);
        System.Console.WriteLine("_y = " + y);
        System.Console.WriteLine("GL0 = " + _gameMap.GetLength(0));
        System.Console.WriteLine("GL1 = " + _gameMap.GetLength(1));
        System.Console.WriteLine("Tile = " + _gameMap[x, y]);
        System.Console.WriteLine("___");

        if (x < 0 || y < 0 || x >= _gameMap.GetLength(0) || y >= _gameMap.GetLength(1))
            return false;

        return _gameMap[x, y] switch
        {
            "grass" => true,
            "forest" => true,
            "water" => false,
            "mountain" => false,
            "rocky" => true,
            _ => false
        };
    }


    public string[,] GetMap()
    {
        return _gameMap;
    }
}
