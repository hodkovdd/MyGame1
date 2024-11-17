using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace MyGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private MapGenerator _mapGenerator;
    private string[,] _gameMap;

    // Параметры карты
    private const int TileSize = 32;
    private const int MapWidth = 25;
    private const int MapHeight = 18;
    private Texture2D _grassTexture;
    private Texture2D _waterTexture;
    private Texture2D _mountainTexture;
    private Texture2D _rockyTexture;
    private Texture2D _forestTexture;
    private Texture2D _characterTexture;
    private Vector2 _characterPosition;
    private const int CharacterSpeed = 2; // Скорость движения персонажа

    // Генерация карты
    //private string[,] _gameMap = new string[MapWidth, MapHeight];

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Загрузка текстур
        _grassTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/grass.png"));
        _waterTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/water.png"));
        _forestTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/forest.png"));
        _mountainTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/mountain.png"));
        _rockyTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/rocky.png"));
        _characterTexture = Texture2D.FromStream(GraphicsDevice, File.OpenRead("Content/character1.png"));
        _characterPosition = new Vector2(10, 10); // Начальная позиция персонажа

         // Генерация карты
        _mapGenerator = new MapGenerator();
        _gameMap = _mapGenerator.GetMap();
        System.Console.WriteLine($"Map size: {_gameMap.GetLength(0)} x {_gameMap.GetLength(1)}");
    }
    
    protected override void Update(GameTime gameTime)
    {
        // Обрабатываем ввод с клавиатуры
        var keyboardState = Keyboard.GetState();

        //Выход по Esc
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //где Персонаж?
        var (tileX, tileY) = GetTilePosition(_characterPosition);
        Vector2 newPosition = _characterPosition;
        
        // проверка нажатия и отработка
        if (keyboardState.IsKeyDown(Keys.Up))
            {
                System.Console.WriteLine("Up key pressed");
                newPosition.X = _characterPosition.X;
                newPosition.Y = System.Math.Max(0, _characterPosition.Y - CharacterSpeed);
                var (tileXn, tileYn) = GetTilePosition(newPosition);
                if (_mapGenerator.IsTileWalkable(tileXn,tileYn) && (_mapGenerator.IsTileWalkable(tileXn+1,tileYn)))
                     _characterPosition = newPosition;
            }
            
        if (keyboardState.IsKeyDown(Keys.Down))
            {
                System.Console.WriteLine("Down key pressed");
                newPosition.X = _characterPosition.X;
                newPosition.Y = System.Math.Min((MapHeight * TileSize) - TileSize, _characterPosition.Y + CharacterSpeed);
                var (tileXn, tileYn) = GetTilePosition(newPosition);
                if (_mapGenerator.IsTileWalkable(tileXn,tileYn+1) && (_mapGenerator.IsTileWalkable(tileXn+1,tileYn+1)))
                     _characterPosition = newPosition;
            }
            
        if (keyboardState.IsKeyDown(Keys.Left))
            {
                System.Console.WriteLine("Left key pressed");
                // проверка тайла
                newPosition.X = System.Math.Max(0, _characterPosition.X - CharacterSpeed);
                newPosition.Y = _characterPosition.Y;
                var (tileXn, tileYn) = GetTilePosition(newPosition);
                if (_mapGenerator.IsTileWalkable(tileXn,tileYn) && (_mapGenerator.IsTileWalkable(tileXn,tileYn+1)))
                     _characterPosition = newPosition;
            }

        if (keyboardState.IsKeyDown(Keys.Right))
            {
                System.Console.WriteLine("Right key pressed");
                // проверка тайла
                newPosition.X = System.Math.Min((MapWidth * TileSize) - TileSize, _characterPosition.X + CharacterSpeed);
                newPosition.Y = _characterPosition.Y;
                var (tileXn, tileYn) = GetTilePosition(newPosition);
                if (_mapGenerator.IsTileWalkable(tileXn+1,tileYn) && (_mapGenerator.IsTileWalkable(tileXn+1,tileYn+1)))
                     _characterPosition = newPosition;
            }

            

        (int, int) GetTilePosition(Vector2 position)
            {
            return ((int)position.X / TileSize, (int)position.Y / TileSize);
            }

       
      

        base.Update(gameTime);
    }

    

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Отрисовка карты
       for (int y = 0; y < _gameMap.GetLength(1); y++)
        {
        for (int x = 0; x < _gameMap.GetLength(0); x++)
            {
                Texture2D texture = _grassTexture; // По умолчанию трава
                switch (_gameMap[x, y])
                {
                    case "water":
                        texture = _waterTexture;
                        break;
                    case "forest":
                        texture = _forestTexture;
                        break;
                    case "mountain":
                        texture = _mountainTexture;
                        break;
                    case "rocky":
                        texture = _rockyTexture;
                        break;
                }

                _spriteBatch.Draw(texture, new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize), Color.White);
            }
        }

        // Отрисовка персонажа
        //_spriteBatch.Draw(_characterTexture, _characterPosition, Color.White);
        _spriteBatch.Draw(_characterTexture, new Rectangle((int)_characterPosition.X, (int)_characterPosition.Y, TileSize, TileSize), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
