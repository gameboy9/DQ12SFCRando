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
            this.lblFlags = new System.Windows.Forms.Label();
            this.txtFlags = new System.Windows.Forms.TextBox();
            this.btnNewSeed = new System.Windows.Forms.Button();
            this.lblSeed = new System.Windows.Forms.Label();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.btnCompareBrowse = new System.Windows.Forms.Button();
            this.lblCompareImage = new System.Windows.Forms.Label();
            this.txtCompare = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.lblReqChecksum = new System.Windows.Forms.Label();
            this.lblRequired = new System.Windows.Forms.Label();
            this.lblSHAChecksum = new System.Windows.Forms.Label();
            this.lblSHA = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblRomImage = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.chkMonsterZones = new System.Windows.Forms.CheckBox();
            this.chkMonsterPatterns = new System.Windows.Forms.CheckBox();
            this.chkHeroStats = new System.Windows.Forms.CheckBox();
            this.chkTreasures = new System.Windows.Forms.CheckBox();
            this.chkStores = new System.Windows.Forms.CheckBox();
            this.chkMonsterZonesCross = new System.Windows.Forms.CheckBox();
            this.lblCrossGame = new System.Windows.Forms.Label();
            this.chkMonsterPatternsCross = new System.Windows.Forms.CheckBox();
            this.chkTreasuresCross = new System.Windows.Forms.CheckBox();
            this.chkStoresCross = new System.Windows.Forms.CheckBox();
            this.trkExperience = new System.Windows.Forms.TrackBar();
            this.trkGoldReq = new System.Windows.Forms.TrackBar();
            this.lblExperience = new System.Windows.Forms.Label();
            this.lblGoldReq = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblExpBoost = new System.Windows.Forms.Label();
            this.lblGPReq = new System.Windows.Forms.Label();
            this.cmdRandomize = new System.Windows.Forms.Button();
            this.chkMap = new System.Windows.Forms.CheckBox();
            this.chkSmallMap = new System.Windows.Forms.CheckBox();
            this.radENG = new System.Windows.Forms.RadioButton();
            this.radJP = new System.Windows.Forms.RadioButton();
            this.lblRandomizeHeader = new System.Windows.Forms.Label();
            this.chkBattleSpeedHacks = new System.Windows.Forms.CheckBox();
            this.chkDoubleWalking = new System.Windows.Forms.CheckBox();
            this.chkHalfEncounterRate = new System.Windows.Forms.CheckBox();
            this.chkTextSpeedHacks = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trkExperience)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkGoldReq)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFlags
            // 
            this.lblFlags.AutoSize = true;
            this.lblFlags.Location = new System.Drawing.Point(291, 113);
            this.lblFlags.Name = "lblFlags";
            this.lblFlags.Size = new System.Drawing.Size(32, 13);
            this.lblFlags.TabIndex = 58;
            this.lblFlags.Text = "Flags";
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
            this.btnNewSeed.Size = new System.Drawing.Size(86, 23);
            this.btnNewSeed.TabIndex = 49;
            this.btnNewSeed.Text = "New Seed";
            this.btnNewSeed.UseVisualStyleBackColor = true;
            this.btnNewSeed.Click += new System.EventHandler(this.btnNewSeed_Click);
            // 
            // lblSeed
            // 
            this.lblSeed.AutoSize = true;
            this.lblSeed.Location = new System.Drawing.Point(12, 113);
            this.lblSeed.Name = "lblSeed";
            this.lblSeed.Size = new System.Drawing.Size(32, 13);
            this.lblSeed.TabIndex = 56;
            this.lblSeed.Text = "Seed";
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
            // lblCompareImage
            // 
            this.lblCompareImage.AutoSize = true;
            this.lblCompareImage.Location = new System.Drawing.Point(12, 35);
            this.lblCompareImage.Name = "lblCompareImage";
            this.lblCompareImage.Size = new System.Drawing.Size(94, 13);
            this.lblCompareImage.TabIndex = 55;
            this.lblCompareImage.Text = "Comparison Image";
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
            // lblRequired
            // 
            this.lblRequired.AutoSize = true;
            this.lblRequired.Location = new System.Drawing.Point(12, 88);
            this.lblRequired.Name = "lblRequired";
            this.lblRequired.Size = new System.Drawing.Size(50, 13);
            this.lblRequired.TabIndex = 53;
            this.lblRequired.Text = "Required";
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
            // lblSHA
            // 
            this.lblSHA.AutoSize = true;
            this.lblSHA.Location = new System.Drawing.Point(12, 64);
            this.lblSHA.Name = "lblSHA";
            this.lblSHA.Size = new System.Drawing.Size(88, 13);
            this.lblSHA.TabIndex = 51;
            this.lblSHA.Text = "SHA1 Checksum";
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
            // lblRomImage
            // 
            this.lblRomImage.AutoSize = true;
            this.lblRomImage.Location = new System.Drawing.Point(12, 9);
            this.lblRomImage.Name = "lblRomImage";
            this.lblRomImage.Size = new System.Drawing.Size(104, 13);
            this.lblRomImage.TabIndex = 50;
            this.lblRomImage.Text = "DQ 1+2 ROM Image";
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
            // lblCrossGame
            // 
            this.lblCrossGame.AutoSize = true;
            this.lblCrossGame.Location = new System.Drawing.Point(208, 154);
            this.lblCrossGame.Name = "lblCrossGame";
            this.lblCrossGame.Size = new System.Drawing.Size(64, 13);
            this.lblCrossGame.TabIndex = 68;
            this.lblCrossGame.Text = "Cross Game";
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
            this.trkExperience.Location = new System.Drawing.Point(224, 323);
            this.trkExperience.Maximum = 50;
            this.trkExperience.Minimum = 5;
            this.trkExperience.Name = "trkExperience";
            this.trkExperience.Size = new System.Drawing.Size(156, 45);
            this.trkExperience.TabIndex = 74;
            this.trkExperience.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkExperience.Value = 10;
            this.trkExperience.Scroll += new System.EventHandler(this.trkExperience_Scroll);
            // 
            // trkGoldReq
            // 
            this.trkGoldReq.Location = new System.Drawing.Point(224, 346);
            this.trkGoldReq.Maximum = 50;
            this.trkGoldReq.Minimum = 10;
            this.trkGoldReq.Name = "trkGoldReq";
            this.trkGoldReq.Size = new System.Drawing.Size(156, 45);
            this.trkGoldReq.TabIndex = 75;
            this.trkGoldReq.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkGoldReq.Value = 10;
            this.trkGoldReq.Scroll += new System.EventHandler(this.trkGoldReq_Scroll);
            // 
            // lblExperience
            // 
            this.lblExperience.AutoSize = true;
            this.lblExperience.Location = new System.Drawing.Point(393, 324);
            this.lblExperience.Name = "lblExperience";
            this.lblExperience.Size = new System.Drawing.Size(33, 13);
            this.lblExperience.TabIndex = 78;
            this.lblExperience.Text = "100%";
            // 
            // lblGoldReq
            // 
            this.lblGoldReq.AutoSize = true;
            this.lblGoldReq.Location = new System.Drawing.Point(393, 347);
            this.lblGoldReq.Name = "lblGoldReq";
            this.lblGoldReq.Size = new System.Drawing.Size(33, 13);
            this.lblGoldReq.TabIndex = 79;
            this.lblGoldReq.Text = "100%";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(10, 384);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 80;
            // 
            // lblExpBoost
            // 
            this.lblExpBoost.AutoSize = true;
            this.lblExpBoost.Location = new System.Drawing.Point(28, 324);
            this.lblExpBoost.Name = "lblExpBoost";
            this.lblExpBoost.Size = new System.Drawing.Size(110, 13);
            this.lblExpBoost.TabIndex = 82;
            this.lblExpBoost.Text = "Boost Experience/GP";
            // 
            // lblGPReq
            // 
            this.lblGPReq.AutoSize = true;
            this.lblGPReq.Location = new System.Drawing.Point(28, 347);
            this.lblGPReq.Name = "lblGPReq";
            this.lblGPReq.Size = new System.Drawing.Size(153, 13);
            this.lblGPReq.TabIndex = 83;
            this.lblGPReq.Text = "Randomize Gold Requirements";
            // 
            // cmdRandomize
            // 
            this.cmdRandomize.Location = new System.Drawing.Point(458, 342);
            this.cmdRandomize.Name = "cmdRandomize";
            this.cmdRandomize.Size = new System.Drawing.Size(96, 23);
            this.cmdRandomize.TabIndex = 84;
            this.cmdRandomize.Text = "Randomize";
            this.cmdRandomize.UseVisualStyleBackColor = true;
            this.cmdRandomize.Click += new System.EventHandler(this.cmdRandomize_Click);
            // 
            // chkMap
            // 
            this.chkMap.AutoSize = true;
            this.chkMap.Enabled = false;
            this.chkMap.Location = new System.Drawing.Point(12, 294);
            this.chkMap.Name = "chkMap";
            this.chkMap.Size = new System.Drawing.Size(103, 17);
            this.chkMap.TabIndex = 85;
            this.chkMap.Text = "Randomize Map";
            this.chkMap.UseVisualStyleBackColor = true;
            // 
            // chkSmallMap
            // 
            this.chkSmallMap.AutoSize = true;
            this.chkSmallMap.Enabled = false;
            this.chkSmallMap.Location = new System.Drawing.Point(233, 294);
            this.chkSmallMap.Name = "chkSmallMap";
            this.chkSmallMap.Size = new System.Drawing.Size(110, 17);
            this.chkSmallMap.TabIndex = 87;
            this.chkSmallMap.Text = "Small map in DQ2";
            this.chkSmallMap.UseVisualStyleBackColor = true;
            // 
            // radENG
            // 
            this.radENG.AutoSize = true;
            this.radENG.Location = new System.Drawing.Point(458, 154);
            this.radENG.Name = "radENG";
            this.radENG.Size = new System.Drawing.Size(48, 17);
            this.radENG.TabIndex = 88;
            this.radENG.TabStop = true;
            this.radENG.Text = "ENG";
            this.radENG.UseVisualStyleBackColor = true;
            this.radENG.CheckedChanged += new System.EventHandler(this.radENG_CheckedChanged);
            // 
            // radJP
            // 
            this.radJP.AutoSize = true;
            this.radJP.Location = new System.Drawing.Point(458, 179);
            this.radJP.Name = "radJP";
            this.radJP.Size = new System.Drawing.Size(37, 17);
            this.radJP.TabIndex = 89;
            this.radJP.TabStop = true;
            this.radJP.Text = "JP";
            this.radJP.UseVisualStyleBackColor = true;
            this.radJP.CheckedChanged += new System.EventHandler(this.radJP_CheckedChanged);
            // 
            // lblRandomizeHeader
            // 
            this.lblRandomizeHeader.AutoSize = true;
            this.lblRandomizeHeader.Location = new System.Drawing.Point(28, 154);
            this.lblRandomizeHeader.Name = "lblRandomizeHeader";
            this.lblRandomizeHeader.Size = new System.Drawing.Size(63, 13);
            this.lblRandomizeHeader.TabIndex = 90;
            this.lblRandomizeHeader.Text = "Randomize:";
            // 
            // chkBattleSpeedHacks
            // 
            this.chkBattleSpeedHacks.AutoSize = true;
            this.chkBattleSpeedHacks.Location = new System.Drawing.Point(433, 237);
            this.chkBattleSpeedHacks.Name = "chkBattleSpeedHacks";
            this.chkBattleSpeedHacks.Size = new System.Drawing.Size(121, 17);
            this.chkBattleSpeedHacks.TabIndex = 91;
            this.chkBattleSpeedHacks.Text = "Battle Speed Hacks";
            this.chkBattleSpeedHacks.UseVisualStyleBackColor = true;
            this.chkBattleSpeedHacks.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkDoubleWalking
            // 
            this.chkDoubleWalking.AutoSize = true;
            this.chkDoubleWalking.Location = new System.Drawing.Point(433, 259);
            this.chkDoubleWalking.Name = "chkDoubleWalking";
            this.chkDoubleWalking.Size = new System.Drawing.Size(136, 17);
            this.chkDoubleWalking.TabIndex = 92;
            this.chkDoubleWalking.Text = "Double Walking Speed";
            this.chkDoubleWalking.UseVisualStyleBackColor = true;
            this.chkDoubleWalking.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkHalfEncounterRate
            // 
            this.chkHalfEncounterRate.AutoSize = true;
            this.chkHalfEncounterRate.Location = new System.Drawing.Point(433, 280);
            this.chkHalfEncounterRate.Name = "chkHalfEncounterRate";
            this.chkHalfEncounterRate.Size = new System.Drawing.Size(124, 17);
            this.chkHalfEncounterRate.TabIndex = 93;
            this.chkHalfEncounterRate.Text = "50% Encounter Rate";
            this.chkHalfEncounterRate.UseVisualStyleBackColor = true;
            this.chkHalfEncounterRate.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkTextSpeedHacks
            // 
            this.chkTextSpeedHacks.AutoSize = true;
            this.chkTextSpeedHacks.Location = new System.Drawing.Point(433, 215);
            this.chkTextSpeedHacks.Name = "chkTextSpeedHacks";
            this.chkTextSpeedHacks.Size = new System.Drawing.Size(115, 17);
            this.chkTextSpeedHacks.TabIndex = 94;
            this.chkTextSpeedHacks.Text = "Text Speed Hacks";
            this.chkTextSpeedHacks.UseVisualStyleBackColor = true;
            this.chkTextSpeedHacks.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 414);
            this.Controls.Add(this.chkTextSpeedHacks);
            this.Controls.Add(this.chkHalfEncounterRate);
            this.Controls.Add(this.chkDoubleWalking);
            this.Controls.Add(this.chkBattleSpeedHacks);
            this.Controls.Add(this.lblRandomizeHeader);
            this.Controls.Add(this.radJP);
            this.Controls.Add(this.radENG);
            this.Controls.Add(this.chkSmallMap);
            this.Controls.Add(this.chkMap);
            this.Controls.Add(this.cmdRandomize);
            this.Controls.Add(this.lblGPReq);
            this.Controls.Add(this.lblExpBoost);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblGoldReq);
            this.Controls.Add(this.lblExperience);
            this.Controls.Add(this.trkGoldReq);
            this.Controls.Add(this.trkExperience);
            this.Controls.Add(this.chkStoresCross);
            this.Controls.Add(this.chkTreasuresCross);
            this.Controls.Add(this.chkMonsterPatternsCross);
            this.Controls.Add(this.lblCrossGame);
            this.Controls.Add(this.chkMonsterZonesCross);
            this.Controls.Add(this.chkStores);
            this.Controls.Add(this.chkTreasures);
            this.Controls.Add(this.chkHeroStats);
            this.Controls.Add(this.chkMonsterPatterns);
            this.Controls.Add(this.chkMonsterZones);
            this.Controls.Add(this.lblFlags);
            this.Controls.Add(this.txtFlags);
            this.Controls.Add(this.btnNewSeed);
            this.Controls.Add(this.lblSeed);
            this.Controls.Add(this.txtSeed);
            this.Controls.Add(this.btnCompareBrowse);
            this.Controls.Add(this.lblCompareImage);
            this.Controls.Add(this.txtCompare);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.lblReqChecksum);
            this.Controls.Add(this.lblRequired);
            this.Controls.Add(this.lblSHAChecksum);
            this.Controls.Add(this.lblSHA);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblRomImage);
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

        private System.Windows.Forms.Label lblFlags;
        private System.Windows.Forms.TextBox txtFlags;
        private System.Windows.Forms.Button btnNewSeed;
        private System.Windows.Forms.Label lblSeed;
        private System.Windows.Forms.TextBox txtSeed;
        private System.Windows.Forms.Button btnCompareBrowse;
        private System.Windows.Forms.Label lblCompareImage;
        private System.Windows.Forms.TextBox txtCompare;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Label lblReqChecksum;
        private System.Windows.Forms.Label lblRequired;
        private System.Windows.Forms.Label lblSHAChecksum;
        private System.Windows.Forms.Label lblSHA;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblRomImage;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.CheckBox chkMonsterZones;
        private System.Windows.Forms.CheckBox chkMonsterPatterns;
        private System.Windows.Forms.CheckBox chkHeroStats;
        private System.Windows.Forms.CheckBox chkTreasures;
        private System.Windows.Forms.CheckBox chkStores;
        private System.Windows.Forms.CheckBox chkMonsterZonesCross;
        private System.Windows.Forms.Label lblCrossGame;
        private System.Windows.Forms.CheckBox chkMonsterPatternsCross;
        private System.Windows.Forms.CheckBox chkTreasuresCross;
        private System.Windows.Forms.CheckBox chkStoresCross;
        private System.Windows.Forms.TrackBar trkExperience;
        private System.Windows.Forms.TrackBar trkGoldReq;
        private System.Windows.Forms.Label lblExperience;
        private System.Windows.Forms.Label lblGoldReq;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblExpBoost;
        private System.Windows.Forms.Label lblGPReq;
        private System.Windows.Forms.Button cmdRandomize;
        private System.Windows.Forms.CheckBox chkMap;
        private System.Windows.Forms.CheckBox chkSmallMap;
        private System.Windows.Forms.RadioButton radENG;
        private System.Windows.Forms.RadioButton radJP;
        private System.Windows.Forms.Label lblRandomizeHeader;
        private System.Windows.Forms.CheckBox chkBattleSpeedHacks;
        private System.Windows.Forms.CheckBox chkDoubleWalking;
        private System.Windows.Forms.CheckBox chkHalfEncounterRate;
        private System.Windows.Forms.CheckBox chkTextSpeedHacks;
    }
}

