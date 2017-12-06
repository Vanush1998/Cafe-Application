using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
using System.Threading;
using System.Media;
using System.Device.Location;

namespace CafeApplication
{
    public partial class CafeApp : Form
    {
        #region Variables
        ActiveUser activeUser = null;
        Cafe activeCafe = null;
        KeyValuePair<string, int> activeItem;
        List<Label> MenuLabels = new List<Label>();
        List<CheckBox> MenuCheckBoxes = new List<CheckBox>();
        List<NumericUpDown> MenuNumbers = new List<NumericUpDown>();
        Dictionary<string, int> tempMenu = new Dictionary<string, int>();
        User selectedUserFromCombobox = null;
        List<Label> Favorites = new List<Label>();
        List<Label> Popular = new List<Label>();
        List<Label> NearBy = new List<Label>();
        private void OpenCafePage(Cafe cafe)
        {
            activeCafe = cafe;
            InitialStateForCafesPanel();
            cafe_panel.Visible = true;
            user_panel.Visible = false;
            login_panel.Visible = false;
        }
        int pageCount = 1;
        #endregion Variables
        #region Initial states for panels

        private void InitialStateForSignInPanel()
        {
            usernameBox_sign_in_panel.Text = "Username";
            usernameBox_sign_in_panel.ForeColor = Color.Silver;
            passwordBox_sign_in_panel.Text = "Password";
            passwordBox_sign_in_panel.ForeColor = Color.Silver;
            combobox_login_panel.Text = "Search Cafes";
            combobox_login_panel.ForeColor = Color.Silver;
            passwordBox_sign_in_panel.UseSystemPasswordChar = true;
            passwordBox_sign_up_panel.UseSystemPasswordChar = true;
            checkBox_show_pass_sign_in_panel.Checked = false;
            checkBox_show_pass_sign_up_panel.Checked = false;
            sign_up_panel.Visible = false;
            sign_in_panel.Visible = true;
            nameBox_sign_up_panel.Text = "Name";
            nameBox_sign_up_panel.ForeColor = Color.Silver;
            lastnameBox_sign_up_panel.Text = "Last name";
            lastnameBox_sign_up_panel.ForeColor = Color.Silver;
            usernameBox_sign_up_panel.Text = "Username";
            usernameBox_sign_up_panel.ForeColor = Color.Silver;
            passwordBox_sign_up_panel.Text = "Password";
            passwordBox_sign_up_panel.ForeColor = Color.Silver;
            warning_label_sign_in_panel.Visible = false;
            warning_label_sign_up_panel.Visible = false;
        }

        private void InitialStateForUserPanel()
        {
            user_page_panel.Visible = true;
            account_settings_panel.Visible = false;
            change_password_panel.Visible = false;
            log_out_ask_panel.Visible = false;
            cash_panel.Visible = false; edit_profile_panel.Visible = false;
            comboBox_cafe_search_user_panel.Text = "Search cafes";
            comboBox_cafe_search_user_panel.ForeColor = Color.Silver;
            comboBox_user_search_user_panel.Text = "Search users";
            comboBox_user_search_user_panel.ForeColor = Color.Silver;
            if (activeUser.isAdmin)
            {
                user_name_lastname_label.Text = activeUser.Name + " " + activeUser.LastName + " (Admin)";
                comboBox_user_search_user_panel.Visible = true;
                add_cafe_button_user_page_panel.Visible = true;
            }
            else
            {
                comboBox_user_search_user_panel.Visible = false;
                add_cafe_button_user_page_panel.Visible = false;
                user_name_lastname_label.Text = activeUser.Name + " " + activeUser.LastName;
            }
            InitialStateForUserPagePanel();
            user_mod_for_admin_panel.Visible = false;
        }


