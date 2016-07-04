﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Factory;
using structures.Views.ListRefreshEngine;
using Marb.Extender.Datgridview;

namespace structures.Views
{
    public partial class UC_FinalRoundsViewer : UserControl
    {
        //Inputlist
        private ActiveBindingList<Wedstrijd> _WedstrijdList;

        //Bindinglists for update
        protected BindingListRefresh<Wedstrijd> _BindingListRefreshWedstrijd;


        public UC_FinalRoundsViewer(ActiveBindingList<Wedstrijd> wedstrijdlist)
        {
            InitializeComponent();
            _WedstrijdList = wedstrijdlist;
            dgv_wedstrijden.DoubleBuffered(true);
            dgv_wedstrijden.DataSource = _WedstrijdList;
        }


        private void PopulateReeksCombobox()
        {
            //Populate cmb_reeksnaam
            List<string> Reeksen = new List<string>();
            foreach (Wedstrijd w in _WedstrijdList)
            {
                if ((w.WedstrijdFormule == ProgramDefinitions.WedstrijdFormule.CrossFinals || 
                    w.WedstrijdFormule == ProgramDefinitions.WedstrijdFormule.PlacementGames)
                    && !Reeksen.Contains(w.ReeksNaam))
                {
                    Reeksen.Add(w.ReeksNaam);
                }
            }
            cmb_ReeksNaam.Items.Clear();
            cmb_ReeksNaam.Items.AddRange(Reeksen.ToArray());
            cmb_ReeksNaam.Width = DropDownWidth(cmb_ReeksNaam);
            if (Reeksen.Count > 0)
            {
                cmb_ReeksNaam.SelectedIndex = 0;
            }
            
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

        private void UC_FinalRoundsViewer_Load(object sender, EventArgs e)
        {
            PopulateReeksCombobox();
            
        }

        private void cmb_ReeksNaam_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrencyManager WedstrijdManager = (CurrencyManager)dgv_wedstrijden.BindingContext[dgv_wedstrijden.DataSource];
            WedstrijdManager.SuspendBinding();
            for (int i = 0; i < _WedstrijdList.Count; i++)
            {
                if (_WedstrijdList[i].ReeksNaam == cmb_ReeksNaam.Text)
                {
                    dgv_wedstrijden.Rows[i].Visible = true;
                }
                else
                {
                    dgv_wedstrijden.Rows[i].Visible = false;
                }
            }
            WedstrijdManager.ResumeBinding();

            //Update Brackets
            UpdateBrackets();

        }

        private void UpdateBrackets()
        {
            foreach (Wedstrijd w in _WedstrijdList)
            {
                if ((w.WedstrijdFormule == ProgramDefinitions.WedstrijdFormule.CrossFinals ||
                    w.WedstrijdFormule == ProgramDefinitions.WedstrijdFormule.PlacementGames)
                    && w.ReeksNaam == cmb_ReeksNaam.Text)
                {
                    

                }
            }





        }


    }
}
