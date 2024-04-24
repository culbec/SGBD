namespace Lab2
{
    partial class Lab2Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridChild = new DataGridView();
            dataGridParent = new DataGridView();
            propertiesPanel = new Panel();
            buttonAdd = new Button();
            buttonUpdate = new Button();
            buttonDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridChild).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridParent).BeginInit();
            SuspendLayout();
            // 
            // dataGridChild
            // 
            dataGridChild.AllowUserToAddRows = false;
            dataGridChild.AllowUserToDeleteRows = false;
            dataGridChild.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridChild.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridChild.Location = new Point(948, 12);
            dataGridChild.Name = "dataGridChild";
            dataGridChild.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridChild.Size = new Size(524, 737);
            dataGridChild.TabIndex = 0;
            dataGridChild.RowHeaderMouseClick += ChildSelected;
            // 
            // dataGridParent
            // 
            dataGridParent.AllowUserToAddRows = false;
            dataGridParent.AllowUserToDeleteRows = false;
            dataGridParent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridParent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridParent.Location = new Point(12, 12);
            dataGridParent.Name = "dataGridParent";
            dataGridParent.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridParent.Size = new Size(511, 737);
            dataGridParent.TabIndex = 1;
            dataGridParent.RowHeaderMouseClick += ParentSelected;
            // 
            // propertiesPanel
            // 
            propertiesPanel.Location = new Point(572, 107);
            propertiesPanel.Name = "propertiesPanel";
            propertiesPanel.Size = new Size(300, 300);
            propertiesPanel.TabIndex = 2;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(572, 502);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(80, 30);
            buttonAdd.TabIndex = 3;
            buttonAdd.Text = "Add";
            buttonAdd.UseVisualStyleBackColor = true;
            buttonAdd.Click += AddChild;
            // 
            // buttonUpdate
            // 
            buttonUpdate.Location = new Point(694, 502);
            buttonUpdate.Name = "buttonUpdate";
            buttonUpdate.Size = new Size(80, 30);
            buttonUpdate.TabIndex = 4;
            buttonUpdate.Text = "Update";
            buttonUpdate.UseVisualStyleBackColor = true;
            buttonUpdate.Click += UpdateChild;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(813, 502);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(80, 30);
            buttonDelete.TabIndex = 5;
            buttonDelete.Text = "Delete";
            buttonDelete.UseVisualStyleBackColor = true;
            buttonDelete.Click += DeleteChild;
            // 
            // Lab2Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.HotTrack;
            ClientSize = new Size(1484, 761);
            Controls.Add(buttonDelete);
            Controls.Add(buttonUpdate);
            Controls.Add(buttonAdd);
            Controls.Add(propertiesPanel);
            Controls.Add(dataGridParent);
            Controls.Add(dataGridChild);
            Name = "Lab2Form";
            Text = "Form1";
            Load += LoadForm;
            ((System.ComponentModel.ISupportInitialize)dataGridChild).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridParent).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridChild;
        private DataGridView dataGridParent;
        private Panel propertiesPanel;
        private Button buttonAdd;
        private Button buttonUpdate;
        private Button buttonDelete;
    }
}