        private void InitialStateForPasswordChangePanel()
        {
            oldPasswordBox_change_password_panel.Text = String.Empty;
            oldPasswordBox_change_password_panel.ForeColor = Color.Black;
            newPasswordBox_change_password_panel.Text = "Password";
            newPasswordBox_change_password_panel.ForeColor = Color.Silver;
            repeatpasswordBox_change_password_panel.Text = "Password";
            repeatpasswordBox_change_password_panel.ForeColor = Color.Silver;
        }
        private void InitialStateForAddCafePanel()
        {
            namebox_add_cafe_panel.Text = "Name";
            namebox_add_cafe_panel.ForeColor = Color.Silver;
            addressBox_add_cafe_panel.Text = "Address";
            addressBox_add_cafe_panel.ForeColor = Color.Silver;
            phoneBox_add_cafe_panel.Text = "Phone";
            phoneBox_add_cafe_panel.ForeColor = Color.Silver;
            webpageBox_add_cafe_panel.Text = "Web Page";
            webpageBox_add_cafe_panel.ForeColor = Color.Silver;
            emailBox_add_cafe_panel.Text = "eMail";
            emailBox_add_cafe_panel.ForeColor = Color.Silver;
            hourBox_open_add_cafe_panel.Text = "Open (hh.mm.ss)";
            hourBox_open_add_cafe_panel.ForeColor = Color.Silver;
            hourBox_close_add_cafe_panel.Text = "Close (hh.mm.ss)";
            hourBox_close_add_cafe_panel.ForeColor = Color.Silver;
            checkBox_monday_cafe_page_panel.Checked = false;
            checkBox_tuesday_cafe_page_panel.Checked = false;
            checkBox_wednesday_cafe_page_panel.Checked = false;
            checkBox_thursday_cafe_page_panel.Checked = false;
            checkBox_friday_cafe_page_panel.Checked = false;
            checkBox_saturday_cafe_page_panel.Checked = false;
            checkBox_sunday_cafe_page_panel.Checked = false;
            warning_label_add_cafe_panel.Visible = false;

        }
        private void InitialStateForUserPagePanel()
        {
            Favorites.Add(favorite_cafe1_label_user_page_panel);
            Favorites.Add(favorite_cafe2_label_user_page_panel);
            Favorites.Add(favorite_cafe3_label_user_page_panel);
            Favorites.Add(favorite_cafe4_label_user_page_panel);
            Favorites.Add(favorite_cafe5_label_user_page_panel);
            Popular.Add(popular_cafe1_user_page_panel);
            Popular.Add(popular_cafe2_user_page_panel);
            Popular.Add(popular_cafe3_user_page_panel);
            Popular.Add(popular_cafe4_user_page_panel);
            Popular.Add(popular_cafe5_user_page_panel);
            NearBy.Add(near_cafe1_label_user_page_panel);
            NearBy.Add(near_cafe2_label_user_page_panel);
            NearBy.Add(near_cafe3_label_user_page_panel);
            NearBy.Add(near_cafe4_label_user_page_panel);
            NearBy.Add(near_cafe5_label_user_page_panel);
            user_mod_for_admin_panel.Visible = false;
            for (int i = 0; i < 5; i++)
            {
                Favorites[i].Visible = false;
                Popular[i].Visible = false;
                NearBy[i].Visible = false;
            }
            // no favorites//////////////////////
            if (activeUser.Favorite.Count == 0)
            {
                no_favorite_label_user_page_panel.Visible = true;
                no_favorite_label_user_page_panel.BringToFront();
            }
            else
            {
                no_favorite_label_user_page_panel.Visible = false;
                no_favorite_label_user_page_panel.SendToBack();
            }
            //favorits label or  combobox//////////
            if (activeUser.Favorite.Count <= 5)
            {
                favorites_label_user_page_panel.Visible = true;
                favorites_caombobox_user_page_panel.Visible = false;
            }
            else
            {
                favorites_caombobox_user_page_panel.Visible = true;
                favorites_label_user_page_panel.Visible = false;
            }

            //favorites display///////////////////////////////
            for (int i = 0; i < Math.Min(5, activeUser.Favorite.Count); i++)
            {
                for (int j = 0; j < Cafe.cafes.Count; j++)
                {
                    if (Cafe.cafes[j].Name.Equals(activeUser.Favorite[i]))
                    {
                        Favorites[i].Text = Cafe.cafes[j].Name + "\n" + Cafe.cafes[i].Address;
                        Favorites[i].Visible = true;
                    }
                }
            }
            //Admin advantages////////////////////////
            if (activeUser.isAdmin)
            {
                add_cafe_button_user_page_panel.Visible = true;
                comboBox_user_search_user_panel.Visible = true;
            }
            else
            {
                comboBox_user_search_user_panel.Visible = false;
                add_cafe_button_user_page_panel.Visible = false;
            }
            //no cafe to show////////////////////////
            if (Cafe.cafes.Count == 0)
            {
                no_cafe_label_user_page_panel.Visible = true;
                no_cafe_label_user_page_panel.BringToFront();
            }
            else
            {
                no_cafe_label_user_page_panel.Visible = false;
                Cafe.SortByRate(); //////yst raitingi
                for (int i = 0; i < Math.Min(5, Cafe.cafes.Count); i++)
                {
                    if (i == 0)
                    {
                        popular_cafe1_user_page_panel.Text = Cafe.cafes[i].Name + "  " + Cafe.cafes[i].Rating + "\n" + Cafe.cafes[i].Address;
                        popular_cafe1_user_page_panel.Visible = true;
                    }
                    else if (i == 1)
                    {
                        popular_cafe2_user_page_panel.Text = Cafe.cafes[i].Name + "  " + Cafe.cafes[i].Rating + "\n" + Cafe.cafes[i].Address;
                        popular_cafe2_user_page_panel.Visible = true;
                    }
                    else if (i == 2)
                    {
                        popular_cafe3_user_page_panel.Text = Cafe.cafes[i].Name + "  " + Cafe.cafes[i].Rating + "\n" + Cafe.cafes[i].Address;
                        popular_cafe3_user_page_panel.Visible = true;
                    }
                    else if (i == 3)
                    {
                        popular_cafe4_user_page_panel.Text = Cafe.cafes[i].Name + "  " + Cafe.cafes[i].Rating + "\n" + Cafe.cafes[i].Address;
                        popular_cafe4_user_page_panel.Visible = true;
                    }
                    else if (i == 4)
                    {
                        popular_cafe5_user_page_panel.Text = Cafe.cafes[i].Name + "  " + Cafe.cafes[i].Rating + "\n" + Cafe.cafes[i].Address;
                        popular_cafe5_user_page_panel.Visible = true;
                    }
                }
                if (activeUser.Location == null)
                    near_cafes_panel_user_page_panel.Visible = false;
                else
                    near_cafes_panel_user_page_panel.Visible = true;
            }
        }
        private void InitialStateForNearCafesPanel()
        {
            if (activeUser.Location == null)
                near_cafes_panel_user_page_panel.Visible = false;
            else
            {
                near_cafes_panel_user_page_panel.Visible = true;
                Cafe.SortByDistance(activeUser); //////// yst active useric unecac heravorutyan
                for (int i = 0; i < Cafe.cafes.Count; i++)
                {
                    if (i == 0)
                    {
                        near_cafe1_label_user_page_panel.Text = Cafe.cafes[i].Name + "  " + Cafe.cafes[i].Rating + "\n" + Cafe.cafes[i].Address;
                        near_cafe1_label_user_page_panel.Visible = true;
                    }
                    else if (i == 1)
                    {
                        near_cafe2_label_user_page_panel.Text = Cafe.cafes[i].Name + "  " + Cafe.cafes[i].Rating + "\n" + Cafe.cafes[i].Address;
                        near_cafe2_label_user_page_panel.Visible = true;
                    }
                    else if (i == 2)
                    {
                        near_cafe3_label_user_page_panel.Text = Cafe.cafes[i].Name + "  " + Cafe.cafes[i].Rating + "\n" + Cafe.cafes[i].Address;
                        near_cafe3_label_user_page_panel.Visible = true;
                    }
                    else if (i == 3)
                    {
                        near_cafe4_label_user_page_panel.Text = Cafe.cafes[i].Name + "  " + Cafe.cafes[i].Rating + "\n" + Cafe.cafes[i].Address;
                        near_cafe4_label_user_page_panel.Visible = true;
                    }
                    else if (i == 4)
                    {
                        near_cafe5_label_user_page_panel.Text = Cafe.cafes[i].Name + "  " + Cafe.cafes[i].Rating + "\n" + Cafe.cafes[i].Address;
                        near_cafe5_label_user_page_panel.Visible = true;
                    }
                }
                active_user_name_lastname_cafe_panel.Text = activeUser.ToString();
            }
        }
        private void InitialStateForCafesPanel()
        {
            worning_label_cafe_panel.Visible = false;
            activeUser.Bill = 0;
            activeUser.OrderList.Clear();
            listBox1.DataSource = new BindingSource();
            active_user_name_lastname_cafe_panel.Text = activeUser.ToString();
            heading_label_cafe_panel.Text = activeCafe.Name;
            address_label_cafe_info_panel.Text = "Address :  " + activeCafe.Address;
            phone_label_cafe_info_panel.Text = "Phone :    " + activeCafe.Phone;
            email_label_cafe_info_panel.Text = "Email :    " + activeCafe.Email;
            web_page__label_cafe_info_panel.Text = "Web page : " + activeCafe.WebPage;
            worktime_label_cafe_info_panel.Text = activeCafe.OpenStatus() + " / " + activeCafe.Open.ToString(@"hh\:mm") + "-" + activeCafe.Close.ToString(@"hh\:mm") + "\n" + "Workdays : ";
            if (activeCafe.WorkDays[0])
                worktime_label_cafe_info_panel.Text += " M";
            if (activeCafe.WorkDays[1])
                worktime_label_cafe_info_panel.Text += " T";
            if (activeCafe.WorkDays[2])
                worktime_label_cafe_info_panel.Text += " W";
            if (activeCafe.WorkDays[3])
                worktime_label_cafe_info_panel.Text += " T";
            if (activeCafe.WorkDays[4])
                worktime_label_cafe_info_panel.Text += " F";
            if (activeCafe.WorkDays[5])
                worktime_label_cafe_info_panel.Text += " S";
            if (activeCafe.WorkDays[6])
                worktime_label_cafe_info_panel.Text += " S";
            rating_label_cafe_info_panel.Text = "Rating " + activeCafe.Rating;
            InitialStateForRatesAndReviews();
            MenuLabels.Add(menu_item1_label);
            MenuLabels.Add(menu_item2_label);
            MenuLabels.Add(menu_item3_label);
            MenuLabels.Add(menu_item4_label);
            MenuLabels.Add(menu_item5_label);
            MenuLabels.Add(menu_item6_label);
            MenuLabels.Add(menu_item7_label);
            MenuLabels.Add(menu_item8_label);
            MenuLabels.Add(menu_item9_label);
            MenuCheckBoxes.Add(menu_checkBox1);
            MenuCheckBoxes.Add(menu_checkBox2);
            MenuCheckBoxes.Add(menu_checkBox3);
            MenuCheckBoxes.Add(menu_checkBox4);
            MenuCheckBoxes.Add(menu_checkBox5);
            MenuCheckBoxes.Add(menu_checkBox6);
            MenuCheckBoxes.Add(menu_checkBox7);
            MenuCheckBoxes.Add(menu_checkBox8);
            MenuCheckBoxes.Add(menu_checkBox9);
            MenuNumbers.Add(menu_numericUpDown1);
            MenuNumbers.Add(menu_numericUpDown2);
            MenuNumbers.Add(menu_numericUpDown3);
            MenuNumbers.Add(menu_numericUpDown4);
            MenuNumbers.Add(menu_numericUpDown5);
            MenuNumbers.Add(menu_numericUpDown6);
            MenuNumbers.Add(menu_numericUpDown7);
            MenuNumbers.Add(menu_numericUpDown8);
            MenuNumbers.Add(menu_numericUpDown9);
            InitialStateForMenu(1);
            if (activeUser.isBlocked)
            {
                for (int i = 0; i < MenuCheckBoxes.Count; i++)
                {
                    MenuCheckBoxes[i].Enabled = false;
                    MenuNumbers[i].Enabled = false;
                }
                orderlist_label_cafe_panel.Enabled = false;
                listBox1.Enabled = false;
                bill_label_cafe_panel.Enabled = false;
                bill_amount_label_cafe_panel.Enabled = false;
                make_order_button_cafe_panel.Enabled = false;
            }
            else
            {
                for (int i = 0; i < MenuCheckBoxes.Count; i++)
                {
                    MenuCheckBoxes[i].Enabled = true; ;
                    MenuNumbers[i].Enabled = true;
                }
                orderlist_label_cafe_panel.Enabled = true;
                listBox1.Enabled = true;
                bill_label_cafe_panel.Enabled = true;
                bill_amount_label_cafe_panel.Enabled = true;
                make_order_button_cafe_panel.Enabled = true;
            }
            if (activeUser.isAdmin)
            {
                delete_cafe_button_cafe_panel.Visible = true;
                modify_cafe_menu_button.Visible = true;
                change_cafe_info_button.Visible = true;
            }
            else
            {
                delete_cafe_button_cafe_panel.Visible = false;
                modify_cafe_menu_button.Visible = false;
                change_cafe_info_button.Visible = false;
            }

        }
        private void InitialStateForRatesAndReviews()
        {
            reviews_label_cafe_info_panel.Visible = true;
            review1_label_cafe_info_panel.Visible = false;
            review2_label_cafe_info_panel.Visible = false;
            review3_label_cafe_info_panel.Visible = false;
            for (int i = 0; i < activeCafe.Reviews.Count; i++)
            {
                if (i == 0)
                {
                    review1_label_cafe_info_panel.Visible = true;
                    review1_label_cafe_info_panel.Text = activeCafe.Reviews[0];
                }
                if (i == 1)
                {
                    review2_label_cafe_info_panel.Visible = true;
                    review2_label_cafe_info_panel.Text = activeCafe.Reviews[1];
                }
                if (i == 2)
                {
                    review3_label_cafe_info_panel.Visible = true;
                    review3_label_cafe_info_panel.Text = activeCafe.Reviews[2];
                }
            }
            if (activeUser.isBlocked)
                add_reviewBox_cafe_info_panel.Enabled = false;
            else
                add_reviewBox_cafe_info_panel.Enabled = true;
            add_reviewBox_cafe_info_panel.Visible = true;
            add_reviewBox_cafe_info_panel.Text = "Add review";
            add_reviewBox_cafe_info_panel.ForeColor = Color.Silver;
            //////////////////////////     Rates      /////////////////////////////
            InitialStateForRates();
        }
        private void InitialStateForRates()
        {
            rate_label_cafe_info_panel.Visible = true;
            star_label1_cafe_panel.ForeColor = Color.Silver;
            star_label2_cafe_panel.ForeColor = Color.Silver;
            star_label3_cafe_panel.ForeColor = Color.Silver;
            star_label4_cafe_panel.ForeColor = Color.Silver;
            star_label5_cafe_panel.ForeColor = Color.Silver;
            if (activeCafe.Rates.ContainsKey(activeUser.ID))
            {
                already_rated_label_cafe_info_panel.Text = "Rated";
                already_rated_label_cafe_info_panel.Visible = true;
                for (int i = 0; i < activeCafe.Rates[activeUser.ID]; i++)
                {
                    if (i == 0)
                        star_label1_cafe_panel.ForeColor = Color.LightSeaGreen;
                    if (i == 1)
                        star_label2_cafe_panel.ForeColor = Color.LightSeaGreen;
                    if (i == 2)
                        star_label3_cafe_panel.ForeColor = Color.LightSeaGreen;
                    if (i == 3)
                        star_label4_cafe_panel.ForeColor = Color.LightSeaGreen;
                    if (i == 4)
                        star_label5_cafe_panel.ForeColor = Color.LightSeaGreen;
                }
            }
            if (activeUser.isBlocked)
            {
                star_label1_cafe_panel.Enabled = false;
                star_label2_cafe_panel.Enabled = false;
                star_label3_cafe_panel.Enabled = false;
                star_label4_cafe_panel.Enabled = false;
                star_label5_cafe_panel.Enabled = false;
            }
            else
            {
                star_label1_cafe_panel.Enabled = true;
                star_label2_cafe_panel.Enabled = true;
                star_label3_cafe_panel.Enabled = true;
                star_label4_cafe_panel.Enabled = true;
                star_label5_cafe_panel.Enabled = true;

            }
        }
        private void InitialStateForUserControlPanel()
        {
            if (selectedUserFromCombobox.isAdmin)
            {
                make_admin_button_user_mod_for_admin_panel.Text = "Make user";
            }
            else
            {
                make_admin_button_user_mod_for_admin_panel.Text = "Make admin";
            }
            if (selectedUserFromCombobox.isBlocked)
            {
                block_button_user_mod_for_admin_panel.Text = "Unblock";
                block_button_user_mod_for_admin_panel.ForeColor = Color.Azure;
            }
            else
            {
                block_button_user_mod_for_admin_panel.Text = "Block";
                block_button_user_mod_for_admin_panel.ForeColor = Color.Maroon;
            }
        }
        private void InitialStateForMenu(int page)
        {
            page_menu_panel.Text = "Page    " + page;
            if (activeCafe.Menu.Count >= (page - 1) * 9)
            {
                for (int i = 0; i < MenuLabels.Count; i++)
                    MenuLabels[i].Visible = false;
                for (int i = 0; i < MenuCheckBoxes.Count; i++)
                {
                    MenuCheckBoxes[i].Checked = false;
                    MenuCheckBoxes[i].Visible = false;
                }
                for (int i = 0; i < 9; i++)
                {
                    MenuNumbers[i].Visible = false;
                    MenuNumbers[i].Value = 1;
                }
                page_down_menu_panel.Visible = false;
                page_up_menu_panel.Visible = false;
                //    foreach (KeyValuePair<string,int> item in tempMenu)
                //    activeUser.OrderList.Add(item.Key,item.Value);
                /////////////////////////////////////////////////////////////////////////
                for (int i = (page - 1) * 9; i < Math.Min(activeCafe.Menu.Count, page * 9 + 1); i++)
                {
                    MenuLabels[i % 9].Text = activeCafe.Menu.Keys.ElementAt((page - 1) * 9 + i % 9) + "\n" + activeCafe.Menu.Values.ElementAt((page - 1) * 9 + i % 9) + " AMD";
                    MenuLabels[i % 9].Visible = true;
                    MenuCheckBoxes[i % 9].Visible = true;
                    MenuNumbers[i % 9].Visible = true;
                }
            }
            if (activeCafe.Menu.Count > page * 9)
            {
                page_up_menu_panel.Visible = true;
            }
            if (pageCount > 1)
                page_down_menu_panel.Visible = true;
            for (int i = 0; i < Math.Min(page * 9, activeCafe.Menu.Count); i++)
            {
                if (activeUser.OrderList.ContainsKey(MenuLabels[i % 9].Text.Split('\n')[0]))
                {
                    MenuCheckBoxes[i % 9].Checked = true;
                    MenuNumbers[i % 9].Value = activeUser.OrderList[MenuLabels[i % 9].Text.Split('\n')[0]];
                }
            }
        }
        #endregion Initial states for panels
        #region Events of panels
        #region login_panel_functions
        #region self_functions
        private void combobox_login_panel_Leave(object sender, EventArgs e)
        {
            combobox_login_panel.Text = "Search cafes";
            combobox_login_panel.ForeColor = Color.Silver;
        }
        private void combobox_login_panel_Enter(object sender, EventArgs e)
        {
            combobox_login_panel.Text = String.Empty;
            combobox_login_panel.ForeColor = Color.Black;
            var dictionary = new Dictionary<string, Cafe>();
            dictionary.Add("", new Cafe());
            for (int i = 0; i < Cafe.cafes.Count; i++)
                dictionary.Add(Cafe.cafes[i].Name, Cafe.cafes[i]);
            combobox_login_panel.DataSource = new BindingSource(dictionary, null);
            combobox_login_panel.DisplayMember = "Key";
            combobox_login_panel.ValueMember = "Value";
        }
        private void combobox_login_panel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show(combobox_login_panel.SelectedValue.ToString());
                OpenCafePage((Cafe)combobox_login_panel.SelectedValue);
            }
        }
        #endregion self_functions
        #region sigin_in_panel
        private void sign_up_label_sign_in_panel_Click(object sender, EventArgs e)
        {
            sign_in_panel.Visible = false;
            sign_up_panel.Visible = true;
        }

        private void sign_up_label_sign_in_panel_MouseHover(object sender, EventArgs e)
        {
            sign_up_label_sign_in_panel.Font = new Font(sign_up_label_sign_in_panel.Font, FontStyle.Underline | FontStyle.Bold);
        }

        private void sign_up_label_sign_in_panel_MouseLeave(object sender, EventArgs e)
        {
            sign_up_label_sign_in_panel.Font = new Font(sign_up_label_sign_in_panel.Font, FontStyle.Bold);
        }
        private void usernameBox_sign_in_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(usernameBox_sign_in_panel.Text))
            {
                usernameBox_sign_in_panel.Text = "Username";
                usernameBox_sign_in_panel.ForeColor = Color.Silver;
            }
        }

        private void usernameBox_sign_in_panel_Enter(object sender, EventArgs e)
        {
            if (usernameBox_sign_in_panel.Text.Equals("Username"))
            {
                usernameBox_sign_in_panel.Text = "";
                usernameBox_sign_in_panel.ForeColor = Color.Black;

            }
        }
        private void passwordBox_sign_in_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(passwordBox_sign_in_panel.Text))
            {
                passwordBox_sign_in_panel.Text = "Password";
                passwordBox_sign_in_panel.ForeColor = Color.Silver;
            }
        }

        private void passwordBox_sign_in_panel_Enter(object sender, EventArgs e)
        {

            if (passwordBox_sign_in_panel.Text.Equals("Password"))
            {
                passwordBox_sign_in_panel.Text = "";
                passwordBox_sign_in_panel.ForeColor = Color.Black;
            }
        }

        private void passwordBox_sign_in_panel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sign_in_button_Click(sender, e);
            }
        }
        private void checkBox_show_pass_sign_in_panel_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_show_pass_sign_in_panel.Checked)
            {
                passwordBox_sign_in_panel.UseSystemPasswordChar = false;
            }
            else
            {
                passwordBox_sign_in_panel.UseSystemPasswordChar = true;
            }
        }
        private void sign_in_button_Click(object sender, EventArgs e)
        {
            if (usernameBox_sign_in_panel.Text.Equals("Username") || passwordBox_sign_in_panel.Text.Equals("Password"))
            {
                warning_label_sign_in_panel.Text = "Please fill up the fields";
                warning_label_sign_in_panel.Visible = true;
                return;
            }
            try
            {
                activeUser =(ActiveUser) User.LogIn(usernameBox_sign_in_panel.Text, EncodePassword(passwordBox_sign_in_panel.Text));
                user_panel.Visible = true;
                login_panel.Visible = false;
                InitialStateForUserPanel();
                InitialStateForUserPagePanel();
                InitialStateForSignInPanel();
                if (activeUser.isAdmin)
                {
                    user_name_lastname_label.Text = activeUser.Name + " " + activeUser.LastName + " (Admin)";
                    comboBox_user_search_user_panel.Visible = true;
                    add_cafe_button_user_page_panel.Visible = true;
                }
                else
                {
                    add_cafe_button_user_page_panel.Visible = false;
                    user_name_lastname_label.Text = activeUser.Name + " " + activeUser.LastName;
                }
                SystemSounds.Beep.Play();
            }
            catch (ArgumentException exception)
            {
                warning_label_sign_in_panel.Text = exception.Message;
                warning_label_sign_in_panel.Visible = true;
            }
        }

        #endregion sign_in_panel
        #region sign_up_panel
        private void sign_up_button_Click(object sender, EventArgs e)
        {
            if (nameBox_sign_up_panel.Text.Equals("Name") ||
                lastnameBox_sign_up_panel.Text.Equals("Last name") ||
                usernameBox_sign_up_panel.Text.Equals("Username") ||
                passwordBox_sign_up_panel.Text.Equals("Password"))
            {
                warning_label_sign_up_panel.Text = "Pleas fill up the fields";
                warning_label_sign_up_panel.Visible = true;
                return;
            }
            try
            {

                User user = new User(nameBox_sign_up_panel.Text, lastnameBox_sign_up_panel.Text, usernameBox_sign_up_panel.Text, EncodePassword(passwordBox_sign_up_panel.Text));
                activeUser = (ActiveUser)User.LogIn(usernameBox_sign_up_panel.Text, EncodePassword(passwordBox_sign_up_panel.Text));
                user_name_lastname_label.Text = activeUser.Name + " " + activeUser.LastName;
                user_panel.Visible = true;
                login_panel.Visible = false;
                InitialStateForUserPanel();
                InitialStateForSignInPanel();
                SystemSounds.Beep.Play();

            }
            catch (ArgumentException exception)
            {
                warning_label_sign_up_panel.Text = exception.Message;
                warning_label_sign_up_panel.Visible = true;
            }
        }
        private void sign_in_label_sign_up_panel_Click(object sender, EventArgs e)
        {
            sign_up_panel.Visible = false;
            sign_in_panel.Visible = true;
        }
        private void sign_in_label_sign_up_panel_MouseHover(object sender, EventArgs e)
        {
            sign_in_label_sign_up_panel.Font = new Font(sign_in_label_sign_up_panel.Font, FontStyle.Underline | FontStyle.Bold);
        }
        private void sign_in_label_sign_up_panel_MouseLeave(object sender, EventArgs e)
        {
            sign_in_label_sign_up_panel.Font = new Font(sign_in_label_sign_up_panel.Font, FontStyle.Bold);
        }
        private void nameBox_sign_up_panel_Enter(object sender, EventArgs e)
        {
            if (nameBox_sign_up_panel.Text.Equals("Name"))
            {
                nameBox_sign_up_panel.Text = "";
                nameBox_sign_up_panel.ForeColor = Color.Black;
            }
        }
        private void nameBox_sign_up_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(nameBox_sign_up_panel.Text))
            {
                nameBox_sign_up_panel.Text = "Name";
                nameBox_sign_up_panel.ForeColor = Color.Silver;
            }

        }
        private void lastnameBox_sign_up_panel_Enter(object sender, EventArgs e)
        {
            if (lastnameBox_sign_up_panel.Text.Equals("Last name"))
            {
                lastnameBox_sign_up_panel.Text = "";
                lastnameBox_sign_up_panel.ForeColor = Color.Black;
            }
        }
        private void lastnameBox_sign_up_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(lastnameBox_sign_up_panel.Text))
            {
                lastnameBox_sign_up_panel.Text = "Last name";
                lastnameBox_sign_up_panel.ForeColor = Color.Silver;
            }

        }
        private void usernameBox_sign_up_panel_Enter(object sender, EventArgs e)
        {
            if (usernameBox_sign_up_panel.Text.Equals("Username"))
            {
                usernameBox_sign_up_panel.Text = "";
                usernameBox_sign_up_panel.ForeColor = Color.Black;
            }
        }
        private void usernameBox_sign_up_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(usernameBox_sign_up_panel.Text))
            {
                usernameBox_sign_up_panel.Text = "Username";
                usernameBox_sign_up_panel.ForeColor = Color.Silver;
            }
        }
        private void passwordBox_sign_up_panel_Enter(object sender, EventArgs e)
        {

            if (passwordBox_sign_up_panel.Text.Equals("Password"))
            {
                passwordBox_sign_up_panel.Text = "";
                passwordBox_sign_up_panel.ForeColor = Color.Black;
            }
        }
        private void passwordBox_sign_up_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(passwordBox_sign_up_panel.Text))
            {
                passwordBox_sign_up_panel.Text = "Password";
                passwordBox_sign_up_panel.ForeColor = Color.Silver;
            }
        }
        private void passwordBox_sign_up_panel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                sign_up_button_Click(sender, e);

        }
        private void checkBox_show_pass_sign_up_panel_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_show_pass_sign_up_panel.Checked)
            {
                passwordBox_sign_up_panel.UseSystemPasswordChar = false;
            }
            else
            {
                passwordBox_sign_up_panel.UseSystemPasswordChar = true;
            }
        }
        #endregion sign_up_panel
        #endregion login_panel_functions
        #region user_panel_functions
        #region user_control_panel
        private void heading_label_user_mod_for_admin_panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void heading_label_user_mod_for_admin_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                user_mod_for_admin_panel.Left = e.X + user_mod_for_admin_panel.Left - MouseDownLocation.X;
                user_mod_for_admin_panel.Top = e.Y + user_mod_for_admin_panel.Top - MouseDownLocation.Y;
            }
        }
        private void make_admin_button_user_mod_for_admin_panel_Click(object sender, EventArgs e)
        {
            selectedUserFromCombobox.adminSetterID = activeUser.ID;
            if (!selectedUserFromCombobox.isAdmin)
            {
                selectedUserFromCombobox.isAdmin = true;
                make_admin_button_user_mod_for_admin_panel.Text = "Make user";
                MessageBox.Show(String.Format("You made {0} {1} admin", selectedUserFromCombobox.Name, selectedUserFromCombobox.LastName));

            }
            else
            {
                selectedUserFromCombobox.isAdmin = false;
                make_admin_button_user_mod_for_admin_panel.Text = "Make admin";
                MessageBox.Show(String.Format("You made {0} {1} user", selectedUserFromCombobox.Name, selectedUserFromCombobox.LastName));
                selectedUserFromCombobox.adminSetterID = 0;
            }
        }
        private void delete_user_button_user_mod_for_admin_panel_Click(object sender, EventArgs e)
        {
            delete_acc_ask_panel_user_control_panel.Visible = true;
            delete_acc_ask_panel_user_control_panel.BringToFront();
        }
        private void cancel_button_user_mod_for_admin_panel_Click(object sender, EventArgs e)
        {
            user_mod_for_admin_panel.Visible = false;
            selectedUserFromCombobox = null;
        }
        private void block_button_user_mod_for_admin_panel_Click(object sender, EventArgs e)
        {
            if (selectedUserFromCombobox.isBlocked)
            {
                selectedUserFromCombobox.isBlocked = false;
                block_button_user_mod_for_admin_panel.Text = "Block";
                block_button_user_mod_for_admin_panel.ForeColor = Color.Maroon;
            }
            else
            {
                selectedUserFromCombobox.isBlocked = true;
                block_button_user_mod_for_admin_panel.Text = "Unblock";
                block_button_user_mod_for_admin_panel.ForeColor = Color.Azure;
            }
        }
        #endregion user_control_panel
        #region self_functions
        private void favorites_caombobox_user_page_panel_Leave(object sender, EventArgs e)
        {
            favorites_caombobox_user_page_panel.Text = "Favorites";
        }
        private void favorites_caombobox_user_page_panel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenCafePage((Cafe)favorites_caombobox_user_page_panel.SelectedValue);
            }
        }
        private void favorites_caombobox_user_page_panel_Enter(object sender, EventArgs e)
        {
            if (favorites_caombobox_user_page_panel.Text.Equals("Favorites"))
            {
                favorites_caombobox_user_page_panel.Text = "";
            }
            var dictionary = new Dictionary<string, Cafe>();
            dictionary.Add("", new Cafe());
            for (int i = 0; i < Cafe.cafes.Count; i++)
                dictionary.Add(Cafe.cafes[i].Name, Cafe.cafes[i]);
            favorites_caombobox_user_page_panel.DataSource = new BindingSource(dictionary, null);
            favorites_caombobox_user_page_panel.DisplayMember = "Key";
            favorites_caombobox_user_page_panel.ValueMember = "Value";
        }
        private void active_user_name_lastname_cafe_panel_Click(object sender, EventArgs e)
        {
            cafe_panel.Visible = false;
            login_panel.Visible = false;
            user_panel.Visible = true;
            activeCafe = null;
            InitialStateForUserPanel();
        }
        private void add_cafe_button_user_page_panel_Click(object sender, EventArgs e)
        {
            add_cafe_panel_user_page_panel.Visible = true;
            add_cafe_panel_user_page_panel.BringToFront();
            InitialStateForAddCafePanel();
        }
        private void show_nearby_button_user_panel_Click(object sender, EventArgs e)
        {
            try
            {
                string address = addressBox_user_panel.Text;
                activeUser.Location = GeoCode.GEOCodeAddress(address);
                near_cafes_panel_user_page_panel.Visible = true;
                InitialStateForNearCafesPanel();
                worning_label_user_page_panel.Text = "Your current location : " + GeoCode.GetFormattedAddress(address);
                worning_label_user_page_panel.Visible = true;
            }
            catch
            {
                worning_label_user_page_panel.Text = "Address was not found";
                worning_label_user_page_panel.Visible = true;
            }

        }
        private void character_open_account_settings_panel_Click(object sender, EventArgs e)
        {
            if (account_settings_panel.Visible == true)
                account_settings_panel.Visible = false;
            else
            {
                account_settings_panel.Visible = true;
                account_settings_panel.BringToFront();
            }
        }
        private void comboBox_cafe_search_user_panel_Leave(object sender, EventArgs e)
        {
            comboBox_cafe_search_user_panel.Text = "Search cafes";
            comboBox_cafe_search_user_panel.ForeColor = Color.Silver;
        }

        private void comboBox_cafe_search_user_panel_Enter(object sender, EventArgs e)
        {
            comboBox_cafe_search_user_panel.Text = String.Empty;
            comboBox_cafe_search_user_panel.ForeColor = Color.Black;
            var dictionary = new Dictionary<string, Cafe>();
            dictionary.Add("", new Cafe());
            for (int i = 0; i < Cafe.cafes.Count; i++)
                dictionary.Add(Cafe.cafes[i].Name, Cafe.cafes[i]);
            comboBox_cafe_search_user_panel.DataSource = new BindingSource(dictionary, null);
            comboBox_cafe_search_user_panel.DisplayMember = "Key";
            comboBox_cafe_search_user_panel.ValueMember = "Value";
        }
        private void comboBox_cafe_search_user_panel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenCafePage((Cafe)comboBox_cafe_search_user_panel.SelectedValue);
            }
        }
        private void comboBox_user_search_user_panel_Enter(object sender, EventArgs e)
        {
            comboBox_user_search_user_panel.Text = String.Empty;
            comboBox_user_search_user_panel.ForeColor = Color.Black;
            var dictionary = new Dictionary<string, User>();
            dictionary.Add("", new User());
            for (int i = 0; i < User.users.Count; i++)
                dictionary.Add(User.users[i].UserName, User.users[i]);
            comboBox_user_search_user_panel.DataSource = new BindingSource(dictionary, null);
            comboBox_user_search_user_panel.DisplayMember = "Key";
            comboBox_user_search_user_panel.ValueMember = "Value";
        }

        private void comboBox_user_search_user_panel_Leave(object sender, EventArgs e)
        {

            comboBox_user_search_user_panel.Text = "Search users";
            comboBox_user_search_user_panel.ForeColor = Color.Silver;

        }
        private void comboBox_user_search_user_panel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                User s = (User)comboBox_user_search_user_panel.SelectedValue;
                selectedUserFromCombobox = s;
                if (s.Equals(activeUser))
                {
                    MessageBox.Show(activeUser.ToString() + ": current user");
                }
                else if (activeUser.ID == s.adminSetterID || (!s.isAdmin))
                {
                    user_mod_for_admin_panel.Visible = true;
                    user_mod_for_admin_panel.BringToFront();
                    InitialStateForUserControlPanel();
                }
                else
                {
                    MessageBox.Show("You don't have permission to modify selected admins account");
                }
            }
        }
        #endregion
        #region add_cafe_panel
        private void addressBox_add_cafe_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(addressBox_add_cafe_panel.Text))
            {
                addressBox_add_cafe_panel.Text = "Address";
                addressBox_add_cafe_panel.ForeColor = Color.Silver;
            }
        }
        private void addressBox_add_cafe_panel_Enter(object sender, EventArgs e)
        {
            if (addressBox_add_cafe_panel.Text.Equals("Address"))
            {
                addressBox_add_cafe_panel.Text = String.Empty;
                addressBox_add_cafe_panel.ForeColor = Color.Black;
            }
        }
        private void emailBox_add_cafe_panel_Enter(object sender, EventArgs e)
        {
            if (emailBox_add_cafe_panel.Text.Equals("eMail"))
            {
                emailBox_add_cafe_panel.Text = String.Empty;
                emailBox_add_cafe_panel.ForeColor = Color.Black;
            }
        }
        private void emailBox_add_cafe_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(emailBox_add_cafe_panel.Text))
            {
                emailBox_add_cafe_panel.Text = "eMail";
                emailBox_add_cafe_panel.ForeColor = Color.Silver;
            }
        }
        private void hourBox_open_add_cafe_panel_Enter(object sender, EventArgs e)
        {
            if (hourBox_open_add_cafe_panel.Text.Equals("Open (hh.mm.ss)"))
            {
                hourBox_open_add_cafe_panel.Text = String.Empty;
                hourBox_open_add_cafe_panel.ForeColor = Color.Black;
            }
        }
        private void hourBox_open_add_cafe_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(hourBox_open_add_cafe_panel.Text))
            {
                hourBox_open_add_cafe_panel.Text = "Open (hh.mm.ss)";
                hourBox_open_add_cafe_panel.ForeColor = Color.Silver;
            }
        }
        private void hourBox_close_add_cafe_panel_Enter(object sender, EventArgs e)
        {
            if (hourBox_close_add_cafe_panel.Text.Equals("Close (hh.mm.ss)"))
            {
                hourBox_close_add_cafe_panel.Text = String.Empty;
                hourBox_close_add_cafe_panel.ForeColor = Color.Black;
            }
        }
        private void hourBox_close_add_cafe_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(hourBox_close_add_cafe_panel.Text))
            {
                hourBox_close_add_cafe_panel.Text = "Close (hh.mm.ss)";
                hourBox_close_add_cafe_panel.ForeColor = Color.Silver;
            }
        }
        private void cancel_button_add_cafe_panel_Click(object sender, EventArgs e)
        {
            add_cafe_panel_user_page_panel.Visible = false;
        }
        private void submit_button_add_cafe_panel_Click(object sender, EventArgs e)
        {
            try
            {
                string name = namebox_add_cafe_panel.Text;
                string address = addressBox_add_cafe_panel.Text;
                string phone = phoneBox_add_cafe_panel.Text;
                string email = emailBox_add_cafe_panel.Text;
                string webpage = webpageBox_add_cafe_panel.Text;
                string[] openH = hourBox_open_add_cafe_panel.Text.Split('.');
                string[] closeH = hourBox_close_add_cafe_panel.Text.Split('.');
                GeoCoordinate location;
                try
                {
                    new System.Net.Mail.MailAddress(email);
                }
                catch (Exception)
                {
                    warning_label_add_cafe_panel.Text = "Invalid email address";
                    warning_label_add_cafe_panel.Visible = true;
                    return;
                }
                try
                {
                    location = GeoCode.GEOCodeAddress(address);
                }
                catch
                {
                    throw new Exception("Address was not found");
                }
                TimeSpan open, close;
                try
                {
                    close = new TimeSpan(int.Parse(closeH[0]), int.Parse(closeH[1]), int.Parse(closeH[2]));
                    open = new TimeSpan(int.Parse(openH[0]), int.Parse(openH[1]), int.Parse(openH[2]));
                    if (!(int.Parse(closeH[0]) >= 0 && int.Parse(closeH[0]) <= 24 &&
                        int.Parse(closeH[1]) >= 0 && int.Parse(closeH[1]) <= 60 &&
                        int.Parse(closeH[2]) >= 0 && int.Parse(closeH[2]) <= 60 &&
                        int.Parse(openH[0]) >= 0 && int.Parse(openH[0]) <= 24 &&
                        int.Parse(closeH[1]) >= 0 && int.Parse(closeH[1]) <= 60 &&
                        int.Parse(closeH[2]) >= 0 && int.Parse(closeH[2]) <= 60))
                        throw new Exception();
                }
                catch
                {
                    throw new Exception("Hours must be in hh.mm.ss format");
                }
                bool[] workdays = new bool[7];
                if (checkBox_monday_cafe_page_panel.Checked) workdays[0] = true;
                if (checkBox_tuesday_cafe_page_panel.Checked) workdays[1] = true;
                if (checkBox_wednesday_cafe_page_panel.Checked) workdays[2] = true;
                if (checkBox_thursday_cafe_page_panel.Checked) workdays[3] = true;
                if (checkBox_friday_cafe_page_panel.Checked) workdays[4] = true;
                if (checkBox_saturday_cafe_page_panel.Checked) workdays[5] = true;
                if (checkBox_sunday_cafe_page_panel.Checked) workdays[6] = true;
                Cafe cafe = new Cafe(name, address, phone, open, close, location, workdays, email, webpage);
                add_cafe_panel_user_page_panel.Visible = false;
                InitialStateForUserPagePanel();

            }
            catch (Exception exc)
            {
                warning_label_add_cafe_panel.Text = exc.Message;
                warning_label_add_cafe_panel.Visible = true;
            }

        }
        private void namebox_add_cafe_panel_Enter(object sender, EventArgs e)
        {
            if (namebox_add_cafe_panel.Text.Equals("Name"))
            {
                namebox_add_cafe_panel.Text = String.Empty;
                namebox_add_cafe_panel.ForeColor = Color.Black;
            }
        }
        private void namebox_add_cafe_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(namebox_add_cafe_panel.Text))
            {
                namebox_add_cafe_panel.Text = "Name";
                namebox_add_cafe_panel.ForeColor = Color.Silver;
            }
        }
        private void phoneBox_add_cafe_panel_Enter(object sender, EventArgs e)
        {
            if (phoneBox_add_cafe_panel.Text.Equals("Phone"))
            {
                phoneBox_add_cafe_panel.Text = String.Empty;
                phoneBox_add_cafe_panel.ForeColor = Color.Black;
            }
        }
        private void phoneBox_add_cafe_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(phoneBox_add_cafe_panel.Text))
            {
                phoneBox_add_cafe_panel.Text = "Phone";
                phoneBox_add_cafe_panel.ForeColor = Color.Silver;
            }
        }
        private void webpageBox_add_cafe_panel_Enter(object sender, EventArgs e)
        {
            if (webpageBox_add_cafe_panel.Text.Equals("Web Page"))
            {
                webpageBox_add_cafe_panel.Text = String.Empty;
                webpageBox_add_cafe_panel.ForeColor = Color.Black;
            }
        }
        private void webpageBox_add_cafe_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(webpageBox_add_cafe_panel.Text))
            {
                webpageBox_add_cafe_panel.Text = "Web Page";
                webpageBox_add_cafe_panel.ForeColor = Color.Silver;
            }
        }
        #endregion add_cafe_panel
        #region user_page

        private void yes_button_delete_user_control_panel_Click(object sender, EventArgs e)
        {
            User.users.Remove(selectedUserFromCombobox);
            selectedUserFromCombobox = null;
            delete_acc_ask_panel_user_control_panel.Visible = false;
            user_mod_for_admin_panel.Visible = false;
        }
        private void no_button_delete_user_control_panel_Click(object sender, EventArgs e)
        {
            delete_acc_ask_panel_user_control_panel.Visible = false;
            InitialStateForUserControlPanel();
        }
        private void yes_button_cafe_delete_ask_panel_Click(object sender, EventArgs e)
        {
            activeUser.DeletCafe(activeCafe);
            InitialStateForUserPanel();
            activeCafe = null;
            cafe_delete_ask_panel.Visible = false;
            cafe_panel.Visible = false;
            user_panel.Visible = true;
        }
        private void no_button_cafe_delete_ask_panel_Click(object sender, EventArgs e)
        {
            cafe_delete_ask_panel.Visible = false;
        }
        private void favorite_cafe_label_user_page_panel_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < Cafe.cafes.Count; j++)
                if (((Label)sender).Text.Split('\n')[0].Equals(Cafe.cafes[j].Name))
                {
                    OpenCafePage(Cafe.cafes[j]);
                    return;
                }
        }
        private void popular_cafe_user_page_panel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
                if (Popular[i] == (Label)sender)
                {
                    Cafe.SortByRate(); //yst raitingi
                    OpenCafePage(Cafe.cafes[i]);
                }
        }
        private void near_cafe_label_user_page_panel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                if (NearBy[i] == (Label)sender)
                {
                    Cafe.SortByDistance(activeUser); //yst activ eUseric unecac her.
                    OpenCafePage(Cafe.cafes[i]);
                }
            }
        }
        #endregion user_page
        #region account_settings_panel
        #region self_functions

        private void change_password_label_account_settings_panel_label_Click(object sender, EventArgs e)
        {
            InitialStateForUserPanel();
            user_page_panel.Visible = false;
            change_password_panel.Visible = true;

        }
        private void edit_profile_label_account_settings_panel_Click(object sender, EventArgs e)
        {
            nameBox_edit_profile.Text = activeUser.Name;
            lastnameBox_edit_profile.Text = activeUser.LastName;
            usernameBox_edit_profile.Text = activeUser.UserName;
            InitialStateForUserPanel();
            edit_profile_panel.Visible = true;
            user_page_panel.Visible = false;
        }

        private void cash_settings_label_account_settings_panel_Click(object sender, EventArgs e)
        {
            InitialStateForUserPanel();
            cash_panel.Visible = true;
            user_page_panel.Visible = false;
            heading_label_cash_panel.Text = "Balance : " + activeUser.Cash + " AMD";
        }

        private void log_out_label_account_settings_panel_Click(object sender, EventArgs e)
        {
            InitialStateForUserPanel();
            log_out_ask_panel.Visible = true;
            user_page_panel.Visible = false;
        }

        #endregion
        #region change_password_panel
        //Mouse move
        private void heading_label_change_password_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                change_password_panel.Left = e.X + change_password_panel.Left - MouseDownLocation.X;
                change_password_panel.Top = e.Y + change_password_panel.Top - MouseDownLocation.Y;
            }
        }

        private void heading_label_change_password_panel_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        //end Mouse move
        private void oldPasswordBox_change_password_panel_Enter(object sender, EventArgs e)
        {
            if (oldPasswordBox_change_password_panel.Text.Equals("Password"))
            {
                oldPasswordBox_change_password_panel.Text = "";
                oldPasswordBox_change_password_panel.ForeColor = Color.Black;
            }
        }

        private void oldPasswordBox_change_password_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(oldPasswordBox_change_password_panel.Text))
            {
                oldPasswordBox_change_password_panel.Text = "Password";
                oldPasswordBox_change_password_panel.ForeColor = Color.Silver;
            }
        }
        private void newPasswordBox_change_password_panel_Enter(object sender, EventArgs e)
        {
            if (newPasswordBox_change_password_panel.Text.Equals("Password"))
            {
                newPasswordBox_change_password_panel.Text = "";
                newPasswordBox_change_password_panel.ForeColor = Color.Black;
            }
        }



        private void newPasswordBox_change_password_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(newPasswordBox_change_password_panel.Text))
            {
                newPasswordBox_change_password_panel.Text = "Password";
                newPasswordBox_change_password_panel.ForeColor = Color.Silver;
            }
        }
        private void submit_button_change_password_panel_Click(object sender, EventArgs e)
        {
            if (newPasswordBox_change_password_panel.Text.Equals("Password") || oldPasswordBox_change_password_panel.Text.Equals("Password")
              || repeatpasswordBox_change_password_panel.Text.Equals("Password") || String.IsNullOrEmpty(oldPasswordBox_change_password_panel.Text)
              || String.IsNullOrEmpty(newPasswordBox_change_password_panel.Text) || String.IsNullOrEmpty(repeatpasswordBox_change_password_panel.Text))
            {
                warning_label_change_password_panel.Text = "Please fill up the fields";
                warning_label_change_password_panel.Visible = true;
            }
            else
            {
                if ((repeatpasswordBox_change_password_panel.Text.Equals(newPasswordBox_change_password_panel.Text)))
                {
                    if (EncodePassword(oldPasswordBox_change_password_panel.Text).Equals(activeUser.Password))
                    {
                        try
                        {
                            activeUser.Password = EncodePassword(newPasswordBox_change_password_panel.Text);
                            change_password_panel.Visible = false;
                            InitialStateForPasswordChangePanel();
                            SystemSounds.Beep.Play();
                        }
                        catch (ArgumentException exc)
                        {
                            warning_label_change_password_panel.Text = exc.Message;
                            warning_label_change_password_panel.Visible = true;
                        }
                    }
                    else
                    {
                        warning_label_change_password_panel.Visible = true;
                        warning_label_change_password_panel.Text = "Current password is incorrect";
                    }
                }
                else
                {
                    warning_label_change_password_panel.Visible = true;
                    warning_label_change_password_panel.Text = "Passwords doesn't match";
                }
            }
            InitialStateForUserPagePanel();
        }

        private void cancel_button_change_password_panel_Click(object sender, EventArgs e)
        {
            change_password_panel.Visible = false;
            InitialStateForPasswordChangePanel();
            InitialStateForUserPanel();
            InitialStateForUserPagePanel();
        }
        private void repeatpasswordBox_change_password_panel_Enter(object sender, EventArgs e)
        {
            if (repeatpasswordBox_change_password_panel.Text.Equals("Password"))
            {
                repeatpasswordBox_change_password_panel.Text = "";
                repeatpasswordBox_change_password_panel.ForeColor = Color.Black;
            }
        }

        private void repeatpasswordBox_change_password_panel_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(repeatpasswordBox_change_password_panel.Text))
            {
                repeatpasswordBox_change_password_panel.Text = "Password";
                repeatpasswordBox_change_password_panel.ForeColor = Color.Silver;
            }
        }

        private void repeatpasswordBox_change_password_panel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                submit_button_change_password_panel_Click(sender, e);
        }
        private void checkBox_show_pass_change_password_panel_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_show_pass_change_password_panel.Checked)
            {
                newPasswordBox_change_password_panel.UseSystemPasswordChar = false;
                repeatpasswordBox_change_password_panel.UseSystemPasswordChar = false;
                oldPasswordBox_change_password_panel.UseSystemPasswordChar = false;
            }
            else
            {
                newPasswordBox_change_password_panel.UseSystemPasswordChar = true;
                repeatpasswordBox_change_password_panel.UseSystemPasswordChar = true;
                oldPasswordBox_change_password_panel.UseSystemPasswordChar = true;
            }
        }


        #endregion change_password_panel
        #region edit_profile_panel
        //Mouse move
        private Point MouseDownLocation;
        private void heading_label_edit_profile_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                edit_profile_panel.Left = e.X + edit_profile_panel.Left - MouseDownLocation.X;
                edit_profile_panel.Top = e.Y + edit_profile_panel.Top - MouseDownLocation.Y;
            }
        }

        private void heading_label_edit_profile_panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        ///end Mouse move
        private void save_button_edit_profile_Click(object sender, EventArgs e)
        {


            if (usernameBox_edit_profile.Text != activeUser.UserName)
            {
                for (int i = 0; i < User.users.Count; i++)
                {
                    if (User.users[i].UserName.Equals(usernameBox_edit_profile.Text))
                    {
                        warning_label_edit_profile_panel.Text = "User with given username already exists";
                        warning_label_edit_profile_panel.Visible = true;
                        return;
                    }
                }
            }
            activeUser.Name = nameBox_edit_profile.Text;
            activeUser.LastName = lastnameBox_edit_profile.Text;
            activeUser.UserName = usernameBox_edit_profile.Text;
            System.Media.SystemSounds.Asterisk.Play();
            edit_profile_panel.Visible = false;
            user_name_lastname_label.Text = activeUser.Name + " " + activeUser.LastName;
            InitialStateForUserPanel();

        }

        private void delete_account_button_edit_profile_Click(object sender, EventArgs e)
        {
            account_delete_warning_panel.Visible = true;
        }

        private void cancel_button_edit_profile_panel_Click(object sender, EventArgs e)
        {
            InitialStateForUserPanel();
            nameBox_edit_profile.Text = String.Empty;
            lastnameBox_edit_profile.Text = String.Empty;
            usernameBox_edit_profile.Text = String.Empty;
        }

        private void no_button_account_delete_warning_panel_Click(object sender, EventArgs e)
        {
            account_delete_warning_panel.Visible = false;
        }

        private void yes_button_account_delete_warning_panel_Click(object sender, EventArgs e)
        {
            if (User.users.Remove(activeUser))
            {
                InitialStateForUserPanel();
                user_panel.Visible = false;
                login_panel.Visible = true;
                warning_label_sign_in_panel.Visible = true;
                warning_label_sign_in_panel.Text = "Account has been succefully deleted";
            }
            else
            {
                account_delete_warning_panel.Visible = false;
                warning_label_edit_profile_panel.Text = "Somethong went wrong :(";
            }
        }
        #endregion edit_profile_panel
        #region cash_panel
        //Mouse move
        private void heading_label_cash_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                cash_panel.Left = e.X + cash_panel.Left - MouseDownLocation.X;
                cash_panel.Top = e.Y + cash_panel.Top - MouseDownLocation.Y;
            }
        }

        private void heading_label_cash_panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        //Mouse move
        private void cancel_button_cash_panel_Click(object sender, EventArgs e)
        {
            InitialStateForUserPanel();
            money_amountBox_cash_panel.Text = "";
            cash_panel.Visible = false;
            warning_label_cash_panel.Visible = false;
        }

        private void add_button_cash_panel_Click(object sender, EventArgs e)
        {
            try
            {
                activeUser.AddMoney(int.Parse(money_amountBox_cash_panel.Text));
                cash_panel.Visible = false;
                SystemSounds.Beep.Play();
                warning_label_cash_panel.Visible = false;
                InitialStateForUserPanel();
            }
            catch
            {
                warning_label_cash_panel.Text = "Money amount must be an integer";
                warning_label_cash_panel.Visible = true;
            }
        }

        private void money_amountBox_cash_panel_Enter(object sender, EventArgs e)
        {
            money_amountBox_cash_panel.Text = "";
        }

        private void money_amountBox_cash_panel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                add_button_cash_panel_Click(sender, e);
        }


        #endregion cash_panel
        #region log_out_ask_panel
        //Mouse move
        private void heading_ask_label_log_out_ask_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                log_out_ask_panel.Left = e.X + log_out_ask_panel.Left - MouseDownLocation.X;
                log_out_ask_panel.Top = e.Y + log_out_ask_panel.Top - MouseDownLocation.Y;
            }
        }

        private void heading_ask_label_log_out_ask_panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        //Mouse move
        private void yes_button_log_out_ask_panel_Click(object sender, EventArgs e)
        {
            InitialStateForUserPanel();
            activeUser = null;
            login_panel.Visible = true;
            user_panel.Visible = false;
            InitialStateForSignInPanel();
        }

        private void no_button_log_out_ask_panel_Click(object sender, EventArgs e)
        {
            log_out_ask_panel.Visible = false;
            InitialStateForUserPanel();
        }
        #endregion log_out_ask_panel
        #endregion account_settings_panel
        #endregion user_panel_functions
        #region cafe_panel
        private void modify_cafe_menu_button_Click(object sender, EventArgs e)
        {
            modify_menu_panel.Visible = true;
            comboBox_menu_modify_menu_panel.DataSource = new BindingSource(activeCafe.Menu, null);
            comboBox_menu_modify_menu_panel.DisplayMember = "Key";
        }
        private void change_cafe_info_button_Click(object sender, EventArgs e)
        {
            change_info_panel.Visible = true;
            nameBox_change_info_panel.Text = activeCafe.Name;
            phoneBox_change_info_panel.Text = activeCafe.Phone;
            addressBox_change_info_panel.Text = activeCafe.Address;
            webPageBox_change_info_panel.Text = activeCafe.WebPage;
            eMailBox_change_info_panel.Text = activeCafe.Email;
            latitudeBox_change_info_panel.Text = "" + activeCafe.Location.Latitude;
            longitudeBox_change_info_panel.Text = "" + activeCafe.Location.Longitude;
            openTimeBox_change_info_panel.Text = activeCafe.Open.ToString(@"hh\.mm\.ss");
            closeTimeBox_change_info_panel.Text = activeCafe.Close.ToString(@"hh\.mm\.ss");
            List<CheckBox> change_info_chechboxes = new List<CheckBox>();
            change_info_chechboxes.Add(checkBox1_change_info_panel);
            change_info_chechboxes.Add(checkBox2_change_info_panel);
            change_info_chechboxes.Add(checkBox3_change_info_panel);
            change_info_chechboxes.Add(checkBox4_change_info_panel);
            change_info_chechboxes.Add(checkBox5_change_info_panel);
            change_info_chechboxes.Add(checkBox6_change_info_panel);
            change_info_chechboxes.Add(checkBox7_change_info_panel);
            for (int i = 0; i < activeCafe.WorkDays.Length; i++)
            {
                if (activeCafe.WorkDays[i])
                {
                    change_info_chechboxes[i].Checked = true;
                }
            }
        }
        private void add_reviewBox_cafe_info_panel_Enter(object sender, EventArgs e)
        {
            if (add_reviewBox_cafe_info_panel.Text.Equals("Add review"))
                add_reviewBox_cafe_info_panel.Text = "";
            add_reviewBox_cafe_info_panel.ForeColor = Color.Black;
        }
        private void add_reviewBox_cafe_info_panel_Leave(object sender, EventArgs e)
        {
            if (add_reviewBox_cafe_info_panel.Text.Equals(""))
            {
                add_reviewBox_cafe_info_panel.Text = "Add review";
                add_reviewBox_cafe_info_panel.ForeColor = Color.Silver;
            }
        }
        private void add_reviewBox_cafe_info_panel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!add_reviewBox_cafe_info_panel.Text.Equals(""))
                {
                    activeCafe.Reviews.Insert(0, activeUser.UserName + ": " + add_reviewBox_cafe_info_panel.Text);
                    add_reviewBox_cafe_info_panel.Text = "";
                    for (int i = 0; i < activeCafe.Reviews.Count; i++)
                    {
                        if (i == 0)
                        {
                            review1_label_cafe_info_panel.Visible = true;
                            review1_label_cafe_info_panel.Text = activeCafe.Reviews[0];
                        }
                        if (i == 1)
                        {
                            review2_label_cafe_info_panel.Visible = true;
                            review2_label_cafe_info_panel.Text = activeCafe.Reviews[1];
                        }
                        if (i == 2)
                        {
                            review3_label_cafe_info_panel.Visible = true;
                            review3_label_cafe_info_panel.Text = activeCafe.Reviews[2];
                        }
                    }
                }
            }
        }
        private void heading_label_add_cafe_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                add_cafe_panel_user_page_panel.Left = e.X + add_cafe_panel_user_page_panel.Left - MouseDownLocation.X;
                add_cafe_panel_user_page_panel.Top = e.Y + add_cafe_panel_user_page_panel.Top - MouseDownLocation.Y;
            }
        }
        private void heading_label_add_cafe_panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void star_label2_cafe_panel_MouseLeave(object sender, EventArgs e)
        {
            InitialStateForRates();
        }
        private void star_label1_cafe_panel_MouseHover(object sender, EventArgs e)
        {
            star_label1_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label2_cafe_panel.ForeColor = Color.Silver;
            star_label3_cafe_panel.ForeColor = Color.Silver;
            star_label4_cafe_panel.ForeColor = Color.Silver;
            star_label5_cafe_panel.ForeColor = Color.Silver;
        }
        private void star_label2_cafe_panel_MouseHover(object sender, EventArgs e)
        {
            star_label1_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label2_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label3_cafe_panel.ForeColor = Color.Silver;
            star_label4_cafe_panel.ForeColor = Color.Silver;
            star_label5_cafe_panel.ForeColor = Color.Silver;
        }
        private void star_label3_cafe_panel_MouseHover(object sender, EventArgs e)
        {
            star_label1_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label2_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label3_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label4_cafe_panel.ForeColor = Color.Silver;
            star_label5_cafe_panel.ForeColor = Color.Silver;
        }
        private void star_label4_cafe_panel_MouseHover(object sender, EventArgs e)
        {
            star_label1_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label2_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label3_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label4_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label5_cafe_panel.ForeColor = Color.Silver;
        }
        private void star_label5_cafe_panel_MouseHover(object sender, EventArgs e)
        {
            star_label1_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label2_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label3_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label4_cafe_panel.ForeColor = Color.LightSeaGreen;
            star_label5_cafe_panel.ForeColor = Color.LightSeaGreen;
        }
        private void star_label1_cafe_panel_Click(object sender, EventArgs e)
        {
            if (activeCafe.Rates.ContainsKey(activeUser.ID))
            {
                activeCafe.Rates[activeUser.ID] = 1;
                already_rated_label_cafe_info_panel.Text = "Rate updated";
                activeUser.Favorite.Remove(activeCafe.Name);
                SystemSounds.Beep.Play();
            }
            else
            {
                activeCafe.Rates.Add(activeUser.ID, 1);
                already_rated_label_cafe_info_panel.Text = "Rated";
                SystemSounds.Beep.Play();
            }
            already_rated_label_cafe_info_panel.Visible = true;
            activeCafe.Rate();
        }
        private void star_label2_cafe_panel_Click(object sender, EventArgs e)
        {
            if (activeCafe.Rates.ContainsKey(activeUser.ID))
            {
                activeCafe.Rates[activeUser.ID] = 1;
                already_rated_label_cafe_info_panel.Text = "Rate updated";
                activeUser.Favorite.Remove(activeCafe.Name);
                SystemSounds.Beep.Play();
            }
            else
            {
                activeCafe.Rates.Add(activeUser.ID, 1);
                already_rated_label_cafe_info_panel.Text = "Rated";
                SystemSounds.Beep.Play();
            }
            already_rated_label_cafe_info_panel.Visible = true; ;
            activeCafe.Rate();
        }
        private void star_label3_cafe_panel_Click(object sender, EventArgs e)
        {
            if (activeCafe.Rates.ContainsKey(activeUser.ID))
            {
                activeCafe.Rates[activeUser.ID] = 3;
                already_rated_label_cafe_info_panel.Text = "Rate updated";
                activeUser.Favorite.Remove(activeCafe.Name);
                SystemSounds.Beep.Play();
            }
            else
            {
                activeCafe.Rates.Add(activeUser.ID, 3);
                already_rated_label_cafe_info_panel.Text = "Rated";
                SystemSounds.Beep.Play();
            }
            already_rated_label_cafe_info_panel.Visible = true;
            activeCafe.Rate();
        }
        private void star_label4_cafe_panel_Click(object sender, EventArgs e)
        {
            if (activeCafe.Rates.ContainsKey(activeUser.ID))
            {
                activeCafe.Rates[activeUser.ID] = 4;
                already_rated_label_cafe_info_panel.Text = "Rate updated";
                if (!activeUser.Favorite.Contains(activeCafe.Name))
                    activeUser.Favorite.Insert(0, activeCafe.Name);
                SystemSounds.Beep.Play();
            }
            else
            {
                activeCafe.Rates.Add(activeUser.ID, 4);
                already_rated_label_cafe_info_panel.Text = "Rated";
                activeUser.Favorite.Insert(0, activeCafe.Name);
                SystemSounds.Beep.Play();
            }
            already_rated_label_cafe_info_panel.Visible = true;
            activeCafe.Rate();
        }
        private void star_label5_cafe_panel_Click(object sender, EventArgs e)
        {
            if (activeCafe.Rates.ContainsKey(activeUser.ID))
            {
                activeCafe.Rates[activeUser.ID] = 5;
                already_rated_label_cafe_info_panel.Text = "Rate updated";
                if (!activeUser.Favorite.Contains(activeCafe.Name))
                    activeUser.Favorite.Insert(0, activeCafe.Name);
                SystemSounds.Beep.Play();
            }
            else
            {
                activeCafe.Rates.Add(activeUser.ID, 5);
                already_rated_label_cafe_info_panel.Text = "Rated";
                activeUser.Favorite.Insert(0, activeCafe.Name);
                SystemSounds.Beep.Play();
            }
            already_rated_label_cafe_info_panel.Visible = true;
            activeCafe.Rate();
        }
        private void star_label_cafe_panel_Leave(object sender, EventArgs e)
        {
            InitialStateForRates();
        }
        private void menu_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
                if ((CheckBox)sender == (MenuCheckBoxes[i]))
                {
                    if (MenuCheckBoxes[i].Checked == true)
                    {
                        activeUser.OrderList.Add(MenuLabels[i].Text.Split('\n')[0], (int)MenuNumbers[i].Value);
                        activeUser.Bill += activeCafe.Menu[MenuLabels[i].Text.Split('\n')[0]] * (int)MenuNumbers[i].Value;
                        bill_amount_label_cafe_panel.Text = "" + activeUser.Bill;
                    }
                    else
                    {
                        activeUser.OrderList.Remove(MenuLabels[i].Text.Split('\n')[0]);
                        activeUser.Bill -= activeCafe.Menu[MenuLabels[i].Text.Split('\n')[0]] * (int)MenuNumbers[i].Value;
                        bill_amount_label_cafe_panel.Text = "" + activeUser.Bill;
                    }
                    listBox1.DataSource = new BindingSource(activeUser.OrderList, null);
                }
        }
        private void menu_numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
                if ((NumericUpDown)sender == (MenuNumbers[i]))
                    if (activeUser.OrderList.ContainsKey(MenuLabels[i].Text.Split('\n')[0]) && MenuCheckBoxes[i].Checked)
                    {
                        activeUser.OrderList[MenuLabels[i].Text.Split('\n')[0]] = (int)MenuNumbers[i].Value;
                        listBox1.DataSource = new BindingSource(activeUser.OrderList, null);
                    }
            activeUser.Bill = 0;

            foreach (var item in activeUser.OrderList)
                activeUser.Bill += activeCafe.Menu[item.Key] * item.Value;
            bill_amount_label_cafe_panel.Text = "" + activeUser.Bill;
        }
        private void make_order_button_cafe_panel_Click(object sender, EventArgs e)
        {
            if (activeUser.Bill != 0)
            {
                if (activeUser.Cash >= activeUser.Bill)
                {
                    MessageBox.Show("Your order has been made");
                    worning_label_cafe_panel.Visible = false;
                    activeUser.Cash -= activeUser.Bill;
                    activeUser.Bill = 0;
                    activeUser.OrderList.Clear();
                    InitialStateForCafesPanel();
                }
                else
                    worning_label_cafe_panel.Visible = true;
            }
        }
        private void delete_cafe_button_cafe_panel_Click(object sender, EventArgs e)
        {
            cafe_delete_ask_panel.Visible = true;
        }
        private void page_up_menu_panel_Click(object sender, EventArgs e)
        {
            pageCount++;
            InitialStateForMenu(pageCount);
            listBox1.DataSource = new BindingSource(activeUser.OrderList, null);
        }
        private void page_down_menu_panel_Click(object sender, EventArgs e)
        {
            pageCount--;
            InitialStateForMenu(pageCount);
            listBox1.DataSource = new BindingSource(activeUser.OrderList, null);
        }
        private void listBox1_Format(object sender, ListControlConvertEventArgs e)
        {
            string key = ((KeyValuePair<string, int>)e.ListItem).Key;
            int value = ((KeyValuePair<string, int>)e.ListItem).Value;

            e.Value = key + "   x" + value;
        }
        #region modify_menu_panel

        private void comboBox_menu_modify_menu_panel_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                activeItem = (KeyValuePair<string, int>)(comboBox_menu_modify_menu_panel.SelectedItem);
            }
        }
        private void cancel_button_modify_menu_panel_Click(object sender, EventArgs e)
        {
            modify_menu_panel.Visible = false;
            warning_label_modify_menu_panel.Visible = false;
            comboBox_menu_modify_menu_panel.Text = "";
            nameBox_modify_menu_panel.Text = "";
            priceBox_modify_menu_panel.Text = "";
            nameBox_new_item_modify_menu_panel.Text = "";
            priceBox_new_item_modify_menu_panel.Text = "";
            mod_panel_modify_menu_panel.Visible = false;
        }
        private void comboBox_menu_modify_menu_panel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!comboBox_menu_modify_menu_panel.Text.Equals("(Collection)"))
                activeItem = (KeyValuePair<string, int>)(comboBox_menu_modify_menu_panel.SelectedItem);
            mod_panel_modify_menu_panel.Visible = true;
            mod_panel_modify_menu_panel.BringToFront();
        }
        private void save_button_modify_menu_panel_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(priceBox_modify_menu_panel.Text) && String.IsNullOrEmpty(nameBox_modify_menu_panel.Text))
                {
                    warning_label_modify_menu_panel.Text = "Please fill up the \nfields given above";
                    warning_label_modify_menu_panel.Visible = true;
                    return;
                }
                if (activeCafe.Menu.ContainsKey(nameBox_modify_menu_panel.Text))
                {
                    warning_label_modify_menu_panel.Text = "Item with this name already exists";
                    warning_label_modify_menu_panel.Visible = true;
                    return;
                }
                int.Parse(priceBox_modify_menu_panel.Text);
                activeCafe.Menu.Remove(activeItem.Key);
                activeCafe.Menu.Add(nameBox_modify_menu_panel.Text, int.Parse(priceBox_modify_menu_panel.Text));
                modify_menu_panel.Visible = false;
                mod_panel_modify_menu_panel.Visible = false;
                InitialStateForMenu(pageCount);
            }
            catch (FormatException)
            {
                warning_label_modify_menu_panel.Text = "The input price was not valid";
                warning_label_modify_menu_panel.Visible = true;
            }
            catch (Exception)
            {
                warning_label_modify_menu_panel.Text = "Please select an item to modify";
                warning_label_modify_menu_panel.Visible = true;
            }

        }
        private void delete_item_button_modify_menu_panel_Click(object sender, EventArgs e)
        {
            try
            {
                activeCafe.Menu.Remove(activeItem.Key);
                InitialStateForMenu(pageCount);
                comboBox_menu_modify_menu_panel.DataSource = new BindingSource(activeCafe.Menu, null);
                mod_panel_modify_menu_panel.Visible = false;

            }
            catch (ArgumentNullException)
            {
                warning_label_modify_menu_panel.Text = "Please select an item to delete";
                warning_label_modify_menu_panel.Visible = true;
                return;
            }

        }
        private void add_button_modify_menu_panel_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(nameBox_new_item_modify_menu_panel.Text) ||
                String.IsNullOrEmpty(priceBox_new_item_modify_menu_panel.Text))
            {
                warning_label_modify_menu_panel.Text = "Please fill up the \nfields given above";
                warning_label_modify_menu_panel.Visible = true;
                return;
            }
            if (activeCafe.Menu.ContainsKey(nameBox_new_item_modify_menu_panel.Text))
            {
                warning_label_modify_menu_panel.Text = "Item with this name already exists";
                warning_label_modify_menu_panel.Visible = true;
                return;
            }
            try
            {
                activeCafe.Menu.Add(nameBox_new_item_modify_menu_panel.Text, int.Parse(priceBox_new_item_modify_menu_panel.Text));
                InitialStateForMenu(pageCount);
                warning_label_modify_menu_panel.Visible = false;
                mod_panel_modify_menu_panel.Visible = false;
                modify_menu_panel.Visible = false;
            }
            catch (FormatException)
            {
                warning_label_modify_menu_panel.Text = "The input price was not valid";
                warning_label_modify_menu_panel.Visible = true;
            }
        }
        private void heading_label_modify_menu_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                modify_menu_panel.Left = e.X + modify_menu_panel.Left - MouseDownLocation.X;
                modify_menu_panel.Top = e.Y + modify_menu_panel.Top - MouseDownLocation.Y;
            }
        }
        private void heading_label_modify_menu_panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        private void comboBox_menu_modify_menu_panel_Leave(object sender, EventArgs e)
        {
            comboBox_menu_modify_menu_panel.Text = "";
        }
        #endregion modify_menu_panel
        #region change_info_panel

        private void save_button_change_info_panel_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.Mail.MailAddress a = new System.Net.Mail.MailAddress(eMailBox_change_info_panel.Text);
            }
            catch (Exception)
            {
                warning_label_change_info_panel.Text = "Invalid email address";
                warning_label_change_info_panel.Visible = true;
                return;
            }
            if (String.IsNullOrEmpty(nameBox_change_info_panel.Text) || String.IsNullOrEmpty(phoneBox_change_info_panel.Text) ||
                String.IsNullOrEmpty(webPageBox_change_info_panel.Text) || String.IsNullOrEmpty(addressBox_change_info_panel.Text) ||
                String.IsNullOrEmpty(eMailBox_change_info_panel.Text) || String.IsNullOrEmpty(longitudeBox_change_info_panel.Text) ||
                String.IsNullOrEmpty(latitudeBox_change_info_panel.Text) || String.IsNullOrEmpty(closeTimeBox_change_info_panel.Text) ||
                String.IsNullOrEmpty(openTimeBox_change_info_panel.Text))
            {
                warning_label_change_info_panel.Text = "Please fill up the fields";
            }
            try
            {
                activeCafe.Open = new TimeSpan(
                     int.Parse(openTimeBox_change_info_panel.Text.Split('.')[0]),
                     int.Parse(openTimeBox_change_info_panel.Text.Split('.')[1]),
                    int.Parse(openTimeBox_change_info_panel.Text.Split('.')[2])
                     );
                activeCafe.Close = new TimeSpan(
                   int.Parse(closeTimeBox_change_info_panel.Text.Split('.')[0]),
                   int.Parse(closeTimeBox_change_info_panel.Text.Split('.')[1]),
                  int.Parse(closeTimeBox_change_info_panel.Text.Split('.')[2])
                   );
                activeCafe.Name = nameBox_change_info_panel.Text;
                activeCafe.Phone = phoneBox_change_info_panel.Text;
                activeCafe.Address = addressBox_change_info_panel.Text;
                activeCafe.WebPage = webPageBox_change_info_panel.Text;
                activeCafe.Email = eMailBox_change_info_panel.Text;
                try
                {
                    activeCafe.Location = GeoCode.GEOCodeAddress(activeCafe.Address);
                }
                catch
                {
                    throw new Exception("Address was not found");
                }
                change_info_panel.Visible = false;
                InitialStateForCafesPanel();
            }
            catch (Exception v)
            {
                warning_label_change_info_panel.Text = v.Message;
                warning_label_change_info_panel.Visible = true;
            }
            List<CheckBox> change_info_chechboxes = new List<CheckBox>();
            change_info_chechboxes.Add(checkBox1_change_info_panel);
            change_info_chechboxes.Add(checkBox2_change_info_panel);
            change_info_chechboxes.Add(checkBox3_change_info_panel);
            change_info_chechboxes.Add(checkBox4_change_info_panel);
            change_info_chechboxes.Add(checkBox5_change_info_panel);
            change_info_chechboxes.Add(checkBox6_change_info_panel);
            change_info_chechboxes.Add(checkBox7_change_info_panel);
            for (int i = 0; i < 7; i++)
            {
                if (change_info_chechboxes[i].Checked)
                    activeCafe.WorkDays[i] = true;
            }

        }
        private void cancel_button_change_info_panel_Click(object sender, EventArgs e)
        {
            change_info_panel.Visible = false;
            warning_label_change_info_panel.Visible = false;
        }
        private void heading_label_change_info_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                change_info_panel.Left = e.X + change_info_panel.Left - MouseDownLocation.X;
                change_info_panel.Top = e.Y + change_info_panel.Top - MouseDownLocation.Y;
            }

        }
        private void heading_label_change_info_panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }
        #endregion change_info_panel
        #endregion cafe_panel
        #endregion Events of panels
        #region Serialization

        public static String EncodePassword(string password)
        {
            String s = String.Empty;
            for (int i = 0; i < password.Length; i++)
                s += (char)(password[i] + 7);
            return s;
        }

        public CafeApp()
        {

            InitializeComponent();
            login_panel.Visible = true;
            user_panel.Visible = false;
            cafe_panel.Visible = false;
            User.users.ToString();
        }

        private void CafeApp_Load(object sender, EventArgs e)
        {

        }

        private void CafeApp_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
        #endregion Serialization                        
    }
}