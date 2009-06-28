using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Embedding;
using MapinfoWrapper.Exceptions;
using MapinfoWrapper.Mapinfo;
using MapinfoWrapper.MapOperations;
using MapinfoWrapper.DataAccess;
using Wrapper.Example.Callback;
using Wrapper.Example.Tables;
using Wrapper.Example.Workspaces;

namespace Wrapper.Example.Forms
{
    /// <summary>
    /// A demonstration of how to do Integrated Mapping (the reparenting of a
    /// MapInfo Professional map window into other applications) using .Net.
    /// 
    /// NOTE! This example has been modifed to use the Mapinfo .NET OLE Wrapper library instead of talking to Mapinfo
    /// directly, most of the code has been replaced with the equivalate Wrapper calls.
    /// 
    /// This program also demostartes the use of LINQ-TO-Mapinfo.
    /// 
    /// Any methods that have been changed due to the wrapper have been marked with the [UsesWrapper] attribute, so
    /// they are easy to find.
    /// </summary>
	public partial class MapForm : Form
	{
		// The Map object.
        MapWindow map;

        // A referance to the table which uses the World entity object as its template,
        // which allows for strong typed access to it rows and columns.
        Table<World> worldTable;

        // A referance to the World workspace, see WorldWorkspace for more information.
        WorldWorkspace worldworkspace;
        
        // Mapping of map tool names to there action which will be invoked when run.
		private Dictionary<string, Action> _toolIdMap;

        //An instance of Mapinfo's COM object.
        private IMapinfoWrapper MapInfoApp { get; set; }

        // Constructor
        public MapForm()
        {
            InitializeComponent();

            InitializeMapToolCombobox();
        }

		// A File Open dialog that the user can use to open one or more .TAB files
		private OpenFileDialog _openFileDlg;
		private OpenFileDialog OpenDlg
		{
			get
			{
				if (_openFileDlg == null)
				{
					_openFileDlg = new OpenFileDialog();
					_openFileDlg.Filter = "MapInfo Tables (*.tab)|*.tab";
					_openFileDlg.Multiselect = true;
					_openFileDlg.RestoreDirectory = false;
					_openFileDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

				}
				return _openFileDlg;
			}
		}

		/// <summary>
		/// Opens all the tables in the list of tables and creates a new map containing the maps.
        /// This method also reperents Mapinfo's map window into the mapPanel control.
		/// </summary>
		/// <param name="tablePaths"></param>
        [UsesWrapper]
		private void NewMap(string[] tablePaths)
		{

            // Create a new list of tables to hold the tables that need to be mapped.
            List<ITable> tablelist = new List<ITable>();

			// Loop through each table and open it.
			foreach (string tablepath in tablePaths)
			{
                // Open the table
				Table table = (Table)Table.OpenTable(tablepath);

                // Add the table to the list.
                tablelist.Add(table);
			}

            // Set the next document parent to the map panel on the form.
            this.mapPanel.SetAsNextDocumentParent(NextDocumentEnum.WIN_STYLE_CHILD);

            // Map all the tables in the list.
            map = MapWindow.MapTables(tablelist);
            
			// Now that there is a map, enable the Zoom In and Zoom Out buttons
			this.buttonZoomIn.Enabled = true;
			this.buttonZoomOut.Enabled = true;
		}


		private void CloseWindow(MapWindow mapToClose)
		{
		    mapToClose.CloseWindow();
		}


		private void CloseAllTables()
		{
            //TODO: This function hasn't been wrapped up yet.
			this.MapInfoApp.RunCommand("Close All");
		}


		private void Form1_Load(object sender, EventArgs e)
		{
			InitializeComObject();

			AddMapperShortcutMenuitem();
		}


		private void Form1_ResizeEnd(object sender, EventArgs e)
		{
			// Make sure that we have a map to work with.
			if (map != null)
			{
				// Update the map to match the current size of the panel. 
				MoveWindow(map.Hwnd, 0, 0, this.mapPanel.Width, this.mapPanel.Height, false);
			}
		}

