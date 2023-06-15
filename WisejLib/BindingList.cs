using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Wisej.Web;

namespace WisejLib
{
    /// <summary>
    /// A Helper class that makes life easier with BindingSources. It contains methods for adding and deleteing DbEntity objects.
    /// Deleted items go into the DeletedItems list.
    /// </summary>
    /// <typeparam name="T">The type that this BindingList contains</typeparam>
    public class BindingList<T> where T : DbEntity
    {
        /// <summary>
        /// Empty constructor, you will have to set DataItems yourself
        /// </summary>
        public BindingList()
        {
            // make sure that property DataItems is not null by default
            BindingSource.DataSource = new List<T>();
        }

        /// <summary>
        /// Create a new BindingList object and sets BindingList.BindingSource.DataSource to 
        /// the passed list of items
        /// </summary>
        /// <param name="dataItems">The list to assign to BindingList.BindingSource.DataSource</param>
        public BindingList(List<T> dataItems)
        {
            BindingSource.DataSource = dataItems;
        }

        /// <summary>
        /// This property holds the data items. For assigning the DataSource you can use the DataItems property
        /// </summary>
        public BindingSource BindingSource { get; } = new BindingSource();

        /// <summary>
        /// Publishes BindingSource.DataSource as a typed List
        /// </summary>
        public List<T> DataItems
        {
            get => BindingSource.DataSource as List<T>;
            set => BindingSource.DataSource = value;
        }

        /// <summary>
        /// Returns item at index
        /// </summary>
        /// <param name="index">The index of the requested item</param>
        /// <returns>An item of class T from the specified index</returns>
        public T this[int index] => DataItems[index];

        /// <summary>
        /// This property holds the items that have been deleted. If a new item was 
        /// deleted, it is assumed that it was never saved so it doesn't go into the 
        /// DeletedItems list
        /// </summary>
        public List<T> DeletedItems { get; } = new List<T>();

        /// <summary>
        /// Declaration of an event handler that is fired whenever items in the BindingList change
        /// </summary>
        /// <param name="item">The changed item. Cou can inspect item.State to know what happend</param>
        public delegate void BindingListChangedEventHandler(T item);

        /// <summary>
        /// Event that is fired whenever items in the BindingList change
        /// </summary>
        public event BindingListChangedEventHandler BindingListChanged;

        /// <summary>
        /// Adds a new item to the BindingSource
        /// </summary>
        /// <param name="item">The item to add. You have to take care of the State yourself</param>
        public void Add(T item)
        {
            // make sure that a similar item doesn't exist in the DeleteItems list
            item.State = DbState.New;
            BindingSource.Add(item);
            BindingListChanged?.Invoke(item);
        }

        /// <summary>
        /// Removes an item from the BindingSource. If the item.State is not DbState.New the item is put into the DeletedItems list
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void Remove(T item)
        {
            if (item.State != DbState.Deleted)
            {
                item.State = DbState.Deleted;
                DeletedItems.Add(item);
            }
            BindingSource.Remove(item);
            BindingListChanged?.Invoke(item);
        }

        /// <summary>
        /// Removes an item from the BindingSource. If the item.State is not DbState.New the 
        /// item is put into the DeletedItems list
        /// </summary>
        /// <param name="index">The index of the item to remove</param>
        public void Remove(int index)
        {
            Remove(BindingSource[index] as T);
        }

        /// <summary>
        /// Removes an item from the BindingSource. If the item.State is not DbState.New the 
        /// item is put into the DeletedItems list
        /// </summary>
        /// <param name="index">The index of the item to remove</param>
        public void Remove(Func<T, bool> removeIt)
        {
            for (int i = DataItems.Count - 1; i >= 0; i--)
            {
                var item = DataItems[i];
                if (item.State != DbState.Deleted && removeIt(item))
                    Remove(item);
            }
        }

        /// <summary>
        /// Tells the BindingSource that an item has changed. item.State is set to 
        /// DbState.Changed if it wasn't DbState.New before.
        /// </summary>
        /// <param name="item">The item that was changed</param>
        public void Update(T item)
        {
            if (item.State != DbState.New)
                item.State = DbState.Changed;
            var index = BindingSource.IndexOf(item);
            BindingSource.ResetItem(index);
            BindingListChanged?.Invoke(item);
        }

        /// <summary>
        /// Tells the BindingSource that an item has changed. item.State is set to 
        /// DbState.Changed if it wasn't DbState.New before.
        /// </summary>
        /// <param name="index">The index of the item that was changed</param>
        public void Update(int index)
        {
            Update(BindingSource[index] as T);
        }

        /// <summary>
        /// Replace an item by a different one. The old item does not go into the DeletedItems list.
        /// </summary>
        /// <param name="existingItem">The item to replace</param>
        /// <param name="newItem">The new item</param>
        public void Replace(T existingItem, T newItem)
        {
            var index = BindingSource.IndexOf(existingItem);
            BindingSource[index] = newItem;
            BindingSource.ResetItem(index);
            BindingListChanged?.Invoke(newItem);
        }
        /// <summary>
        /// Replace an item by a different one. The old item does not go into the DeletedItems list.
        /// </summary>
        /// <param name="indexOfExistingItem">The index of the item to replace</param>
        /// <param name="newItem">The new item</param>
        public void Replace(int indexOfExistingItem, T newItem)
        {
            var existingItem = BindingSource[indexOfExistingItem];
            var index = BindingSource.IndexOf(existingItem);
            BindingSource[index] = newItem;
            BindingSource.ResetItem(index);
            BindingListChanged?.Invoke(newItem);
        }

        /// <summary>
        /// Save changes to the database.
        /// First the items from the DletedItems list are deleted, the all items whose 
        /// state is not DbState.None will be inserted or updated depending on their 
        /// State. When the method finishes the DeletedItems list will be empty and all
        /// items in the DataItems list will have a State of DbState.None
        /// </summary>
        /// <param name="tx">(required) A transaction object</param>
        /// <param name="beforeSave">(optional) this is invloked for each item before it is saved to the database.
        /// In beforeSave you can for example set foreign key values or modify the content of the item otherwise</param>
        public void SaveChanges(IDbTransaction tx, Action<T> beforeSave)
        {
            // deletion must come first to avoid conflicts with added or updated items
            if (DeletedItems != null && DeletedItems.Any())
            {
                foreach (var item in DeletedItems)
                    item.SaveChanges(tx);
                DeletedItems.Clear();
            }

            // now let's add the new and update the changed items
            if (DataItems != null && DataItems.Any())
            {
                foreach (var item in DataItems)
                    if (item.State != DbState.None)
                    {
                        beforeSave?.Invoke(item);
                        item.SaveChanges(tx);
                    }
            }
        }
    }
}
