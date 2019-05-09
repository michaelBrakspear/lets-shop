using System;
using System.Threading.Tasks;
using LetsShop.Client;

namespace LetsShop.Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var productOne = new Tuple<Guid, int, decimal>(Guid.NewGuid(), 3, 150m);
            var productTwo = new Tuple<Guid, int, decimal>(Guid.NewGuid(), 5, 100m);
            var productThree = new Tuple<Guid, int, decimal>(Guid.NewGuid(), 80, 0.50m);
            
            using (var cart = new Cart("https://localhost:44317"))
            {
                try
                {
                    cart.ListProducts();
                }
                catch (CartInitialisationException)
                {
                    Console.WriteLine("Cart is not Initialised! Let's do that now");
                }

                await cart.InitialiseAsync();
                Console.WriteLine("Cart Initialised");

                await AddProductAsync(cart, productOne);
                await AddProductAsync(cart, productTwo);
                await AddProductAsync(cart, productThree);
                await RemoveProductAsync(cart, productOne.Item1);
                await UpdateProductAsync(cart, productThree.Item1, 100);

                Console.WriteLine("Let's empty the cart");
                await cart.EmptyCart();
                Console.WriteLine($"Cart emptied, cart contains {cart.ListProducts().Count} items");

                Console.WriteLine("Finished shopping, let's delete the cart");
                await cart.DeleteCart();
            }
        }

        private static async Task AddProductAsync(Cart cart, Tuple<Guid, int, decimal> product)
        {
            Console.WriteLine($"Adding {product.Item2} of item with id {product.Item1} at £{product.Item3} a unit");
            await cart.AddProductAsync(product.Item1, product.Item2, product.Item3);
            Console.WriteLine($"Product added, running total now: {cart.CurrentTotal}");
        }

        private static async Task RemoveProductAsync(Cart cart, Guid productId)
        {
            Console.WriteLine($"Removing item with id {productId} from cart");
            await cart.RemoveProductAsync(productId);
            Console.WriteLine($"Product removed, running total now: {cart.CurrentTotal}");
        }

        private static async Task UpdateProductAsync(Cart cart, Guid productId, int newQty)
        {
            Console.WriteLine($"Updating quantity of item with id {productId} from cart to {newQty}");
            await cart.UpdateProductAsync(productId, newQty);
            Console.WriteLine($"Quantity updated, running total now: {cart.CurrentTotal}");
        }
    }
}
