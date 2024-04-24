using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Lab2
{
    public partial class Lab2Form : Form
    {
        private readonly DataSet dataSet = new();

        private readonly SqlDataAdapter parentAdapter = new();
        private readonly BindingSource bindingSourceParent = new();

        private readonly SqlDataAdapter childAdapter = new();
        private readonly BindingSource bindingSourceChild = new();

        private readonly List<String> childColumns = new(ConfigurationManager.AppSettings["childColumns"]!.Split(','));

        public Lab2Form()
        {
            InitializeComponent();
        }

        private void InitParentAdapter(SqlConnection conn)
        {
            try
            {
                var parentTable = ConfigurationManager.AppSettings["parentTable"];

                // Filling the data set with parent data.
                parentAdapter.SelectCommand = DbUtils.GetSelectParentsCommand(conn);
                parentAdapter.Fill(dataSet, parentTable!);

                // Setting the data sources of the Data Grid View and of the Binding Source.
                bindingSourceParent.DataSource = dataSet.Tables[parentTable];
                dataGridParent.DataSource = bindingSourceParent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitChildAdapter(SqlConnection conn)
        {
            try
            {
                var parentTable = ConfigurationManager.AppSettings["parentTable"]!;
                var childTable = ConfigurationManager.AppSettings["childTable"]!;
                var fk = ConfigurationManager.AppSettings["FK"]!;

                // Filling the data set with parent data.
                childAdapter.SelectCommand = DbUtils.GetSelectChildrenCommand(conn);
                childAdapter.Fill(dataSet, childTable!);

                // Retrieving the columns in 1-n relation.
                var parentColumn = dataSet.Tables[parentTable]!.Columns[fk]!;
                var childColumn = dataSet.Tables[childTable]!.Columns[fk]!;

                // Creating the relation.
                var dataRelation = new DataRelation("FK", parentColumn, childColumn);
                dataSet.Relations.Add(dataRelation);

                // Setting the data sources of the Data Grid View and of the Binding Source.
                bindingSourceChild.DataSource = bindingSourceParent;
                bindingSourceChild.DataMember = "FK";
                dataGridChild.DataSource = bindingSourceChild;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitPropertiesPanel()
        {
            try
            {
                // Adding a textbox for each column.


                var textBoxWidth = 200;
                var textBoxHeight = 20;
                var verticalPadding = 20;

                for (int i = 0; i < childColumns.Count; i++)
                {
                    var column = childColumns[i];
                    var textBoxY = i * (textBoxHeight + verticalPadding);

                    var textBox = new TextBox
                    {
                        Size = new Size(textBoxWidth, textBoxHeight),
                        Location = new Point(50, textBoxY),
                        Name = column
                    };

                    // Setting readonly properties for the ids.
                    if (i < 2)
                    {
                        textBox.ReadOnly = true;
                    }
                    propertiesPanel.Controls.Add(textBox);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadForm(object sender, EventArgs e)
        {
            try
            {
                using (var conn = DbUtils.GetConnection())
                {
                    conn.Open();
                    InitParentAdapter(conn);
                    InitChildAdapter(conn);
                    InitPropertiesPanel();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshChildView(SqlConnection conn)
        {
            try
            {
                var childTable = ConfigurationManager.AppSettings["childTable"]!;

                // Updating the connection of the select command.
                childAdapter.SelectCommand.Connection = conn;

                // Clearing the data set.
                dataSet.Tables[childTable]!.Clear();

                // Filling the data set again.
                childAdapter.Fill(dataSet, childTable);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddChild(object sender, EventArgs e)
        {
            // Checking if there are any parents selected.
            if (dataGridParent.SelectedRows.Count == 0)
            {
                MessageBox.Show("You need to select a parent!");
                return;
            }

            // Verifying the content of the text boxes.
            for (int i = 1; i < childColumns.Count; i++)
            {
                var textBox = propertiesPanel.Controls.Find(childColumns[i], true)[0];
                if (textBox.Text == "")
                {
                    MessageBox.Show("Properties are not set.");
                    return;
                }
            }

            // Inserting.
            try
            {
                using (var conn = DbUtils.GetConnection())
                {
                    conn.Open();
                    childAdapter.InsertCommand = DbUtils.GetInsertChildCommand(conn, propertiesPanel);
                    int result = childAdapter.InsertCommand.ExecuteNonQuery();

                    if (result == 0)
                    {
                        MessageBox.Show("The child couldn't be added.");
                        return;
                    }

                    RefreshChildView(conn);
                    MessageBox.Show("Child added!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ParentSelected(object sender, DataGridViewCellMouseEventArgs e)
        {

            foreach (var column in childColumns)
            {
                var textBox = (TextBox)propertiesPanel.Controls.Find(column, true)[0];
                textBox.Clear();
            }

            // Setting the ID of the parent in the text box.
            var parent = dataGridParent.SelectedRows[0];
            var id = parent.Cells[0].Value.ToString();

            var parentIdColumn = childColumns[1];
            var _textBox = propertiesPanel.Controls.Find(parentIdColumn, true)[0];

            _textBox.Text = id;
        }

        private void ChildSelected(object sender, DataGridViewCellMouseEventArgs e)
        {
            var selection = dataGridChild.SelectedRows[0];

            for (int i = 0; i < childColumns.Count; i++)
            {
                var textBox = (TextBox)propertiesPanel.Controls.Find(childColumns[i], true)[0];
                textBox.Text = selection.Cells[i].Value.ToString();
            }
        }

        private void UpdateChild(object sender, EventArgs e)
        {
            if (dataGridChild.SelectedRows.Count == 0)
            {
                MessageBox.Show("You need to select a child!");
                return;
            }

            // Verifying if the text boxes are empty.
            for(int i = 2; i < childColumns.Count; i++)
            {
                var textBox = (TextBox)propertiesPanel.Controls.Find(childColumns[i], true)[0];
                if (textBox.Text == "")
                {
                    MessageBox.Show("Properties are not set.");
                    return;
                }
            }

            try
            {
                using (var conn = DbUtils.GetConnection())
                {
                    conn.Open();
                    childAdapter.UpdateCommand = DbUtils.GetUpdateChildCommand(conn, propertiesPanel);
                    int result = childAdapter.UpdateCommand.ExecuteNonQuery();

                    if (result == 0)
                    {
                        MessageBox.Show("The child couldn't be updated.");
                        return;
                    }

                    RefreshChildView(conn);
                    MessageBox.Show("Child updated!");
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteChild(object sender, EventArgs e)
        {
            if (dataGridChild.SelectedRows.Count == 0)
            {
                MessageBox.Show("You need to select a child!");
                return;
            }

            // Verifying if the text boxes are empty.

            try
            {
                using (var conn = DbUtils.GetConnection())
                {
                    conn.Open();
                    childAdapter.DeleteCommand = DbUtils.GetDeleteChildCommand(conn, propertiesPanel);
                    int result = childAdapter.DeleteCommand.ExecuteNonQuery();

                    if (result == 0)
                    {
                        MessageBox.Show("The child couldn't be deleted.");
                        return;
                    }

                    RefreshChildView(conn);
                    MessageBox.Show("Child deleted!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
