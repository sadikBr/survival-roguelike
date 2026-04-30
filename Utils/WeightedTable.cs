using System.Collections.Generic;
using System.Linq;

using Godot;

namespace Game.Utils;

public partial class WeightedTable<T>
{

    private class TableItem
    {
        public T Item;
        public int Weight;
    }

    private readonly List<TableItem> _items = [];
    private int _totalWeight;

    public int Count => _items.Count;

    public void AddItem(T item, int weight)
    {
        _totalWeight += weight;
        _items.Add(new TableItem
        {
            Item = item,
            Weight = weight
        });
    }

    public T[] PickItems(int count)
    {
        List<T> result = [];

        while (result.Count < count)
        {
            var item = PickItem();
            if (!result.Contains(item))
            {
                result.Add(item);
            }
        }

        return [.. result];
    }

    public T PickItem()
    {
        T result = _items[0].Item;
        var chosen_weight = GD.Randi() % _totalWeight;
        var iteration_sum = 0;

        foreach (var item in _items)
        {
            iteration_sum += item.Weight;
            if (chosen_weight < iteration_sum)
            {
                result = item.Item;
                break;
            }
        }

        return result;
    }

    public void RemoveItem(T item)
    {
        var itemToRemove = _items.FirstOrDefault(i => i.Item.Equals(item));
        if (itemToRemove == null) return;

        _totalWeight -= itemToRemove.Weight;
        _items.Remove(itemToRemove);
    }

    public T[] ToArray()
    {
        return [.. _items.Select(i => i.Item)];
    }

}
