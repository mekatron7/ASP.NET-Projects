using MyVendingMachineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MyVendingMachineAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VendingController : ApiController
    {
        private Random rnd = new Random();
        private static Item chosenItem;
        private static List<int> stuckItems = new List<int>();

        private static List<Item> items = new List<Item>
        {
            new Item(){ Id = 1, Name = "Reese's", Price = 1.25m, Quantity = 12 },
            new Item(){ Id = 2, Name = "Snickers", Price = 1.35m, Quantity = 3 },
            new Item(){ Id = 3, Name = "Take 5", Price = 1.35m, Quantity = 5 },
            new Item(){ Id = 4, Name = "Twix", Price = 1.25m, Quantity = 9 },
            new Item(){ Id = 5, Name = "Skittles", Price = 1.25m, Quantity = 15 },
            new Item(){ Id = 6, Name = "Starburst", Price = 1.25m, Quantity = 4 },
            new Item(){ Id = 7, Name = "Gobstoppers", Price = 1.25m, Quantity = 2 },
            new Item(){ Id = 8, Name = "Welch's Fruit Snacks", Price = 1.50m, Quantity = 7 },
            new Item(){ Id = 9, Name = "Doritos", Price = 1.25m, Quantity = 4 },
            new Item(){ Id = 10, Name = "Lay's Original", Price = 1.25m, Quantity = 5 },
            new Item(){ Id = 11, Name = "Hot Fries", Price = 1.00m, Quantity = 8 },
            new Item(){ Id = 12, Name = "Sydney's of Hangover", Price = 1.55m, Quantity = 10 },
        };

        [Route("items")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetItems()
        {
            return Ok(items);
        }

        [Route("money/{amount}/item/{id}")]
        [AcceptVerbs("GET")]
        public IHttpActionResult VendItem(decimal amount, int id)
        {
            chosenItem = items.SingleOrDefault(i => i.Id == id);

            if(chosenItem.Quantity == 0)
            {
                return BadRequest("SOLD OUT :(");
            }
            else
            {
                if(amount < chosenItem.Price)
                {
                    return BadRequest($"Please insert {(chosenItem.Price - amount):c}");
                }
                else
                {
                    Change coins = new Change();
                    decimal change = amount - chosenItem.Price;
                    int quarters = (int)(change / .25m);
                    change = change - quarters * .25m;
                    int dimes = (int)(change / .1m);
                    change = change - dimes * .1m;
                    int nickels = (int)(change / .05m);

                    coins.Quarters = quarters;
                    coins.Dimes = dimes;
                    coins.Nickels = nickels;
                    coins.Stuck = "N";

                    if (rnd.Next(10) < 3)
                    {
                        coins.Stuck = "Y";
                        stuckItems.Add(chosenItem.Id);
                        return Ok(coins);
                    }

                    if (stuckItems.Contains(chosenItem.Id))
                    {
                        if(chosenItem.Quantity > 1)
                        {
                            coins.Stuck = $"Two {chosenItem.Name} ended up falling down. Lucky you.";
                            if(chosenItem.Quantity > 2)
                            {
                                stuckItems.Remove(chosenItem.Id);
                            }
                            else
                            {
                                stuckItems.RemoveAll(i => i == chosenItem.Id);
                            }
                            chosenItem.Quantity = chosenItem.Quantity - 2;
                        }
                        else
                        {
                            chosenItem.Quantity--;
                            stuckItems.RemoveAll(i => i == chosenItem.Id);
                        }
                    }
                    else
                    {
                        chosenItem.Quantity--;
                    }

                    return Ok(coins);
                }
            }
        }

        [Route("shakesuccess")]
        [AcceptVerbs("GET")]
        public IHttpActionResult ShakeSuccess()
        {
            if(chosenItem.Quantity == 0)
            {
                return BadRequest("You get nothing. Good day sir.");
            }
            else
            {
                chosenItem.Quantity--;
                stuckItems.Remove(chosenItem.Id);
                return Ok($"Lucky for you, the {chosenItem.Name} fell down! You took it and ate that shit like a real G.");
            }
        }
    }
}
