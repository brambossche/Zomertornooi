﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Factory;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Marb.Bindinglist;
using log4net;
using structures.Views.ListRefreshEngine;
using Marb.Extender.Datgridview;

namespace structures.Views
{
    public partial class UC_TornooiAdministratie : UserControl
    {

        private static readonly ILog logger = LogManager.GetLogger(typeof(UC_TornooiAdministratie));
        private  BindingListRefresh<Wedstrijd> _BindingListRefresh;
        

        //Wedstrijden
        private ActiveBindingList<Wedstrijd> _WedstrijdList;
        private ExtBindingList<Terrein> _TerreinList;

        private BindingList<Wedstrijd> _fileteredWedstrijdReekslist;
        private BindingList<Wedstrijd> _fileteredWedstrijdUurlist;
        private BindingList<Terrein> _TerreinReeksList;

        //Reeks (ploegen + terreinen)
        private AdministratieReeks Reeks;

        #region Constructor & load

        public UC_TornooiAdministratie(ActiveBindingList<Wedstrijd> WedstrijdList, ExtBindingList<Terrein> TerreinList)
        {
            InitializeComponent();
            _WedstrijdList = WedstrijdList;
            _TerreinList = TerreinList;
            _WedstrijdList.ListChanged += _WedstrijdList_ListChanged;
            _TerreinList.ListChanged += _TerreinList_ListChanged;


            if (_WedstrijdList.Count > 0)
            {
                PopulateReeksCombobox();
                cmb_ReeksNaam.SelectedIndex = 0;
            }


        }

        private void UC_TornooiAdministratie_Load(object sender, EventArgs e)
        {
            if (_WedstrijdList.Count > 0)
            {
                DisableRows(0);
                ChangeRowColor();
            }
            UpdateTerreinen();

            _BindingListRefresh = new BindingListRefresh<Wedstrijd>(_WedstrijdList);

            dgv_Terreinen.DoubleBuffered(true);
            dgv_Klassement.DoubleBuffered(true);
            dgv_Wedstrijden.DoubleBuffered(true);

            this.Enter += UC_TornooiAdministratie_Enter;
            this.Leave += UC_TornooiAdministratie_Leave;
            dgv_Wedstrijden.CellBeginEdit += Dgv_Wedstrijden_CellBeginEdit;
            dgv_Wedstrijden.CellEndEdit += Dgv_Wedstrijden_CellEndEdit;
            _BindingListRefresh.ListRefreshed += _BindingListRefresh_ListRefreshed;
            _BindingListRefresh.StartRefreshing();
        }


