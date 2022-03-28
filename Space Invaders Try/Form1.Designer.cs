using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Space_Invaders_Try.GameObjects;

namespace Space_Invaders_Try
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Label lifeLabel;
        private PictureBox lifeIcon;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Form1";
            this.Text = "Space Invaders";
            this.Click += new System.EventHandler(this.getPositionCursor);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Pressed);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Released);
            this.game = true;
            //this.player = new Player();
            //this.backGround = new Background();
            this.ResumeLayout(false);

        }


        private void InsertAliens()
        {

            foreach (PictureBox enemy in enemies)
            {
                this.Controls.Add(enemy);
            }
            //foreach (Control c in this.Controls)
            //{
            //    if (c is PictureBox && c.Name == "Alien")
            //    {
            //        PictureBox alien = (PictureBox)c;
            //        aliens.Add(alien);
            //    }
            //}
        }

        private void insertLifeLabel()
        {
            lifeLabel = new Label();
            lifeLabel.Location = new Point(0, 400);
            lifeLabel.AutoSize = true;
            lifeLabel.Text = "Life : ";
            lifeLabel.Font = new Font("Calibri", 18);
            lifeLabel.ForeColor = Color.White;
            lifeLabel.Padding = new Padding(6);
            this.Controls.Add(lifeLabel);
        }

        private void insertLifePictureBox()
        {
            lifeIcon = new PictureBox();
            lifeIcon.Location = new Point(5, 400);
            lifeIcon.Image = Properties.Resources.tank;
            lifeIcon.ForeColor = Color.White;
            lifeIcon.Name = "Life";
            this.Controls.Add(lifeIcon);
            lifeIcon = new PictureBox();
            lifeIcon.Location = new Point(40, 400);
            lifeIcon.Image = Properties.Resources.tank;
            lifeIcon.ForeColor = Color.White;
            lifeIcon.Name = "Life";
            this.Controls.Add(lifeIcon);
        }
        #endregion
    }
}

