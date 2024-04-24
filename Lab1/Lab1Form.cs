using Microsoft.Data.SqlClient;
using System.Data;
using System.Windows.Forms.VisualStyles;

namespace Lab1
{
    public partial class Lab1Form : Form
    {
        private readonly DataSet dataSet;

        private SqlDataAdapter parentAdapter;
        private readonly BindingSource bindingSourceParent;

        private SqlDataAdapter childAdapter;
        private readonly BindingSource bindingSourceChild;

        public Lab1Form()
        {
            InitializeComponent();
            dataSet = new DataSet();

            parentAdapter = new SqlDataAdapter();
            bindingSourceParent = new BindingSource();

            childAdapter = new SqlDataAdapter();
            bindingSourceChild = new BindingSource();
        }

        private void initParentAdapter(SqlConnection connection)
        {
            // Initializing the Select Command of the parent adapter.
            this.parentAdapter.SelectCommand = DbUtils.GetSelectParentsCommand(connection);

            // Filling the data set, specifying the source table.
            this.parentAdapter.Fill(dataSet, "Evenimente");

            // Creating a binding source on a table.
            this.bindingSourceParent.DataSource = this.dataSet.Tables["Evenimente"];

            // Specifying the data source for the view.
            this.parentDataGridView.DataSource = this.bindingSourceParent;
        }

        private void initChildAdapter(SqlConnection connection, BindingSource parentBindingSource)
        {
            // Initializing the child adapter.
            this.childAdapter.SelectCommand = DbUtils.GetSelectChildrenCommand(connection);

            // Filling the data set, specifying the source table.
            this.childAdapter.Fill(dataSet, "Inventar");

            // Retrieving the columns that are in a FK relation.
            DataColumn parentColumn = dataSet.Tables["Evenimente"]!.Columns["Eid"]!;
            DataColumn childColumn = dataSet.Tables["Inventar"]!.Columns["Eid"]!;

            // Creating a new relation, encapsulating the FK relation.
            DataRelation dataRelation = new DataRelation("FK_Evenimente_Inventare", parentColumn, childColumn);
            dataSet.Relations.Add(dataRelation);

            // Binding the child to the same source as the parent, to extract the rows in a relation.
            this.bindingSourceChild.DataSource = this.bindingSourceParent;
            this.bindingSourceChild.DataMember = "FK_Evenimente_Inventare";

            // Specifying the data source for the view.
            this.childrenDataGridView.DataSource = this.bindingSourceChild;
        }

        private void LoadForm(object sender, EventArgs e)
        {
            try
            {
                using SqlConnection connection = DbUtils.GetConnection();
                connection.Open();
                //MessageBox.Show("Starea conexiunii: " + connection.State);
                this.initParentAdapter(connection);
                this.initChildAdapter(connection, bindingSourceParent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refreshChildView(SqlConnection connection)
        {
            this.childAdapter.SelectCommand.Connection = connection;
            this.dataSet.Tables["Inventar"]!.Clear();
            this.childAdapter.Fill(this.dataSet, "Inventar");
        }

        private void addChild(object sender, EventArgs e)
        {
            // Checking if a parent row has been selected.
            if (parentDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Trebuie sa selectati un eveniment!");
                return;
            }

            // Checking if the attributes have been passed correctly (Produs = string, Cantitate = int > 0).
            if (textBoxProdus.Text.Length == 0 || textBoxCantitate.Text.Length == 0)
            {
                MessageBox.Show("Trebuie sa introduceti ambele atribute pentru a adauga!");
                return;
            }

            try
            {
                if (Int32.Parse(textBoxCantitate.Text) <= 0)
                {
                    MessageBox.Show("Cantitatea trebuie sa fie mai mare decat 0!");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Cantitatea trebuie sa fie un numar!");
                return;
            }

            // Saving the passed attributes.
            DataGridViewRow row = parentDataGridView.SelectedRows[0];

            int eid = (int)row.Cells[0].Value;
            string produs = textBoxProdus.Text;
            int cantitate = Int32.Parse(textBoxCantitate.Text);

            try
            {
                using SqlConnection connection = DbUtils.GetConnection();
                connection.Open();

                this.childAdapter.InsertCommand = DbUtils.GetInsertChildCommand(connection, eid, produs, cantitate);
                int result = this.childAdapter.InsertCommand.ExecuteNonQuery();

                if (result == 0)
                {
                    MessageBox.Show("Nu s-a putut adauga inregistrarea!");
                    return;
                }

                this.refreshChildView(connection);

                MessageBox.Show("O noua inregistrare a fost adaugata!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteChild(object sender, EventArgs e)
        {
            // Checking if a child row has been selected.
            if (this.childrenDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Trebuie sa selectati o inregistrare din tabela fiu!");
                return;
            }

            // Retrieving the id of the row to delete.
            DataGridViewRow row = this.childrenDataGridView.SelectedRows[0];
            int iid = (int)row.Cells[0].Value;

            // Deleting the row.
            try
            {
                using SqlConnection connection = DbUtils.GetConnection();
                connection.Open();

                this.childAdapter.DeleteCommand = DbUtils.GetDeleteChildCommand(connection, iid);

                int result = this.childAdapter.DeleteCommand.ExecuteNonQuery();

                if (result == 0)
                {
                    MessageBox.Show("Nu s-a putut sterge inregistrarea!");
                    return;
                }

                this.refreshChildView(connection);
                MessageBox.Show("Inregistrarea a fost stearsa!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateChild(object sender, EventArgs e)
        {
            // Checking if a child row has been selected.
            if (this.childrenDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Trebuie sa selectati o inregistrare din tabela fiu!");
                return;
            }

            // Checking if the attributes have been passed correctly (Produs = string, Cantitate = int > 0).
            if (textBoxProdus.Text.Length == 0 || textBoxCantitate.Text.Length == 0)
            {
                MessageBox.Show("Trebuie sa introduceti ambele atribute pentru a adauga!");
                return;
            }

            try
            {
                if (int.Parse(textBoxCantitate.Text) <= 0)
                {
                    MessageBox.Show("Cantitatea trebuie sa fie mai mare decat 0!");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Cantitatea trebuie sa fie un numar!");
                return;
            }

            // Retrieving the attributes.
            DataGridViewRow row = childrenDataGridView.SelectedRows[0];

            int iid = (int)row.Cells[0].Value;
            string produs = textBoxProdus.Text;
            int cantitate = int.Parse(textBoxCantitate.Text);

            try
            {
                using SqlConnection connection = DbUtils.GetConnection();
                connection.Open();

                this.childAdapter.UpdateCommand = DbUtils.GetUpdateChildCommand(connection, iid, produs, cantitate);

                int result = this.childAdapter.UpdateCommand.ExecuteNonQuery();
                if (result == 0)
                {
                    MessageBox.Show("Nu s-a putut actualiza inregistrarea!");
                    return;
                }

                this.refreshChildView(connection);
                MessageBox.Show("Inregistrare actualizata cu succes!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void childSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            var selection = this.childrenDataGridView.SelectedRows[0];

            var produs = selection.Cells[2];
            var cantitate = selection.Cells[3];

            this.textBoxProdus.Text = produs.Value.ToString();
            this.textBoxCantitate.Text = cantitate.Value.ToString();
        }

        private void parentSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.textBoxProdus.Text = "";
            this.textBoxCantitate.Text = "";
        }
    }
}
