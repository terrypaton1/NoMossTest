using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlCaTrAzzGames.Utilities
{
    public static class ListFunctions
    {
        //================================================================//
        //===================Fisher_Yates_CardDeck_Shuffle================//
        //================================================================//

        /// With the Fisher-Yates shuffle, first implemented on computers by Durstenfeld in 1964, 
        ///   we randomly sort elements. This is an accurate, effective shuffling method for all array types.

        public static List<Object> Fisher_Yates_CardDeck_Shuffle(List<Object> aList)
        {

            System.Random _random = new System.Random();

            Object myGO;

            int n = aList.Count;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                myGO = aList[r];
                aList[r] = aList[i];
                aList[i] = myGO;
            }

            return aList;
        }

        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}
