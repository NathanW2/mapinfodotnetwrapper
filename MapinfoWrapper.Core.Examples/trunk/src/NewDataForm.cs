using System;
using System.Windows.Forms;
using Wrapper.Example.Tables;

namespace Wrapper.Example
{
    public partial class NewDataForm : Form
    {
        public NewDataForm()
        {
            InitializeComponent();
        }

        public AddResult Show(World worldData)
        {
            this.worldBindingSource.DataSource = worldData;
            if (this.ShowDialog() == DialogResult.OK)
            {                
                AddResult returnresult = new AddResult();
                returnresult.Result = DialogResult.OK;
                returnresult.EditedData = worldData;
                return returnresult;
            }
            else
            {
                return null;
            }
        }

        public class AddResult
        {
            public DialogResult Result { get; set; }
            public World EditedData { get; set; }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
