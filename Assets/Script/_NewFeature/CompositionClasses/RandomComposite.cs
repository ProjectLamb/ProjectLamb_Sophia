using UnityEngine;
using UnityEngine.Events;
using Sophia.DataSystem;
using System.Threading;

namespace Sophia.Composite
{
    public class RandomComposite {
        public Stat Luck {get; protected set;}
        public readonly System.Random random;

        public RandomComposite(float luck) {
            random = new System.Random();
        }

        public bool GetRandomByLuck() {
            return Luck >= (random.NextDouble() * 100);
        }

        public bool GetRandomByFix(float percent) {
            return percent >= (random.NextDouble() * 100);
        }
    }
}