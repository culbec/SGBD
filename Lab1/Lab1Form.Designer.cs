namespace Lab1
{
    partial class Lab1Form
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
            parentDataGridView = new DataGridView();
            childrenDataGridView = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            textBoxProdus = new TextBox();
            textBoxCantitate = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)parentDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)childrenDataGridView).BeginInit();
            SuspendLayout();
            // 
            // parentDataGridView
            // 
            parentDataGridView.AllowUserToAddRows = false;
            parentDataGridView.AllowUserToDeleteRows = false;
            parentDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            parentDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            parentDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            parentDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            parentDataGridView.Location = new Point(12, 21);
            parentDataGridView.MultiSelect = false;
            parentDataGridView.Name = "parentDataGridView";
            parentDataGridView.ReadOnly = true;
            parentDataGridView.Size = new Size(448, 572);
            parentDataGridView.TabIndex = 0;
            parentDataGridView.RowHeaderMouseClick += parentSelected;
            // 
            // childrenDataGridView
            // 
            childrenDataGridView.AllowUserToAddRows = false;
            childrenDataGridView.AllowUserToDeleteRows = false;
            childrenDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            childrenDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            childrenDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            childrenDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            childrenDataGridView.Location = new Point(858, 21);
            childrenDataGridView.MultiSelect = false;
            childrenDataGridView.Name = "childrenDataGridView";
            childrenDataGridView.ReadOnly = true;
            childrenDataGridView.Size = new Size(427, 572);
            childrenDataGridView.TabIndex = 1;
            childrenDataGridView.RowHeaderMouseClick += childSelected;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.ControlLightLight;
            label1.Location = new Point(550, 192);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 2;
            label1.Text = "Produs";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.ControlLightLight;
            label2.Location = new Point(550, 291);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 3;
            label2.Text = "Cantitate";
            // 
            // textBoxProdus
            // 
            textBoxProdus.Location = new Point(550, 219);
            textBoxProdus.Name = "textBoxProdus";
            textBoxProdus.PlaceholderText = "Ex: Pepsi";
            textBoxProdus.Size = new Size(237, 23);
            textBoxProdus.TabIndex = 4;
            // 
            // textBoxCantitate
            // 
            textBoxCantitate.Location = new Point(550, 318);
            textBoxCantitate.Name = "textBoxCantitate";
            textBoxCantitate.PlaceholderText = "Ex: 10";
            textBoxCantitate.Size = new Size(237, 23);
            textBoxCantitate.TabIndex = 5;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(550, 371);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 6;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += addChild;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(631, 371);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(75, 23);
            btnUpdate.TabIndex = 7;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += updateChild;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(712, 371);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += deleteChild;
            // 
            // Lab1Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.HotTrack;
            ClientSize = new Size(1297, 605);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(textBoxCantitate);
            Controls.Add(textBoxProdus);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(childrenDataGridView);
            Controls.Add(parentDataGridView);
            Name = "Lab1Form";
            Text = "Form1";
            Load += LoadForm;
            ((System.ComponentModel.ISupportInitialize)parentDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)childrenDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView parentDataGridView;
        private DataGridView childrenDataGridView;
        private Label label1;
        private Label label2;
        private TextBox textBoxProdus;
        private TextBox textBoxCantitate;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
    }
}
