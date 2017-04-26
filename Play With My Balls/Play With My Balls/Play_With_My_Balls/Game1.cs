using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Play_With_My_Balls
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        enum GameStates { TitleScreen, Playing, FailedAtLife, Gameover };
        GameStates gameState = GameStates.TitleScreen;
        Texture2D titleScreen;
        Texture2D spriteSheet;
        Texture2D backGround;
        Sprite ball;
        StarField starField;
        SpriteFont fontAwesome;
        Texture2D endingScreen;

        float ballSpeed = -450;
        int score = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            endingScreen = Content.Load <Texture2D>(@"Textures\Endingscreen");
            titleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
            spriteSheet = Content.Load<Texture2D>(@"Textures\SpriteSheet");
            backGround = Content.Load<Texture2D>(@"Textures\BackGround");
            fontAwesome = Content.Load<SpriteFont>(@"Awesome");
            starField = new StarField(
                this.Window.ClientBounds.Width,
                this.Window.ClientBounds.Height,
                200,
                new Vector2(0, 30f),
                spriteSheet,
                new Rectangle(0, 450, 2, 2));
            ball = new Sprite(new Vector2(200, 30f), spriteSheet, new Rectangle(5, 12, 220, 208), new Vector2(0, 100));         // TODO: use this.Content to load your game content here
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
                


            // TODO: Add your update logic here
            switch (gameState)
            {
                case GameStates.TitleScreen:
                    KeyboardState kb = Keyboard.GetState();

                    if (kb.IsKeyDown(Keys.Space))
                    {
                        gameState = GameStates.Playing;
                    }

                    break;

                case GameStates.Playing:

                    ball.Velocity += new Vector2(0, 55);

                    MouseState ms = Mouse.GetState();
                    Vector2 clk = new Vector2(ms.X, ms.Y);

                    if ((ms.LeftButton == ButtonState.Pressed && Vector2.Distance(clk, ball.Center) < ball.BoundingBoxRect.Width / 2 && ball.Velocity.Y > 0))
                    {
                        ball.Velocity *= new Vector2(1, -1);
                        Vector2 move = ball.Center - clk;
                        move.Normalize();
                        move *= 300; // Scale the vector by the speed you want
                        move.Y = -150;  // Set the 
                        ball.Velocity += move;
                        score++; 
                    }

                    if (ball.Location.Y > this.Window.ClientBounds.Height - ball.BoundingBoxRect.Height)
                    {
                        //ball.Velocity *= new Vector2(1, -1);
                        gameState = GameStates.Gameover;
                    }

                    // Enforce a maximum speed
                    Vector2 vel = ball.Velocity;
                    float speed = vel.Length();
                    vel.Normalize();
                    vel *= MathHelper.Clamp(speed, 0, 1200);
                    ball.Velocity = vel;

                    ball.Update(gameTime);

                    break;
                    
                case GameStates.FailedAtLife:
                    break;

                case GameStates.Gameover:
                    
                    break;  
            }




            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (gameState == GameStates.TitleScreen)
            {
                spriteBatch.Draw(titleScreen, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
            }
           

            if ((gameState == GameStates.Playing) ||
                (gameState == GameStates.Gameover))
            {

                spriteBatch.Draw(backGround, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
                starField.Draw(spriteBatch);
                ball.Draw(spriteBatch);
                spriteBatch.DrawString(fontAwesome, "Score: " + score, new Vector2(10, 10), Color.White);
            }
            if (gameState == GameStates.Gameover)
            {
                spriteBatch.Draw(endingScreen, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
