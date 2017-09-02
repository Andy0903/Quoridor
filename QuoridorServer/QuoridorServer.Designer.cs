namespace QuoridorServer
{
    partial class QuoridorServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PortGroup = new System.Windows.Forms.GroupBox();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.NumberOfPlayersGroup = new System.Windows.Forms.GroupBox();
            this.RadioButton4Player = new System.Windows.Forms.RadioButton();
            this.RadioButton2Players = new System.Windows.Forms.RadioButton();
            this.LogGroup = new System.Windows.Forms.GroupBox();
            this.LogTextBox = new System.Windows.Forms.RichTextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.PlayerLabel = new System.Windows.Forms.Label();
            this.tickTimer = new System.Windows.Forms.Timer(this.components);
            this.delayNumeric = new System.Windows.Forms.NumericUpDown();
            this.DelayGroup = new System.Windows.Forms.GroupBox();
            this.PortGroup.SuspendLayout();
            this.NumberOfPlayersGroup.SuspendLayout();
            this.LogGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).BeginInit();
            this.DelayGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // PortGroup
            // 
            this.PortGroup.Controls.Add(this.PortTextBox);
            this.PortGroup.Location = new System.Drawing.Point(12, 12);
            this.PortGroup.Name = "PortGroup";
            this.PortGroup.Size = new System.Drawing.Size(158, 43);
            this.PortGroup.TabIndex = 0;
            this.PortGroup.TabStop = false;
            this.PortGroup.Text = "Port";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(6, 17);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(146, 20);
            this.PortTextBox.TabIndex = 4;
            this.PortTextBox.Text = "14242";
            // 
            // NumberOfPlayersGroup
            // 
            this.NumberOfPlayersGroup.Controls.Add(this.RadioButton4Player);
            this.NumberOfPlayersGroup.Controls.Add(this.RadioButton2Players);
            this.NumberOfPlayersGroup.Location = new System.Drawing.Point(12, 61);
            this.NumberOfPlayersGroup.Name = "NumberOfPlayersGroup";
            this.NumberOfPlayersGroup.Size = new System.Drawing.Size(158, 55);
            this.NumberOfPlayersGroup.TabIndex = 1;
            this.NumberOfPlayersGroup.TabStop = false;
            this.NumberOfPlayersGroup.Text = "Number of Players";
            // 
            // RadioButton4Player
            // 
            this.RadioButton4Player.AutoSize = true;
            this.RadioButton4Player.Location = new System.Drawing.Point(43, 19);
            this.RadioButton4Player.Name = "RadioButton4Player";
            this.RadioButton4Player.Size = new System.Drawing.Size(31, 17);
            this.RadioButton4Player.TabIndex = 3;
            this.RadioButton4Player.Text = "4";
            this.RadioButton4Player.UseVisualStyleBackColor = true;
            // 
            // RadioButton2Players
            // 
            this.RadioButton2Players.AutoSize = true;
            this.RadioButton2Players.Checked = true;
            this.RadioButton2Players.Location = new System.Drawing.Point(6, 19);
            this.RadioButton2Players.Name = "RadioButton2Players";
            this.RadioButton2Players.Size = new System.Drawing.Size(31, 17);
            this.RadioButton2Players.TabIndex = 2;
            this.RadioButton2Players.TabStop = true;
            this.RadioButton2Players.Text = "2";
            this.RadioButton2Players.UseVisualStyleBackColor = true;
            // 
            // LogGroup
            // 
            this.LogGroup.Controls.Add(this.LogTextBox);
            this.LogGroup.Location = new System.Drawing.Point(12, 185);
            this.LogGroup.Name = "LogGroup";
            this.LogGroup.Size = new System.Drawing.Size(283, 292);
            this.LogGroup.TabIndex = 1;
            this.LogGroup.TabStop = false;
            this.LogGroup.Text = "Log";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(6, 19);
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.ReadOnly = true;
            this.LogTextBox.Size = new System.Drawing.Size(271, 330);
            this.LogTextBox.TabIndex = 2;
            this.LogTextBox.Text = "";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(190, 156);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(104, 23);
            this.StartButton.TabIndex = 3;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // PlayerLabel
            // 
            this.PlayerLabel.AutoSize = true;
            this.PlayerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerLabel.Location = new System.Drawing.Point(198, 25);
            this.PlayerLabel.Name = "PlayerLabel";
            this.PlayerLabel.Size = new System.Drawing.Size(91, 24);
            this.PlayerLabel.TabIndex = 4;
            this.PlayerLabel.Text = "Players: 0";
            // 
            // tickTimer
            // 
            this.tickTimer.Tick += new System.EventHandler(this.tickTimer_Tick);
            // 
            // delayNumeric
            // 
            this.delayNumeric.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.delayNumeric.Location = new System.Drawing.Point(6, 19);
            this.delayNumeric.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.delayNumeric.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.delayNumeric.Name = "delayNumeric";
            this.delayNumeric.Size = new System.Drawing.Size(146, 20);
            this.delayNumeric.TabIndex = 5;
            this.delayNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // DelayGroup
            // 
            this.DelayGroup.Controls.Add(this.delayNumeric);
            this.DelayGroup.Location = new System.Drawing.Point(12, 122);
            this.DelayGroup.Name = "DelayGroup";
            this.DelayGroup.Size = new System.Drawing.Size(158, 57);
            this.DelayGroup.TabIndex = 6;
            this.DelayGroup.TabStop = false;
            this.DelayGroup.Text = "Delay (ms)";
            // 
            // QuoridorServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 489);
            this.Controls.Add(this.DelayGroup);
            this.Controls.Add(this.PlayerLabel);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.LogGroup);
            this.Controls.Add(this.NumberOfPlayersGroup);
            this.Controls.Add(this.PortGroup);
            this.Name = "QuoridorServerForm";
            this.Text = "Quoridor Server";
            this.PortGroup.ResumeLayout(false);
            this.PortGroup.PerformLayout();
            this.NumberOfPlayersGroup.ResumeLayout(false);
            this.NumberOfPlayersGroup.PerformLayout();
            this.LogGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.delayNumeric)).EndInit();
            this.DelayGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox PortGroup;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.GroupBox NumberOfPlayersGroup;
        private System.Windows.Forms.RadioButton RadioButton4Player;
        private System.Windows.Forms.RadioButton RadioButton2Players;
        private System.Windows.Forms.GroupBox LogGroup;
        public System.Windows.Forms.RichTextBox LogTextBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label PlayerLabel;
        private System.Windows.Forms.Timer tickTimer;
        private System.Windows.Forms.NumericUpDown delayNumeric;
        private System.Windows.Forms.GroupBox DelayGroup;
    }
}

