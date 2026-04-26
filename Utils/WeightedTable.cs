using Godot;
using Godot.Collections;

namespace Game.Utils;

public partial class WeightedTable<T> : RefCounted
{

    private partial class TableItem : Resource
    {
        public T Item;
        public int Weight;
    }

    private Array<TableItem> _items = [];
    private int _totalWeight;

    public void AddItem(T item, int weight)
    {
        _totalWeight += weight;
        _items.Add(new TableItem
        {
            Item = item,
            Weight = weight
        });
    }

    public T PickItem()
    {
        T result = _items[0].Item;
        var chosen_weight = GD.Randi() % _totalWeight;
        var iteration_sum = 0;

        foreach (var item in _items)
        {
            iteration_sum += item.Weight;
            if (chosen_weight <= iteration_sum)
            {
                result = item.Item;
                break;
            }
        }

        return result;
    }

}
