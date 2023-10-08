#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

#endregion

namespace Pathfinder
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        //sprite texture for tiles, player, and ai bot
        Texture2D tile1Texture;
        Texture2D tile2Texture;
        Texture2D aiTexture;
        Texture2D playerTexture;

        //objects representing the level map, bot, and player 
        private Level level;

        //*Switch betweem these two depending on the bot you are using
        //AiBotAStar bot;
        AIBotLRTA bot;

        private Player player;


        //graph
        Graph g;
        double[,] graph_matrix;

        //screen size and frame rate
        private const int TargetFrameRate = 50;
        private const int BackBufferWidth = 600;
        private const int BackBufferHeight = 600;

        public Game1()
        {
            //constructor
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = BackBufferHeight;
            graphics.PreferredBackBufferWidth = BackBufferWidth;
            Window.Title = "Pathfinder";
            Content.RootDirectory = "../../../Content";
            //set frame rate
            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / TargetFrameRate);
            //load level map
            level = new Level();
            level.Loadmap("../../../Content/4.txt");
            //instantiate bot and player objects

            g = new Graph(level);
            graph_matrix = g.GenerateGraph();

            Coord2 botPos = new Coord2(10, 20);
            Coord2 plrPos = new Coord2(20, 20);

            //*Switch between these two depending on the bot you are using*
            //bot = new AiBotAStar(10, 20);
            bot = new AIBotLRTA(graph_matrix, botPos, plrPos, level.tiles.GetLength(0));

            player = new Player(plrPos.X, plrPos.Y);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //load the sprite textures
            Content.RootDirectory = "../../../Content";
            tile1Texture = Content.Load<Texture2D>("tile1");
            tile2Texture = Content.Load<Texture2D>("tile2");
            aiTexture = Content.Load<Texture2D>("ai");
            playerTexture = Content.Load<Texture2D>("target");
            font = Content.Load<SpriteFont>("Font");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //player movement: read keyboard
            KeyboardState keyState = Keyboard.GetState();
            Coord2 currentPos = new Coord2();
            currentPos = player.GridPosition;
            if (keyState.IsKeyDown(Keys.Up))
            {
                currentPos.Y -= 1;
                player.SetNextLocation(currentPos, level);
            }
            else if (keyState.IsKeyDown(Keys.Down))
            {
                currentPos.Y += 1;
                player.SetNextLocation(currentPos, level);
            }
            else if (keyState.IsKeyDown(Keys.Left))
            {
                currentPos.X -= 1;
                player.SetNextLocation(currentPos, level);
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                currentPos.X += 1;
                player.SetNextLocation(currentPos, level);
            }

            //update bot and player
            bot.Update(gameTime, level, player);
            player.Update(gameTime, level);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //draw level map
            DrawGrid();
            //draw bot
            spriteBatch.Draw(aiTexture, bot.ScreenPosition, Color.White);
            //drawe player
            spriteBatch.Draw(playerTexture, player.ScreenPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGrid()
        {
            //draws the map grid
            int sz = level.GridSize;
            for (int x = 0; x < sz; x++)
            {
                for (int y = 0; y < sz; y++)
                {
                    Coord2 temp = new Coord2(x, y);
                    Coord2 pos = new Coord2((x * 15), (y * 15));

                    //*Drawing Tool for the LRT Bot
                    if (level.tiles[x, y] == 0)
                    { spriteBatch.Draw(tile1Texture, pos, Color.White); }
                    else
                        spriteBatch.Draw(tile2Texture, pos, Color.White);

                    int v = bot.GridPostionToVertex(new Coord2(x, y));
                    NodeLRTA tmp;
                    bot.nodeList.TryGetValue(v, out tmp);
                    if (tmp.stateCost != 0)
                    { spriteBatch.DrawString(font, tmp.stateCost.ToString(), new Vector2(pos.X, pos.Y), Color.Black); } 


                    //Drawing Tool for the A*Star Bot
                   /*  if (level.tiles[x, y] == 0)   {
                    if (bot.Path.Exists(node => node.GridPosition == temp))
                       spriteBatch.Draw(tile1Texture, pos, Color.Red);
                    else
                        spriteBatch.Draw(tile1Texture, pos, Color.White); }
                     else
                      spriteBatch.Draw(tile2Texture, pos, Color.White); */


                }
            }
        }
    }
}
