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
            private int movementSpeed;
            private int laserSpeed;
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
                movementSpeed = 5;
                laserSpeed = 20;
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

            public int GetMovementSpeed()
            {
                return movementSpeed;
            }
            public void SetMovementSpeed(int speed)
            {
                this.movementSpeed = speed;
            }

            public int GetLaserSpeed()
            {
                return laserSpeed;
            }

            public void SetLaserSpeed(int laserSpeed)
            {
                this.laserSpeed = laserSpeed;
            }

        }

        internal class Player
        {
            private Point Location;
            private int Left, Width;
            private Rectangle Bounds;
            private Size sizeOfPlayer;
            private PictureBox pb;
            private int movementSpeed;
            private int bulletSpeed;

            public Player()
            {
                Location = new Point(350, 400);
                Left = 0;
                Width = 0;
                movementSpeed = 5;
                bulletSpeed = 15;
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

            public int GetMovementSpeed()
            {
                return movementSpeed;
            }
            public void SetMovementSpeed(int movementSpeed)
            {
                this.movementSpeed = movementSpeed;
            }

            public int GetBulletSpeed()
            {
                return bulletSpeed;
            }
            public void SetBulletSpeed(int bulletSpeed)
            {
                this.bulletSpeed = bulletSpeed;
            }


        }

        internal class Background
        {
            public Label score;
            public int x, y;
            public Size size;

            public Background()
            {
                x = 500;
                y = 400;
            }


            public Label CreateLabelScore(Form p)
            {
                Label score = new Label();
                score.Location = new Point(x, y);
                score.AutoSize = true;
                score.Text = "Score :";
                score.Font = new Font("Calibri", 18);
                score.ForeColor = Color.White;
                score.Padding = new Padding(6);
                //p.Controls.Add(score);
                return score;
            }

            public Label CreateLabelFinish(Form p)
            {
                Label finish = new Label();
                finish.Location = new Point(x - 100, y - 100);
                finish.AutoSize = true;
                finish.Text = "";
                finish.Font = new Font("Calibri", 18);
                finish.ForeColor = Color.White;
                finish.Padding = new Padding(6);
                finish.Name = "Finish";
                finish.Visible = false;
                return finish;

            }
        }


            
        
    }
}
