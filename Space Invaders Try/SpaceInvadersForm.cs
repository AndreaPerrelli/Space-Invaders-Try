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
    public partial class SpaceInvadersForm : Form
    {
        private bool fired, moveLeft, moveRight, game;
        private int limit = 700, top, left, cnt, speed, pts, x, y;
        private Timer timer;
        private Timer Observer = new Timer();
        private List<PictureBox> delay, enemiesBox;
        private Label scoreLabel, finishLabel;
        private Player player;
        private Enemies enemies;
        private PictureBox playerBox;
        public int timerMoveAliens = 500, timerFireAliens = 2000, timerFirePlayer = 100, timerDetectLaser = 100; // 500, 2000, 100, 100 milliseconds

        public SpaceInvadersForm()
        {
            InitializeComponent();
            initializeGameState();

        }

        public void initTimer()
        {
            timer = new Timer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 100;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {

            timerMoveAliens -= 100; // -100 milliseconds 
            timerFireAliens -= 100;
            timerFirePlayer -= 100;
            timerDetectLaser -= 100;

            if (timerMoveAliens.Equals(0))
            {
                MoveAliens(sender, e);
                timerMoveAliens = 500;
            }
            if (timerFireAliens.Equals(0))
            {
                StrikeSpan(sender, e);
                timerFireAliens = 2000;
            }
            if (timerFirePlayer.Equals(0))
            {
                FireBullet(sender, e);
                timerFirePlayer = 100;
            }
            if (timerDetectLaser.Equals(0))
            {
                DetectLaser(sender, e);
                timerDetectLaser = 100;
            }
        }
        
       /* public void InitTimerMoveAliens()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(MoveAliens);
            timer1.Interval = 500; // in miliseconds
            timer1.Start();
        }
        public void InitTimerFireAliens()
        {
            timer2 = new Timer();
            timer2.Tick += new EventHandler(StrikeSpan);
            timer2.Interval = 2000; // in miliseconds
            timer2.Start();
        }
        
        public void InitTimerFirePlayer()
        {
            timer4 = new Timer();
            timer4.Tick += new EventHandler(FireBullet);
            timer4.Interval = 100;
            timer4.Start();
        }

        public void InitTimerDetectLaser()
        {
            timer5 = new Timer();
            timer5.Tick += new EventHandler(DetectLaser);
            timer5.Interval = 50;
            timer5.Start();
        }
       */
        private void Pressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                //if (!enemiesTimerStatus)
                //{
                //    InitTimerMoveAliens();
                //    InitTimerFireAliens();
                //    InitTimerDetectLaser();

                //}
                moveLeft = true;
                PlayerMove(sender, e);
            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                /*if (!enemiesTimerStatus)
                {
                    InitTimerMoveAliens();
                    InitTimerFireAliens();
                    InitTimerDetectLaser();

                }*/
                moveRight = true;
                PlayerMove(sender, e);
            }
            else if (e.KeyCode == Keys.Space && game && !fired)
            {
                /*if (!enemiesTimerStatus)
                {
                    InitTimerMoveAliens();
                    InitTimerFireAliens();
                    InitTimerDetectLaser();

                }
                */
                Missile();
                fired = true;
                //InitTimerFirePlayer();
            }

            else if(e.KeyCode == Keys.E)
            {
                ResetGame(sender, e);
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
                playerBox.Left -= player.GetMovementSpeed();
            }
            else if (moveRight && playerBox.Location.X <= limit)
            {
                playerBox.Left += player.GetMovementSpeed();
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
            foreach (PictureBox enemy in enemiesBox)
            {
                left = player.GetMovementSpeed();
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
            //enemiesTimerStatus = true;
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

            if (enemiesBox.Count > 0)
            {
                pick = r.Next(enemiesBox.Count);
                Beam(enemiesBox[pick]);
            }

        }

        private void DetectLaser(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && c.Name == "Laser")
                {
                    PictureBox laser = (PictureBox)c;
                    laser.Top += enemies.GetLaserSpeed();

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
                    bullet.Top -= player.GetBulletSpeed();

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
                                enemiesBox.Remove(alien);
                                ModifyGameStats();
                                pts += 5;
                                Score(pts);
                                CheckForWinner();
                            }
                            else if (bullet.Bounds.IntersectsWith(alien.Bounds) && Touched(alien))
                            {
                                this.Controls.Remove(bullet);
                                this.Controls.Remove(alien);
                                //delay.Add(alien);
                                pts += 5;
                                Score(pts);
                                CheckForWinner();
                            }
                        }
                    }
                }
            }
        }

        private void ModifyGameStats()
        {
            int enemyLaserSpeed = enemies.GetLaserSpeed();
            int enemyMovementSpeed = enemies.GetMovementSpeed();
            int playerMovementSpeed = player.GetMovementSpeed();
            int playerBulletSpeed = player.GetBulletSpeed();
               player.SetMovementSpeed(playerMovementSpeed++);
            player.SetBulletSpeed(playerBulletSpeed++);
            enemies.SetLaserSpeed(enemyLaserSpeed++);
            enemies.SetMovementSpeed(enemyMovementSpeed++);
        }

        private void FireLaser(object sender, EventArgs e)
        {

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
                enemiesBox.Remove(delayed);
            }
            delay.Clear();
        }

        private void Score(int pts)
        {

            scoreLabel.Text = "Score: " + pts.ToString();
        }

        private void LoseLife()
        {
            playerBox.Location = new Point(350, 400);

            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && c.Name.Contains("Life") && c.Visible == false)
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
            timer.Stop(); Observer.Stop();

            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Name == "Finish")
                {
                    finishLabel.Text = "Game Over!";
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
                    finishLabel.Text = "You Won!" + "\n"
                               + "Score: " + pts.ToString();

                }
                else
                {
                    c.Visible = false;
                }
            }
        }

        private void initializeGameState()
        {
            InitializeGameObjects();
            InitializePictureBox();
            InitializeLabel();
            InsertAliens();
            insertLifeLabel();
            insertLifePictureBox();
            initTimer();

        } 

        private void InitializeGameObjects()
        {
            player = new Player();
            enemies = new Enemies();
        }

        private void InitializePictureBox()
        {
            enemiesBox = new GameObjects.Enemies().CreateSprites(this);
            playerBox = new GameObjects.Player().CreateControlPlayer(this);
            Controls.Add(playerBox);
        }

        private void InitializeLabel()
        {
            scoreLabel = new GameObjects.Background().CreateLabelScore(this);
            Controls.Add(scoreLabel);
            finishLabel = new GameObjects.Background().CreateLabelFinish(this);
            Controls.Add(finishLabel);


        }

        private void ResetGame(object sender, EventArgs e)
        {
            timer.Stop(); Observer.Stop();
            SpaceInvadersForm NewForm = new SpaceInvadersForm();
            NewForm.Show();
            this.Dispose(false);
        }

    }


}
