﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsVirtualDesktopHelper {
    public partial class SettingsForm : Form {

        private bool IsLoading;

        public SettingsForm() {
            IsLoading = true;

            // Init UI
            InitializeComponent();
            LoadSettingsIntoUI();

            if(App.Instance.IsDarkThemeMode()) {
                // Dark mode - icons should be white
                notifyIconPrev.Icon = Icons.chevron_left_256_white;
                notifyIconNext.Icon = Icons.chevron_right_256_white;
            } else {
                // Light mode - icons should be black
                notifyIconPrev.Icon = Icons.chevron_left_256_black;
                notifyIconNext.Icon = Icons.chevron_right_256_black;
            }

            // Apply some settings
            if (checkBoxShowPrevNextIcons.Checked) {
                notifyIconPrev.Visible = true;
                notifyIconNext.Visible = true;
            } else {
                notifyIconPrev.Visible = false;
                notifyIconNext.Visible = false;
            }

            //TODO: how to sync the startup setting - best would be to see if the reg key is actually there

            IsLoading = false;
        }

        private void LoadSettingsIntoUI() {
            this.checkBoxShowPrevNextIcons.CheckState = GetBool("ShowPrevNextIcons", false) ? CheckState.Checked: CheckState.Unchecked;
            this.checkBoxStartupWithWindows.CheckState = GetBool("StartupWithWindows", false) ? CheckState.Checked : CheckState.Unchecked;

            
        }
        private void SaveSettingsFromUI() {
            SetBool("ShowPrevNextIcons", this.checkBoxShowPrevNextIcons.Checked);
            SetBool("StartupWithWindows", this.checkBoxStartupWithWindows.Checked);
        }

        static bool GetBool(string key, bool defaultValue) {
            try {
                var config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                var settings = config.AppSettings.Settings;
                if (settings[key] == null || settings[key].Value == null) return defaultValue;
                return bool.Parse(settings[key].Value);
            } catch (Exception e) {
                Console.WriteLine("Error reading app settings: "+e.Message);
                return defaultValue;
            }
        }

        static void SetBool(string key, bool value) {
            try {
                var config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                var settings = config.AppSettings.Settings;
                if(settings[key] == null) {
                    settings.Add(key, value.ToString().ToLower());
                } else {
                    settings[key].Value = value.ToString().ToLower();
                }
                config.Save(ConfigurationSaveMode.Modified);
            } catch (Exception e) {
                Console.WriteLine("Error saving app settings: " + e.Message);
            }
        }

        public void UpdateIconForVDDisplayNumber(uint number) {
            number++;
            if (App.Instance.IsDarkThemeMode()) {
                // White icon
                if (number == 1) {
                    this.notifyIconNumber.Icon = Icons.number_1_256_white;
                } else if (number == 2) {
                    this.notifyIconNumber.Icon = Icons.number_2_256_white;
                } else if (number == 3) {
                    this.notifyIconNumber.Icon = Icons.number_3_256_white;
                } else if (number == 4) {
                    this.notifyIconNumber.Icon = Icons.number_4_256_white;
                } else if (number == 5) {
                    this.notifyIconNumber.Icon = Icons.number_5_256_white;
                } else if (number == 6) {
                    this.notifyIconNumber.Icon = Icons.number_6_256_white;
                } else if (number == 7) {
                    this.notifyIconNumber.Icon = Icons.number_7_256_white;
                } else if (number == 8) {
                    this.notifyIconNumber.Icon = Icons.number_8_256_white;
                } else if (number == 9) {
                    this.notifyIconNumber.Icon = Icons.number_9_256_white;
                } else {
                    this.notifyIconNumber.Icon = Icons.number_plus_256_white;
                }
            } else {
                // Black icon
                if (number == 1) {
                    this.notifyIconNumber.Icon = Icons.number_1_256_black;
                } else if (number == 2) {
                    this.notifyIconNumber.Icon = Icons.number_2_256_black;
                } else if (number == 3) {
                    this.notifyIconNumber.Icon = Icons.number_3_256_black;
                } else if (number == 4) {
                    this.notifyIconNumber.Icon = Icons.number_4_256_black;
                } else if (number == 5) {
                    this.notifyIconNumber.Icon = Icons.number_5_256_black;
                } else if (number == 6) {
                    this.notifyIconNumber.Icon = Icons.number_6_256_black;
                } else if (number == 7) {
                    this.notifyIconNumber.Icon = Icons.number_7_256_black;
                } else if (number == 8) {
                    this.notifyIconNumber.Icon = Icons.number_8_256_black;
                } else if (number == 9) {
                    this.notifyIconNumber.Icon = Icons.number_9_256_black;
                } else {
                    this.notifyIconNumber.Icon = Icons.number_plus_256_black;
                }
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e) {
            App.Instance.ShowSplash();
            App.Instance.MonitorVDSwitch();
        }

        private void SettingsForm_Shown(object sender, EventArgs e) {
            UpdateIconForVDDisplayNumber(App.Instance.CurrentVDDisplayNumber);
            this.notifyIconNumber.Visible = true;
            this.Hide();
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
            if (e.ClickedItem.Tag.ToString() == "exit") App.Instance.Exit();
            else if (e.ClickedItem.Tag.ToString() == "settings") this.Show();
            else if (e.ClickedItem.Tag.ToString() == "about") App.Instance.ShowAbout();
        }

        private void notifyIconPrev_Click(object sender, EventArgs e) {
            App.Instance.VDAPI.SwitchBackward();
        }

        private void notifyIconNext_Click(object sender, EventArgs e) {
            App.Instance.VDAPI.SwitchForward();
        }

        private void checkBoxShowPrevNextIcons_CheckedChanged(object sender, EventArgs e) {
            if (IsLoading) return;
            
            if(checkBoxShowPrevNextIcons.Checked) {
                notifyIconPrev.Visible = true;
                notifyIconNext.Visible = true;
            } else {
                notifyIconPrev.Visible = false;
                notifyIconNext.Visible = false;
            }
            SaveSettingsFromUI();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e) {
            
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                Hide();
            } else if (e.CloseReason == CloseReason.ApplicationExitCall || e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.TaskManagerClosing) {
                // Remove all notif icons
                notifyIconNumber.Visible = false;
                notifyIconPrev.Visible = false;
                notifyIconNext.Visible = false;
            }
        }

        private void checkBoxStartupWithWindows_CheckedChanged(object sender, EventArgs e) {
            if (IsLoading) return;

            if (checkBoxStartupWithWindows.Checked) {
                App.Instance.EnableStartupWithWindows();
            } else {
                App.Instance.DisableStartupWithWindows();
            }
            SaveSettingsFromUI();
        }
    }
}
