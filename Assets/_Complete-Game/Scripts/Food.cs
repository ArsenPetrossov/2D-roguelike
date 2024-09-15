namespace Completed
{
    public class Food
    {
        public Food (int initialValue)
        {
            Amount = initialValue;
        }

        
        public int Amount { get; private set; }

        public void Add(int value) => Amount += value;
        public void Remove(int value) => Amount += value;
    }
}