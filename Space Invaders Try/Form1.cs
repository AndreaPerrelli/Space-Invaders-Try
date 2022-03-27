using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Space_Invaders_Try.GameObjects;

namespace Space_Invaders_Try
{
    public partial class Form1 : Form
    {
        private bool fired;
        private bool moveLeft;
        private bool moveRight;
        private bool game;
        private int limit;
        private int top;
        private int left;
        private int cnt;
        private int speed;
        private Timer timer1, timer2, timer3, timer4, timer5, Observer;
        private int pts;
        private List<PictureBox> delay;
        private PictureBox label2;
        private int x, y;
        private Player player;
        private Background backGround;

        public Form1()
        {
            InitializeComponent();
            new GameObjects.Enemies().CreateSprites(this);
            new GameObjects.Player().CreateControlPlayer(this);
            new GameObjects.Background().CreateControlText(this);
            InsertAliens();
        }

        private void Pressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                moveLeft = true;
                PlayerMove();
            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                moveRight = true;
                PlayerMove();
            }
            else if (e.KeyCode == Keys.Space && game && !fired)
            {
                Missile();
                fired = true;
            }
        }

        private void Missile()
        {
            PictureBox bullet = new PictureBox();
            bullet.Location = new Point(player.Location.X + player.Width / 2, player.Location.Y - 20);
            bullet.Size = new Size(5, 20);
            bullet.BackgroundImage = Properties.Resources.bullet;
            bullet.BackgroundImageLayout = ImageLayout.Stretch;
            bullet.Name = "Bullet";
            this.Controls.Add(bullet);
        }

        private void Released(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                moveLeft = false;
            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                moveRight = false;
            }
            else if (e.KeyCode == Keys.Space)
            {
                fired = false;
            }
        }

        private void PlayerMove()
        {
            if (moveLeft && player.Location.X >= 0)
            {
                player.pb.Left--;
            }
            else if (moveRight && player.Location.X <= limit)
            {
                player.pb.Left++;
            }
        }


        private bool Touched(PictureBox a)
        {
            return a.Location.X <= 0 || a.Location.X >= limit;
        }

        private void SetDirection(PictureBox a)
        {
            int size = a.Height;

            if (Touched(a))
            {
                top = 1; left = 0; cnt++;

                if (cnt == size)
                {
                    top = 0; left = speed * (-1); Observer.Start();
                }
                else if (cnt == size * 2)
                {
                    top = 0; left = speed; cnt = 0; Observer.Start();
                }
            }
        }

        private void AlienMove()
        {
            foreach (PictureBox alien in aliens)
            {
                alien.Location = new Point(alien.Location.X + left, alien.Location.Y + top);
                SetDirection(alien);
                Collided(alien);
            }
        }

        private void Collided(PictureBox a)
        {
            if (a.Bounds.IntersectsWith(player.Bounds))
            {
                gameOver();
            }
        }

        private void MoveAliens(object sender, EventArgs e)
        {
            AlienMove();
        }

        private void Beam(PictureBox a)
        {
            PictureBox laser = new PictureBox();
            laser.Location = new Point(a.Location.X + a.Width / 3, a.Location.Y + 20);
            laser.Size = new Size(5, 20);
            laser.BackgroundImage = Properties.Resources.bullet;
            laser.BackgroundImageLayout = ImageLayout.Stretch;
            laser.Name = "Laser";
            this.Controls.Add(laser);
        }

        private void StrikeSpan(object sender, EventArgs e)
        {
            Random r = new Random();
            int pick;

            if (aliens.Count > 0)
            {
                pick = r.Next(aliens.Count);
                Beam(aliens[pick]);
            }
        }

        private void DetectLaser(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && c.Name == "Laser")
                {
                    PictureBox laser = (PictureBox)c;
                    laser.Top += 5;

                    if (laser.Location.Y >= limit)
                    {
                        this.Controls.Remove(laser);
                    }
                    if (laser.Bounds.IntersectsWith(player.Bounds))
                    {
                        this.Controls.Remove(laser);
                        LoseLife();
                    }
                }
            }
        }

        private void FireBullet(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && c.Name == "Bullet")
                {
                    PictureBox bullet = (PictureBox)c;
                    bullet.Top -= 5;

                    if (bullet.Location.Y <= 0)
                    {
                        this.Controls.Remove(bullet);
                    }
                    foreach (Control ct in this.Controls)
                    {
                        if (ct is PictureBox && ct.Name == "Laser")
                        {
                            PictureBox laser = (PictureBox)ct;

                            if (bullet.Bounds.IntersectsWith(laser.Bounds))
                            {
                                this.Controls.Remove(bullet);
                                this.Controls.Remove(laser);
                                pts++;
                                Score(pts);
                            }
                        }
                    }
                    foreach (Control ctrl in this.Controls)
                    {
                        if (ctrl is PictureBox && ctrl.Name == "Alien")
                        {
                            PictureBox alien = (PictureBox)ctrl;

                            if (bullet.Bounds.IntersectsWith(alien.Bounds) && !Touched(alien))
                            {
                                this.Controls.Remove(bullet);
                                this.Controls.Remove(alien);
                                aliens.Remove(alien);
                                pts += 5;
                                Score(pts);
                                CheckForWinner();
                            }
                            else if (bullet.Bounds.IntersectsWith(alien.Bounds) && Touched(alien))
                            {
                                this.Controls.Remove(bullet);
                                this.Controls.Remove(alien);
                                delay.Add(alien);
                                pts += 5;
                                Score(pts);
                                CheckForWinner();
                            }
                        }
                    }
                }
            }
        }

        private void getPositionCursor(object sender, EventArgs e)
        {
            MessageBox.Show(Cursor.Position.ToString());
        }

        private void Observe(object sender, EventArgs e)
        {
            Observer.Stop();

            foreach (PictureBox delayed in delay)
            {
                aliens.Remove(delayed);
            }
            delay.Clear();
        }

        private void Score(int pts)
        {
            backGround.score.Text = "Score: " + pts.ToString();
        }

        private void LoseLife()
        {
            player.Location = new Point(x, y);

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && c.Name.Contains("Life") && c.Visible == true)
                {
                    PictureBox player = (PictureBox)c;
                    player.Visible = false;
                    return;
                }
            }
            gameOver();
        }

        private void gameOver()
        {
            timer1.Stop(); timer2.Stop(); timer3.Stop(); timer4.Stop(); timer5.Stop(); Observer.Stop();

            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Name == "Finish")
                {
                    Label lbl = (Label)c;
                    lbl.Text = "Game Over!";
                    game = false;
                }
                else
                {
                    c.Visible = false;
                }
            }
        }

        private void CheckForWinner()
        {
            int count = 0;

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && c.Name == "Alien") count++;
            }

            if (count == 0) YouWon();
        }

        private void YouWon()
        {
            game = false;

            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Name == "Finish")
                {
                    Label lbl = (Label)c;
                    lbl.Text = "You Won!" + "\n"
                               + "Score: " + pts.ToString();
                }
                else
                {
                    c.Visible = false;
                }
            }
        }
    }
}