		[DllImport("user32.dll")]
		static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);


        [UsesWrapper]
		private void InitializeComObject()
		{
            // Create the MapInfo Professional object, this will create an instance of
            // Mapinfo and return a the instance as wrapped in a IMapinfoWrapper which provides basic
            // Mapinfo OLE commnds like Do and Eval.
            this.MapInfoApp = COMMapinfo.CreateInstance();

            // Set the parent window for Mapinfo Professional dialogs to this form.
            this.SetAsMapinfoApplicationWindow();

            // Create a new callback object.
            CustomCallback callback = new CustomCallback();

            // Sink the events to the form.
            callback.OnStatusChanged += new Action<string>(callback_OnStatusChanged);
            callback.OnMenuItemClick += new Action<string>(callback_OnMenuItemClick);

            // Register the callback with Mapinfo.
            // At the moment we have to cast the IMapinfoWrapper object to COMMapinfo to set the
            // callback object.  This will change in the future.
            ((COMMapinfo)this.MapInfoApp).Callback = callback;
		}

        void callback_OnMenuItemClick(string obj)
        {
            MessageBox.Show(this, "A menu item was clicked");
        }

        // The method called when the MapInfo Professional status bar text changes. 
        // This can happen due to changes in the map view (zoom level) or selection, 
        // or can happen because the user highlights an item on the map's context menu.
        void callback_OnStatusChanged(string command)
        {
            toolStripStatusLabel1.Text = command.Replace("\t", "        ");
        }

        public void AddToComboboxAndDictinonary(string command, int id)
        {
            this.comboBoxMapTool.Items.Add(command);
            _toolIdMap.Add(command, () => this.InvokeButton(id));
        }

		// Set up the combo box that lets the user choose a map tool 
		private void InitializeMapToolCombobox()
		{
			// Create the dictionary collection
			_toolIdMap = new Dictionary<string, Action>();

            // Add "Select" tool to combobox and dictionary
            this.AddToComboboxAndDictinonary("Select", 1701);
            // Add "Pan" tool to combobox and dictionary
            this.AddToComboboxAndDictinonary("Pan", 1702);
            // Add "Zoom In" tool to combobox and dictionary
            this.AddToComboboxAndDictinonary("Zoom In", 1705);
            // Add "Zoom Out" tool to combobox and dictionary
            this.AddToComboboxAndDictinonary("Zoom Out", 1706);

			// Set the combobox item to Select
			comboBoxMapTool.SelectedIndex = 0;
		}

		// Add a custom item to the Map window's context menu 
		private void AddMapperShortcutMenuitem()
		{
			// Issue Alter Menu command, adding an OLE menuitem. 
			// When the user chooses a custom OLE menuitem from the context menu,
			// MapInfo Professional calls MapInfoCallback.MenuItemHandler,
			// which in turn calls the OnMenuItemClick item below. 
            string cmd = @"Alter Menu ""MapperShortcut"" 
                                  Add ""Custom Item"" 
                                      ID {0} 
                                      Calling OLE {1}".FormatWith(10000, "MenuItemHandler".InQuotes());
			this.MapInfoApp.RunCommand(cmd);
		}


