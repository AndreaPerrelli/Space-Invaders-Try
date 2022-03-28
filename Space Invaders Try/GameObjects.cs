using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Space_Invaders_Try
{
    internal class GameObjects
    {
        internal class Enemies
        {
            private int width, height;
            private int columns, rows;
            private int x, y, space;
            private PictureBox enemy;
            private List<PictureBox> enemyGroup;

            public Enemies()
            {
                width = 40;
                height = 40;
                columns = 10;
                rows = 5;
                space = 10;
                x = 150;
                y = 0;
            }
            private PictureBox CreateControlEnemy(Form p)
            {

                PictureBox pb = new PictureBox();
                pb.Location = new Point(x, y);
                pb.Size = new Size(width, height);
                pb.BackgroundImage = Properties.Resources.inavders;
                pb.BackgroundImageLayout = ImageLayout.Stretch;
                pb.Name = "Alien";
                return pb;
                p.Controls.Add(pb);
            }
            public List<PictureBox> CreateSprites(Form p)
            {
                enemyGroup = new List<PictureBox>();
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        enemy = CreateControlEnemy(p);
                        enemyGroup.Add(enemy);
                        x += width + space;
                    }
                    y += height + space;
                    x = 150;
                }
                return enemyGroup;
            }

        }

        internal class Player
        {
            public Point Location;
            public int Left, Width;
            public Rectangle Bounds;
            public Size sizeOfPlayer;
            public PictureBox pb;

            public Player()
            {
                Location = new Point(350, 400);
                Left = 0;
                Width = 0;
                sizeOfPlayer = new Size(50, 50);
                Bounds = new Rectangle(Location, sizeOfPlayer);
            }

            public PictureBox CreateControlPlayer(Form p)
            {
                pb = new PictureBox();
                pb.Location = Location;
                pb.Size = sizeOfPlayer;
                pb.BackgroundImage = Properties.Resources.tank;
                pb.BackgroundImageLayout = ImageLayout.Stretch;
                pb.Name = "Player";               
                return pb;
            }

            private Point GetLocation()
            {
                return Location;
            }
            private void SetLocation(Point point)
            {
                Location = point;
            }

            private int GetLeft()
            {
                return Left;
            }

            private void SetLeft(int left)
            {
                Left = left;
            }



        }

        internal class Background
        {
            public Label score;
            public Point location;
            public Size size;

            public Background()
            {

                location = new Point(500, 400);
            }


            public Label CreateControlText(Form p)
            {
                Label score = new Label();
                score.Location = location;
                score.AutoSize = true;
                score.Text = "Score :";
                score.Font = new Font("Calibri", 18);
                score.ForeColor = Color.White;
                score.Padding = new Padding(6);
                //p.Controls.Add(score);
                return score;
            }
        }


            
        
    }
}
