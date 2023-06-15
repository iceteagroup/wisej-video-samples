using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Wisej.Web;

namespace WisejLib
{
    /// <summary>
    /// The GridExtender class adds a few extension methods that help dealing with grid data and column layouts
    /// </summary>
    public static class GridExtender
    {
        /// <summary>
        /// Setting DataGridView properties to frequntly used values, at least for my needs
        /// </summary>
        public static void SetDefaults(this DataGridView grid, bool multiSelect = true, bool editable = false)
        {
            if (grid is null)
                throw new ArgumentNullException(nameof(grid));

            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToOrderColumns = true;
            grid.KeepSameRowHeight = true;
            grid.MultiSelect = multiSelect;
            //grid.NoDataMessage = "Es sind keine Daten vorhanden";
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowHeadersWidth = 16;
            grid.SelectionMode = editable ? DataGridViewSelectionMode.CellSelect : DataGridViewSelectionMode.FullRowSelect;
            grid.ShowFocusCell = editable;
        }

        /// <summary>
        /// Returns the number of selected rows. This also works when the grid's selection 
        /// mode is set to CellSelect in which case the method loops over all rows to
        /// check IsSelected. A row is considere selected then, if at least 1 cell is selected
        /// </summary>
        public static int SelectedCount(this DataGridView grid)
        {
            if (grid is null)
                throw new ArgumentNullException(nameof(grid));

            var count = 0;
            if (grid.SelectionMode == DataGridViewSelectionMode.CellSelect)
            {
                foreach (var row in grid.Rows)
                    if (row.IsSelected())
                        count++;
            }
            else
                count = grid.SelectedRows.Count;

            return count;
        }

        /// <summary>
        /// Finds a column by DataPropertyName
        /// </summary>
        public static DataGridViewColumn FindColumn(this DataGridView grid, string dataPropertyName)
        {
            if (grid is null)
                throw new ArgumentNullException(nameof(grid));

            foreach (DataGridViewColumn column in grid.Columns.Cast<DataGridViewColumn>())
                if (string.Equals(column.DataPropertyName, dataPropertyName, StringComparison.OrdinalIgnoreCase))
                    return column;

            return null;
        }

