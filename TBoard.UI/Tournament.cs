using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBoard.UI
{
    public class Tournament : IList<Stage>
    {
        List<Stage> stages = new List<Stage>();

        public int IndexOf(Stage item)
        {
            return stages.IndexOf(item);
        }

        public void Insert(int index, Stage item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public Stage this[int index]
        {
            get
            {
                return stages[index];
            }
            set
            {
                stages[index] = value;
            }
        }

        public void Add(Stage item)
        {
            stages.Add(item);


            /*
             * if the tournament doesnt use manual matching
             *      if the tournament has more than 1 stage after the addition of this stage
             *          if the previous stage and this stage dont use manual matching
             *              and the previous stage has an even number of spots
             *                  and this stage has a number of spots equal to half d number of spots in the previous stage
             *                      ...
             */
            if (!UseManualMatching) 
            {
                if (stages.Count > 1) 
                {
                    Stage currStage = stages[stages.Count - 1];
                    Stage prevStage = stages[stages.Count - 2];
                    if (currStage.UseManualMatching == false && prevStage.UseManualMatching == false)
                    {
                        if (prevStage.Count % 2 == 0 && currStage.Count == prevStage.Count / 2)
                        {
                            /*
                             * loop thru previous stage, setting the 'Next' of each pair of spots to appropriate spots in current stage
                             */
                            int currIndex = 0;
                            for (int prevIndex = 0; prevIndex < prevStage.Count; prevIndex += 2) 
                            {
                                prevStage[prevIndex].SetNext(currStage[currIndex]);
                                prevStage[prevIndex].Twin.SetNext(currStage[currIndex]);
                                currIndex++;
                            }
                        } 
                    }
                }
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Stage item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Stage[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(Stage item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Stage> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        public bool UseManualMatching { get; set; }
    }
}
