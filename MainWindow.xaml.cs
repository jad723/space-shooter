﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Space_Shooter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();

        bool moveLeft, moveRight;

        List<Rectangle> itemRemover = new List<Rectangle>();

        Random rand = new Random();

        int enemySpriteCounter = 0, enemyCounter = 100, playerSpeed = 10, limit = 50, score = 0, damage = 0, enemySpeed = 10;

        Rect playerHitBox;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            MyCanvas.Focus();

            ImageBrush bg = new ImageBrush();

            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/purple.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0, 0, 0.15, 0.15);
            bg.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            MyCanvas.Background = bg;

            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player.png"));
            player.Fill = playerImage;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            enemyCounter -= 1;

            scoreText.Content = "Score: " + score;
            damageText.Content = "Damage " + damage;

            if(enemyCounter < 0)
            {
                MakeEnemies();
                enemyCounter = limit;
            }

            if(moveLeft == true && Canvas.GetLeft(player) > 10) Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);

            if(moveRight == true && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width) Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {

                if(x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetTop(x) < 10) itemRemover.Add(x);
                }

                if(x is Rectangle && (string)x.Tag == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left) moveLeft = true;

            if(e.Key == Key.Right) moveRight = true;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left) moveLeft = false;

            if(e.Key == Key.Right) moveRight = false;

            if(e.Key == Key.Space)
            {

                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red
                };

                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
                Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);

                MyCanvas.Children.Add(newBullet);
            }
        }

        private void MakeEnemies()
        {
            ImageBrush enemySprite = new ImageBrush();

            enemySpriteCounter = rand.Next(1, 5);

            switch (enemySpriteCounter)
            {
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/1.png"));
                    break;

                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/2.png"));
                    break;

                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/3.png"));
                    break;

                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/4.png"));
                    break;

                case 5:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/5.png"));
                    break;
            }

            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite
            };

            Canvas.SetTop(newEnemy, -100);
            Canvas.SetLeft(newEnemy, rand.Next(30, 430));
            MyCanvas.Children.Add(newEnemy);
        }
    }
}
