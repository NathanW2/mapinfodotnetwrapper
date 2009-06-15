using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Wrapper.Embedding;
using Wrapper.Example.Callback;
using Wrapper.Example.Tables;
using Wrapper.Example.Workspaces;
using Wrapper.Extensions;
using Wrapper.MapinfoFactory;
using Wrapper.MapOperations;
using Wrapper.TableOperations;

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
        private COMMapinfo MapInfoApp { get; set; }

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
            List<Table> tablelist = new List<Table>();

			// Loop through each table and open it.
			foreach (string tablepath in tablePaths)
			{
                // Open the table
				Table table = Table.OpenTable(this.MapInfoApp,tablepath);

                // Add the table to the list.
                tablelist.Add(table);
			}

            // Set the next document parent to the map panel on the form.
            this.mapPanel.SetAsNextDocumentParent(this.MapInfoApp, NextDocumentEnum.WIN_STYLE_CHILD);

            // Map all the tables in the list.
            map = MapWindow.MapTables(this.MapInfoApp,tablelist);
            
			// Now that there is a map, enable the Zoom In and Zoom Out buttons
			this.buttonZoomIn.Enabled = true;
			this.buttonZoomOut.Enabled = true;
		}


		private void CloseWindow(MapWindow map)
		{
            //TODO: This function hasn't been wrapped up yet.
            this.MapInfoApp.RunCommand("Close window {0}".FormatWith(map.WindowId));
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
            this.MapInfoApp = Wrapper.MapinfoFactory.COMMapinfo.CreateInstance();

            // Set the parent window for Mapinfo Professional dialogs to this form.
            this.SetAsMapinfoApplicationWindow(this.MapInfoApp);

            // Create a new callback object.
            CustomCallback callback = new CustomCallback();

            // Sink the events to the form.
            callback.OnStatusChanged += new Action<string>(callback_OnStatusChanged);
            callback.OnMenuItemClick += new Action<string>(callback_OnMenuItemClick);

            // Register the callback with Mapinfo.
            this.MapInfoApp.Callback = callback;
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
            this.AddToComboboxAndDictinonary(Properties.Resources.MapTool_Select, 1701);
            // Add "Pan" tool to combobox and dictionary
            this.AddToComboboxAndDictinonary(Properties.Resources.MapTool_Pan, 1702);
            // Add "Zoom In" tool to combobox and dictionary
            this.AddToComboboxAndDictinonary(Properties.Resources.MapTool_ZoomIn, 1705);
            // Add "Zoom Out" tool to combobox and dictionary
            this.AddToComboboxAndDictinonary(Properties.Resources.MapTool_ZoomOut, 1706);

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
					CloseWindow(map);
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

            this.mapPanel.SetAsNextDocumentParent(this.MapInfoApp, NextDocumentEnum.WIN_STYLE_CHILD);

            map = MapWindow.MapTable(this.MapInfoApp, table);

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
            worldTable = Table.OpenTable<World>(this.MapInfoApp, Application.StartupPath + @"\Maps\WORLD.TAB");
            
            this.Maptable(worldTable);
        }

        [UsesWrapper]
        private void OpenWorldWorkSpace()
        {
            this.mapPanel.SetAsNextDocumentParent(this.MapInfoApp, NextDocumentEnum.WIN_STYLE_CHILD);

            // Open the workspace that contains the world table.
            worldworkspace = WorldWorkspace.Open(this.MapInfoApp);

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
            Table<World> table;

            if (worldTable == null)
                table = worldworkspace.WorldTable;
            else
                table = worldTable;

            switch (comboBox1.Text)
            {
                case "Select all where Continent is equal to North America":
                    dataGridView1.DataSource = this.GetContinentsForNorthAmerica(table);
                    break;
                case "Select all where Pop_1994 is greater then 4,000,000":
                    dataGridView1.DataSource = this.GetGreaterThen4Million(table);
                    break;
                default:
                    break;
            }
        }

        #region LINQ-TO-MAPINFO example

        [UsesWrapper]
        IEnumerable<World> GetGreaterThen4Million(Table<World> table)
        {
            // Use a linq query that selects all the records from the table where 
            // the pop for 1994 is greater then 4 million.
            // 
            // This will build a SQL string then run it in Mapinfo.
            // NOTE! LINQ-To-Mapinfo is only in a very early stage, so a lot of operations are not supported yet.
            var query = from row in table
                        where row.Pop_1994 > 4000000
                        select row;

            // Return the result as a list so that the query only gets run once.
            return query.ToList();
        }

        [UsesWrapper]
        IEnumerable<World> GetContinentsForNorthAmerica(Table<World> table)
        {
            // Use a linq query that selects all the records from the table where 
            // the continent is equal to name.
            // 
            // This will build a SQL string then run it in Mapinfo.
            // NOTE! LINQ-To-Mapinfo is only in a very early stage, so a lot of operations are not supported yet.
            var query = from row in table
                        where row.Continent == "North America"
                        select row;

            // Return the result as a list so that the query only gets run once.
            return query.ToList();
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
	}
}