		// Display the File Open dialog to let the user choose table(s) to open.
		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Prompt the user to open one or more TAB files
			if (OpenDlg.ShowDialog(this) == DialogResult.OK)
			{
				// Close window and tables, if they exist
				if (map != null)
				{
				    map.CloseWindow();
					CloseAllTables();
                    map = null;
				}
				// Create a new map
				NewMap(OpenDlg.FileNames);
				// Enable the tool picker 
				comboBoxMapTool.Enabled = true;
			}
		}


		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Close form to end the application
			this.Close();
		}


		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
            About aboutform = new About();
            aboutform.ShowDialog(this);
		}

        public void InvokeButton(int buttonID)
        {
            this.MapInfoApp.RunCommand("Run Menu Command " + buttonID);
        }

		// Handle the selection of a different tool from the combo box. 
		private void comboBoxMapTool_SelectionChangeCommitted(object sender, EventArgs e)
		{
			// Get the current combobox selection
			string selectedText = comboBoxMapTool.SelectedItem.ToString();
			// Invoke the method assigned to the menu.
            _toolIdMap[selectedText].Invoke();			
		}


		public void ShowMessage(string msg)
		{
			MessageBox.Show(this, msg);
		}

		private void buttonZoomIn_Click(object sender, EventArgs e)
		{
			zoomMap(0.5);   // zoom in (show an area half as wide)
		}

		private void buttonZoomOut_Click(object sender, EventArgs e)
		{
			zoomMap(2.0);  // zoom out (show an area twice as wide)
		}

		private void zoomMap(double zoomFactor)
		{
            // Make sure that we have a map to do the zoom work on.
			if (map != null)
			{
				// Call:  MapperInfo(id, MAPPER_INFO_DISTUNITS) 
				// to get a units string such as "mi" or "km"
				string strUnit = MapInfoApp.Evaluate("MapperInfo( " + map.WindowId + " , 12)");

				// Call:  MapperInfo(id, MAPPER_INFO_ZOOM) 
                double dZoom = Double.Parse(MapInfoApp.Evaluate("MapperInfo( " + map.WindowId + " , 1)"));

				dZoom *= zoomFactor;
				dZoom = Math.Min(dZoom, 10000000);
				dZoom = Math.Max(dZoom, 0.0001);
				// Apply the new zoom level with a statement of this form: 
				//     Set Map Window 123456 Zoom 123.456  Units "mi"  
				string cmd = string.Format(@"Set Map Window {0} Zoom {1} Units ""{2}""", map.WindowId, dZoom, strUnit);
				this.MapInfoApp.RunCommand(cmd);
			}

		}

        [UsesWrapper]
        private void Maptable(ITable table)
        {
            if (map != null)
            {
                // Close the exsiting map window.
                this.CloseWindow(map);
            }

            this.mapPanel.SetAsNextDocumentParent(NextDocumentEnum.WIN_STYLE_CHILD);

            map = MapWindow.MapTable(table);

            MapInformation.SelectedObject = map;

            this.comboBoxMapTool.Enabled = true;
            this.comboBox1.Enabled = true;
            this.buttonZoomIn.Enabled = true;
            this.buttonZoomOut.Enabled = true;  
        }

        /// <summary>
        /// Opens the world table and maps it to the current window.
        /// </summary>
        [UsesWrapper]
        public void OpenWorldTable()
        {

            // Open the world table in Mapinfo and use the World entity as the tempate to get strong typed access.
            worldTable = (Table<World>)Table.OpenTable<World>(Application.StartupPath + @"\Maps\WORLD.TAB");
            
            this.TabInfoGrid.SelectedObject = worldTable;
            
            this.Maptable(worldTable);
        }

        [UsesWrapper]
        private void OpenWorldWorkSpace()
        {
            if (map != null)
            {
                map.CloseWindow();
                this.CloseAllTables();
            }     

            this.mapPanel.SetAsNextDocumentParent(NextDocumentEnum.WIN_STYLE_CHILD);

            // Open the workspace that contains the world table.
            worldworkspace = WorldWorkspace.Open();

            this.TabInfoGrid.SelectedObject = worldworkspace.WorldTable;

            // Get the front map window.
            map = MapWindow.GetFrontWindow();

            this.comboBoxMapTool.Enabled = true;
            this.comboBox1.Enabled = true;
            this.buttonZoomIn.Enabled = true;
            this.buttonZoomOut.Enabled = true;  
        }


        private void openWorldTableLINQExampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenWorldTable();           
        }

        [UsesWrapper]
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IQueryable<World> query = null;

            Table<World> table;

            if (worldTable == null)
                table = worldworkspace.WorldTable;
            else
                table = worldTable;


            switch (comboBox1.Text)
            {
                case "Select all where Continent is equal to North America":
                    query = this.GetContinentsForNorthAmerica(table);
                    break;
                case "Select all where Pop_1994 is greater then 4,000,000":
                    query = this.GetGreaterThenFilter(table, 4000000);
                    break;
            }

            QueryTextbox.Clear();
            if (query != null) QueryTextbox.Text = query.ToQueryString();
        }

        #region LINQ-TO-MAPINFO example

        [UsesWrapper]
        IQueryable<World> GetGreaterThenFilter(Table<World> table, int popFilter)
        {
            // Use a linq query that selects all the records from the table where 
            // the pop for 1994 is greater then the popFilter million.
            // 
            // This will build a SQL string then run it in Mapinfo.
            // NOTE! LINQ-To-Mapinfo is only in a very early stage, so a lot of operations are not supported yet.
            var query = from row in table
                        where row.Pop_1994 > popFilter
                        select row;

            return query;
        }

        [UsesWrapper]
        IQueryable<World> GetContinentsForNorthAmerica(Table<World> table)
        {
            // Use a linq query that selects all the records from the table where 
            // the continent is equal to North America.
            // 
            // This will build a SQL string then run it in Mapinfo.
            // NOTE! LINQ-To-Mapinfo is only in a very early stage, so a lot of operations are not supported yet.
            var query = from row in table
                        where row.Continent == "North America"
                        select row;

            return query;
        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
            this.OpenWorldTable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.OpenWorldWorkSpace();
        }

        private void RunQueryButton_Click(object sender, EventArgs e)
        {
            Table<World> table;

            if (worldTable == null)
                table = worldworkspace.WorldTable;
            else
                table = worldTable;

            switch (comboBox1.Text)
            {
                case "Select all where Continent is equal to North America":
                    IQueryable<World> query = this.GetContinentsForNorthAmerica(table);
                    
                    // The query won't get run until we try and move through the records, as we want
                    // the data right now we cast the query to a list so that it fires the query in Mapinfo
                    // and returns the results.
                    //
                    // NOTE! It would not be advisable to do this on a large dataset as it has to loop
                    // every row in the table, but because the Data grid view has to take a list we have
                    // no other option ATM.
                    List<World> results = query.ToList();
                    dataGridView1.DataSource = results;
                    break;
                case "Select all where Pop_1994 is greater then 4,000,000":
                    dataGridView1.DataSource = this.GetGreaterThenFilter(table, 4000000).ToList();
                    break;
                case "Select Name, Pop_1994, obj from world.":
                    dataGridView1.DataSource = this.GetNamePopAndObject(table);
                    break;
                default:
                    break;
            }
        }

        private object GetNamePopAndObject(Table<World> table)
        {
            // This query will select the Country, Pop_1994 and obj column in
            // Mapinfo, it will also take the data from the column and map it to
            // a property in a new type.  
            //
            // In the case of row.obj.ObjectType, it will call the
            // object type property on the underlying object and store the result in ObjectType property
            // in the new Type.
            var query = from row in table
                        select new
                                   {
                                       Name = row.Country, 
                                       Pop = row.Pop_1994, 
                                       ObjectType = row.obj.ObjectType
                                   };
            
            // Cast the result query to a list, so that we can have the data now.
            return query.ToList();
        }

        /// <summary>
        /// Creates a new World entity, opens an edit form and lets the user enter data.
        /// If ok is selected then the new data is inserted into the Mapinfo table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewButton_Click(object sender, EventArgs e)
        {
            // Create a new World entity, when you create a new entity it will
            // have a rowid of 0 which means the entity is not inserted into a table.
            World newData = new World();
            
            //Show the edit form and pass in the new world object.
            NewDataForm form = new NewDataForm();
            NewDataForm.AddResult result = form.Show(newData);

            if (result != null)
            {
                try
                {
                    // Insert the new record into the world table.
                    worldTable.InsertRow(result.EditedData);
                }
                catch (MapinfoException mapinfoex)
                {
                    if (mapinfoex.MapinfoErrorCode == 1448)
                    {
                        ShowMessage(
                            @"Sorry something else is already editing this table.
                                      Check to see if crashed Mapinfo ar running in the background.");
                        return;
                    }
                    throw;
                }
            }
        }
	}
}