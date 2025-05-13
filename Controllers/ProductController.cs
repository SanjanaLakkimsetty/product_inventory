using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Inventory.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Product_Inventory.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync(); // Fetch all products from the database
            return View(products); // Pass the list of products to the view
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Return NotFound if no ID is provided
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id); // Find the product by ID

            if (product == null)
            {
                return NotFound(); // Return NotFound if the product doesn't exist
            }

            return View(product); // Return the product to the Details view
        }


        // GET: Product/Create
        public IActionResult Create()
        {
            return View(); // Return the Create view to allow adding a new product
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Description,StockQuantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product); // Add the product to the context
                await _context.SaveChangesAsync(); // Save changes to the database
                return RedirectToAction(nameof(Index)); // Redirect to the Index action to view the list of products
            }
            return View(product); // If the model is invalid, return the same view
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // If no ID is provided, return NotFound
            }

            var product = await _context.Products.FindAsync(id); // Find the product by ID
            if (product == null)
            {
                return NotFound(); // If the product doesn't exist, return NotFound
            }
            return View(product); // Return the product to the Edit view
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,StockQuantity")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound(); // If the ID in the URL does not match the product ID, return NotFound
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product); // Update the product in the context
                    await _context.SaveChangesAsync(); // Save changes to the database
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == product.Id))
                    {
                        return NotFound(); // If the product was deleted by another user, return NotFound
                    }
                    else
                    {
                        throw; // Rethrow the exception for other errors
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirect to Index after successful update
            }
            return View(product); // If the model is invalid, return the Edit page with validation errors
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // If no ID is provided, return NotFound
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id); // Find the product by ID

            if (product == null)
            {
                return NotFound(); // If the product doesn't exist, return NotFound
            }

            return View(product); // Return the product to the Delete confirmation view
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id); // Find the product by ID
            _context.Products.Remove(product); // Remove the product from the context
            await _context.SaveChangesAsync(); // Save changes to the database
            return RedirectToAction(nameof(Index)); // Redirect to Index after successful deletion
        }
    }
}
