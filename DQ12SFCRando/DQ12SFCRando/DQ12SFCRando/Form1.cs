﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace DQ12SFCRando
{
    public partial class Form1 : Form
    {
        bool loading = true;
        byte[] romData;
        byte[] romData2;

        int game = 1;

        int[,] map = new int[128, 128];
        int[,] island = new int[128, 128];
        int[,] zone = new int[8, 8];
        int maxIsland;
        List<int> islands = new List<int>();

        int[,] map2 = new int[256, 256];
        int[,] island2 = new int[256, 256];
        int[,] zone2 = new int[16, 16];
        int[] maxIsland2 = new int[4];
        List<int> islands2 = new List<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;
                runChecksum();
            }
        }

        private void runChecksum()
        {
            try
            {
                using (var md5 = SHA1.Create())
                {
                    using (var stream = File.OpenRead(txtFileName.Text))
                    {
                        lblSHAChecksum.Text = BitConverter.ToString(md5.ComputeHash(stream)).ToLower().Replace("-", "");
                    }
                }
            }
            catch
            {
                lblSHAChecksum.Text = "????????????????????????????????????????";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSeed.Text = (DateTime.Now.Ticks % 2147483647).ToString();

            try
            {
                using (TextReader reader = File.OpenText("lastDQ12.txt"))
                {
                    txtFlags.Text = reader.ReadLine();
                    txtFileName.Text = reader.ReadLine();

                    determineChecks(null, null);

                    runChecksum();
                    loading = false;
                }
            }
            catch
            {
                // ignore error
                loading = false;
            }
        }

        private void btnNewSeed_Click(object sender, EventArgs e)
        {
            txtSeed.Text = (DateTime.Now.Ticks % 2147483647).ToString();
        }

        private bool loadRom(bool extra = false)
        {
            try
            {
                romData = File.ReadAllBytes(txtFileName.Text);
                if (extra)
                    romData2 = File.ReadAllBytes(txtCompare.Text);
            }
            catch
            {
                MessageBox.Show("Empty file name(s) or unable to open files.  Please verify the files exist.");
                return false;
            }
            return true;
        }

        private void saveRom()
        {
            string options = "";
            string finalFile = Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DQ12R_" + txtSeed.Text + "_" + txtFlags.Text + ".smc");
            File.WriteAllBytes(finalFile, romData);
            lblStatus.Text = "ROM hacking complete!  (" + finalFile + ")";
            txtCompare.Text = finalFile;
        }

        private void swap(int firstAddress, int secondAddress)
        {
            byte holdAddress = romData[secondAddress];
            romData[secondAddress] = romData[firstAddress];
            romData[firstAddress] = holdAddress;
        }

        private int[] swapArray(int[] array, int first, int second)
        {
            int holdAddress = array[second];
            array[second] = array[first];
            array[first] = holdAddress;
            return array;
        }

        private int ScaleValue(int value, double scale, double adjustment, Random r1)
        {
            var exponent = (double)r1.Next() / int.MaxValue * 2.0 - 1.0;
            var adjustedScale = 1.0 + adjustment * (scale - 1.0);

            return (int)Math.Round(Math.Pow(adjustedScale, exponent) * value, MidpointRounding.AwayFromZero);
        }

        private int[] inverted_power_curve(int min, int max, int arraySize, double powToUse, Random r1)
        {
            int range = max - min;
            double p_range = Math.Pow(range, 1 / powToUse);
            int[] points = new int[arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                double section = (double)r1.Next() / int.MaxValue;
                points[i] = (int)Math.Round(max - Math.Pow(section * p_range, powToUse));
            }
            Array.Sort(points);
            return points;
        }

        private void determineFlags(object sender, EventArgs e)
        {
            if (loading)
                return;

            string flags = "";
            int number = (chkMonsterZones.Checked ? 1 : 0) + (chkMonsterPatterns.Checked ? 2 : 0) + (chkHeroStats.Checked ? 4 : 0) +
                (chkTreasures.Checked ? 8 : 0) + (chkStores.Checked ? 16 : 0);
            flags += convertIntToChar(number);
            number = (chkMonsterZonesCross.Checked ? 1 : 0) + (chkMonsterPatternsCross.Checked ? 2 : 0) + (chkTreasuresCross.Checked ? 4 : 0) + (chkStoresCross.Checked ? 8 : 0);
            flags += convertIntToChar(number);
            number = (chkBattleSpeedHacks.Checked ? 1 : 0) + (chkDoubleWalking.Checked ? 2 : 0) + (chkHalfEncounterRate.Checked ? 4 : 0) + (chkTextSpeedHacks.Checked ? 8 : 0);
            flags += convertIntToChar(number);
            flags += convertIntToChar(trkExperience.Value);
            flags += convertIntToChar(trkGoldReq.Value);

            txtFlags.Text = flags;
        }

        private void determineChecks(object sender, EventArgs e)
        {
            string flags = txtFlags.Text;
            int number = convertChartoInt(Convert.ToChar(flags.Substring(0, 1)));
            chkMonsterZones.Checked = (number % 2 == 1);
            chkMonsterPatterns.Checked = (number % 4 >= 2);
            chkHeroStats.Checked = (number % 8 >= 4);
            chkTreasures.Checked = (number % 16 >= 8);
            chkStores.Checked = (number % 32 >= 16);

            number = convertChartoInt(Convert.ToChar(flags.Substring(1, 1)));
            chkMonsterZonesCross.Checked = (number % 2 == 1);
            chkMonsterPatternsCross.Checked = (number % 4 >= 2);
            chkTreasuresCross.Checked = (number % 8 >= 4);
            chkStoresCross.Checked = (number % 16 >= 8);
            number = convertChartoInt(Convert.ToChar(flags.Substring(2, 1)));
            chkBattleSpeedHacks.Checked = (number % 2 == 1);
            chkDoubleWalking.Checked = (number % 4 >= 2);
            chkHalfEncounterRate.Checked = (number % 8 >= 4);
            chkTextSpeedHacks.Checked = (number % 16 >= 8);
            trkExperience.Value = convertChartoInt(Convert.ToChar(flags.Substring(3, 1)));
            trkExperience_Scroll(null, null);
            trkGoldReq.Value = convertChartoInt(Convert.ToChar(flags.Substring(4, 1)));
            trkGoldReq_Scroll(null, null);
        }

        private string convertIntToChar(int number)
        {
            if (number >= 0 && number <= 9)
                return number.ToString();
            if (number >= 10 && number <= 35)
                return Convert.ToChar(55 + number).ToString();
            if (number >= 36 && number <= 61)
                return Convert.ToChar(61 + number).ToString();
            if (number == 62) return "!";
            if (number == 63) return "@";
            return "";
        }

        private int convertChartoInt(char character)
        {
            if (character >= Convert.ToChar("0") && character <= Convert.ToChar("9"))
                return character - 48;
            if (character >= Convert.ToChar("A") && character <= Convert.ToChar("Z"))
                return character - 55;
            if (character >= Convert.ToChar("a") && character <= Convert.ToChar("z"))
                return character - 61;
            if (character == Convert.ToChar("!")) return 62;
            if (character == Convert.ToChar("@")) return 63;
            return 0;
        }

        private void trkExperience_Scroll(object sender, EventArgs e)
        {
            lblExperience.Text = (trkExperience.Value * 10).ToString() + "%";
            determineFlags(null, null);
        }

        private void trkGoldReq_Scroll(object sender, EventArgs e)
        {
            lblGoldReq.Text = (trkGoldReq.Value == 10 ? "100%" : (1000 / trkGoldReq.Value) + "-" + (trkGoldReq.Value * 10).ToString() + "%");
            determineFlags(null, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (StreamWriter writer = File.CreateText("lastDQ12.txt"))
            {
                writer.WriteLine(txtFlags.Text);
                writer.WriteLine(txtFileName.Text);
            }
        }

        private void cmdRandomize_Click(object sender, EventArgs e)
        {
            try
            {
                loadRom();
                Random r1 = new Random(Convert.ToInt32(txtSeed.Text));
                monsterAdjustments();
                if (chkMonsterZones.Checked) randomizeMonsterZones(r1);
                if (chkMonsterPatterns.Checked) randomizeMonsterPatterns(r1);
                if (chkHeroStats.Checked) randomizeHeroStats(r1);
                if (chkTreasures.Checked) randomizeTreasures(r1);
                if (chkStores.Checked) randomizeStores(r1);
                boostExp();
                goldRequirements(r1);
                speedHacks();
                saveRom();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:  " + ex.Message);
            }
        }

        private void romPlugin(int locationStart, byte[] rom)
        {
            for (int lnI = 0; lnI < rom.Length; lnI++)
                romData[locationStart + lnI] = rom[lnI];
        }

        private void speedHacks()
        {
            if (chkTextSpeedHacks.Checked)
            {
                romPlugin(0x31c4, new byte[] { 0xea, 0xea, 0xea, 0xea }); // Talk to NPC speed hack
                // Speed up DQ2 opening scene
                romData[0x161e57] = romData[0x161ea2] = romData[0x16202c] = romData[0x16212a] = 0x01;
                romData[0x161e6e] = romData[0x161e80] = romData[0x161e88] = romData[0x161e90] = romData[0x161e98] = romData[0x161eae] = romData[0x161eb5] = 0x01;
                romData[0x1620e4] = romData[0x1621d6] = romData[0x1623bd] = romData[0x162404] = romData[0x1622c2] = romData[0x1622db] = romData[0x1622f4] = romData[0x16230d] = romData[0x162363] = romData[0x162392] = romData[0x162478] = 0x01;
                romData[0x161ef2] = romData[0x161ef9] = romData[0x161eeb] = romData[0x161ee4] = romData[0x161ed7] = romData[0x161eda] = romData[0x161e3c] = romData[0x161e36] = 0x01;
                romData[0x161f5b] = romData[0x161ed4] = romData[0x161f6c] = romData[0x161f7d] = romData[0x161f8e] = romData[0x161f9f] = romData[0x161fad] = romData[0x161fb2] = 0x01;
                romData[0x161f00] = romData[0x161f07] = romData[0x161f0e] = romData[0x161f15] = romData[0x161f1c] = romData[0x161f23] = romData[0x161fc1] = romData[0x161fc7] = romData[0x161fca] = romData[0x161fcd] = romData[0x161fd0] = 0x01;
                romData[0x30b2] = 0x02;
                //romPlugin(0xb4e8, new byte[] { 0x20, 0x2e, 0xb7, 0xea }); // End of battle speed hack
            }
            // 1 flash instead of 8 when a monster is hit.
            if (chkBattleSpeedHacks.Checked)
            {
                romData[0x5cfd4] = 0x01;
            }
            // Double Walking Speed
            if (chkDoubleWalking.Checked)
            {
                romPlugin(0x370, new byte[] { 0x4c, 0x90, 0xfb });
                romPlugin(0x7b90, new byte[] { 0xa5, 0x66, 0x29, 0x01, 0xd0, 0x03, 0x4c, 0xe0, 0x82, 0x4c, 0xcc, 0x82 });
                romPlugin(0x656, new byte[] { 0xea, 0xea, 0xea, 0xea, 0xea, 0xea, 0xea });
            }
            if (chkHalfEncounterRate.Checked)
            {
                // Single encounter rate
                // romPlugin(0x5b515, new byte[] { 0x00, 0x00, 0x0C, 0x05, 0x0A, 0x0A, 0x0E, 0x0A, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x03, 0x07, 0x05, 0x07, 0x09, 0x0C, 0x07, 0x00, 0x00, 0x0A });
                // Half encounter rate
                romPlugin(0x5b515, new byte[] { 0x00, 0x00, 0x06, 0x03, 0x05, 0x05, 0x07, 0x05, 0x00, 0x00, 0x05, 0x00, 0x00, 0x02, 0x04, 0x03, 0x04, 0x05, 0x06, 0x04, 0x00, 0x00, 0x05 });
                romData[0x59739] = 0x03; // Also reduce encounter rate in caves.
            }
        }

        private void monsterAdjustments()
        {
            int byteToUse = 0x5da0e + (25 * 18);
            // Set Goldman to 48 XP, 1000 GP
            romData[byteToUse + 16] = 48;
            romData[byteToUse + 14] = 0xc1;
            romData[byteToUse + 5] = 0xe8;

            // Set Stoneman to 210 XP
            byteToUse = 0x5da0e + (35 * 18);
            romData[byteToUse + 16] = 210;

            // Set Golem to 300 XP
            byteToUse = 0x5da0e + (24 * 18);
            romData[byteToUse + 16] = 44;
            romData[byteToUse + 17] = 1;

            // Set Dragon Loop monster to 262 XP (65% strength * 100% defense * sqrt(75% agility) * 230% HP * 1.5 = 262 XP)
            romData[0x591ce] = 0x06;
            romData[0x591cf] = 0x01;
        }

        private void randomizeMap(Random r1)
        {
            // Write the map on bank 0x28 for DQ1, starting at A200, and bank 0x26 for DQ2, starting at 8000
            romData[0xd8c3b] = 0x28;
            romData[0xd8c3e] = 0xa2;

            romData[0xd875c] = 0x26;
        }

        private bool randomizeMapDQ1(Random r1)
        {
            for (int lnI = 0; lnI < 128; lnI++)
                for (int lnJ = 0; lnJ < 128; lnJ++)
                {
                        map[lnI, lnJ] = 0x07;
                        island[lnI, lnJ] = -1;
                }

            int islandSize = (r1.Next() % 5000) + 7500; // (lnI == 0 ? 1500 : lnI == 1 ? 2500 : lnI == 2 ? 1500 : lnI == 3 ? 1500 : lnI == 4 ? 5000 : 5000);

            // Set up three special zones.  Zone 1000 = 25 squares and has Cannock stuff.  Zone 2000 = 30 squares and has Moonbrooke stuff.  
            // Zone 3000 = 48 squares and has Hargon stuff.  It will be surrounded by eight tiles of mountains.
            // This takes up 94 / 256 of the total squares available.

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    zone[i, j] = 0;

            generateZoneMap(0, false, islandSize, r1);
            createBridges(r1);
            resetIslands();

            // We should mark islands and inaccessible land...
            int lakeNumber = 256;

            int maxPlots = 0;
            int maxLake = 0;
            for (int lnI = 0; lnI < 128; lnI++)
                for (int lnJ = 0; lnJ < 128; lnJ++)
                {
                    if (island2[lnI, lnJ] == -1)
                    {
                        int plots = lakePlot(lakeNumber, lnI, lnJ);
                        if (plots > maxPlots)
                        {
                            maxPlots = plots;
                            maxLake = lakeNumber;
                        }
                        lakeNumber++;
                    }
                }

            // Establish Tantegel location
            bool midenOK = false;
            int midenX = 0;
            int midenY = 0;
            while (!midenOK)
            {
                midenX = r1.Next() % 124;
                midenY = r1.Next() % 124;
                if (validPlot(midenY, midenX, 2, 2, new int[] { maxIsland }))
                    midenOK = true;
            }

            bool tokenLegal = false;
            while (!tokenLegal)
            {
                int x = r1.Next() % 122;
                int y = r1.Next() % 122;
                if (validPlot(y, x, 5, 5, islands.ToArray()) && reachable(y, x, true, midenX, midenY, maxLake))
                {
                    map2[y + 1, x + 1] = 0x01;
                    map2[y + 1, x + 2] = 0x01;
                    map2[y + 1, x + 3] = 0x01;
                    map2[y + 2, x + 1] = 0x01;
                    map2[y + 2, x + 2] = 0x30;
                    map2[y + 2, x + 3] = 0x01;
                    map2[y + 3, x + 1] = 0x01;
                    map2[y + 3, x + 2] = 0x30;
                    map2[y + 3, x + 3] = 0x01;
                    // TODO:  Also need to update the ROM to indicate the token location.
                    romData[0x19f20] = (byte)(x + 2);
                    romData[0x19f21] = (byte)(y + 2);

                    tokenLegal = true;
                }
            }

            bool charlockLegal = false;
            while (!charlockLegal)
            {
                int x = r1.Next() % 115;
                int y = r1.Next() % 115;
                if (validPlot(y, x, 11, 11, islands.ToArray()) && reachable(y, x, true, midenX, midenY, maxLake))
                {
                    map2[y + 1, x + 1] = 0x01;
                    map2[y + 1, x + 2] = 0x01;
                    map2[y + 1, x + 3] = 0x01;
                    map2[y + 1, x + 4] = 0x01;
                    map2[y + 1, x + 5] = 0x01;
                    map2[y + 5, x + 1] = 0x01;
                    map2[y + 5, x + 2] = 0x01;
                    map2[y + 5, x + 3] = 0x01;
                    map2[y + 5, x + 4] = 0x01;
                    map2[y + 5, x + 5] = 0x01;
                    map2[y + 2, x + 1] = 0x01;
                    map2[y + 2, x + 5] = 0x01;
                    map2[y + 3, x + 1] = 0x01;
                    map2[y + 3, x + 5] = 0x07;
                    map2[y + 4, x + 1] = 0x01;
                    map2[y + 4, x + 5] = 0x01;
                    map2[y + 2, x + 2] = 0xc2;
                    map2[y + 2, x + 3] = 0xc3;
                    map2[y + 3, x + 2] = 0xf6;
                    map2[y + 3, x + 3] = 0xf7;
                    map2[y + 4, x + 2] = 0xfe;
                    map2[y + 4, x + 3] = 0xff;

                    // TODO:  Also need to update the ROM to indicate Charlock's location and the rainbow drop bridge.
                    //romData[0x19f20] = (byte)(x + 2);
                    //romData[0x19f21] = (byte)(y + 2);

                    charlockLegal = true;
                }
            }

            // We'll place all of the castles now.
            // Midenhall can go anywhere.  But Cannock has to be 15-30 squares or less away from there.
            // Don't place Hargon's Castle for now.  OK, place it for now.  But I may change my mind later.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                int x = 300;
                int y = 300;
                if (lnI == 0) { x = midenX; y = midenY; }
                else
                {
                    x = r1.Next() % 125;
                    y = r1.Next() % 125;
                }

                if (validPlot(y, x, 2, 2, new int[] { maxIsland }) && reachable(y, x, false, midenX, midenY, maxLake))
                {
                    map[y + 0, x + 0] = 0xd8;
                    map[y + 0, x + 1] = 0xd9;
                    map[y + 1, x + 0] = 0xe0;
                    map[y + 1, x + 1] = 0xe1;

                    // TODO:  Map warp data
                    int byteToUse = 0xa2b3;
                    romData[byteToUse] = (byte)(x + 1);
                    romData[byteToUse + 1] = (byte)(y + 1);

                    // TODO:  Return points
                    int byteMultiplier = lnI - (lnI >= 3 ? 1 : 0);
                    romData[0xa27a + (3 * byteMultiplier)] = (byte)x;
                    if (map2[y + 2, x] == 0x04)
                        romData[0xa27a + (3 * byteMultiplier) + 1] = (byte)(y + 2);
                    else
                        romData[0xa27a + (3 * byteMultiplier) + 1] = (byte)(y + 1);
                }
                else
                    lnI--;
            }

            // Now we'll place all of the towns now, including Hauksness.  They should all be villages?
            for (int lnI = 0; lnI < 6; lnI++)
            {
                //if (lnI == 6) lnI = lnI;
                int x = r1.Next() % 125;
                int y = r1.Next() % 125;

                if (validPlot(y, x, 1, 2, new int[] { maxIsland } ) && reachable(y, x, false, midenX, midenY, maxLake))
                {
                    map2[y, x + 0] = 0xda;
                    map2[y, x + 1] = 0xdb;

                    // TODO:  Location information
                    int byteToUse2 = (lnI == 0 ? 0xa292 : lnI == 1 ? 0xa298 : lnI == 2 ? 0xa29e : lnI == 3 ? 0xa2a7 : lnI == 4 ? 0xa2aa : lnI == 5 ? 0xa2ad : 0xa2b0);
                    romData[byteToUse2] = (byte)(x + 1);
                    romData[byteToUse2 + 1] = (byte)(y);
                }
                else
                    lnI--;
            }

            // Then the monoliths.
            // All of these can go anywhere.
            for (int lnI = 0; lnI < 2; lnI++)
            {
                if ((lnI == 0) && chkSmallMap.Checked) continue; // Remove the Midenhall Island shrine which is of no importance.
                // lnI == 1 is probably the Cannock shrine... want to put that in Zone 1...

                int x = r1.Next() % 125;
                int y = r1.Next() % 125;

                if (validPlot(y, x, 1, 1, new int[] { maxIsland }) && reachable(y, x, false, midenX, midenY, maxLake))
                {
                    map2[y, x] = 0x1d;

                    // TODO:  Location information
                    int byteToUse2 = 0xa2b6 + (lnI * 3); // (lnI < 11 ? 0xa2b6 + (lnI * 3) : 0xa2da);
                    romData[byteToUse2] = (byte)(x);
                    romData[byteToUse2 + 1] = (byte)(y);
                }
                else
                    lnI--;
            }

            // TODO:  Send Erdrick's cave location warp into some water somewhere.

            // Then the caves.
            for (int lnI = 0; lnI < 3; lnI++)
            {
                int x = 300;
                int y = 300;
                x = r1.Next() % 125;
                y = r1.Next() % 125;

                if (validPlot(y, x, 1, 1, new int[] { maxIsland }) && reachable(y, x, false, midenX, midenY, maxLake))
                {
                    map2[y, x] = 0x11;

                    // TODO:  Location information
                    int byteToUse2 = (lnI == 0 ? 0xa2dd : lnI == 1 ? 0xa2e0 : lnI == 2 ? 0xa2e3 : lnI == 3 ? 0xa2ef : lnI == 4 ? 0xa2fb : lnI == 5 ? 0xa2fe : lnI == 6 ? 0xa304 : lnI == 7 ? 0xa307 : 0xa30a);
                    romData[byteToUse2] = (byte)x;
                    romData[byteToUse2 + 1] = (byte)(y);
                }
                else
                    lnI--;
            }

            int[,] monsterZones = new int[8, 8];
            for (int lnI = 0; lnI < 8; lnI++)
                for (int lnJ = 0; lnJ < 8; lnJ++)
                    monsterZones[lnI, lnJ] = 0xff;

            int midenMZX = midenX / 8;
            int midenMZY = midenY / 8;

            // TODO:  Monster Zones could start at 1, not 0.  Also, want to mark zone 2 for left and north of Tantegel, and zone 3 for south and east of Tantegel.
            // ALSO:  Consider moving the monster zones to a new region in the ROM, because I think that bank has space it can use.  HOWEVER, consider DW2 before doing this...
            for (int mzX = 0; mzX < 8; mzX++)
                for (int mzY = 0; mzY < 8; mzY++)
                {
                    if (mzX == midenMZX && mzY == midenMZY)
                        monsterZones[midenMZY, midenMZX] = 0x00;
                    else
                        monsterZones[mzY, mzX] = r1.Next() % 17 + 0x2;
                }

            // Now let's enter all of this into the ROM...
            // TODO:  Map information
            int lnPointer = 0x9f97;

            for (int lnI = 0; lnI <= 256; lnI++) // <---- There is a final pointer for lnI = 256, probably indicating the conclusion of the map.
            {
                romData[0xdda5 + (lnI * 2)] = (byte)(lnPointer % 256);
                romData[0xdda6 + (lnI * 2)] = (byte)(lnPointer / 256);

                int lnJ = 0;
                while (lnI < 256 && lnJ < 256)
                {
                    if (map2[lnI, lnJ] >= 1 && map2[lnI, lnJ] <= 7)
                    {
                        int tileNumber = 0;
                        int numberToMatch = map2[lnI, lnJ];
                        while (lnJ < 256 && tileNumber < 32 && map2[lnI, lnJ] == numberToMatch && tileNumber < 32)
                        {
                            tileNumber++;
                            lnJ++;
                        }
                        romData[lnPointer + 0x4010] = (byte)((0x20 * numberToMatch) + (tileNumber - 1));
                        lnPointer++;
                    }
                    else
                    {
                        romData[lnPointer + 0x4010] = (byte)map2[lnI, lnJ];
                        lnPointer++;
                        lnJ++;
                    }
                }
            }
            //lnPointer = lnPointer;
            if (lnPointer >= 0xb8f7)
            {
                MessageBox.Show("WARNING:  The map might have taken too much ROM space...");
                // Might have to compress further to remove one byte stuff
                // Must compress the map by getting rid of further 1 byte lakes
            }

            // TODO:  Enter monster zones
            for (int lnI = 0; lnI < 8; lnI++)
                for (int lnJ = 0; lnJ < 8; lnJ++)
                {
                    romData[0x103d6 + (lnI * 16) + lnJ] = (byte)monsterZones[lnI, lnJ];
                }

            return true;
        }

        private bool randomizeMapv5(Random r1)
        {
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    if (chkSmallMap.Checked && (lnI >= 128 || lnJ >= 128))
                    {
                        map2[lnI, lnJ] = 0x05;
                        island2[lnI, lnJ] = 200;
                    }
                    else
                    {
                        map2[lnI, lnJ] = 0x04;
                        island2[lnI, lnJ] = -1;
                    }
                }

            int islandSize = (r1.Next() % 20000) + 30000; // (lnI == 0 ? 1500 : lnI == 1 ? 2500 : lnI == 2 ? 1500 : lnI == 3 ? 1500 : lnI == 4 ? 5000 : 5000);
            islandSize /= (chkSmallMap.Checked ? 4 : 1);

            // Set up three special zones.  Zone 1000 = 25 squares and has Cannock stuff.  Zone 2000 = 30 squares and has Moonbrooke stuff.  
            // Zone 3000 = 48 squares and has Hargon stuff.  It will be surrounded by eight tiles of mountains.
            // This takes up 94 / 256 of the total squares available.

            bool zonesCreated = false;
            while (!zonesCreated)
            {
                zone2 = new int[16, 16];
                if (createZone(3000, 48, true, r1) && createZone(1000, 25, false, r1) && createZone(2000, 32, false, r1))
                    zonesCreated = true;
            }

            markZoneSides();
            generateZoneMap(3000, true, islandSize * 24 / 256, r1);
            generateZoneMap(1000, false, islandSize * 25 / 256, r1);
            generateZoneMap(2000, false, islandSize * 32 / 256, r1);
            generateZoneMap(0, false, islandSize * 175 / 256, r1);
            createBridges(r1);
            resetIslands();


            // We should mark islands and inaccessible land...
            int lakeNumber = 256;

            int maxPlots = 0;
            int maxLake = 0;
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    if (island2[lnI, lnJ] == -1)
                    {
                        int plots = lakePlot(lakeNumber, lnI, lnJ);
                        if (plots > maxPlots)
                        {
                            maxPlots = plots;
                            maxLake = lakeNumber;
                        }
                        lakeNumber++;
                    }
                }

            // Establish Midenhall location
            bool midenOK = false;
            int[] midenX = new int[4];
            int[] midenY = new int[4];
            while (!midenOK)
            {
                midenX[1] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                midenY[1] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(midenY[1], midenX[1], 2, 2, new int[] { maxIsland2[1] }))
                    midenOK = true;
            }

            // Cannock Cave
            midenOK = false;
            while (!midenOK)
            {
                midenX[2] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                midenY[2] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(midenY[2], midenX[2], 1, 1, new int[] { maxIsland2[2] }))
                    midenOK = true;
            }

            // Rhone Shrine
            midenOK = false;
            while (!midenOK)
            {
                midenX[3] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                midenY[3] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(midenY[3], midenX[3], 1, 1, new int[] { maxIsland2[3] }))
                    midenOK = true;
            }

            // Moonbrooke Shrine (west)
            midenOK = false;
            while (!midenOK)
            {
                midenX[0] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                midenY[0] = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(midenY[0], midenX[0], 1, 1, new int[] { maxIsland2[0] }))
                    midenOK = true;
            }

            islands2.Remove(maxIsland2[1]);
            islands2.Remove(maxIsland2[2]);
            islands2.Remove(maxIsland2[3]);

            bool treeLegal = false;
            while (!treeLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 5, 5, islands2.ToArray()) && reachable(y, x, true, midenX[1], midenY[1], maxLake))
                {
                    map2[y + 1, x + 1] = 0x05;
                    map2[y + 1, x + 2] = 0x05;
                    map2[y + 1, x + 3] = 0x05;
                    map2[y + 2, x + 1] = 0x05;
                    map2[y + 2, x + 2] = 0x03;
                    map2[y + 2, x + 3] = 0x05;
                    map2[y + 3, x + 1] = 0x05;
                    map2[y + 3, x + 2] = 0x03;
                    map2[y + 3, x + 3] = 0x05;
                    // Also need to update the ROM to indicate the World Tree location.
                    romData[0x19f20] = (byte)(x + 2);
                    romData[0x19f21] = (byte)(y + 2);

                    treeLegal = true;
                }
            }

            bool treasuresLegal = false;
            while (!treasuresLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 5, 5, islands2.ToArray()) && reachable(y, x, true, midenX[1], midenY[1], maxLake))
                {
                    map2[y + 1, x + 1] = 0x13;
                    map2[y + 1, x + 2] = 0x13;
                    map2[y + 1, x + 3] = 0x13;
                    map2[y + 2, x + 1] = 0x13;
                    map2[y + 2, x + 2] = 0x03;
                    map2[y + 2, x + 3] = 0x13;
                    map2[y + 3, x + 1] = 0x13;
                    map2[y + 3, x + 2] = 0x03;
                    map2[y + 3, x + 3] = 0x13;
                    // Also need to update the ROM to indicate the World Tree location.
                    romData[0x19f1c] = (byte)(x + 2);
                    romData[0x19f1d] = (byte)(y + 2);

                    treasuresLegal = true;
                }
            }

            // Mirror Of Ra
            bool mirrorLegal = false;
            while (!mirrorLegal)
            {
                int x = r1.Next() % (chkSmallMap.Checked ? 121 : 249);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                if (validPlot(y, x, 5, 6, new int[] { maxIsland2[2] }) && reachable(y, x, false, midenX[2], midenY[2], maxLake))
                {
                    for (int lnJ = 1; lnJ < 4; lnJ++)
                        for (int lnK = 1; lnK < 5; lnK++)
                        {
                            if (lnJ == 1 || lnK == 1 || lnK == 4)
                                map2[y + lnJ, x + lnK] = 0x13;
                            else
                                map2[y + lnJ, x + lnK] = 0x08;
                        }
                    // Also need to update the ROM to indicate the new Mirror Of Ra search spot.
                    romData[0x19f18] = (byte)(x + 2);
                    romData[0x19f19] = (byte)(y + 2);

                    mirrorLegal = true;
                }
            }

            // We'll place all of the castles now.
            // Midenhall can go anywhere.  But Cannock has to be 15-30 squares or less away from there.
            // Don't place Hargon's Castle for now.  OK, place it for now.  But I may change my mind later.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                int x = 300;
                int y = 300;
                if (lnI == 0) { x = midenX[1]; y = midenY[1]; }
                else
                {
                    x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                    y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                }

                if (validPlot(y, x, 2, 2, (lnI == 0 || lnI == 1 ? new int[] { maxIsland2[1] } : lnI == 6 ? new int[] { maxIsland2[3] } : islands2.ToArray())) && reachable(y, x, (lnI != 0 && lnI != 1),
                    lnI == 6 ? midenX[3] : midenX[1], lnI == 6 ? midenY[3] : midenY[1], maxLake))
                {
                    map2[y + 0, x + 0] = 0x00;
                    map2[y + 0, x + 1] = 0x12;
                    map2[y + 1, x + 0] = 0x10;
                    map2[y + 1, x + 1] = 0x11;

                    int byteToUse = (lnI == 0 ? 0xa28f : lnI == 1 ? 0xa295 : lnI == 2 ? 0xa29b : lnI == 3 ? 0xa2a1 : lnI == 4 ? 0xa2a4 : lnI == 5 ? 0xa2e9 : 0xa2b3);
                    romData[byteToUse] = (byte)(x + 1);
                    romData[byteToUse + 1] = (byte)(y + 1);
                    if (lnI == 5) // Charlock castle, out of order as far as byte sequence is concerned.
                    {
                        romData[0xa334] = (byte)(x);
                        romData[0xa335] = (byte)(y + 1);
                    }
                    else
                    {
                        romData[byteToUse + 0x7e] = (byte)(x);
                        romData[byteToUse + 1 + 0x7e] = (byte)(y + 1);
                    }
                    if (lnI == 3)
                    {
                        // Replace Tantegel music with the zone surrounding Tantegel.
                        romData[0x3e356] = (byte)((x / 8) * 8);
                        romData[0x3e35a] = (byte)(((x / 8) + 1) * 8);
                        romData[0x3e360] = (byte)((y / 8) * 8);
                        romData[0x3e364] = (byte)(((y / 8) + 1) * 8);
                    }
                    //if (lnI == 6)
                    //{
                    //    romData[0xa301] = (byte)(x);
                    //    romData[0xa302] = (byte)(y + 1);
                    //    romData[0xfd95] = 0x80;
                    //    romData[0xfd96] = 0x0d;
                    //    romData[0xfd97] = 0x18;
                    //}

                    // Return points
                    if (lnI == 0 || lnI == 1 || lnI == 3 || lnI == 4)
                    {
                        int byteMultiplier = lnI - (lnI >= 3 ? 1 : 0);
                        romData[0xa27a + (3 * byteMultiplier)] = (byte)x;
                        if (map2[y + 2, x] == 0x04)
                            romData[0xa27a + (3 * byteMultiplier) + 1] = (byte)(y + 2);
                        else
                            romData[0xa27a + (3 * byteMultiplier) + 1] = (byte)(y + 1);
                        shipPlacement(0x1bf84 + (2 * byteMultiplier), y, x, maxLake);
                    }
                }
                else
                    lnI--;
            }

            // Now we'll place all of the towns now.
            // Leftwyne must be 15/30 squares or less away from Midenhall.  Hamlin has to be 30/60 squares or less away from Midenhall.
            for (int lnI = 0; lnI < 7; lnI++)
            {
                //if (lnI == 6) lnI = lnI;
                int x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                int y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);

                if (validPlot(y, x, 1, 2, (lnI == 0 ? new int[] { maxIsland2[1] } : lnI == 1 ? new int[] { maxIsland2[2] } : lnI == 2 ? new int[] { maxIsland2[0] } : islands2.ToArray()))
                    && reachable(y, x, (lnI != 0 && lnI != 1 && lnI != 2), (lnI == 1 ? midenX[2] : lnI == 2 ? midenX[0] : midenX[1]), (lnI == 1 ? midenY[2] : lnI == 2 ? midenY[0] : midenY[1]), maxLake))
                {
                    map2[y, x + 0] = 0x0e;
                    map2[y, x + 1] = 0x0f;

                    int byteToUse2 = (lnI == 0 ? 0xa292 : lnI == 1 ? 0xa298 : lnI == 2 ? 0xa29e : lnI == 3 ? 0xa2a7 : lnI == 4 ? 0xa2aa : lnI == 5 ? 0xa2ad : 0xa2b0);
                    romData[byteToUse2] = (byte)(x + 1);
                    romData[byteToUse2 + 1] = (byte)(y);
                    romData[byteToUse2 + 0x7e] = (byte)(x);
                    romData[byteToUse2 + 1 + 0x7e] = (byte)(y);

                    // Return points
                    if (lnI == 2)
                        shipPlacement(0x3d6be, y, x, maxLake);
                    // Return points
                    else if (lnI == 1)
                    {
                        romData[0xa27a + 18] = (byte)(x);
                        if (map2[y + 1, x] == 0x04)
                            romData[0xa27a + 19] = (byte)(y);
                        else
                            romData[0xa27a + 19] = (byte)(y + 1);
                        shipPlacement(0x1bf84 + 12, y, x, maxLake);
                    }
                    else if (lnI == 6)
                    {
                        romData[0xa27a + 12] = (byte)(x);
                        if (map2[y + 1, x] == 0x04)
                            romData[0xa27a + 13] = (byte)(y);
                        else
                            romData[0xa27a + 13] = (byte)(y + 1);
                        // We are placing the ship in both Beran and the Rhone Shrine at the same time.
                        shipPlacement(0x1bf84 + 8, y, x, maxLake);
                        shipPlacement(0x1bf84 + 10, y, x, maxLake);
                    }
                }
                else
                    lnI--;
            }

            // Then the monoliths.
            // All of these can go anywhere.
            for (int lnI = 0; lnI < 13; lnI++)
            {
                if ((lnI == 0) && chkSmallMap.Checked) continue; // Remove the Midenhall Island shrine which is of no importance.
                // lnI == 1 is probably the Cannock shrine... want to put that in Zone 1...

                int x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                int y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                if (lnI == 6)
                {
                    x = midenX[3];
                    y = midenY[3];
                }
                else if (lnI == 7)
                {
                    x = midenX[0];
                    y = midenY[0];
                }

                if (validPlot(y, x, 1, 1, (lnI == 1 || lnI == 12 ? new int[] { maxIsland2[1] } : lnI == 6 ? new int[] { maxIsland2[3] } : lnI == 7 ? new int[] { maxIsland2[0] } : lnI == 8 ? new int[] { maxIsland2[2] } : islands2.ToArray()))
                    && reachable(y, x, (lnI != 1 && lnI != 12 && lnI != 8 && lnI != 7 && lnI != 6), lnI == 6 ? midenX[3] : lnI == 7 ? midenX[0] : lnI == 8 ? midenX[2] : midenX[1],
                    lnI == 6 ? midenY[3] : lnI == 7 ? midenY[0] : lnI == 8 ? midenY[2] : midenY[1], maxLake))
                {
                    map2[y, x] = 0x0b;

                    int byteToUse2 = 0xa2b6 + (lnI * 3); // (lnI < 11 ? 0xa2b6 + (lnI * 3) : 0xa2da);
                    romData[byteToUse2] = (byte)(x);
                    romData[byteToUse2 + 1] = (byte)(y);

                    // Return points
                    if (lnI == 6)
                    {
                        romData[0xa27a + 15] = (byte)(x);
                        if (map2[y + 1, x] == 0x04)
                            romData[0xa27a + 16] = (byte)(y);
                        else
                            romData[0xa27a + 16] = (byte)(y + 1);
                    }
                }
                else
                    lnI--;
            }

            // Then the caves.
            // Make sure the lake and spring cave is no more than 16/32 squares outside of Midenhall
            for (int lnI = 0; lnI < 9; lnI++)
            {
                int x = 300;
                int y = 300;
                if (lnI == 6)
                {
                    x = midenX[2];
                    y = midenY[2];
                }
                else
                {
                    x = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                    y = r1.Next() % (chkSmallMap.Checked ? 125 : 253);
                }

                if (validPlot(y, x, 1, 1, (lnI == 0 || lnI == 6 ? new int[] { maxIsland2[2] } : lnI == 1 || lnI == 5 ? new int[] { maxIsland2[1] } : lnI == 7 ? new int[] { maxIsland2[3] } : islands2.ToArray()))
                    && reachable(y, x, (lnI != 0 && lnI != 1 && lnI != 5 && lnI != 6 && lnI != 7),
                    lnI == 0 || lnI == 6 ? midenX[2] : lnI == 7 ? midenX[3] : midenX[1], lnI == 0 || lnI == 6 ? midenY[2] : lnI == 7 ? midenY[3] : midenY[1], maxLake))
                {
                    map2[y, x] = 0x0c;

                    int byteToUse2 = (lnI == 0 ? 0xa2dd : lnI == 1 ? 0xa2e0 : lnI == 2 ? 0xa2e3 : lnI == 3 ? 0xa2ef : lnI == 4 ? 0xa2fb : lnI == 5 ? 0xa2fe : lnI == 6 ? 0xa304 : lnI == 7 ? 0xa307 : 0xa30a);
                    romData[byteToUse2] = (byte)x;
                    romData[byteToUse2 + 1] = (byte)(y);
                }
                else
                    lnI--;
            }

            // Finally the towers
            // Need to make sure the wind tower is no more than 14/28 squares outside of Midenhall
            for (int lnI = 0; lnI < 5; lnI++)
            {
                if ((lnI == 3 || lnI == 4) && chkSmallMap.Checked) continue; // Remove the Dragon's Horns from the small map
                int x = r1.Next() % (chkSmallMap.Checked ? 122 : 250);
                int y = r1.Next() % (chkSmallMap.Checked ? 122 : 250);

                // Need to make sure it's a valid 7x7 plot due to dropping with the Cloak of wind...
                if (validPlot(y, x, 3, 3, (lnI == 0 ? new int[] { maxIsland2[2] } : islands2.ToArray()))
                    && reachable(y, x, (lnI != 0), lnI == 0 ? midenX[2] : midenX[1], lnI == 0 ? midenY[2] : midenY[1], maxLake))
                {
                    map2[y + 3, x + 3] = 0x0a;

                    int byteToUse2 = (lnI == 0 ? 0xa2e6 : lnI == 1 ? 0xa2ec : lnI == 2 ? 0xa2f2 : lnI == 3 ? 0xa2f5 : 0xa2f8);
                    romData[byteToUse2] = (byte)(x + 3);
                    romData[byteToUse2 + 1] = (byte)(y + 3);
                }
                else
                    lnI--;
            }

            int[,] monsterZones = new int[16, 16];
            for (int lnI = 0; lnI < 16; lnI++)
                for (int lnJ = 0; lnJ < 16; lnJ++)
                    monsterZones[lnI, lnJ] = 0xff;

            int midenMZX = midenX[1] / 8;
            int midenMZY = midenY[1] / 8;

            for (int mzX = 0; mzX < 16; mzX++)
                for (int mzY = 0; mzY < 16; mzY++)
                {
                    if (zone2[mzX, mzY] / 1000 == 1)
                    {
                        if (Math.Abs(midenMZX - mzX) == 0 && Math.Abs(midenMZY - mzY) == 0)
                            monsterZones[mzY, mzX] = 0;
                        else if (Math.Abs(midenMZX - mzX) <= 1 && Math.Abs(midenMZY - mzY) <= 1)
                            monsterZones[mzY, mzX] = 2;
                        else if (Math.Abs(midenMZX - mzX) <= 1 || Math.Abs(midenMZY - mzY) <= 1)
                            monsterZones[mzY, mzX] = 1;
                        else if (Math.Abs(midenMZX - mzX) <= 2 || Math.Abs(midenMZY - mzY) <= 2)
                            monsterZones[mzY, mzX] = r1.Next() % 9;
                        else
                            monsterZones[mzY, mzX] = r1.Next() % 18;
                    }
                    else if (zone2[mzX, mzY] / 1000 == 2)
                        monsterZones[mzY, mzX] = r1.Next() % 5 + 0x0d;
                    else if (zone2[mzX, mzY] / 1000 == 3)
                        monsterZones[mzY, mzX] = r1.Next() % 2 + 0x32;
                    else
                    {
                        while (monsterZones[mzY, mzX] > 0x27 || (monsterZones[mzY, mzX] >= 0x1c && monsterZones[mzY, mzX] <= 0x1f))
                            monsterZones[mzY, mzX] = r1.Next() % 19 + 0x15;
                        if (monsterZones[mzY, mzX] == 0x26) monsterZones[mzY, mzX] = 0x39;
                        if (monsterZones[mzY, mzX] == 0x27) monsterZones[mzY, mzX] = 0x3b;
                    }

                    monsterZones[mzY, mzX] += (64 * (r1.Next() % 4));
                }

            // Now let's enter all of this into the ROM...
            int lnPointer = 0x9f97;

            for (int lnI = 0; lnI <= 256; lnI++) // <---- There is a final pointer for lnI = 256, probably indicating the conclusion of the map.
            {
                romData[0xdda5 + (lnI * 2)] = (byte)(lnPointer % 256);
                romData[0xdda6 + (lnI * 2)] = (byte)(lnPointer / 256);

                int lnJ = 0;
                while (lnI < 256 && lnJ < 256)
                {
                    if (map2[lnI, lnJ] >= 1 && map2[lnI, lnJ] <= 7)
                    {
                        int tileNumber = 0;
                        int numberToMatch = map2[lnI, lnJ];
                        while (lnJ < 256 && tileNumber < 32 && map2[lnI, lnJ] == numberToMatch && tileNumber < 32)
                        {
                            tileNumber++;
                            lnJ++;
                        }
                        romData[lnPointer + 0x4010] = (byte)((0x20 * numberToMatch) + (tileNumber - 1));
                        lnPointer++;
                    }
                    else
                    {
                        romData[lnPointer + 0x4010] = (byte)map2[lnI, lnJ];
                        lnPointer++;
                        lnJ++;
                    }
                }
            }
            //lnPointer = lnPointer;
            if (lnPointer >= 0xb8f7)
            {
                MessageBox.Show("WARNING:  The map might have taken too much ROM space...");
                // Might have to compress further to remove one byte stuff
                // Must compress the map by getting rid of further 1 byte lakes
            }

            // Ensure monster zones are 8x8
            if (chkSmallMap.Checked)
            {
                romData[0x10083] = 0x85;
                romData[0x10084] = 0xd5;
                romData[0x10085] = 0xa5;
                romData[0x10086] = 0x17;
                romData[0x10087] = 0x29;
                romData[0x10088] = 0x78;
                romData[0x10089] = 0x0a;
            }

            // Enter monster zones
            for (int lnI = 0; lnI < 16; lnI++)
                for (int lnJ = 0; lnJ < 16; lnJ++)
                {
                    if (monsterZones[lnI, lnJ] == 0xff)
                        monsterZones[lnI, lnJ] = (r1.Next() % 60) + ((r1.Next() % 4) * 64);
                    romData[0x103d6 + (lnI * 16) + lnJ] = (byte)monsterZones[lnI, lnJ];
                }

            return true;
        }

        private void markZoneSides()
        {
            for (int x = 0; x < 16; x++)
                for (int y = 0; y < 16; y++)
                {
                    // 1 = north, 2 = east, 4 = south, 8 = west
                    if (y == 0) zone2[x, y] += 1;
                    else if (zone2[x, y - 1] / 1000 != zone2[x, y] / 1000) zone2[x, y] += 1;

                    if (x == 15) zone2[x, y] += 2;
                    else if (zone2[x + 1, y] / 1000 != zone2[x, y] / 1000) zone2[x, y] += 2;

                    if (y == 15) zone2[x, y] += 4;
                    else if (zone2[x, y + 1] / 1000 != zone2[x, y] / 1000) zone2[x, y] += 4;

                    if (x == 0) zone2[x, y] += 8;
                    else if (zone2[x - 1, y] / 1000 != zone2[x, y] / 1000) zone2[x, y] += 8;
                }
        }

        private void generateZoneMap(int zoneToUse, bool mountains, int islandSize, Random r1)
        {
            if (mountains)
                for (int x = 0; x < 16; x++)
                    for (int y = 0; y < 16; y++)
                        if (zone2[x, y] / 1000 == zoneToUse / 1000 && zone2[x, y] % 1000 > 0)
                            for (int x2 = x * 8; x2 < (x * 8) + 8; x2++)
                                for (int y2 = y * 8; y2 < (y * 8) + 8; y2++)
                                    map2[y2, x2] = 0x05;

            int[] terrainTypes = { 1, 1, 1, 2, 2, 7, 7, 5, 3, 3, 3, 6, 6, 6 };

            for (int lnI = 0; lnI < 100; lnI++)
            {
                int swapper1 = r1.Next() % terrainTypes.Length;
                int swapper2 = r1.Next() % terrainTypes.Length;
                int temp = terrainTypes[swapper1];
                terrainTypes[swapper1] = terrainTypes[swapper2];
                terrainTypes[swapper2] = temp;
            }

            int lnMarker = -1;
            int totalLand = 0;

            while (totalLand < islandSize)
            {
                lnMarker++;
                lnMarker = (lnMarker >= terrainTypes.Length ? 0 : lnMarker);
                int sizeToUse = (r1.Next() % 400) + 150;
                //if (terrainTypes[lnMarker] == 5) sizeToUse /= 2;

                List<int> points = new List<int> { (r1.Next() % 125) + 2, (r1.Next() % 125) + 2 };
                if (validPoint(points[0], points[1], zoneToUse, mountains))
                {
                    while (sizeToUse > 0)
                    {
                        List<int> newPoints = new List<int>();
                        for (int lnI = 0; lnI < points.Count; lnI += 2)
                        {
                            int lnX = points[lnI];
                            int lnY = points[lnI + 1];

                            //if (lnX <= 1 || lnY <= 1 || lnY >= 126 || lnY >= 126) continue;

                            int direction = (r1.Next() % 16);
                            map2[lnY, lnX] = terrainTypes[lnMarker];
                            island2[lnY, lnX] = zoneToUse;
                            // 1 = North, 2 = east, 4 = south, 8 = west
                            if (direction % 8 >= 4 && lnY <= 125)
                            {
                                if (validPoint(lnX, lnY + 1, zoneToUse, mountains))
                                {
                                    if (map2[lnY + 1, lnX] == 4)
                                        totalLand++;
                                    map2[lnY + 1, lnX] = terrainTypes[lnMarker];
                                    island2[lnY + 1, lnX] = zoneToUse;
                                    newPoints.Add(lnX);
                                    newPoints.Add(lnY + 1);
                                }
                            }
                            if (direction % 2 >= 1 && lnY >= 2)
                            {
                                if (validPoint(lnX, lnY - 1, zoneToUse, mountains))
                                {
                                    if (map2[lnY - 1, lnX] == 4)
                                        totalLand++;
                                    map2[lnY - 1, lnX] = terrainTypes[lnMarker];
                                    island2[lnY - 1, lnX] = zoneToUse;
                                    newPoints.Add(lnX);
                                    newPoints.Add(lnY - 1);
                                }
                            }
                            if (direction % 4 >= 2 && lnX <= 125)
                            {
                                if (validPoint(lnX + 1, lnY, zoneToUse, mountains))
                                {
                                    if (map2[lnY, lnX + 1] == 4)
                                        totalLand++;
                                    map2[lnY, lnX + 1] = terrainTypes[lnMarker];
                                    island2[lnY, lnX + 1] = zoneToUse;
                                    newPoints.Add(lnX + 1);
                                    newPoints.Add(lnY);
                                }
                            }
                            if (direction % 16 >= 8 && lnX >= 2)
                            {
                                if (validPoint(lnX - 1, lnY, zoneToUse, mountains))
                                {
                                    if (map2[lnY, lnX - 1] == 4)
                                        totalLand++;
                                    map2[lnY, lnX - 1] = terrainTypes[lnMarker];
                                    island2[lnY, lnX - 1] = zoneToUse;
                                    newPoints.Add(lnX - 1);
                                    newPoints.Add(lnY);
                                }
                            }

                            int takeaway = 1 + (direction > 8 ? 1 : 0) + (direction % 8 > 4 ? 1 : 0) + (direction % 4 > 2 ? 1 : 0) + (direction % 2 > 1 ? 1 : 0);
                            sizeToUse--;
                        }
                        if (sizeToUse <= 0) break;
                        if (newPoints.Count != 0)
                            points = newPoints;
                    }
                }
            }

            // Fill in water...
            for (int lnY = 0; lnY < 128; lnY++)
                for (int lnX = 0; lnX < 125; lnX++)
                {
                    if (island2[lnY, lnX] == zoneToUse && island2[lnY, lnX + 1] == zoneToUse && island2[lnY, lnX + 2] == zoneToUse && island2[lnY, lnX + 3] == zoneToUse)
                    {
                        List<int> land = new List<int> { 1, 2, 3, 5, 6, 7 };
                        if (map2[lnY, lnX] == map2[lnY, lnX + 2] && map2[lnY, lnX] != map2[lnY, lnX + 1]) { map2[lnY, lnX + 1] = map2[lnY, lnX]; island2[lnY, lnX + 1] = island2[lnY, lnX]; }
                        if (lnX < 124 && land.Contains(map2[lnY, lnX]) && !land.Contains(map2[lnY, lnX + 1]) && !land.Contains(map2[lnY, lnX + 2]) && land.Contains(map2[lnY, lnX + 3]))
                        {
                            map2[lnY, lnX + 1] = map2[lnY, lnX];
                            map2[lnY, lnX + 2] = map2[lnY, lnX + 3];
                            island2[lnY, lnX + 1] = island2[lnY, lnX];
                            island2[lnY, lnX + 2] = island2[lnY, lnX + 3];
                        }
                    }
                }


            markIslands(zoneToUse);
        }

        private bool validPoint(int x, int y, int zoneToUse, bool mountains = false)
        {
            // Establish zone
            int zoneX = x / 8;
            int zoneY = y / 8;
            int zoneSides = zone2[zoneX, zoneY] % 1000;
            if (zone2[zoneX, zoneY] % 1000 != 0 && mountains) return false;
            if (zone2[zoneX, zoneY] / 1000 != zoneToUse / 1000) return false;
            // 1 = north, 2 = east, 4 = south, 8 = west
            if (y % 8 == 0 && zoneSides % 2 == 1) return false;
            if (x % 8 == 7 && zoneSides % 4 >= 2) return false;
            if (y % 8 == 7 && zoneSides % 8 >= 4) return false;
            if (x % 8 == 0 && zoneSides % 16 >= 8) return false;

            return true;
        }

        private void markIslands(int zoneToUse)
        {
            // We should mark islands and inaccessible land...
            int landNumber = zoneToUse + 1;
            int maxLand = -2;

            int maxLandPlots = 0;
            int lastIsland = 0;
            for (int lnI = 0; lnI < 256; lnI++)
                for (int lnJ = 0; lnJ < 256; lnJ++)
                {
                    if (island2[lnI, lnJ] == zoneToUse && map2[lnI, lnJ] != 0x05)
                    {
                        int plots = landPlot(landNumber, lnI, lnJ, zoneToUse);
                        if (plots > maxLandPlots)
                        {
                            maxLandPlots = plots;
                            maxLand = landNumber;

                        }
                        islands2.Add(landNumber);
                        landNumber++;

                        lastIsland = island2[lnI, lnJ];
                    }
                }

            maxIsland2[zoneToUse / 1000] = maxLand;
        }

        private void resetIslands()
        {
            for (int y = 0; y < 256; y++)
                for (int x = 0; x < 256; x++)
                {
                    if (island2[y, x] != 200 && island2[y, x] != -1)
                    {
                        island2[y, x] /= 1000;
                        island2[y, x] *= 1000;
                    }
                }

            islands2.Clear();

            markIslands(3000);
            markIslands(1000);
            markIslands(2000);
            markIslands(0);
        }

        private void createBridges(Random r1)
        {
            List<BridgeList> bridgePossible = new List<BridgeList>();
            List<islandLinks> islandPossible = new List<islandLinks>();
            // Create bridges for points three spaces or less from two distinctly numbered islands.  Extend islands if there is interference.
            for (int y = 1; y < 252; y++)
                for (int x = 1; x < 252; x++)
                {
                    if (y == 78 && x == 3) map2[y, x] = map2[y, x];
                    if (map2[y, x] == 0x05 || map2[y, x] == 0x04) continue;

                    for (int lnI = 2; lnI <= 4; lnI++)
                    {
                        if (island2[y, x] != island2[y + lnI, x] && island2[y, x] / 1000 == island2[y + lnI, x] / 1000 && map2[y + lnI, x] != 0x04 && map2[y + lnI, x] != 0x05)
                        {
                            bool fail = false;
                            for (int lnJ = 1; lnJ < lnI; lnJ++)
                            {
                                if (map2[y + lnJ, x] != 0x04)
                                {
                                    fail = true;
                                    //map[y + lnJ, x - 1] = 0x04; map[y + lnJ, x + 1] = 0x04;
                                    //island[y + lnJ, x - 1] = 0x04; island[y + lnJ, x + 1] = 0x04;
                                } // else
                                //{
                                //    fail = true;
                                //}
                                //if (map[y + lnJ, x] != 0x04 || map[y + lnJ, x + 1] != 0x04 || map[y + lnJ, x - 1] != 0x04) fail = true;
                            }
                            if (!fail)
                            {
                                bridgePossible.Add(new BridgeList(x, y, true, lnI, island2[y, x], island2[y + lnI, x]));
                                if (islandPossible.Where(c => c.island1 == island2[y, x] && c.island2 == island2[y + lnI, x]).Count() == 0)
                                    islandPossible.Add(new islandLinks(island2[y, x], island2[y + lnI, x]));
                            }
                        }

                        if (island2[y, x] != island2[y, x + lnI] && island2[y, x] / 1000 == island2[y, x + lnI] / 1000 && map2[y, x + lnI] != 0x04 && map2[y, x + lnI] != 0x05)
                        {
                            bool fail = false;
                            for (int lnJ = 1; lnJ < lnI; lnJ++)
                            {
                                if (map2[y, x + lnJ] != 0x04)
                                {
                                    fail = true;
                                    //    map[y - 1, x + lnJ] = 0x04; map[y + 1, x + lnJ] = 0x04;
                                    //    island[y - 1, x + lnJ] = 200; island[y + 1, x + lnJ] = 200;
                                    //} else
                                    //{
                                    //    fail = true;
                                }

                                //if (map[y, x + lnJ] != 0x04 || map[y + 1, x + lnJ] != 0x04 || map[y - 1, x + lnJ] != 0x04) fail = true;
                            }
                            if (!fail)
                            {
                                bridgePossible.Add(new BridgeList(x, y, false, lnI, island2[y, x], island2[y, x + lnI]));
                                if (islandPossible.Where(c => c.island1 == island2[y, x] && c.island2 == island2[y, x + lnI]).Count() == 0)
                                    islandPossible.Add(new islandLinks(island2[y, x], island2[y, x + lnI]));
                            }
                        }
                    }
                }

            foreach (islandLinks islandLink in islandPossible)
            {
                List<BridgeList> test = bridgePossible.Where(c => c.island1 == islandLink.island1 && c.island2 == islandLink.island2).ToList();

                int tries = 50;
                bool pass = false;
                while (!pass && tries > 0)
                {
                    tries--;

                    // Choose one bridge out of the possibilities
                    BridgeList bridgeToBuild = test[r1.Next() % test.Count];
                    // Then confirm that the bridge is still possible...
                    int bridgeTest = map2[bridgeToBuild.y, bridgeToBuild.x];
                    int bridgeTest2 = (bridgeToBuild.south ? map2[bridgeToBuild.y + bridgeToBuild.distance, bridgeToBuild.x] : map2[bridgeToBuild.y, bridgeToBuild.x + bridgeToBuild.distance]);

                    if (bridgeTest == 0x04 || bridgeTest == 0x0d || bridgeTest == 0x09 || bridgeTest2 == 0x04 || bridgeTest2 == 0x0d || bridgeTest2 == 0x09)
                        continue;

                    for (int lnI = 1; lnI <= bridgeToBuild.distance - 1; lnI++)
                    {
                        int bridgeTest3 = (bridgeToBuild.south ? map2[bridgeToBuild.y + lnI, bridgeToBuild.x] : map2[bridgeToBuild.y, bridgeToBuild.x + lnI]);
                        if (bridgeTest3 != 0x04)
                            continue;
                    }

                    for (int lnI = 1; lnI <= bridgeToBuild.distance - 1; lnI++)
                    {
                        if (bridgeToBuild.south)
                        {
                            map2[bridgeToBuild.y + lnI, bridgeToBuild.x - 1] = 0x04; map2[bridgeToBuild.y + lnI, bridgeToBuild.x + 1] = 0x04;
                            island2[bridgeToBuild.y + lnI, bridgeToBuild.x - 1] = 0x04; island2[bridgeToBuild.y + lnI, bridgeToBuild.x + 1] = 0x04;

                            map2[bridgeToBuild.y + lnI, bridgeToBuild.x] = 0x0d;
                            island2[bridgeToBuild.y + lnI, bridgeToBuild.x] = bridgeToBuild.island1;
                        }
                        else
                        {
                            map2[bridgeToBuild.y - 1, bridgeToBuild.x + lnI] = 0x04; map2[bridgeToBuild.y + 1, bridgeToBuild.x + lnI] = 0x04;
                            island2[bridgeToBuild.y - 1, bridgeToBuild.x + lnI] = 200; island2[bridgeToBuild.y + 1, bridgeToBuild.x + lnI] = 200;

                            map2[bridgeToBuild.y, bridgeToBuild.x + lnI] = 0x09;
                            island2[bridgeToBuild.y, bridgeToBuild.x + lnI] = bridgeToBuild.island1;
                        }
                    }
                    pass = true;
                }
            }
        }

        private class islandLinks
        {
            public int island1;
            public int island2;

            public islandLinks(int pI1, int pI2)
            {
                island1 = pI1; island2 = pI2;
            }
        }

        private class BridgeList
        {
            public int x;
            public int y;
            public bool south;
            public int distance;
            public int island1;
            public int island2;

            public BridgeList(int pX, int pY, bool pS, int pDist, int pI1, int pI2)
            {
                x = pX; y = pY; south = pS; distance = pDist; island1 = pI1; island2 = pI2;
            }
        }

        private bool createZone(int zoneNumber, int size, bool rectangle, Random r1)
        {
            int tries = 1000;
            bool firstZone = true;

            if (!rectangle)
            {
                while (size > 0 && tries > 0)
                {
                    int x = r1.Next() % 16;
                    int y = r1.Next() % 16;
                    int minX = x, maxX = x, minY = y, maxY = y;
                    if (!firstZone && zone2[x, y] != zoneNumber)
                    {
                        continue;
                    }
                    if (firstZone)
                    {
                        firstZone = false;
                        zone2[x, y] = zoneNumber;
                    }

                    tries--;
                    int direction = r1.Next() % 16;
                    int totalDirections = 0;
                    if (direction % 16 >= 8) totalDirections++;
                    if (direction % 8 >= 4) totalDirections++;
                    if (direction % 4 >= 2) totalDirections++;
                    if (direction % 2 >= 1) totalDirections++;
                    if (totalDirections > size) continue;

                    // 1 = north, 2 = east, 4 = south, 8 = west
                    if (direction % 16 >= 8 && x != 0 && zone2[x - 1, y] == 0 && (minX <= (x - 1) || maxX - minX <= 11))
                    {
                        zone2[x - 1, y] = zoneNumber;
                        minX = (x - 1 < minX ? x - 1 : minX);
                        size--;
                        tries = 100;
                    }
                    if (direction % 8 >= 4 && y != 15 && zone2[x, y + 1] == 0 && (maxY >= (y + 1) || maxY - minY <= 11))
                    {
                        zone2[x, y + 1] = zoneNumber;
                        maxY = (y + 1 > maxY ? y + 1 : maxY);
                        size--;
                        tries = 100;
                    }
                    if (direction % 4 >= 2 && x != 15 && zone2[x + 1, y] == 0 && (minX >= (x + 1) || maxX - minX <= 11))
                    {
                        zone2[x + 1, y] = zoneNumber;
                        maxX = (x + 1 > maxX ? x + 1 : maxX);
                        size--;
                        tries = 100;
                    }
                    if (direction % 2 >= 1 && y != 0 && zone2[x, y - 1] == 0 && (minY <= (y - 1) || maxY - minY <= 11))
                    {
                        zone2[x, y - 1] = zoneNumber;
                        minY = (y - 1 < minY ? y - 1 : minY);
                        size--;
                        tries = 100;
                    }
                }
                return (size <= 0);
            }
            else
            {
                int minMeasurement = (int)Math.Ceiling((double)size / 12);
                int maxMeasurement = (int)Math.Ceiling((double)size / minMeasurement);

                int length = ((r1.Next() % (maxMeasurement - minMeasurement)) + minMeasurement);
                int width = size / length;

                int x = (r1.Next() % (16 - length));
                int y = (r1.Next() % (16 - width));

                for (int i = x; i < x + length; i++)
                    for (int j = y; j < y + width; j++)
                        zone2[i, j] = zoneNumber;

                // Snow definition
                romData[0x3e2b6] = (byte)(y * 8);
                romData[0x3e2ba] = (byte)((y + width) * 8);
                romData[0x3e2ac] = (byte)(x * 8);
                romData[0x3e2b0] = (byte)((x + length) * 8);

                // Tantegel definition - TODO:  Find romData location, then change so it's on an 8x8 grid around Tantegel
                //romData[0x3e2b6] = (byte)(y * 8);
                //romData[0x3e2ba] = (byte)((y + width) * 8);
                //romData[0x3e2ac] = (byte)(x * 8);
                //romData[0x3e2b0] = (byte)((x + length) * 8);

                return true;
            }
        }

        private bool reachable(int startY, int startX, bool water, int finishX, int finishY, int maxLake)
        {
            int x = startX;
            int y = startY;

            List<int> validPlots = new List<int> { 0, 1, 2, 3, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            if (water) validPlots.Add(4);

            bool first = true;
            List<int> toPlot = new List<int>();
            bool[,] plotted = new bool[256, 256];

            while (first || toPlot.Count != 0)
            {
                if (!first)
                {
                    y = toPlot[0];
                    toPlot.RemoveAt(0);
                    x = toPlot[0];
                    toPlot.RemoveAt(0);
                }
                else
                {
                    first = false;
                }

                for (int dir = 0; dir < 5; dir++)
                {
                    int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
                    dirX = (dirX == 256 ? 0 : dirX == -1 ? 255 : dirX);
                    int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
                    dirY = (dirY == 256 ? 0 : dirY == -1 ? 255 : dirY);

                    if (validPlots.Contains(map2[dirY, dirX]) && (map2[dirY, dirX] != 4 || island2[dirY, dirX] == maxLake))
                    {
                        if (dir != 0 && plotted[dirY, dirX] == false)
                        {
                            if (finishX == dirX && finishY == dirY)
                                return true;
                            toPlot.Add(dirY);
                            toPlot.Add(dirX);
                            plotted[dirY, dirX] = true;
                        }
                    }
                }
            }

            return false;
        }

        private int lakePlot(int lakeNumber, int y, int x, bool fill = false, int islandNumber = -1)
        {
            bool first = true;
            List<int> toPlot = new List<int>();
            int plots = 1;
            //if (islandNumber >= 0) plots = 1;
            while (first || toPlot.Count != 0)
            {
                if (!first)
                {
                    y = toPlot[0];
                    toPlot.RemoveAt(0);
                    x = toPlot[0];
                    toPlot.RemoveAt(0);
                }
                else
                {
                    if (fill)
                        map2[y, x] = (islandNumber == 0 ? 0x01 : islandNumber == 1 ? 0x06 : islandNumber == 2 ? 0x03 : islandNumber == 3 ? 0x02 : islandNumber == 4 ? 0x07 : 0x05);
                    first = false;
                }

                for (int dir = 0; dir < 5; dir++)
                {
                    int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
                    dirX = (dirX == 256 ? 0 : dirX == -1 ? 255 : dirX);
                    int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
                    dirY = (dirY == 256 ? 0 : dirY == -1 ? 255 : dirY);

                    if (island2[dirY, dirX] == -1 || (island2[dirY, dirX] == lakeNumber && fill))
                    {
                        plots++;
                        island2[dirY, dirX] = (fill ? islandNumber : lakeNumber);
                        if (fill)
                            map2[dirY, dirX] = (islandNumber == 0 ? 0x01 : islandNumber == 1 ? 0x06 : islandNumber == 2 ? 0x03 : islandNumber == 3 ? 0x02 : islandNumber == 4 ? 0x07 : 0x05);

                        if (dir != 0)
                        {
                            toPlot.Add(dirY);
                            toPlot.Add(dirX);
                        }
                        //plots += lakePlot(lakeNumber, y, x, fill);
                    }
                }
            }

            return plots;
        }

        private int landPlot(int landNumber, int y, int x, int zoneToUse = 0)
        {
            bool first = true;
            List<int> toPlot = new List<int>();
            int plots = 1;
            while (first || toPlot.Count != 0)
            {
                if (!first)
                {
                    y = toPlot[0];
                    toPlot.RemoveAt(0);
                    x = toPlot[0];
                    toPlot.RemoveAt(0);
                }
                else
                {
                    first = false;
                }

                for (int dir = 0; dir < 5; dir++)
                {
                    int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
                    dirX = (dirX == 256 ? 0 : dirX == -1 ? 255 : dirX);
                    int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
                    dirY = (dirY == 256 ? 0 : dirY == -1 ? 255 : dirY);

                    if (island2[dirY, dirX] == zoneToUse)
                    {
                        plots++;
                        island2[dirY, dirX] = landNumber;

                        if (dir != 0)
                        {
                            toPlot.Add(dirY);
                            toPlot.Add(dirX);
                        }
                    }
                }
            }

            return plots;
        }

        private bool validPlot(int y, int x, int height, int width, int[] legalIsland)
        {
            //y++;
            //x++;
            for (int lnI = 0; lnI < height; lnI++)
                for (int lnJ = 0; lnJ < width; lnJ++)
                {
                    if (y + lnI >= (chkSmallMap.Checked ? 128 : 256) || x + lnJ >= (chkSmallMap.Checked ? 128 : 256)) return false;

                    int legalY = (y + lnI >= 256 ? y - 256 + lnI : y + lnI);
                    int legalX = (x + lnJ >= 256 ? x - 256 + lnJ : x + lnJ);

                    bool ok = false;
                    for (int lnK = 0; lnK < legalIsland.Length; lnK++)
                        if (island2[legalY, legalX] == legalIsland[lnK])
                            ok = true;
                    if (!ok) return false;
                    // map[legalY, legalX] == 0x04 || 
                    if (map2[legalY, legalX] == 0x00 || map2[legalY, legalX] == 0x05 || map2[legalY, legalX] == 0x0a || map2[legalY, legalX] == 0x0b || map2[legalY, legalX] == 0x0c ||
                        map2[legalY, legalX] == 0x0e || map2[legalY, legalX] == 0x0f || map2[legalY, legalX] == 0x10 || map2[legalY, legalX] == 0x11 || map2[legalY, legalX] == 0x12 || map2[legalY, legalX] == 0x13)
                        return false;
                }
            return true;
        }

        private void shipPlacement(int byteToUse, int top, int left, int maxLake = 0)
        {
            int minDirection = -99;
            int minDistance = 999;
            int finalX = 0;
            int finalY = 0;
            int distance = 0;
            int lnJ = top;
            int lnK = left;
            for (int lnI = 0; lnI < 4; lnI++)
            {
                lnJ = top;
                lnK = left;
                if (lnI == 0)
                {
                    while (island2[lnJ, lnK] != maxLake && distance < 200)
                    {
                        distance++;
                        lnJ = (lnJ == 0 ? 255 : lnJ - 1);
                    }
                }
                else if (lnI == 1)
                {
                    while (island2[lnJ, lnK] != maxLake && distance < 200)
                    {
                        distance++;
                        lnJ = (lnJ == 255 ? 0 : lnJ + 1);
                    }
                }
                else if (lnI == 2)
                {
                    while (island2[lnJ, lnK] != maxLake && distance < 200)
                    {
                        distance++;
                        lnK = (lnK == 255 ? 0 : lnK + 1);
                    }
                }
                else
                {
                    while (island2[lnJ, lnK] != maxLake && distance < 200)
                    {
                        distance++;
                        lnK = (lnK == 0 ? 255 : lnK - 1);
                    }
                }
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minDirection = lnI;
                    finalX = lnK;
                    finalY = lnJ;
                }
                distance = 0;
            }
            romData[byteToUse] = (byte)(finalX);
            romData[byteToUse + 1] = (byte)(finalY);
            if (minDirection == 0)
            {
                lnJ = (finalY == 255 ? 0 : finalY + 1);
                while (map2[lnJ, finalX] == 0x05)
                {
                    map2[lnJ, finalX] = 0x07;
                    lnJ = (lnJ == 255 ? 0 : lnJ + 1);
                }
            }
            else if (minDirection == 1)
            {
                lnJ = (finalY == 0 ? 255 : finalY - 1);
                while (map2[lnJ, finalX] == 0x05)
                {
                    map2[lnJ, finalX] = 0x07;
                    lnJ = (lnJ == 0 ? 255 : lnJ - 1);
                }
            }
            else if (minDirection == 2)
            {
                lnK = (finalX == 0 ? 255 : finalX - 1);
                while (map2[finalY, lnK] == 0x05)
                {
                    map2[finalY, lnK] = 0x07;
                    lnK = (lnK == 0 ? 255 : lnK - 1);
                }
            }
            else
            {
                lnK = (finalX == 255 ? 0 : finalX + 1);
                while (map2[finalY, lnK] == 0x05)
                {
                    map2[finalY, lnK] = 0x07;
                    lnK = (lnK == 255 ? 0 : lnK + 1);
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////

        private void randomizeMonsterZones(Random r1)
        {
            for (int lnI = 0; lnI < 20; lnI++)
            {
                for (int lnJ = 0; lnJ < 5; lnJ++)
                {
                    int byteToUse = 0x5b52d + (lnI * 5) + lnJ;
                    int min = (lnI == 16 || lnI == 17 || lnI == 18 ? 30 : 0);
                    int max = (lnI == 0 ? 6 : lnI == 1 ? 11 : lnI == 2 ? 16 : 38);

                    romData[byteToUse] = (byte)((r1.Next() % (max - min)) + min + 1);
                }
            }

            // Alter princess monster - 10% chance of totally randomized monster, 90% chance of Demon Knight or worse.  It can never be a metal slime.
            int princessMonster = 17;
            if (r1.Next() % 10 == 0)
            {
                while (princessMonster == 17)
                    princessMonster = ((r1.Next() % (38 - 1)) + 1);
            }
            else
            {
                int[] legalMonsters = { 0x19, 0x1d, 0x1e, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26 };
                princessMonster = legalMonsters[(r1.Next() % (legalMonsters.Length))];
            }

            int strength = romData[0x5DA0E + (18 * (princessMonster - 1)) + 1];
            int defense = romData[0x5DA0E + (18 * (princessMonster - 1)) + 2];
            int agility = romData[0x5DA0E + (18 * (princessMonster - 1))];
            int hp = romData[0x5DA0E + (18 * (princessMonster - 1)) + 3];

            int newStr = ScaleValue(strength, 2.0, 1.0, r1);
            int newAgi = ScaleValue(agility, 2.0, 1.0, r1);
            int newDef = ScaleValue(agility, 2.0, 1.0, r1);
            int newHP = (hp * 3 / 2) + ScaleValue(hp, 2.0, 1.0, r1);

            if (newHP < 125) newHP = 125;
            if (newHP > 255) newHP = 255;

            int xp = romData[0x5da0e + (18 * (princessMonster - 1)) + 16];
            xp += (romData[0x5da0e + (18 * (princessMonster - 1)) + 17] * 256);

            double newXP = Math.Floor(((double)newStr / strength) * ((double)newDef / defense) * Math.Sqrt((double)newAgi / agility) * ((double)newHP / hp) * xp * 1.5);

            romData[0x5084] = (byte)princessMonster;
            romData[0x591ce] = (byte)(newXP % 256);
            romData[0x591cf] = (byte)(newXP / 256);

            romData[0x59bc7] = (byte)(newHP);
            romData[0x59bcf] = 0x14; // 20 MP automatically
            romData[0x59bd7] = (byte)(newAgi);
            romData[0x59bdc] = (byte)(newDef);
            romData[0x59be4] = (byte)(newStr);

            // Alter Erdrick's Armor monster - anywhere from Green Dragon to Red Dragon
            int[] legalMonsters2 = { 0x19, 0x1f, 0x22, 0x23, 0x24, 0x25, 0x26 };
            princessMonster = legalMonsters2[(r1.Next() % (legalMonsters2.Length))];

            romData[0x500f] = (byte)princessMonster;

            ////////////////////////////////////////////////////////////////////////////
            // Dragon Quest 2 Monster Zones start here
            ////////////////////////////////////////////////////////////////////////////

            for (int lnI = 0; lnI < 68; lnI++)
            {
                if (lnI >= 60 && lnI <= 63) continue;
                for (int lnJ = 0; lnJ < 6; lnJ++)
                {
                    int byteToUse = 0x5bec5 + (lnI * 8) + lnJ;
                    int min = (lnI >= 40 && lnI <= 57 ? 4 * (lnI - 39) : 0);
                    int max = (lnI < 21 ? (lnI + 1) * 2 : 80);

                    romData[byteToUse] = (byte)((r1.Next() % (max - min)) + min + 1);
                }
            }


        }

        private void randomizeMonsterPatterns(Random r1)
        {
            int[] dq1Pattern = { 0, 4, 5, 6, 7, 9, 10, 18, 19, 23, 24, 25 } ;
            int[] dq2Pattern = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };

            int[] monsterPattern = { 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int lnI = 0; lnI <= 121; lnI++)
            {
                if (lnI == 39 || lnI == 121) continue; // Do not randomize DL2 and Malroth

                int byteToUse = 0x5da0e + (lnI * 18);

                if (r1.Next() % 2 == 1)
                {
                    for (int lnJ = 0; lnJ < 8; lnJ++)
                        if (r1.Next() % 2 != 0)
                            monsterPattern[lnJ] = (lnI <= 40 ? dq1Pattern[r1.Next() % dq1Pattern.Length] : dq2Pattern[r1.Next() % dq2Pattern.Length]);
                        else
                            monsterPattern[lnJ] = (lnI <= 40 ? 0 : (r1.Next() % 100 <= (lnI - 40) ? 30 : 0));

                    romData[byteToUse + 4] = (byte)(2 + (r1.Next() % 19));
                    if (lnI == 38) romData[byteToUse + 4] *= 3;
                }
                else
                    for (int lnJ = 0; lnJ < 8; lnJ++)
                        monsterPattern[lnJ] = (lnI <= 40 ? 0 : (r1.Next() % 100 <= (lnI - 40) ? 30 : 0));

                if (lnI == 16 || lnI == 87 || lnI == 105) // Metal Slime, Metal Babble
                {
                    monsterPattern[1] = 0x05;
                    monsterPattern[3] = 0x05;
                    monsterPattern[5] = 0x05;
                }

                for (int lnJ = 0; lnJ < 8; lnJ++)
                {
                    int initial = romData[byteToUse + 6 + lnJ] - (romData[byteToUse + 6 + lnJ] % 32);
                    romData[byteToUse + 6 + lnJ] = (byte)(initial + monsterPattern[lnJ]);
                }
            }
        }

        private void randomizeHeroStats(Random r1)
        {
            for (int lnI = 0; lnI < 32; lnI++)
            {
                int byteToUse = 0xda03d + lnI;
                // We're going to change the code so we tack on strength and agility.  We'll also introduce MP and spells if required.
                if (romData[byteToUse] == 0x66) romData[byteToUse] = 0x01;
                if (romData[byteToUse] == 0x75) romData[byteToUse] = 0x10;
                if (romData[byteToUse] == 0x04) romData[byteToUse] = 0x00;
                if (romData[byteToUse] == 0x05) romData[byteToUse] = 0x01;
                if (romData[byteToUse] == 0x15) romData[byteToUse] = 0x11;
                if (romData[byteToUse] == 0x24) romData[byteToUse] = 0x20;
                if (romData[byteToUse] == 0x25) romData[byteToUse] = 0x21;
                if (romData[byteToUse] == 0x26) romData[byteToUse] = 0x22;
                if (romData[byteToUse] == 0x27) romData[byteToUse] = 0x23;
                if (romData[byteToUse] == 0x35) romData[byteToUse] = 0x31;
                if (romData[byteToUse] == 0x37) romData[byteToUse] = 0x33;
            }

            int[] dq1Spells = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            //int[] dq2Spells = { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
            int[] dq2PrinceSpells = { 0x16, 0x0b, 0x1a, 0x11, 0x1c, 0x1b, 0x17, 0x1e, 0x0c, 0x14, 0x0f, 0x20, 0x15 };
            int[] dq2PrincessSpells = { 0x17, 0x10, 0x0e, 0x12, 0x1d, 0x13, 0x1a, 0x18, 0x1b, 0x0d, 0x20, 0x19, 0x1f };

            for (int lnJ = 0; lnJ < dq1Spells.Length * 20; lnJ++)
                swapArray(dq1Spells, (r1.Next() % dq1Spells.Length), (r1.Next() % dq1Spells.Length));
            for (int lnJ = 0; lnJ < dq2PrinceSpells.Length * 20; lnJ++)
                swapArray(dq2PrinceSpells, (r1.Next() % dq2PrinceSpells.Length), (r1.Next() % dq2PrinceSpells.Length));
            for (int lnJ = 0; lnJ < dq2PrincessSpells.Length * 20; lnJ++)
                swapArray(dq2PrincessSpells, (r1.Next() % dq2PrincessSpells.Length), (r1.Next() % dq2PrincessSpells.Length));

            int[] dq1SpellsLearned = inverted_power_curve(0, 16, 10, 1.0, r1);
            int prevLevel = 0;
            byte starterSpells = 0;
            for (int lnJ = 0; lnJ < dq1Spells.Length; lnJ++)
            {
                if (dq1SpellsLearned[lnJ] == 0) dq1SpellsLearned[lnJ] = 1;
                if (dq1SpellsLearned[lnJ] == 1) starterSpells++;
                if (prevLevel >= 2 && dq1SpellsLearned[lnJ] <= prevLevel)
                {
                    dq1SpellsLearned[lnJ]++;
                    lnJ--;
                } else
                {
                    prevLevel = dq1SpellsLearned[lnJ];
                }
            }

            int[] dq2PrinceSpellsLearned = inverted_power_curve(0, 25, 13, 1.0, r1);
            prevLevel = 0;
            int dq2StarterSpells = 0;
            for (int lnJ = 0; lnJ < dq2PrinceSpells.Length; lnJ++)
            {
                if (dq2PrinceSpellsLearned[lnJ] == 0) dq2PrinceSpellsLearned[lnJ] = 1;
                if (dq2PrinceSpellsLearned[lnJ] == 1) dq2StarterSpells++;
                if (prevLevel >= 2 && dq2PrinceSpellsLearned[lnJ] <= prevLevel)
                {
                    dq2PrinceSpellsLearned[lnJ]++;
                    lnJ--;
                }
                else
                {
                    prevLevel = dq2PrinceSpellsLearned[lnJ];
                }
            }
            romData[0xda100] = (byte)dq2StarterSpells;

            int[] dq2PrincessSpellsLearned = inverted_power_curve(0, 22, 13, 1.0, r1);
            prevLevel = 0;
            dq2StarterSpells = 0;
            for (int lnJ = 0; lnJ < dq2PrincessSpells.Length; lnJ++)
            {
                if (dq2PrincessSpellsLearned[lnJ] == 0) dq2PrincessSpellsLearned[lnJ] = 1;
                if (dq2PrincessSpellsLearned[lnJ] == 1) dq2StarterSpells++;
                if (prevLevel >= 2 && dq2PrincessSpellsLearned[lnJ] <= prevLevel)
                {
                    dq2PrincessSpellsLearned[lnJ]++;
                    lnJ--;
                }
                else
                {
                    prevLevel = dq2PrincessSpellsLearned[lnJ];
                }
            }

            romData[0xda105] = (byte)dq2StarterSpells;

            for (int lnJ = 0; lnJ < dq1SpellsLearned.Length; lnJ++)
                romData[0x5bcd5 + lnJ] = (byte)dq1SpellsLearned[lnJ];

            for (int lnJ = 0; lnJ < dq1Spells.Length; lnJ++)
                romData[0x5bd05 + lnJ] = (byte)dq1Spells[lnJ];

            for (int lnJ = 0; lnJ < dq2PrinceSpellsLearned.Length; lnJ++)
                romData[0x5bce1 + lnJ] = (byte)dq2PrinceSpellsLearned[lnJ];

            for (int lnJ = 0; lnJ < dq2PrinceSpells.Length; lnJ++)
                romData[0x5bd0f + lnJ] = (byte)dq2PrinceSpells[lnJ];

            for (int lnJ = 0; lnJ < dq2PrincessSpellsLearned.Length; lnJ++)
                romData[0x5bcef + lnJ] = (byte)dq2PrincessSpellsLearned[lnJ];

            for (int lnJ = 0; lnJ < dq2PrincessSpells.Length; lnJ++)
                romData[0x5bd1c + lnJ] = (byte)dq2PrincessSpells[lnJ];


            int[] strength = inverted_power_curve(4, 150, 30, 1.09, r1); // Was 1.18
            int[] agility = inverted_power_curve(4, 145, 30, 1.16, r1); // Was 1.32
            int[] hp = inverted_power_curve(10, 230, 30, 1, r1); // Was 0.98
            int[] mp = inverted_power_curve(0, 220, 30, 0.99, r1); // Was 0.95
            int[] resilience = new int[30];
            for (int lnI = 0; lnI < 30; lnI++)
                resilience[lnI] = agility[lnI] / 2;

            // First, infuse starting statistic code block
            // @ 9FCA - 4C 00 DD - JMP DD00
            // @ DD00 - AGILITY:  B9 3D A0, 48, 29 0F, 18, 69 XX, 8D 21 0C, 4A, 8D 39 0C,
            // STRENGTH:  68, 4A, 4A, 4A, 4A, 18, 69 XX, 8D 1D 0C
            // MP:  B9 3E A0, 48, 29 0F, 18, 69 XX, 8D 31 0C, 8D 0D 0C
            // HP:  68, 4A, 4A, 4A, 4A, 18, 69 XX, 8D 2D 0C, 8D 09 0C
            // Spells:  A9 XX, 8D 05 0C
            // Jump back:  4C EF 9F

            romData[0xd9fca] = 0x4c;
            romData[0xd9fcb] = 0x00;
            romData[0xd9fcc] = 0xd0;

            byte[] startingStats = { 0xB9, 0x3D, 0xA0, 0x48, 0x29, 0x0F, 0x18, 0x69, (byte)agility[0], 0x8D, 0x21, 0x0C, 0x4A, 0x8D, 0x39, 0x0C,
                0x68, 0x4A, 0x4A, 0x4A, 0x4A, 0x18, 0x69, (byte)strength[0], 0x8D, 0x1D, 0x0C,
                0xB9, 0x3E, 0xA0, 0x48, 0x29, 0x0F, 0x18, 0x69, (byte)mp[0], 0x8D, 0x31, 0x0C, 0x8D, 0x0D, 0x0C,
                0x68, 0x4A, 0x4A, 0x4A, 0x4A, 0x18, 0x69, (byte)hp[0], 0x8D, 0x2D, 0x0C, 0x8D, 0x09, 0x0C,
                0xA9, starterSpells, 0x8D, 0x05, 0x0C,
                0x4c, 0xef, 0x9f };

            for (int lnI = 0; lnI < startingStats.Length; lnI++)
                romData[0xdd000 + lnI] = startingStats[lnI];

            // Implement stat gains here
            for (int lnI = 1; lnI < 30; lnI++)
            {
                romData[0x5b909 + 1 + lnI] = (byte)(strength[lnI] - strength[lnI - 1]);
                romData[0x5b9d6 + 1 + lnI] = (byte)(agility[lnI] - agility[lnI - 1]);
                romData[0x5baa3 + 1 + lnI] = (byte)(resilience[lnI] - resilience[lnI - 1]);
                romData[0x5bb70 + 1 + lnI] = (byte)(hp[lnI] - hp[lnI - 1]);
                romData[0x5bc3d + 1 + lnI] = (byte)(mp[lnI] - mp[lnI - 1]);
            }

            for (int lnI = 0; lnI < 30; lnI++)
            {
                strength[lnI] = (strength[lnI] * 9 / 10);
                agility[lnI] = (agility[lnI] * 9 / 10);
                resilience[lnI] = agility[lnI] / 2;
                hp[lnI] = (hp[lnI] * 9 / 10);
                mp[lnI] = (mp[lnI] * 9 / 10);
            }

            for (int lnI = 1; lnI < 30; lnI++)
            {
                romData[0x5b928 + 1 + lnI] = (byte)(strength[lnI] - strength[lnI - 1]);
                romData[0x5b9f5 + 1 + lnI] = (byte)(agility[lnI] - agility[lnI - 1]);
                romData[0x5bac2 + 1 + lnI] = (byte)(resilience[lnI] - resilience[lnI - 1]);
                romData[0x5bb8f + 1 + lnI] = (byte)(hp[lnI] - hp[lnI - 1]);
                romData[0x5bc5c + 1 + lnI] = (byte)(mp[lnI] - mp[lnI - 1]);
            }

            ////////////////////////////////////////////////////////////////////////
            // Dragon Quest II Stats
            ////////////////////////////////////////////////////////////////////////

            // OK, time to set up Hero stats
            strength = inverted_power_curve(16, 170, 50, 1, r1);
            agility = inverted_power_curve(10, 150, 50, 1, r1);
            hp = inverted_power_curve(20, 245, 50, 1, r1);
            mp = inverted_power_curve(0, 0, 50, 1, r1);
            resilience = new int[50];
            for (int lnI = 0; lnI < 50; lnI++)
                resilience[lnI] = agility[lnI] / 2;

            romData[0xda0f7] = (byte)strength[0];
            romData[0xda0f8] = (byte)agility[0];
            romData[0xda0f9] = (byte)hp[0];
            for (int lnI = 1; lnI < 50; lnI++)
            {
                romData[0x5b909 + 62 + 1 + lnI] = (byte)(strength[lnI] - strength[lnI - 1]);
                romData[0x5b9d6 + 62 + 1 + lnI] = (byte)(agility[lnI] - agility[lnI - 1]);
                romData[0x5baa3 + 62 + 1 + lnI] = (byte)(resilience[lnI] - resilience[lnI - 1]);
                romData[0x5bb70 + 62 + 1 + lnI] = (byte)(hp[lnI] - hp[lnI - 1]);
            }

            // Prince stats
            strength = inverted_power_curve(4, 150, 45, 1, r1);
            agility = inverted_power_curve(4, 150, 45, 1, r1);
            hp = inverted_power_curve(10, 210, 45, 1, r1);
            mp = inverted_power_curve(0, 170, 45, 1, r1);
            resilience = new int[45];
            for (int lnI = 0; lnI < 45; lnI++)
                resilience[lnI] = agility[lnI] / 2;

            romData[0xda0fc] = (byte)strength[0];
            romData[0xda0fd] = (byte)agility[0];
            romData[0xda0fe] = (byte)hp[0];
            romData[0xda0ff] = (byte)mp[0];
            //romData[0xda100] = (byte)spellStart[0];
            for (int lnI = 1; lnI < 45; lnI++)
            {
                romData[0x5b909 + 113 + 1 + lnI] = (byte)(strength[lnI] - strength[lnI - 1]);
                romData[0x5b9d6 + 113 + 1 + lnI] = (byte)(agility[lnI] - agility[lnI - 1]);
                romData[0x5baa3 + 113 + 1 + lnI] = (byte)(resilience[lnI] - resilience[lnI - 1]);
                romData[0x5bb70 + 113 + 1 + lnI] = (byte)(hp[lnI] - hp[lnI - 1]);
                romData[0x5bc3d + 62 + 1 + lnI] = (byte)(mp[lnI] - mp[lnI - 1]);
            }

            // Princess stats
            strength = inverted_power_curve(4, 100, 35, 1, r1);
            agility = inverted_power_curve(4, 180, 35, 1, r1);
            hp = inverted_power_curve(10, 190, 35, 1, r1);
            mp = inverted_power_curve(0, 210, 35, 1, r1);
            resilience = new int[35];
            for (int lnI = 0; lnI < 35; lnI++)
                resilience[lnI] = agility[lnI] / 2;

            romData[0xda101] = (byte)strength[0];
            romData[0xda102] = (byte)agility[0];
            romData[0xda103] = (byte)hp[0];
            romData[0xda104] = (byte)mp[0];
            //romData[0xda105] = (byte)spellStart[0];
            for (int lnI = 1; lnI < 35; lnI++)
            {
                romData[0x5b909 + 159 + 1 + lnI] = (byte)(strength[lnI] - strength[lnI - 1]);
                romData[0x5b9d6 + 159 + 1 + lnI] = (byte)(agility[lnI] - agility[lnI - 1]);
                romData[0x5baa3 + 159 + 1 + lnI] = (byte)(resilience[lnI] - resilience[lnI - 1]);
                romData[0x5bb70 + 159 + 1 + lnI] = (byte)(hp[lnI] - hp[lnI - 1]);
                romData[0x5bc3d + 108 + 1 + lnI] = (byte)(mp[lnI] - mp[lnI - 1]);
            }
        }

        private void randomizeTreasures(Random r1)
        {
            romData[0xd9101] = 0x02; // Instead of 0x03.  This prevents loading of a sprite that doesn't exist when picking up a special item (Stones of Sunlight... Erdrick's Sword...)
            romData[0xe23ea] = 0x08; // Push the silver harp guy out of the way due to a trigger that I can't find that fires when you find the vanilla Silver Harp.  (It doesn't fire when you get the randomized Silver Harp)  This is temporary until the trigger is found.
            romData[0xe2934] = 0x04; // Move the stones guy out of the way to save about 10 seconds talking to him.
            romData[0xe102c] = 0x06; // NEXT 6 LINES:  Get rid of Garinham blockers
            romData[0xe102d] = 0x0e;
            romData[0xe10d5] = 0x0a;
            romData[0xe10d6] = 0x0e;
            romData[0xe113d] = 0x26;
            romData[0xe113e] = 0x06;

            int[] dq1Zone1 = { 0xe1c47, 0xe1c7b, 0xe1ca2, // 0-2
                0xe095d, 0xe099e, 0xe0984, 0xe09d2, 0xe09b8, // 3-7
                0xe296c, 0xe29ba, 0xe29d4, // 8-10
                0xe0e1d, // 11
                0xe23ba, // 12
                0xe12cc, 0xe12f3, // 13-14
                0xe2570, 0xe2555, 0xe25a4, 0xe258a, 0xe25be, 0xe25d8, 0xe25f2, // 15-21
                0xe1b62, 0xe1b48, 0xe1b2e, 0xe1ac6, // 22-25
                0xe10e6, 0xe1100, 0xe111a, // 26-28
                0xe2211, 0xe21f7, 0xe21dd, 0xe2260, 0xe227a, // 29-33
                0xe16e8, 0xe1702, 0xe171c, // 34-36
                0xe22a3, 0xe22bd, 0xe27ed, 0xe1f74, 0xe1fdc, 0xe1fc2, 0xe1fa8, 0xe1f8e, 0xe1f5a, 0xe1f0c, 0xe1f26, 0xe1f40 }; // 37-48

            int[] dq1Items = { 0x04, 0x08, 0x0a,
                0xbc, 0x0a, 0x24, 0x26, 0x28,
                0x10, 0x38, 0x2a,
                0xc4,
                0x1e,
                0x30, 0x32,
                0x14, 0x24, 0x28, 0x3c, 0xe6, 0xe8, 0xec,
                0x34, 0x3a, 0xc4, 0xd4,
                0xe4, 0x2c, 0x08,
                0xd4, 0xea, 0x0a, 0x24, 0x18, //0x1a
                0xbe, 0xc0, 0xc2,
                0x0c, 0x18, 0x1c, 0x12, 0x28, 0x0c, 0x3e, 0x02, 0x0a, 0x02, 0x18, 0x0e };

            // Limits:  2, 36, 48
            int[] dq1Max = { 48, 48, 2,
                48, 48, 48, 48, 48,
                36, 48, 48,
                48,
                36,
                48, 48,
                48, 48, 48, 48, 48, 48, 48,
                48, 48, 48, 48,
                48, 48, 48,
                48, 48, 48, 48, 48,
                48, 48, 48,
                48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48 };

            int[] specialZone = { 0x4ae33, 0x4ae38, 0x4ae43 };
            int[] specialItems = { 0x1a, 0x1f, 0x0e };

            for (int lnI = 0; lnI < dq1Zone1.Length * 25; lnI++)
            {
                int first = r1.Next() % dq1Zone1.Length;
                int second = r1.Next() % dq1Zone1.Length;
                if (first == second) continue;
                if (second > dq1Max[first] || first > dq1Max[second]) continue;
                //if ((keyItems.Contains(dq1Items[first]) || keyItems.Contains(dq1Items[second])) && (dq1Jars.Contains(first) || dq1Jars.Contains(second))) continue; // Do not place key items in a jar or a shelf
                swapArray(dq1Items, first, second);
                swapArray(dq1Max, first, second);
            }

            for (int lnI = 0; lnI < dq1Zone1.Length; lnI++)
            {
                romData[dq1Zone1[lnI]] = (byte)((dq1Items[lnI]) / 2 + 0x80);
            }

            ////////////////////////////////////////////////////////////////////
            // Dragon Quest II
            ////////////////////////////////////////////////////////////////////

            int[] dq2Zone1 = { //0xe2e56, // 0
                0xe32a0, // 0
                0xe4c23, 0xe4c3d, 0xe4c57, // 1-3
                0xe4b6a, 0xe4b84, 0xe4b9f, 0xe4bb9, 0xe4bd3, 0xe4bed, 0xe4c07, // 4-10
                0xe4fa1, 0xe4fc9, 0xe4fd7, 0xe4ff6, // 11-14
                0xe2630, 0xe264a, // 15-16
                // Post Cloak
                0xe4093, 0xe40ad, // 17-18
                0xe65d8, // Grump - 19
                0xe4ca8, // Charlock - 20
                0xe3add, // Tantegel - 21
                0xe51ac, 0xe55f0, 0xe564b, 0xe5665, // Lighthouse - 22-25
                // Golden Key
                0xe2dee, 0xe2dd4, 0xe2dba, 0xe2da0, 0xe2d6c, 0xe2d86, 0xe2dba, // Midenhall - 26-32
                0xe33e8, // Cannock - 33
                //0xe51ee, // Lighthouse - 34 - do not change; this is for the stars crest.
                0xe64fa, 0xe6514, 0xe652e, 0xe6548, // Charlock - 34-37
                0xe3ef5, 0xe3f0f, // Osterfair - 38-39
                0xe501e, 0xe5038, 0xe5052, 0xe5087, 0xe50bc, 0xe50d7, 0xe50f1, 0xe5183, // Moon Tower - 40-47
                // Sea Cave
                0xe4cdc, 0xe4cf6, 0xe4d2b, 0xe4d45, 0xe4d89, 0xe4da3, 0xe4dbd, 0xe4e90, // 48-55 // 0xe4e0c (PLACE IN DEAD ZONE!!!!!  Linked to another chest)
                // Rhone Cave
                0xe625d, 0xe6379, 0xe6e86, 0xe6ea0, 0xe63e5, 0xe63ff, 0xe6419, 0xe6433, // 56-63
                // Dead Zone
                0xe600b, 0xe6025 // 64-65
            }; 

            int[] dq2Items = { //0xa2,
                0xef,
                0xc3, 0x97, 0xc4,
                0x93, 0xc8, 0xea, 0xc4, 0xe4, 0xc6, 0xd0,
                0xc3, 0x92, 0xc9, 0xcc,
                0x81, 0x81,
                // Post Cloak (17)
                0xd8, 0xc9,
                0xc1,
                0xab,
                0xed,
                0x8c, 0xa5, 0xe7, 0xea,
                // Golden Key (9/26)
                0x94, 0xd9, 0x9e, 0xe8, 0xea, 0xc3, 0xef,
                0xbe,
                //0x81,
                0xe3, 0xf0, 0xb4, 0xec,
                0xa4, 0xb7,
                0x9e, 0x94, 0xeb, 0xe8, 0xe5, 0xeb, 0xc9, 0xce,
                // Sea Cave (22/48)
                0xe8, 0x94, 0xf1, 0xc9, 0xf1, 0xec, 0xb5, 0xcf,
                // Rhone Cave (8/56)
                0xcd, 0xe9, 0xc2, 0xc8, 0xb9, 0xbf, 0xe6, 0xae,
                // Dead Zone (8/64)
                0xda, 0xba };

            // Limits:  16, 25, 47, 55, 63, 65
            int[] dq2Max = { //66,
                65,
                65, 65, 65,
                65, 65, 65, 65, 65, 65, 25,
                65, 65, 65, 16,
                65, 65,
                65, 65,
                65,
                65,
                65,
                65, 65, 65, 65,
                65, 65, 65, 65, 65, 65, 65,
                65,
                //67,
                65, 65, 65, 65,
                65, 65,
                65, 65, 65, 65, 65, 65, 65, 47,
                65, 65, 65, 65, 65, 65, 65, 55,
                63, 65, 65, 65, 65, 65, 65, 63,
                65, 65 };

            for (int lnI = 0; lnI < dq2Zone1.Length * 25; lnI++)
            {
                int first = r1.Next() % dq2Zone1.Length;
                int second = r1.Next() % dq2Zone1.Length;
                if (first == second) continue;
                if (second > dq2Max[first] || first > dq2Max[second]) continue;
                swapArray(dq2Items, first, second);
                swapArray(dq2Max, first, second);
            }

            for (int lnI = 0; lnI < dq2Zone1.Length; lnI++)
            {
                romData[dq2Zone1[lnI]] = (byte)(dq2Items[lnI]);
            }

            // Randomize starting item
            byte[] legalItems = { 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
                0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
                0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e,
                0x5a, 0x5e, 0x5f,
                0x60, 0x61, 0x62, 0x63, 0x64 };
            romData[0xd9143] = legalItems[r1.Next() % legalItems.Length];

            // Randomize starting gold
            int startGold = inverted_power_curve(1, 255, 1, 0.5, r1)[0];
            romData[0xd913e] = (byte)startGold;
        }

        private void randomizeStores(Random r1)
        {
            int[] dq1Weapons = { 1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 15, 16, 17 };
            int[] dq2Weapons = { 37, 38, 39, 40, 41, 43, 44, 45, 46, 48, 49, 54, 55, 56, 57, 58, 63, 64, 65, 68 };
            int[] dq1ItemsCommon = { 18, 19, 20, 21 };
            int[] dq2ItemsCommon = { 71, 72, 73, 74, 75 };
            int[] dq1ItemsRare = { 35 };
            //int[] dq2ItemsRare = { 86 };

            List<int> inUse = new List<int>();
            for (int lnI = 0; lnI < 103; lnI++)
            {
                int byteToUse = 0x4efe1 + lnI;
                if (romData[byteToUse] == 255)
                {
                    inUse.Sort();
                    for (int lnJ = 0 - inUse.Count; lnJ < 0; lnJ++)
                        romData[0x4efe1 + lnI + lnJ] = (byte)inUse[lnJ + inUse.Count];
                    inUse.Clear();
                    continue;
                }
                bool legal = false;
                while (!legal)
                {
                    int item = (lnI <= 41 ? dq1Weapons[r1.Next() % dq1Weapons.Length] : dq2Weapons[r1.Next() % dq2Weapons.Length]);
                    if (!inUse.Contains(item))
                    {
                        romData[byteToUse] = (byte)item;
                        inUse.Add(item);
                        legal = true;
                    }
                }
            }
            inUse.Clear();
            for (int lnI = 0; lnI < 59; lnI++)
            {
                int byteToUse = 0x4f0c1 + lnI;
                if (romData[byteToUse] == 255)
                {
                    inUse.Sort();
                    for (int lnJ = 0 - inUse.Count; lnJ < 0; lnJ++)
                        romData[0x4f0c1 + lnI + lnJ] = (byte)inUse[lnJ + inUse.Count];
                    inUse.Clear();
                    continue;
                }
                bool legal = false;
                while (!legal)
                {
                    int item = (lnI <= 18 ? dq1ItemsCommon[r1.Next() % dq1ItemsCommon.Length] : dq2ItemsCommon[r1.Next() % dq2ItemsCommon.Length]);
                    if (lnI > 18 && r1.Next() % 4 == 0) item = dq2Weapons[r1.Next() % dq2Weapons.Length];
                    if (!inUse.Contains(item))
                    {
                        romData[byteToUse] = (byte)item;
                        inUse.Add(item);
                        legal = true;
                    }
                }
            }

            // Place the Jailor's Key in a random DQ2 store
            bool legalJail = false;
            while (!legalJail)
            {
                int jailSlot = r1.Next() % 41;
                int byteToUse = 0x4f0d4 + jailSlot;
                if (romData[byteToUse] == 0xff) continue;
                romData[byteToUse] = 0x56;
                legalJail = true;
            }
        }

        private void boostExp()
        {
            for (int lnI = 0; lnI < 121; lnI++)
            {
                int byteToUse = 0x5da0e + (18 * lnI) + 16;
                int xp = romData[byteToUse] + (256 * romData[byteToUse + 1]);
                xp = xp * trkExperience.Value / 10;
                xp = (xp > 65000 ? 65000 : xp);
                romData[byteToUse] = (byte)(xp % 256);
                romData[byteToUse + 1] = (byte)(xp / 256);

                byteToUse = 0x5da0e + (18 * lnI) + 5;
                int gp = romData[byteToUse] + (256 * (romData[byteToUse + 9] / 64));
                gp = gp * trkExperience.Value / 10;
                gp = (gp > 1000 ? 1000 : gp);
                romData[byteToUse] = (byte)(gp % 256);
                romData[byteToUse + 9] = (byte)((romData[byteToUse + 9] % 64) + (64 * (gp / 256)));
            }

            // Set Dragon Loop monster to 262 XP (65% strength * 100% defense * sqrt(75% agility) * 230% HP * 1.5 = 262 XP)
            int xpPrincess = romData[0x591ce] + (romData[0x591cf] * 256);
            xpPrincess = xpPrincess * trkExperience.Value / 10;
            xpPrincess = (xpPrincess > 65000 ? 65000 : xpPrincess);
            romData[0x591ce] = (byte)(xpPrincess % 256);
            romData[0x591cf] = (byte)(xpPrincess / 256);
        }

        private void goldRequirements(Random r1)
        {
            for (int lnI = 0; lnI < 86; lnI++) // Not 101; the ROM will crash.
            {
                int romCheck = 0x4ec61 + (2 * lnI);
                if (romData[romCheck] == 0x00 && romData[romCheck + 1] == 0x00) continue;

                int byteToUse = 0x4eac9 + (2 * lnI);
                int gp = romData[byteToUse] + (256 * romData[byteToUse + 1]);
                gp = ScaleValue(gp, trkGoldReq.Value / 10, 1.0, r1);
                romData[byteToUse] = (byte)(gp % 256);
                romData[byteToUse + 1] = (byte)(gp / 256);

                int textByte = 0x40000 + romData[romCheck] + (256 * romData[romCheck + 1]);
                if (textByte == 0x4ed0d) continue; // Jailor's Key doesn't have a printed price
                textByte += 6;
                romData[textByte + 0] = (byte)(gp < 10000 ? 0 : (gp / 10000) + 1);
                romData[textByte + 1] = (byte)(gp < 1000 ? 0 : ((gp / 1000) % 10) + 1);
                romData[textByte + 2] = (byte)(gp < 100 ? 0 : ((gp / 100) % 10) + 1);
                romData[textByte + 3] = (byte)(gp < 10 ? 0 : ((gp / 10) % 10) + 1);
                romData[textByte + 4] = (byte)((gp % 10) + 1);
            }
        }

        private void radENG_CheckedChanged(object sender, EventArgs e)
        {
            lblRomImage.Text = "DQ 1+2 ROM Image";
            lblCompareImage.Text = "Comparison Image";
            lblSHA.Text = "SHA Checksum";
            lblRequired.Text = "Required";
            lblSeed.Text = "Seed";
            lblFlags.Text = "Flags";
            chkMonsterZones.Text = "Monster Zones";
            chkMonsterPatterns.Text = "Monster Patterns";
            chkHeroStats.Text = "Hero Stats";
            chkTreasures.Text = "Treasures";
            chkStores.Text = "Stores";
            chkMap.Text = "Map";
            lblExpBoost.Text = "Boost Experience/GP";
            lblGPReq.Text = "Gold Requirements";
            lblCrossGame.Text = "Cross Game";
            lblRandomizeHeader.Text = "Randomize:";
            chkSmallMap.Text = "Small map in DQ2";

            btnBrowse.Text = btnCompareBrowse.Text = "Browse";
            btnCompare.Text = "Compare";
            btnNewSeed.Text = "New Seed";
            cmdRandomize.Text = "Randomize";
        }

        private void radJP_CheckedChanged(object sender, EventArgs e)
        {
            lblRomImage.Text = "DQ 1+2 ROMイメージ";
            lblCompareImage.Text = "比較のイメージ";
            lblSHA.Text = "SHA チェックサム";
            lblRequired.Text = "必要なチェックサム";
            lblSeed.Text = "シード";
            lblFlags.Text = "フラグ";
            chkMonsterZones.Text = "エンカウントエリア";
            chkMonsterPatterns.Text = "敵の行動";
            chkHeroStats.Text = "勇者の強さ";
            chkTreasures.Text = "宝箱";
            chkStores.Text = "ショッピング";
            chkMap.Text = "世界地図";
            lblExpBoost.Text = "EXP/Gブースト";
            lblGPReq.Text = "ショッピング価格";
            lblCrossGame.Text = "クロスゲーム";
            lblRandomizeHeader.Text = "ランダム化";
            chkSmallMap.Text = "DQ2の小さい世界地図";

            btnBrowse.Text = btnCompareBrowse.Text = "ブラウズ";
            btnCompare.Text = "比較する";
            btnNewSeed.Text = "ニューシード";
            cmdRandomize.Text = "ランダム化する";
        }
    }
}