        /// <summary>
        /// Returns the number of selected rows or counts the number of 
        /// rows that have at least 1 cell selected
        /// </summary>
        public static bool IsSelected(this DataGridViewRow row)
        {
            if (row.Selected)
                return true;
            foreach (DataGridViewCell cell in row.Cells.Cast<DataGridViewCell>())
                if (cell.Selected)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns the index of the selected row in single-select mode
        /// </summary>
        public static int GetSelectedIndex(this DataGridView grid)
        {
            CheckSingleSelection(grid);

            return grid.SelectedRows[0].Index;
        }

        /// <summary>
        /// Returns a list of indexes of the selected rows in multi-select mode
        /// </summary>
        public static List<int> CollectSelectedIndexes(this DataGridView grid)
        {
            if (grid is null)
                throw new ArgumentNullException(nameof(grid));

            List<int> list = new List<int>();
            foreach (DataGridViewRow row in grid.Rows)
                if (row.IsSelected())
                    list.Add(row.Index);
            return list;
        }

        /// <summary>
        /// For a grid this method retrieves the cell value of a column in the 
        /// selected row. The grid must have exactly 1 row selected
        /// </summary>
        /// <typeparam name="T">The type of the retrieved value</typeparam>
        public static T GetSelectedValue<T>(this DataGridView grid, int columnIndex)
        {
            CheckSingleSelection(grid);

            return (T)grid.SelectedRows[0].Cells[columnIndex].Value;
        }

        /// <summary>
        /// Retrieves the cell value of a column in the selected row. The grid 
        /// must have exactly 1 row selected
        /// </summary>
        /// <typeparam name="T">The type of the retrieved value</typeparam>
        public static T GetSelectedValue<T>(this DataGridViewColumn column)
        {
            DataGridView grid = column.DataGridView;
            return GetSelectedValue<T>(grid, column.Index);
        }

        /// <summary>
        /// For a grid this method retrieves all cell values of a column in the selected rows
        /// </summary>
        /// <typeparam name="T">The type of the retrieved value</typeparam>
        public static List<T> CollectSelectedValues<T>(this DataGridView grid, int columnIndex)
        {
            if (grid is null)
                throw new ArgumentNullException(nameof(grid));

            List<T> values = new List<T>();
            foreach (DataGridViewRow row in grid.Rows)
                if (row.IsSelected())
                    values.Add((T)row.Cells[columnIndex].Value);

            return values;
        }

        /// <summary>
        /// For a column this method retrieves all cell values in the selected rows
        /// </summary>
        /// <typeparam name="T">The type of the retrieved value</typeparam>
        public static List<T> CollectSelectedValues<T>(this DataGridViewColumn column)
        {
            DataGridView grid = column.DataGridView;
            return CollectSelectedValues<T>(grid, column.Index);
        }

        /// <summary>
        /// For a grid this method retrieves all cell values of a column
        /// </summary>
        /// <typeparam name="T">The type of the retrieved value</typeparam>
        public static List<T> CollectValues<T>(this DataGridView grid, int columnIndex)
        {
            if (grid is null)
                throw new ArgumentNullException(nameof(grid));

            List<T> values = new List<T>();
            foreach (DataGridViewRow row in grid.Rows)
                values.Add((T)row.Cells[columnIndex].Value);

            return values;
        }

        /// <summary>
        /// For a column this method retrieves all cell values
        /// </summary>
        /// <typeparam name="T">The type of the retrieved value</typeparam>
        public static List<T> CollectValues<T>(this DataGridViewColumn column)
        {
            DataGridView grid = column.DataGridView;
            return CollectValues<T>(grid, column.Index);
        }

        /// <summary>
        /// Checks if a grid has exactly 1 row selected and throws an exception if not
        /// </summary>
        private static void CheckSingleSelection(DataGridView grid)
        {
            if (grid == null)
                throw new Exception($"Parameter {nameof(grid)} cannot be null");
            if (grid.SelectedRows.Count != 1)
                throw new Exception("Unexpected Error|The command is only allowed if there is exactly 1 row selected");
        }

        /// <summary>
        /// Calls GetLayout to get a list of layout objects and serializes them to json
        /// </summary>
        /// <param name="grid">Read columns from this grid</param>
        /// <returns>The serialized layout</returns>
        public static string GetLayoutString(this DataGridView grid)
        {
            var layout = grid.GetLayout();
            return JsonConvert.SerializeObject(layout);
        }

        /// <summary>
        /// Loops over all grid columns and collects certain column properties which 
        /// can be restored later by using ApplyLayout
        /// </summary>
        /// <param name="grid">Read columns from this grid</param>
        /// <returns>A list of GridLayoutItem that can be passed to ApplyLayout</returns>
        public static List<GridLayoutItem> GetLayout(this DataGridView grid)
        {
            if (grid is null)
                throw new ArgumentNullException(nameof(grid));

            var items = new List<GridLayoutItem>();

            foreach (DataGridViewColumn column in grid.Columns.Cast<DataGridViewColumn>())
                items.Add(new GridLayoutItem(column));

            return items;
        }

        /// <summary>
        /// Takes a serialized list of GridLayoutItems and applies their values to the according column
        /// </summary>
        /// <param name="grid">The grid whose columns are to be restored</param>
        /// <param name="layout">A serialized layout</param>
        public static void ApplyLayout(this DataGridView grid, string layout)
        {
            var items = JsonConvert.DeserializeObject<List<GridLayoutItem>>(layout);
            grid.ApplyLayout(items);
        }

        /// <summary>
        /// Takes a list of GridLayoutItems and applies their values to the according column
        /// </summary>
        /// <param name="grid">The grid whose columns are to be restored</param>
        /// <param name="items">The list of GridLayoutItems that have been retrieved by GetLayout()</param>
        public static void ApplyLayout(this DataGridView grid, List<GridLayoutItem> items)
        {
            if (grid is null)
                throw new ArgumentNullException(nameof(grid));

            foreach (var item in items)
            {
                DataGridViewColumn column = grid.Columns.SingleOrDefault(x => x.Name == item.Name);
                item.ApplyToColumn(column);
            }
        }

    }

    /// <summary>
    /// The GridLayoutItem holds column properties. It is used in GetLayout() and ApplyLayout()
    /// </summary>
    public class GridLayoutItem
    {
        /// <summary>
        /// NewtonSoft.Json.JsonConvert expects a parameterless constructor
        /// </summary>
        public GridLayoutItem() { }

        /// <summary>
        /// Creates a GridLayoutItem by inspecting the passed column
        /// </summary>
        public GridLayoutItem(DataGridViewColumn column)
        {
            Name = column.Name;
            Header = column.HeaderText;
            DisplayIndex = column.DisplayIndex;
            Width = column.Width;
            Visible = column.Visible;
            ShowInVisibilityMenu = column.ShowInVisibilityMenu;
        }

        /// <summary>
        /// The name of the column
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  The column header
        /// </summary>
        public string Header { get; set; }
        /// <summary>
        /// The display index
        /// </summary>
        public int DisplayIndex { get; set; }
        /// <summary>
        /// THe current width of the column
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// True if the column is visible, false if hidden
        /// </summary>
        public bool Visible { get; set; }
        /// <summary>
        /// True if the column is listed in the visibility menu of the grid
        /// </summary>
        public bool ShowInVisibilityMenu { get; set; }

        /// <summary>
        /// Applies the layout values to the passed column
        /// </summary>
        /// <param name="column">The column where the values are applied to</param>
        public void ApplyToColumn(DataGridViewColumn column)
        {
            if (column is null)
                return;
            column.HeaderText = Header;
            column.DisplayIndex = DisplayIndex;
            column.Width = Width;
            column.Visible = Visible;
            column.ShowInVisibilityMenu = ShowInVisibilityMenu;
        }
    }
}
