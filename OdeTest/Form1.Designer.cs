﻿namespace OdeTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            formsPlot = new ScottPlot.WinForms.FormsPlot();
            SuspendLayout();
            // 
            // formsPlot
            // 
            formsPlot.DisplayScale = 1F;
            formsPlot.Dock = DockStyle.Fill;
            formsPlot.Location = new Point(0, 0);
            formsPlot.Name = "formsPlot";
            formsPlot.Size = new Size(800, 450);
            formsPlot.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(formsPlot);
            Name = "Form1";
            Text = "Example";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private ScottPlot.WinForms.FormsPlot formsPlot;
    }
}