        void _TerreinList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                for (int i = 0; i < _TerreinReeksList.Count; i++)
                {

                    if (_TerreinReeksList[i].Status)
                    {
                        dgv_Terreinen.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Green;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionForeColor = Color.White;
                    }
                    else
                    {
                        dgv_Terreinen.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Red;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionForeColor = Color.White;
                    }

                }

            }
        }

        void _WedstrijdList_ListChanged(object sender, ListChangedEventArgs e)
        {

            if (e.ListChangedType == ListChangedType.ItemChanged)
            {


            }
            else
            {
                if (_WedstrijdList.Count == 0)
                {
                    dgv_Terreinen.DataSource = null;
                    dgv_Klassement.DataSource = null;
                    dgv_Wedstrijden.DataSource = null;
                }

                
                cmb_ReeksNaam.Items.Clear();
                cmb_ReeksNaam.Text = "";
                PopulateReeksCombobox();

            }



           UpdateTerreinen();
        }








        #endregion

        #region refresh data

        private void Dgv_Wedstrijden_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _BindingListRefresh.StopRefreshing();
        }

        private void Dgv_Wedstrijden_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            _BindingListRefresh.StartRefreshing();
        }


        void _BindingListRefresh_ListRefreshed()
        {
            RefreshList();
        }

        private void RefreshList()
        {
            if (_BindingListRefresh != null)
            {
                _BindingListRefresh.RefreshList();
                dgv_Wedstrijden.Refresh();
                dgv_Wedstrijden.Update();
            }
        }


       

        void UC_TornooiAdministratie_Leave(object sender, EventArgs e)
        {
            if (_BindingListRefresh != null)
            {
                _BindingListRefresh.StopRefreshing();
            }
        }

        void UC_TornooiAdministratie_Enter(object sender, EventArgs e)
        {
            if (_BindingListRefresh != null)
            {
                if (_BindingListRefresh.AllowDataRefresh)
                {
                    _BindingListRefresh.StartRefreshing();
                }
            }
        }
        #endregion

     

        #region Comboboxes

        private void PopulateReeksCombobox()
        {
            //List<string> ReeksNamen = new List<string>();
            List<string> ReeksNamen = new List<string>();
            foreach (Wedstrijd w in _WedstrijdList)
            {
                if (!ReeksNamen.Contains(w.ReeksNaam))
                {
                    ReeksNamen.Add(w.ReeksNaam);
                }

            }
            cmb_ReeksNaam.Items.Clear();
            foreach (string s in ReeksNamen)
            {
                cmb_ReeksNaam.Items.Add(s);
            }

            cmb_ReeksNaam.Width = DropDownWidth(cmb_ReeksNaam);




        }

        private void PopulateUurCombobox()
        {
            List<string> Aanvangsuren = new List<string>();
            Aanvangsuren.Clear();
            foreach (Wedstrijd w in _fileteredWedstrijdReekslist)
            {
                if (!Aanvangsuren.Contains(w.Aanvangsuur.ToString()))
                {
                    Aanvangsuren.Add(w.Aanvangsuur.ToString());
                }
            }

            cmb_Aanvangsuur.Items.Clear();
            cmb_Aanvangsuur.Items.Add("Alle wedstrijden");
            foreach (string s in Aanvangsuren)
            {
                cmb_Aanvangsuur.Items.Add(s);
            }

            cmb_Aanvangsuur.Width = DropDownWidth(cmb_Aanvangsuur);

        }

        private int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0, temp = 0;
            foreach (var obj in myCombo.Items)
            {
                temp = TextRenderer.MeasureText(obj.ToString(), myCombo.Font).Width;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            return maxWidth + 20;
        }

        //Events
        private void cmb_ReeksNaam_SelectedIndexChanged(object sender, EventArgs e)
        {
            _fileteredWedstrijdReekslist = new BindingList<Wedstrijd>(_WedstrijdList.Where(x => x.ReeksNaam == cmb_ReeksNaam.Text).ToList());
            _TerreinReeksList = new BindingList<Terrein>(_TerreinList.Where(x => x.ReeksNaam == cmb_ReeksNaam.Text).ToList());
            PopulateUurCombobox();

            //
            cmb_Aanvangsuur.SelectedIndex = 0;
            CreateAdministratieReeks();
            UpdateKlassement();
            UpdateTerreinen();

            DisableRows(0);
            ChangeRowColor();


        }

        private void cmb_Aanvangsuur_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Aanvangsuur.Text == "Alle wedstrijden")
            {
                _fileteredWedstrijdUurlist = _fileteredWedstrijdReekslist;
            }
            else
            {
                _fileteredWedstrijdUurlist = new BindingList<Wedstrijd>(_fileteredWedstrijdReekslist.Where(x => x.Aanvangsuur.ToString() == cmb_Aanvangsuur.Text).ToList());
            }
            UpdateWedstrijden();
            ChangeRowColor();
            DisableRows(0);
        }

        #endregion

        #region Datagridviews

        private void dgv_Wedstrijden_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //Update het klassement
            UpdateKlassement();

            //Disablez row als terrein niet vrij is
            DisableRows(0);

            //Update alledatagrids
            UpdateAllDataGrids();
        }

        private void dgv_Wedstrijden_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Make changes in database
            dgv_Wedstrijden.EndEdit();


            //Print wedstrijdblad indien wedstrijd gestart wordt
            //int index = dgv_Wedstrijden.Columns["IsBusy"].Index;
            //if (e.ColumnIndex == index && _fileteredWedstrijdUurlist[e.RowIndex].IsBusy == true && _fileteredWedstrijdUurlist[e.RowIndex].Isplayed == false)
            //{
            //    CreateGameSheet(e.RowIndex);
            //}

            
            ChangeRowColor();

        }

        private void dgv_Terreinen_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


            int index = dgv_Terreinen.Columns["Status"].Index;
            DataGridViewCheckBoxCell c = (DataGridViewCheckBoxCell)dgv_Terreinen.Rows[e.RowIndex].Cells[index];
            if ((bool)c.FormattedValue)
            {
                dgv_Terreinen.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green;
                dgv_Terreinen.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Green;
                dgv_Terreinen.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                dgv_Terreinen.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.White;
            }
            else
            {
                dgv_Terreinen.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                dgv_Terreinen.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Red;
                dgv_Terreinen.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                dgv_Terreinen.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.White;
            }



            UpdateAllDataGrids();
        }

        #endregion

        #region Buttons
        private void btn_lock_Click(object sender, EventArgs e)
        {
            cmb_ReeksNaam.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (cmb_Aanvangsuur.SelectedIndex < cmb_Aanvangsuur.Items.Count - 1)
            {
                cmb_Aanvangsuur.SelectedIndex++;
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            if (cmb_Aanvangsuur.SelectedIndex > 0)
            {
                cmb_Aanvangsuur.SelectedIndex--;
            }
        }



        #endregion

        #region Update routines

        private void UpdateWedstrijden()
        {

            dgv_Wedstrijden.DataSource = _fileteredWedstrijdUurlist;
            foreach (DataGridViewColumn column in dgv_Wedstrijden.Columns)
            {
                if (column.Index <= 5)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                else
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

            }

            if (cmb_Aanvangsuur.SelectedIndex == 0)
            {
                btn_previousRound.Enabled = false;
                btn_nextRound.Enabled = true;
            }
            else if (cmb_Aanvangsuur.SelectedIndex == cmb_Aanvangsuur.Items.Count - 1)
            {
                btn_nextRound.Enabled = false;
                btn_previousRound.Enabled = true;
            }
            else
            {
                btn_previousRound.Enabled = true;
                btn_nextRound.Enabled = true;
            }



        }

        private void UpdateKlassement()
        {
            Reeks.CalculateRankings();
            dgv_Klassement.DataSource = Reeks.Klassement.Ranking;
            foreach (DataGridViewColumn column in dgv_Klassement.Columns)
            {
                if (column.Index == 0)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                }
                else
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

            }


            foreach (DataGridViewRow row in dgv_Klassement.Rows)
            {
                row.HeaderCell.Value = (row.Index + 1).ToString() + ".";
            }
            dgv_Klassement.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;


        }

        private void UpdateTerreinen()
        {
            if (_TerreinReeksList != null)
            {
                dgv_Terreinen.DataSource = _TerreinReeksList;
                foreach (DataGridViewColumn column in dgv_Terreinen.Columns)
                {

                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }


                for (int i = 0; i < _TerreinReeksList.Count; i++)
                {
                    if (_TerreinReeksList[i].Status == true)
                    {
                        dgv_Terreinen.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Green;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionForeColor = Color.White;
                    }
                    else
                    {
                        dgv_Terreinen.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Red;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                        dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionForeColor = Color.White;

                    }

                }
            }

        }

        private void UpdateAllDataGrids()
        {
            //dgv_Klassement.EndEdit();
            //dgv_Terreinen.EndEdit();
            //dgv_Wedstrijden.EndEdit();

            dgv_Terreinen.Refresh();
            dgv_Wedstrijden.Refresh();
            dgv_Klassement.Refresh();
        }

        #endregion



        #region Methods

        private void CreateAdministratieReeks()
        {
            Reeks = new AdministratieReeks(cmb_ReeksNaam.Text, _fileteredWedstrijdReekslist);

            foreach (Wedstrijd w in _fileteredWedstrijdReekslist)
            {
                if (!Reeks.ReeksTerreinen.Contains(w.Terrein))
                {
                    Reeks.ReeksTerreinen.Add(w.Terrein);
                }
            }

            foreach (Wedstrijd w in _fileteredWedstrijdReekslist)
            {
                if (!Reeks.ReeksPloegen.Contains(w.Home))
                {
                    Reeks.ReeksPloegen.Add(w.Home);
                }
                if (!Reeks.ReeksPloegen.Contains(w.Away))
                {
                    Reeks.ReeksPloegen.Add(w.Away);
                }
            }


        }

        private void DisableRows(int p)
        {
            for (int i = p; i < dgv_Wedstrijden.Rows.Count; i++)
            {
                if (_fileteredWedstrijdUurlist[i].Terrein.Status == false && !_fileteredWedstrijdUurlist[i].IsBusy)
                {
                    dgv_Wedstrijden.Rows[i].ReadOnly = true;
                }
                else if (_fileteredWedstrijdUurlist[i].Isplayed)
                {
                    int index = dgv_Wedstrijden.Columns["IsBusy"].Index;
                    dgv_Wedstrijden.Rows[i].Cells[index].ReadOnly = true;
                    dgv_Wedstrijden.Rows[i].Cells[index + 1].ReadOnly = true;
                }
                else
                {
                    dgv_Wedstrijden.Rows[i].ReadOnly = false;
                }
            }
        }

        private void ChangeRowColor()
        {
            //Change color of played games
            for (int i = 0; i < _fileteredWedstrijdUurlist.Count; i++)
            {
                if (_fileteredWedstrijdUurlist[i].Isplayed == true)
                {
                    dgv_Wedstrijden.Rows[i].DefaultCellStyle.BackColor = Color.DarkSeaGreen;
                }
                else if (_fileteredWedstrijdUurlist[i].IsBusy == true)
                {
                    dgv_Wedstrijden.Rows[i].DefaultCellStyle.BackColor = Color.Coral;
                }
                else
                {
                    dgv_Wedstrijden.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }

            }
        }


        #endregion



        #region Printing

        private void CreateGameSheet(int index)
        {
            try
            {
                Assembly _assembly;
                StreamReader objReader;

                //Read the complete file and replace the text
                _assembly = Assembly.GetExecutingAssembly();
                //string[] rel = _assembly.GetManifestResourceNames();

                objReader = new StreamReader(_assembly.GetManifestResourceStream("structures.Printing.gameSheet.html"));




                string content = objReader.ReadToEnd();
                objReader.Close();

                //Replace the text
                Wedstrijd w = _fileteredWedstrijdUurlist[index];

                string home = w.Home.ToString();
                content = content.Replace("[HOME]", home);

                string away = w.Away.ToString();
                content = content.Replace("[AWAY]", away);

                string title = home + " - " + away;
                content = content.Replace("[TITLE]", title);



                string terrein = w.Terrein.ToString();
                content = content.Replace("[TERREIN]", terrein);

                string scheidsrechter = w.Scheidsrechter.ToString();
                content = content.Replace("[SCHEIDSRECHTER]", scheidsrechter);

                string reeks = w.ReeksNaam;
                content = content.Replace("[REEKS]", reeks);

                string uur = w.Aanvangsuur.ToShortTimeString();
                content = content.Replace("[AANVANGSUUR]", uur);





                //Write content to new html-file
                StreamWriter writer = new StreamWriter(("gameSheet_adj.html"));
                writer.Write(content);
                writer.Close();

                //Open HTML file 
                Process.Start("gameSheet_adj.html");



            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void dgv_Wedstrijden_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            CreateGameSheet(e.RowIndex);
        }

        private void dgv_Wedstrijden_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Assembly _assembly;

            //Read the complete file and replace the text
            _assembly = Assembly.GetExecutingAssembly();
            //string[] rel = _assembly.GetManifestResourceNames();
            Bitmap myBitmap = new Bitmap(_assembly.GetManifestResourceStream("structures.Printing.printerIcon.png"));
            Icon myIcon = Icon.FromHandle(myBitmap.GetHicon());

            Graphics graphics = e.Graphics;

            //Set Image dimension - User's choice
            int iconHeight = 14;
            int iconWidth = 14;

            //Set x/y position - As the center of the RowHeaderCell
            int xPosition = e.RowBounds.X + (dgv_Wedstrijden.RowHeadersWidth / 2);
            int yPosition = e.RowBounds.Y +
            ((dgv_Wedstrijden.Rows[e.RowIndex].Height - iconHeight) / 2);

            Rectangle rectangle = new Rectangle(xPosition, yPosition, iconWidth, iconHeight);
            graphics.DrawIcon(myIcon, rectangle);










        }

        #endregion

        /*
        private void dgv_Terreinen_DataSourceChanged(object sender, EventArgs e)
        {
            int index = dgv_Terreinen.Columns["Status"].Index;
            DataGridViewCheckBoxCell c = (DataGridViewCheckBoxCell)dgv_Terreinen.Rows[0].Cells[index];

            for (int i = 0; i < dgv_Terreinen.Rows.Count; i++)
            {
                if ((bool)c.FormattedValue)
                {
                    dgv_Terreinen.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                    dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Green;
                    dgv_Terreinen.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionForeColor = Color.White;
                }
                else
                {
                    dgv_Terreinen.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Red;
                    dgv_Terreinen.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    dgv_Terreinen.Rows[i].DefaultCellStyle.SelectionForeColor = Color.White;
                }

            }








        }

        */


    }
}
