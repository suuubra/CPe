﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace CottonPlantation
{
    public partial class Form2 : Form
    {

        Image player;
        Image massa;
        List<string> playerMovements = new List<string>();
        List<string> massaMovements = new List<string>();
        int steps = 0;
        int slowDownFrameRate = 0;
        bool goLeft, goRight, goUp, goDown;
        int massaX;
        int massaY;
        int massaHeight = 100;
        int massaWidth = 100;
        int massaSpeed = 8;

        int playerX;
        int playerY;
        int playerHeight = 100;
        int playerWidth = 100;
        int playerSpeed = 8;

        List<string> item_locations = new List<string>();
        List<Items> items_list = new List<Items>();
        int spawnTimeLimit = 50;
        int timeCounter = 0;
        Random rand = new Random();
        string[] itemNames = { "cotton" };
        List<int> counter = new List<int>();

        public Form2()
        {
            InitializeComponent();
            SetUp();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }


        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = true;
            }
            if(e.KeyCode == Keys.Escape)
            {
                CottonPlantation Form1 = new CottonPlantation();
                Form1.Show();
                this.Hide();
            }
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void FormPaintEvent(object sender, PaintEventArgs e)
        {
            Graphics Canvas = e.Graphics;

            if (items_list != null)
            {
                foreach (Items item in items_list)
                {
                    Canvas.DrawImage(item.item_image, item.positionX, item.positionY, item.width, item.height);
                }
            }

            Canvas.DrawImage(player, playerX, playerY, playerWidth, playerHeight);
            Canvas.DrawImage(massa, 50, 50, massaWidth, massaHeight);
            Canvas.DrawImage(massa, massaX, massaY, massaWidth, massaHeight);
        }

        private void TimerEvent(object sender, EventArgs e)
        {

            CheckCollision();

            if (timeCounter > 1)
            {
                timeCounter--;
            }
            else
            {
                MakeItems();
            }



            if (goLeft && massaX > 0)
            {
                massaX -= playerSpeed;
                AnimateMassa(4, 7);
            }
            else if (goRight && massaX + massaWidth < this.ClientSize.Width)
            {
                massaX += massaSpeed;
                AnimateMassa(8, 11);
            }
            else if (goUp && playerY > 0)
            {
                massaY -= massaSpeed;
                AnimateMassa(12, 15);
            }
            else if (goDown && massaY + massaHeight < this.ClientSize.Height)
            {
                massaY += massaSpeed;
               AnimateMassa(0, 3);
            }
            else
            {
                AnimateMassa(0, 0);
            }


            if (goLeft && playerX > 0)
            {
                playerX -= playerSpeed;
                AnimatePlayer(4, 7);
            }
            else if (goRight && playerX + playerWidth < this.ClientSize.Width)
            {
                playerX += playerSpeed;
                AnimatePlayer(8, 11);
            }
            else if (goUp && playerY > 0)
            {
                playerY -= playerSpeed;
                AnimatePlayer(12, 15);
            }
            else if (goDown && playerY + playerHeight < this.ClientSize.Height)
            {
                playerY += playerSpeed;
                AnimatePlayer(0, 3);
            }
            else
            {
                AnimatePlayer(0, 0);
            }



            this.Invalidate();
        }


        private void SetUp()
        {
            this.BackgroundImage = Image.FromFile("bg.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.DoubleBuffered = true;
            
            // load the player files to the list
            playerMovements = Directory.GetFiles("player", "*.png").ToList();
            massaMovements = Directory.GetFiles("massa", "*.png").ToList();
            player = Image.FromFile(playerMovements[0]);
            massa = Image.FromFile(massaMovements[0]);

            item_locations = Directory.GetFiles("items", "*.png").ToList();

        }

    private void AnimatePlayer(int start, int end)
        {

            slowDownFrameRate += 1;

            if (slowDownFrameRate == 4)
            {
                steps++;
                slowDownFrameRate = 0;
            }
            if (steps > end || steps < start)
            {
                steps = start;
            }

            player = Image.FromFile(playerMovements[steps]);

        }

        private void AnimateMassa(int start, int end)
        {

            slowDownFrameRate += 1;

            if (slowDownFrameRate == 4)
            {
                steps++;
                slowDownFrameRate = 0;
            }
            if (steps > end || steps < start)
            {
                steps = start;
            }

            player = Image.FromFile(massaMovements[steps]);

        }

        private void MakeItems()
        {
            int i = rand.Next(0, item_locations.Count);

            Items newItem = new Items();
            newItem.item_image = Image.FromFile(item_locations[i]);
            newItem.name = itemNames[i];
            timeCounter = spawnTimeLimit;
            items_list.Add(newItem);    


        }

        private void CheckCollision()
        {
            foreach (Items item in items_list.ToList())
            {

                item.CheckLifeTime();
                if (item.expired)
                {
                    item.item_image = null;
                    items_list.Remove(item);
                }


                bool collision = DetectCollision(playerX, playerY, player.Width, player.Height, item.positionX, item.positionY, item.width, item.height);

                if (collision)
                {

                    lblCollected.Text = "Collected the: " + item.name;
                    item.item_image = null;
                    items_list.Remove(item);

                }


            }



        }

        private bool DetectCollision(int object1X, int object1Y, int object1Width, int object1Height, int object2X, int object2Y, int object2Width, int object2Height)
        {

            if (object1X + object1Width <= object2X || object1X >= object2X + object2Width || object1Y + object1Height <= object2Y || object1Y >= object2Y + object2Height)
            {
                return false; // no collision
            }
            else
            {
                return true; // found collision
            }



        }

        
    }
}
