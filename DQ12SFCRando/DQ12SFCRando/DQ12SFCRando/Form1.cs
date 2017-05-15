﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DQ12SFCRando
{
    public partial class Form1 : Form
    {
        bool loading = true;
        byte[] romData;
        byte[] romData2;

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
            trkExperience.Value = convertChartoInt(Convert.ToChar(flags.Substring(2, 1)));
            trkExperience_Scroll(null, null);
            trkGoldReq.Value = convertChartoInt(Convert.ToChar(flags.Substring(3, 1)));
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
            loadRom();
            Random r1 = new Random(Convert.ToInt32(txtSeed.Text));
            if (chkMonsterZones.Checked) randomizeMonsterZones(r1);
            if (chkMonsterPatterns.Checked) randomizeMonsterPatterns(r1);
            if (chkHeroStats.Checked) randomizeHeroStats(r1);
            if (chkTreasures.Checked) randomizeTreasures(r1);
            if (chkStores.Checked) randomizeStores(r1);
            boostExp();
            goldRequirements(r1);
            saveRom();
        }

        private void randomizeMonsterZones(Random r1)
        {
            for (int lnI = 0; lnI < 19; lnI++)
            {
                for (int lnJ = 0; lnJ < 5; lnJ++)
                {
                    int byteToUse = 0x5b52d + (lnI * 5) + lnJ;
                    int min = (lnI >= 16 ? 30 : 1);
                    int max = (lnI <= 3 ? 3 * (lnI + 1) : 38);

                    romData[byteToUse] = (byte)((r1.Next() % (max - min)) + min);
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
                    for (int lnJ = 0; lnJ < 8; lnJ++)
                        if (r1.Next() % 2 != 0)
                            monsterPattern[lnJ] = (lnI <= 40 ? dq1Pattern[r1.Next() % dq1Pattern.Length] : dq2Pattern[r1.Next() % dq2Pattern.Length]);
                        else
                            monsterPattern[lnJ] = (lnI <= 40 ? 0 : (r1.Next() % 100 <= (lnI - 40) ? 30 : 0));
                else
                    for (int lnJ = 0; lnJ < 8; lnJ++)
                        monsterPattern[lnJ] = (lnI <= 40 ? 0 : (r1.Next() % 100 <= (lnI - 40) ? 30 : 0));

                for (int lnJ = 0; lnJ < 8; lnJ++)
                    romData[byteToUse + 6 + lnJ] = (byte)(monsterPattern[lnJ] + (romData[byteToUse + 6] - (romData[byteToUse + 6] % 32)));
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
            int[] dq2Spells = { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

            for (int lnJ = 0; lnJ < dq1Spells.Length * 20; lnJ++)
                swapArray(dq1Spells, (r1.Next() % dq1Spells.Length), (r1.Next() % dq1Spells.Length));
            for (int lnJ = 0; lnJ < dq2Spells.Length * 20; lnJ++)
                swapArray(dq2Spells, (r1.Next() % dq2Spells.Length), (r1.Next() % dq2Spells.Length));

            int[] dq1SpellsLearned = inverted_power_curve(0, 16, 10, 1.0, r1);
            int prevLevel = 0;
            byte starterSpells = 0;
            for (int lnJ = 0; lnJ < dq1Spells.Length; lnJ++)
            {
                if (dq1SpellsLearned[lnJ] <= 1) starterSpells++;
                if (prevLevel >= 2 && dq1SpellsLearned[lnJ] <= prevLevel)
                {
                    dq1SpellsLearned[lnJ]++;
                    lnJ--;
                } else
                {
                    prevLevel = dq1SpellsLearned[lnJ];
                }
            }

            for (int lnJ = 0; lnJ < dq1SpellsLearned.Length; lnJ++)
                romData[0x5bcd5 + lnJ] = (byte)dq1SpellsLearned[lnJ];

            for (int lnJ = 0; lnJ < dq1Spells.Length; lnJ++)
                romData[0x5bd05 + lnJ] = (byte)dq1Spells[lnJ];

            int[] strength = inverted_power_curve(4, 150, 30, 1.18, r1);
            int[] agility = inverted_power_curve(4, 145, 30, 1.32, r1);
            int[] hp = inverted_power_curve(10, 230, 30, 0.98, r1);
            int[] mp = inverted_power_curve(0, 220, 30, 0.95, r1);
            int[] resilience = new int[30];
            for (int lnI = 0; lnI < 30; lnI++)
                resilience[lnI] = agility[lnI] / 2;

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
        }

        private void randomizeTreasures(Random r1)
        {
            
        }

        private void randomizeStores(Random r1)
        {
            int[] dq1Weapons = { 1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12, 13, 15, 16, 17 };
            int[] dq2Weapons = { 37, 38, 39, 40, 41, 43, 44, 45, 46, 48, 49, 54, 55, 56, 57, 58, 63, 64, 65, 68 };
            int[] dq1ItemsCommon = { 18, 19, 20, 21 };
            int[] dq2ItemsCommon = { 71, 72, 73, 74, 75 };
            int[] dq1ItemsRare = { 35 };
            int[] dq2ItemsRare = { 86 };

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
        }

        private void boostExp()
        {
            for (int lnI = 0; lnI < 121; lnI++)
            {
                int byteToUse = 0x5da0e + (18 * lnI) + 16;
                int xp = romData[byteToUse] + (256 * romData[byteToUse + 1]);
                xp = xp * trkExperience.Value / 10;
                romData[byteToUse] = (byte)(xp % 256);
                romData[byteToUse + 1] = (byte)(xp / 256);
            }
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
    }
}