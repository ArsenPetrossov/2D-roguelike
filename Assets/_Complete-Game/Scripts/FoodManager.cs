namespace Completed
{
    public class FoodManager
    {
        public static FoodManager Instance => _instance ??= new FoodManager();

        private static FoodManager _instance;
        
        private FoodManager(){}

        private Food _storedFood;

        public void Save(Food food)
        {
            _storedFood = food;
        }

        public Food Load()
        {
            return _storedFood;
        }
    }
}