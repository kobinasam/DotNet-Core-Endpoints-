using Hubtel.eCommerce.Carts.Api.Data;
using Hubtel.eCommerce.Carts.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Hubtel.eCommerce.Carts.Api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly eCommerceContext _context;
        public CartController(eCommerceContext context)
        {
            _context = context;
        }
        //This is a GetAll method 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCartItems(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                return await _context.Cart.AsNoTracking().ToListAsync();
            }
            return await _context.Cart.AsNoTracking().Where(x => x.ItemName.Contains(itemName)).ToListAsync();

          
        }

        //This is a Get method By ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCartItem(int id)
        {
            try
            {
                var CartItem = await _context.Cart.FindAsync(id);

                if (CartItem == null)
                {
                    return NotFound();
                }

                return Ok(CartItem);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }

        //This is a Post Method and also checks if the item aleady exits to update the quantity
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCartItem(Cart cart)
        {

            var CartItem = await _context.Cart.FindAsync(cart.ItemId);
            
            if (CartItem == null)
            {
                cart.ItemId = 0 ;
                await _context.Cart.AddAsync(cart);
                 await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCartItem), new { id = cart.ItemId }, cart);
            }

            CartItem.Quantity += cart.Quantity;
            _context.Cart.Update(CartItem);
           await  _context.SaveChangesAsync();

            return Ok(cart);
           
        }

        // this is a put method by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.ItemId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(cart);
        }

        private async Task<bool> CartExists(int id)
        {
          return await _context.Cart.AnyAsync(x=>x.ItemId == id);
           
        }



        //this is a delete method
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            
            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
