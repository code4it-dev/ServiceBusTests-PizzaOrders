using System.Linq;

namespace PizzaChef
{
    public class ProcessedPizzaOrder
    {
        public string Name { get; set; }
        public string[] Toppings { get; set; }

        public override string ToString()
        {
            if (Toppings?.Any() == true)
                return $"Pizza {Name} with some toppings: {string.Join(',', Toppings)}";
            else
                return $"Pizza {Name} without toppings";
        }
    }
}