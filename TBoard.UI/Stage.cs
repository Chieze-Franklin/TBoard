using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBoard.UI
{
    public class Stage : IList<Spot>
    {
        List<Spot> spots = new List<Spot>();

        public Stage(string name) 
        {
            Name = name;
        }

        public int IndexOf(Spot item)
        {
            return spots.IndexOf(item);
        }

        public void Insert(int index, Spot item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public Spot this[int index]
        {
            get
            {
                return spots[index];
            }
            set
            {
                spots[index] = value;
            }
        }

        public void Add(Spot item)
        {
            spots.Add(item);
            item.Stage = this;

            if (!UseManualMatching) 
            {
                //ensure there are an even num of spots
                if (spots.Count % 2 == 0) 
                {
                    spots[spots.Count - 1].MakeTwins(spots[spots.Count - 2]);
                }
            }
        }
        public void AddRange(IEnumerable<Spot> items) 
        {
            foreach (var item in items) 
            {
                Add(item);
            }
        }

        public void Clear()
        {
            spots.Clear();
        }

        public bool Contains(Spot item)
        {
            return spots.Contains(item);
        }

        public void CopyTo(Spot[] array, int arrayIndex)
        {
            spots.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return spots.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Spot item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Spot> GetEnumerator()
        {
            return spots.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return spots.GetEnumerator();
        }

        public bool UseManualMatching { get; set; }
        public string Name { get; private set; }
    }
}