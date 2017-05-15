namespace DQ12SFCRando
{
    partial class Form1
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
            this.label10 = new System.Windows.Forms.Label();
            this.txtFlags = new System.Windows.Forms.TextBox();
            this.btnNewSeed = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.btnCompareBrowse = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCompare = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.lblReqChecksum = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSHAChecksum = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.chkMonsterZones = new System.Windows.Forms.CheckBox();
            this.chkMonsterPatterns = new System.Windows.Forms.CheckBox();
            this.chkHeroStats = new System.Windows.Forms.CheckBox();
            this.chkTreasures = new System.Windows.Forms.CheckBox();
            this.chkStores = new System.Windows.Forms.CheckBox();
            this.chkMonsterZonesCross = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkMonsterPatternsCross = new System.Windows.Forms.CheckBox();
            this.chkTreasuresCross = new System.Windows.Forms.CheckBox();
            this.chkStoresCross = new System.Windows.Forms.CheckBox();
            this.trkExperience = new System.Windows.Forms.TrackBar();
            this.trkGoldReq = new System.Windows.Forms.TrackBar();
            this.lblExperience = new System.Windows.Forms.Label();
            this.lblGoldReq = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmdRandomize = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trkExperience)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkGoldReq)).BeginInit();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(291, 113);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 13);
            this.label10.TabIndex = 58;
            this.label10.Text = "Flags";
            // 
            // txtFlags
            // 
            this.txtFlags.Location = new System.Drawing.Point(329, 109);
            this.txtFlags.Name = "txtFlags";
            this.txtFlags.Size = new System.Drawing.Size(200, 20);
            this.txtFlags.TabIndex = 57;
            this.txtFlags.Leave += new System.EventHandler(this.determineChecks);
            // 
            // btnNewSeed
            // 
            this.btnNewSeed.Location = new System.Drawing.Point(186, 109);
            this.btnNewSeed.Name = "btnNewSeed";
            this.btnNewSeed.Size = new System.Drawing.Size(75, 23);
            this.btnNewSeed.TabIndex = 49;
            this.btnNewSeed.Text = "New Seed";
            this.btnNewSeed.UseVisualStyleBackColor = true;
            this.btnNewSeed.Click += new System.EventHandler(this.btnNewSeed_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 56;
            this.label3.Text = "Seed";
            // 
            // txtSeed
            // 
            this.txtSeed.Location = new System.Drawing.Point(69, 111);
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(100, 20);
            this.txtSeed.TabIndex = 48;
            // 
            // btnCompareBrowse
            // 
            this.btnCompareBrowse.Location = new System.Drawing.Point(458, 33);
            this.btnCompareBrowse.Name = "btnCompareBrowse";
            this.btnCompareBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnCompareBrowse.TabIndex = 46;
            this.btnCompareBrowse.Text = "Browse";
            this.btnCompareBrowse.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 55;
            this.label5.Text = "Comparison Image";
            // 
            // txtCompare
            // 
            this.txtCompare.Location = new System.Drawing.Point(132, 35);
            this.txtCompare.Name = "txtCompare";
            this.txtCompare.Size = new System.Drawing.Size(320, 20);
            this.txtCompare.TabIndex = 45;
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(458, 62);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(75, 23);
            this.btnCompare.TabIndex = 47;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            // 
            // lblReqChecksum
            // 
            this.lblReqChecksum.AutoSize = true;
            this.lblReqChecksum.Location = new System.Drawing.Point(129, 88);
            this.lblReqChecksum.Name = "lblReqChecksum";
            this.lblReqChecksum.Size = new System.Drawing.Size(238, 13);
            this.lblReqChecksum.TabIndex = 54;
            this.lblReqChecksum.Text = "1c0c6d78bf2bc29160adf48b17ebf5a5bc46230e";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 53;
            this.label4.Text = "Required";
            // 
            // lblSHAChecksum
            // 
            this.lblSHAChecksum.AutoSize = true;
            this.lblSHAChecksum.Location = new System.Drawing.Point(129, 64);
            this.lblSHAChecksum.Name = "lblSHAChecksum";
            this.lblSHAChecksum.Size = new System.Drawing.Size(247, 13);
            this.lblSHAChecksum.TabIndex = 52;
            this.lblSHAChecksum.Text = "????????????????????????????????????????";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "SHA1 Checksum";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(458, 7);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 44;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "DQ 1+2 ROM Image";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(132, 9);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(320, 20);
            this.txtFileName.TabIndex = 43;
            // 
            // chkMonsterZones
            // 
            this.chkMonsterZones.AutoSize = true;
            this.chkMonsterZones.Location = new System.Drawing.Point(12, 179);
            this.chkMonsterZones.Name = "chkMonsterZones";
            this.chkMonsterZones.Size = new System.Drawing.Size(153, 17);
            this.chkMonsterZones.TabIndex = 59;
            this.chkMonsterZones.Text = "Randomize Monster Zones";
            this.chkMonsterZones.UseVisualStyleBackColor = true;
            this.chkMonsterZones.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkMonsterPatterns
            // 
            this.chkMonsterPatterns.AutoSize = true;
            this.chkMonsterPatterns.Location = new System.Drawing.Point(12, 202);
            this.chkMonsterPatterns.Name = "chkMonsterPatterns";
            this.chkMonsterPatterns.Size = new System.Drawing.Size(162, 17);
            this.chkMonsterPatterns.TabIndex = 60;
            this.chkMonsterPatterns.Text = "Randomize Monster Patterns";
            this.chkMonsterPatterns.UseVisualStyleBackColor = true;
            this.chkMonsterPatterns.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkHeroStats
            // 
            this.chkHeroStats.AutoSize = true;
            this.chkHeroStats.Location = new System.Drawing.Point(12, 225);
            this.chkHeroStats.Name = "chkHeroStats";
            this.chkHeroStats.Size = new System.Drawing.Size(132, 17);
            this.chkHeroStats.TabIndex = 61;
            this.chkHeroStats.Text = "Randomize Hero Stats";
            this.chkHeroStats.UseVisualStyleBackColor = true;
            this.chkHeroStats.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkTreasures
            // 
            this.chkTreasures.AutoSize = true;
            this.chkTreasures.Enabled = false;
            this.chkTreasures.Location = new System.Drawing.Point(12, 248);
            this.chkTreasures.Name = "chkTreasures";
            this.chkTreasures.Size = new System.Drawing.Size(129, 17);
            this.chkTreasures.TabIndex = 62;
            this.chkTreasures.Text = "Randomize Treasures";
            this.chkTreasures.UseVisualStyleBackColor = true;
            this.chkTreasures.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkStores
            // 
            this.chkStores.AutoSize = true;
            this.chkStores.Location = new System.Drawing.Point(12, 271);
            this.chkStores.Name = "chkStores";
            this.chkStores.Size = new System.Drawing.Size(112, 17);
            this.chkStores.TabIndex = 63;
            this.chkStores.Text = "Randomize Stores";
            this.chkStores.UseVisualStyleBackColor = true;
            this.chkStores.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkMonsterZonesCross
            // 
            this.chkMonsterZonesCross.AutoSize = true;
            this.chkMonsterZonesCross.Enabled = false;
            this.chkMonsterZonesCross.Location = new System.Drawing.Point(233, 179);
            this.chkMonsterZonesCross.Name = "chkMonsterZonesCross";
            this.chkMonsterZonesCross.Size = new System.Drawing.Size(15, 14);
            this.chkMonsterZonesCross.TabIndex = 67;
            this.chkMonsterZonesCross.UseVisualStyleBackColor = true;
            this.chkMonsterZonesCross.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(208, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 68;
            this.label6.Text = "Cross Game";
            // 
            // chkMonsterPatternsCross
            // 
            this.chkMonsterPatternsCross.AutoSize = true;
            this.chkMonsterPatternsCross.Enabled = false;
            this.chkMonsterPatternsCross.Location = new System.Drawing.Point(233, 202);
            this.chkMonsterPatternsCross.Name = "chkMonsterPatternsCross";
            this.chkMonsterPatternsCross.Size = new System.Drawing.Size(15, 14);
            this.chkMonsterPatternsCross.TabIndex = 69;
            this.chkMonsterPatternsCross.UseVisualStyleBackColor = true;
            this.chkMonsterPatternsCross.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkTreasuresCross
            // 
            this.chkTreasuresCross.AutoSize = true;
            this.chkTreasuresCross.Enabled = false;
            this.chkTreasuresCross.Location = new System.Drawing.Point(233, 248);
            this.chkTreasuresCross.Name = "chkTreasuresCross";
            this.chkTreasuresCross.Size = new System.Drawing.Size(15, 14);
            this.chkTreasuresCross.TabIndex = 72;
            this.chkTreasuresCross.UseVisualStyleBackColor = true;
            this.chkTreasuresCross.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkStoresCross
            // 
            this.chkStoresCross.AutoSize = true;
            this.chkStoresCross.Enabled = false;
            this.chkStoresCross.Location = new System.Drawing.Point(233, 271);
            this.chkStoresCross.Name = "chkStoresCross";
            this.chkStoresCross.Size = new System.Drawing.Size(15, 14);
            this.chkStoresCross.TabIndex = 73;
            this.chkStoresCross.UseVisualStyleBackColor = true;
            this.chkStoresCross.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // trkExperience
            // 
            this.trkExperience.Location = new System.Drawing.Point(224, 294);
            this.trkExperience.Maximum = 50;
            this.trkExperience.Minimum = 5;
            this.trkExperience.Name = "trkExperience";
            this.trkExperience.Size = new System.Drawing.Size(104, 45);
            this.trkExperience.TabIndex = 74;
            this.trkExperience.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkExperience.Value = 10;
            this.trkExperience.Scroll += new System.EventHandler(this.trkExperience_Scroll);
            // 
            // trkGoldReq
            // 
            this.trkGoldReq.Location = new System.Drawing.Point(224, 317);
            this.trkGoldReq.Maximum = 50;
            this.trkGoldReq.Minimum = 10;
            this.trkGoldReq.Name = "trkGoldReq";
            this.trkGoldReq.Size = new System.Drawing.Size(104, 45);
            this.trkGoldReq.TabIndex = 75;
            this.trkGoldReq.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkGoldReq.Value = 10;
            this.trkGoldReq.Scroll += new System.EventHandler(this.trkGoldReq_Scroll);
            // 
            // lblExperience
            // 
            this.lblExperience.AutoSize = true;
            this.lblExperience.Location = new System.Drawing.Point(334, 295);
            this.lblExperience.Name = "lblExperience";
            this.lblExperience.Size = new System.Drawing.Size(33, 13);
            this.lblExperience.TabIndex = 78;
            this.lblExperience.Text = "100%";
            // 
            // lblGoldReq
            // 
            this.lblGoldReq.AutoSize = true;
            this.lblGoldReq.Location = new System.Drawing.Point(334, 318);
            this.lblGoldReq.Name = "lblGoldReq";
            this.lblGoldReq.Size = new System.Drawing.Size(33, 13);
            this.lblGoldReq.TabIndex = 79;
            this.lblGoldReq.Text = "100%";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(10, 355);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 80;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 295);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 13);
            this.label8.TabIndex = 82;
            this.label8.Text = "Boost Experience/GP";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 318);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(153, 13);
            this.label9.TabIndex = 83;
            this.label9.Text = "Randomize Gold Requirements";
            // 
            // cmdRandomize
            // 
            this.cmdRandomize.Location = new System.Drawing.Point(458, 313);
            this.cmdRandomize.Name = "cmdRandomize";
            this.cmdRandomize.Size = new System.Drawing.Size(75, 23);
            this.cmdRandomize.TabIndex = 84;
            this.cmdRandomize.Text = "Randomize";
            this.cmdRandomize.UseVisualStyleBackColor = true;
            this.cmdRandomize.Click += new System.EventHandler(this.cmdRandomize_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 382);
            this.Controls.Add(this.cmdRandomize);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblGoldReq);
            this.Controls.Add(this.lblExperience);
            this.Controls.Add(this.trkGoldReq);
            this.Controls.Add(this.trkExperience);
            this.Controls.Add(this.chkStoresCross);
            this.Controls.Add(this.chkTreasuresCross);
            this.Controls.Add(this.chkMonsterPatternsCross);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkMonsterZonesCross);
            this.Controls.Add(this.chkStores);
            this.Controls.Add(this.chkTreasures);
            this.Controls.Add(this.chkHeroStats);
            this.Controls.Add(this.chkMonsterPatterns);
            this.Controls.Add(this.chkMonsterZones);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtFlags);
            this.Controls.Add(this.btnNewSeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSeed);
            this.Controls.Add(this.btnCompareBrowse);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCompare);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.lblReqChecksum);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSHAChecksum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFileName);
            this.Name = "Form1";
            this.Text = "Dragon Quest 1+2 Randomizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trkExperience)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkGoldReq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtFlags;
        private System.Windows.Forms.Button btnNewSeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSeed;
        private System.Windows.Forms.Button btnCompareBrowse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCompare;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label lblReqChecksum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblSHAChecksum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.CheckBox chkMonsterZones;
        private System.Windows.Forms.CheckBox chkMonsterPatterns;
        private System.Windows.Forms.CheckBox chkHeroStats;
        private System.Windows.Forms.CheckBox chkTreasures;
        private System.Windows.Forms.CheckBox chkStores;
        private System.Windows.Forms.CheckBox chkMonsterZonesCross;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkMonsterPatternsCross;
        private System.Windows.Forms.CheckBox chkTreasuresCross;
        private System.Windows.Forms.CheckBox chkStoresCross;
        private System.Windows.Forms.TrackBar trkExperience;
        private System.Windows.Forms.TrackBar trkGoldReq;
        private System.Windows.Forms.Label lblExperience;
        private System.Windows.Forms.Label lblGoldReq;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button cmdRandomize;
    }
}

