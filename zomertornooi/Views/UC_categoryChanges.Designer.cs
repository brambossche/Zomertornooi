﻿namespace structures.Views
{
    partial class UC_categoryChanges
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstbx_categoryinput = new System.Windows.Forms.ListBox();
            this.lstbx_outpucategory = new System.Windows.Forms.ListBox();
            this.btn_changecategory = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bnt_setbackcategory = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstbx_categoryinput
            // 
            this.lstbx_categoryinput.FormattingEnabled = true;
            this.lstbx_categoryinput.Location = new System.Drawing.Point(12, 21);
            this.lstbx_categoryinput.Name = "lstbx_categoryinput";
            this.lstbx_categoryinput.Size = new System.Drawing.Size(120, 121);
            this.lstbx_categoryinput.TabIndex = 0;
            this.lstbx_categoryinput.SelectedIndexChanged += new System.EventHandler(this.lstbx_categoryinput_SelectedIndexChanged);
            // 
            // lstbx_outpucategory
            // 
            this.lstbx_outpucategory.FormattingEnabled = true;
            this.lstbx_outpucategory.Location = new System.Drawing.Point(278, 21);
            this.lstbx_outpucategory.Name = "lstbx_outpucategory";
            this.lstbx_outpucategory.Size = new System.Drawing.Size(120, 121);
            this.lstbx_outpucategory.TabIndex = 1;
            this.lstbx_outpucategory.SelectedIndexChanged += new System.EventHandler(this.lstbx_outpucategory_SelectedIndexChanged);
            // 
            // btn_changecategory
            // 
            this.btn_changecategory.BackColor = System.Drawing.SystemColors.Control;
            this.btn_changecategory.ForeColor = System.Drawing.Color.Red;
            this.btn_changecategory.Location = new System.Drawing.Point(135, 65);
            this.btn_changecategory.Name = "btn_changecategory";
            this.btn_changecategory.Size = new System.Drawing.Size(137, 30);
            this.btn_changecategory.TabIndex = 2;
            this.btn_changecategory.Text = ">> Change catgory >>";
            this.btn_changecategory.UseVisualStyleBackColor = false;
            this.btn_changecategory.Click += new System.EventHandler(this.btn_changecategory_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_changecategory);
            this.groupBox1.Controls.Add(this.lstbx_categoryinput);
            this.groupBox1.Controls.Add(this.lstbx_outpucategory);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 160);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Change category ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bnt_setbackcategory);
            this.groupBox2.Location = new System.Drawing.Point(3, 169);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(411, 80);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // bnt_setbackcategory
            // 
            this.bnt_setbackcategory.BackColor = System.Drawing.SystemColors.Control;
            this.bnt_setbackcategory.ForeColor = System.Drawing.Color.Red;
            this.bnt_setbackcategory.Location = new System.Drawing.Point(12, 29);
            this.bnt_setbackcategory.Name = "bnt_setbackcategory";
            this.bnt_setbackcategory.Size = new System.Drawing.Size(196, 30);
            this.bnt_setbackcategory.TabIndex = 3;
            this.bnt_setbackcategory.Text = "Set back to subscribed category";
            this.bnt_setbackcategory.UseVisualStyleBackColor = false;
            this.bnt_setbackcategory.Click += new System.EventHandler(this.bnt_setbackcategory_Click);
            // 
            // UC_categoryChanges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "UC_categoryChanges";
            this.Size = new System.Drawing.Size(423, 289);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstbx_categoryinput;
        private System.Windows.Forms.ListBox lstbx_outpucategory;
        private System.Windows.Forms.Button btn_changecategory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button bnt_setbackcategory;
    }
}
