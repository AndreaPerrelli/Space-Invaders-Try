using System.Collections.Generic;
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
        private List<PictureBox> aliens = new List<PictureBox>();

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
            this.player = new Player();
            this.backGround = new Background();
            this.ResumeLayout(false);

        }


        private void InsertAliens()
        {
            foreach (Control c in this.Controls)
            {
                if (c is PictureBox && c.Name == "Alien")
                {
                    PictureBox alien = (PictureBox)c;
                    aliens.Add(alien);
                }
            }
        }

        #endregion
    }
}

