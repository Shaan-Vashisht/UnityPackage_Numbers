using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SV.Numbers
{
    /// Stateful probability that reduces the chances of lucky/unlucky streaks
    public class MarbleBag<T>
    {
        [SerializeField]
        private Dictionary<T, int> bagSetup = new Dictionary<T, int>();
        
        private List<T> marbles = new List<T>();

        /// Defines the bag's setup and resets it.
        /// <param name="setup">The bag's setup, where the keys are the different types of marbles and the values are the amount of each type.</param>
        public void Init(Dictionary<T, int> setup)
        {
            bagSetup.Clear();
            foreach (KeyValuePair<T, int> marble in setup) 
                bagSetup.Add(marble.Key, marble.Value);
            
            Reset();
        }
        /// Defines the bag's setup and resets it.
        /// <param name="totalMarbles">The total number of marbles in the bag.</param>
        /// <param name="distribution">The distribution of the different marbles where the coefficients are [0, 1] and the total sum is 1.</param>
        public void Init(int totalMarbles, (T, float)[] distribution)
        {
            bagSetup.Clear();
            foreach ((T, float) marbleDist in distribution)
            {
                int tot = Mathf.RoundToInt(totalMarbles * marbleDist.Item2);
                bagSetup.Add(marbleDist.Item1, tot);
            }
            
            Reset();
        }
        
        /// Pulls a random marble out of the bag.
        public T DrawOne()
        {
            if (marbles.Count == 0)
                Reset();

            int i = Random.Range(0, marbles.Count);
            T res = marbles[i];
            marbles.RemoveAt(i);
            return res;
        }

        /// Refills the bag so that it matches the initial setup.
        /// <exception cref="Exception">If the bag has an empty setup (no positive entries).</exception>
        public void Reset()
        {
            marbles.Clear();

            foreach (T key in bagSetup.Keys)
            {
                marbles.AddRange(Enumerable.Repeat(key, bagSetup[key]));
            }
            
            if (marbles.Count == 0)
                throw new Exception($"Bag was reset with empty setup {bagSetup}");
        }
    }
    
    [Serializable]
    public class IntMarbleBag :  MarbleBag<int> {}
    [Serializable]
    public class BoolMarbleBag :  MarbleBag<bool> {}
    [Serializable]
    public class StringMarbleBag :  MarbleBag<string> {}
    [Serializable]
    public class EnumMarbleBag<T> : MarbleBag<T> where T : Enum {}
}
