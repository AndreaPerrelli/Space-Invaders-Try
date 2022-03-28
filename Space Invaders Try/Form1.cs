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
        private int limit = 700;
        private int top;
        private int left;
        private int cnt;
        private int speed;
        private bool enemiesTimerStatus;
        private Timer timer1;
        private Timer timer2;
        private Timer timer3 = new Timer();
        private Timer timer4 = new Timer();
        private Timer timer5 = new Timer();
        private Timer Observer = new Timer();
        private int pts;
        private List<PictureBox> delay;
        private Label label2;
        private int x, y;
        //private Player player;
        private Background backGround;
        private PictureBox playerBox;
        private List<PictureBox> enemies;

        public Form1()
        {
            InitializeComponent();
            enemies = new GameObjects.Enemies().CreateSprites(this);
            playerBox = new GameObjects.Player().CreateControlPlayer(this);
            Controls.Add(playerBox);
            label2 = new GameObjects.Background().CreateControlText(this);
            Controls.Add(label2);
            InsertAliens();
            insertLifeLabel();
            insertLifePictureBox();

        }
        public void InitTimerMoveAliens()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(MoveAliens);
            timer1.Interval = 100; // in miliseconds
            timer1.Start();
        }
        public void InitTimerFireAliens()
        {
            timer2 = new Timer();
            timer2.Tick += new EventHandler(StrikeSpan);
            timer2.Interval = 500; // in miliseconds
            timer2.Start();
        }

        private void Pressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                if (!enemiesTimerStatus)
                {
                    InitTimerMoveAliens();
                    InitTimerFireAliens();
                }
                moveLeft = true;
                PlayerMove(sender, e);
            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                if (!enemiesTimerStatus)
                {
                    InitTimerMoveAliens();
                    InitTimerFireAliens();
                    
                }
                moveRight = true;
                PlayerMove(sender, e);
            }
            else if (e.KeyCode == Keys.Space && game && !fired)
            {
                if (!enemiesTimerStatus)
                {
                    InitTimerMoveAliens();
                    InitTimerFireAliens();

                }

                Missile();
                fired = true;
                FireBullet(sender, e);
            }
        }

        private void Missile()
        {
            PictureBox bullet = new PictureBox();
            bullet.Location = new Point(playerBox.Location.X + playerBox.Width / 2, playerBox.Location.Y - 20);
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

        private void PlayerMove(object sender, EventArgs e)
        {
            if (moveLeft && playerBox.Location.X >= 0)
            {
                playerBox.Left--;
            }
            else if (moveRight && playerBox.Location.X <= limit)
            {
                playerBox.Left++;
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
            foreach (PictureBox enemy in enemies)
            {
                left = 1;
                top = 0;
                BeginInvoke(new Action(() =>
                {
                    enemy.Location = new Point(enemy.Location.X + left, enemy.Location.Y + top);
                }));
                SetDirection(enemy);
                Collided(enemy);
            }
        }

        private void Collided(PictureBox a)
        {
            if (a.Bounds.IntersectsWith(playerBox.Bounds))
            {
                gameOver();
            }
        }

        private void MoveAliens(object sender, EventArgs e)
        {
            AlienMove();
            enemiesTimerStatus = true;
        }

        private void Beam(PictureBox a)
        {
            PictureBox laser = new PictureBox();
            laser.Location = new Point(a.Location.X + a.Width / 3, a.Location.Y + 20);
            laser.Size = new Size(5, 20);
            laser.BackgroundImage = Properties.Resources.laser;
            laser.BackgroundImageLayout = ImageLayout.Stretch;
            laser.Name = "Laser";
            BeginInvoke(new Action(() =>
            {
                this.Controls.Add(laser);
            }));
        }

        private void StrikeSpan(object sender, EventArgs e)
        {
            Random r = new Random();
            int pick;

            if (enemies.Count > 0)
            {
                pick = r.Next(enemies.Count);
                Beam(enemies[pick]);
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
                    if (laser.Bounds.IntersectsWith(playerBox.Bounds))
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
                                enemies.Remove(alien);
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
                enemies.Remove(delayed);
            }
            delay.Clear();
        }

        private void Score(int pts)
        {

            label2.Text = "Score: " + pts.ToString();
        }

        private void LoseLife()
        {
            playerBox.Location = new Point(x, y);

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && c.Name.Contains("Life") && c.Visible == true)
                {
                    playerBox = (PictureBox)c;
                    playerBox.Visible = false;
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
