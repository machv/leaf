namespace DescriptorCreator
{
	partial class MainForm
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
			this.LeafPicture = new System.Windows.Forms.PictureBox();
			this.NormalizeColors = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.referencialPointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.getListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.drawLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ArgbLabel = new System.Windows.Forms.Label();
			this.RevertButton = new System.Windows.Forms.Button();
			this.controlPanel = new System.Windows.Forms.Panel();
			this.buttonEdgeDetection = new System.Windows.Forms.Button();
			this.histogramaDesenat1 = new Histograma.HistogramaDesenat();
			this.buttonHistogram = new System.Windows.Forms.Button();
			this.centroidLinesButton = new System.Windows.Forms.Button();
			this.centroidButton = new System.Windows.Forms.Button();
			this.pathButton = new System.Windows.Forms.Button();
			this.NormalizeTreshold = new System.Windows.Forms.NumericUpDown();
			this.drawDescriptorHistorgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.LeafPicture)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.controlPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.NormalizeTreshold)).BeginInit();
			this.SuspendLayout();
			// 
			// LeafPicture
			// 
			this.LeafPicture.Location = new System.Drawing.Point(12, 47);
			this.LeafPicture.Name = "LeafPicture";
			this.LeafPicture.Size = new System.Drawing.Size(400, 400);
			this.LeafPicture.TabIndex = 0;
			this.LeafPicture.TabStop = false;
			this.LeafPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.LeafPicture_Paint);
			this.LeafPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LeafPicture_MouseMove);
			// 
			// NormalizeColors
			// 
			this.NormalizeColors.Location = new System.Drawing.Point(16, 4);
			this.NormalizeColors.Name = "NormalizeColors";
			this.NormalizeColors.Size = new System.Drawing.Size(140, 43);
			this.NormalizeColors.TabIndex = 1;
			this.NormalizeColors.Text = "Threshold";
			this.NormalizeColors.UseVisualStyleBackColor = true;
			this.NormalizeColors.Click += new System.EventHandler(this.NormalizeColors_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.referencialPointsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(638, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// referencialPointsToolStripMenuItem
			// 
			this.referencialPointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getListToolStripMenuItem,
            this.drawLinesToolStripMenuItem,
            this.drawDescriptorHistorgramToolStripMenuItem});
			this.referencialPointsToolStripMenuItem.Name = "referencialPointsToolStripMenuItem";
			this.referencialPointsToolStripMenuItem.Size = new System.Drawing.Size(113, 20);
			this.referencialPointsToolStripMenuItem.Text = "Referencial Points";
			// 
			// getListToolStripMenuItem
			// 
			this.getListToolStripMenuItem.Name = "getListToolStripMenuItem";
			this.getListToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
			this.getListToolStripMenuItem.Text = "Get list";
			this.getListToolStripMenuItem.Click += new System.EventHandler(this.getListToolStripMenuItem_Click);
			// 
			// drawLinesToolStripMenuItem
			// 
			this.drawLinesToolStripMenuItem.Name = "drawLinesToolStripMenuItem";
			this.drawLinesToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
			this.drawLinesToolStripMenuItem.Text = "Draw lines";
			this.drawLinesToolStripMenuItem.Click += new System.EventHandler(this.drawLinesToolStripMenuItem_Click);
			// 
			// ArgbLabel
			// 
			this.ArgbLabel.AutoSize = true;
			this.ArgbLabel.Location = new System.Drawing.Point(13, 454);
			this.ArgbLabel.Name = "ArgbLabel";
			this.ArgbLabel.Size = new System.Drawing.Size(0, 13);
			this.ArgbLabel.TabIndex = 3;
			// 
			// RevertButton
			// 
			this.RevertButton.Location = new System.Drawing.Point(116, 411);
			this.RevertButton.Name = "RevertButton";
			this.RevertButton.Size = new System.Drawing.Size(75, 23);
			this.RevertButton.TabIndex = 5;
			this.RevertButton.Text = "Revert";
			this.RevertButton.UseVisualStyleBackColor = true;
			this.RevertButton.Click += new System.EventHandler(this.RevertButton_Click);
			// 
			// controlPanel
			// 
			this.controlPanel.Controls.Add(this.buttonEdgeDetection);
			this.controlPanel.Controls.Add(this.histogramaDesenat1);
			this.controlPanel.Controls.Add(this.buttonHistogram);
			this.controlPanel.Controls.Add(this.centroidLinesButton);
			this.controlPanel.Controls.Add(this.centroidButton);
			this.controlPanel.Controls.Add(this.pathButton);
			this.controlPanel.Controls.Add(this.RevertButton);
			this.controlPanel.Controls.Add(this.NormalizeTreshold);
			this.controlPanel.Controls.Add(this.NormalizeColors);
			this.controlPanel.Location = new System.Drawing.Point(424, 36);
			this.controlPanel.Name = "controlPanel";
			this.controlPanel.Size = new System.Drawing.Size(202, 445);
			this.controlPanel.TabIndex = 6;
			// 
			// buttonEdgeDetection
			// 
			this.buttonEdgeDetection.Location = new System.Drawing.Point(16, 231);
			this.buttonEdgeDetection.Name = "buttonEdgeDetection";
			this.buttonEdgeDetection.Size = new System.Drawing.Size(140, 40);
			this.buttonEdgeDetection.TabIndex = 11;
			this.buttonEdgeDetection.Text = "Edge Detection";
			this.buttonEdgeDetection.UseVisualStyleBackColor = true;
			this.buttonEdgeDetection.Click += new System.EventHandler(this.buttonEdgeDetection_Click);
			// 
			// histogramaDesenat1
			// 
			this.histogramaDesenat1.DisplayColor = System.Drawing.Color.Black;
			this.histogramaDesenat1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.histogramaDesenat1.Location = new System.Drawing.Point(3, 309);
			this.histogramaDesenat1.Name = "histogramaDesenat1";
			this.histogramaDesenat1.Offset = 20;
			this.histogramaDesenat1.Size = new System.Drawing.Size(196, 96);
			this.histogramaDesenat1.TabIndex = 10;
			// 
			// buttonHistogram
			// 
			this.buttonHistogram.Location = new System.Drawing.Point(16, 411);
			this.buttonHistogram.Name = "buttonHistogram";
			this.buttonHistogram.Size = new System.Drawing.Size(75, 23);
			this.buttonHistogram.TabIndex = 9;
			this.buttonHistogram.Text = "Histogram";
			this.buttonHistogram.UseVisualStyleBackColor = true;
			this.buttonHistogram.Click += new System.EventHandler(this.buttonHistogram_Click);
			// 
			// centroidLinesButton
			// 
			this.centroidLinesButton.Location = new System.Drawing.Point(16, 185);
			this.centroidLinesButton.Name = "centroidLinesButton";
			this.centroidLinesButton.Size = new System.Drawing.Size(140, 40);
			this.centroidLinesButton.TabIndex = 8;
			this.centroidLinesButton.Text = "Draw Centroid Lines";
			this.centroidLinesButton.UseVisualStyleBackColor = true;
			this.centroidLinesButton.Click += new System.EventHandler(this.centroidLinesButton_Click);
			// 
			// centroidButton
			// 
			this.centroidButton.Location = new System.Drawing.Point(16, 139);
			this.centroidButton.Name = "centroidButton";
			this.centroidButton.Size = new System.Drawing.Size(140, 40);
			this.centroidButton.TabIndex = 7;
			this.centroidButton.Text = "Draw Centroid";
			this.centroidButton.UseVisualStyleBackColor = true;
			this.centroidButton.Click += new System.EventHandler(this.centroidButton_Click);
			// 
			// pathButton
			// 
			this.pathButton.Location = new System.Drawing.Point(16, 93);
			this.pathButton.Name = "pathButton";
			this.pathButton.Size = new System.Drawing.Size(140, 40);
			this.pathButton.TabIndex = 6;
			this.pathButton.Text = "Draw Contour";
			this.pathButton.UseVisualStyleBackColor = true;
			this.pathButton.Click += new System.EventHandler(this.pathButton_Click);
			// 
			// NormalizeTreshold
			// 
			this.NormalizeTreshold.Location = new System.Drawing.Point(36, 53);
			this.NormalizeTreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.NormalizeTreshold.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
			this.NormalizeTreshold.Name = "NormalizeTreshold";
			this.NormalizeTreshold.Size = new System.Drawing.Size(120, 20);
			this.NormalizeTreshold.TabIndex = 4;
			this.NormalizeTreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// drawDescriptorHistorgramToolStripMenuItem
			// 
			this.drawDescriptorHistorgramToolStripMenuItem.Name = "drawDescriptorHistorgramToolStripMenuItem";
			this.drawDescriptorHistorgramToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
			this.drawDescriptorHistorgramToolStripMenuItem.Text = "Draw descriptor histogram";
			this.drawDescriptorHistorgramToolStripMenuItem.Click += new System.EventHandler(this.drawDescriptorHistorgramToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(638, 493);
			this.Controls.Add(this.controlPanel);
			this.Controls.Add(this.ArgbLabel);
			this.Controls.Add(this.LeafPicture);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.Text = "Descriptor Extractor";
			((System.ComponentModel.ISupportInitialize)(this.LeafPicture)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.controlPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.NormalizeTreshold)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox LeafPicture;
		private System.Windows.Forms.Button NormalizeColors;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.Label ArgbLabel;
		private System.Windows.Forms.Button RevertButton;
		private System.Windows.Forms.Panel controlPanel;
		private System.Windows.Forms.NumericUpDown NormalizeTreshold;
		private System.Windows.Forms.Button pathButton;
		private System.Windows.Forms.Button centroidButton;
		private System.Windows.Forms.Button centroidLinesButton;
        private System.Windows.Forms.Button buttonHistogram;
        private Histograma.HistogramaDesenat histogramaDesenat1;
        private System.Windows.Forms.Button buttonEdgeDetection;
		private System.Windows.Forms.ToolStripMenuItem referencialPointsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem getListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawLinesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem drawDescriptorHistorgramToolStripMenuItem;
	}
}

