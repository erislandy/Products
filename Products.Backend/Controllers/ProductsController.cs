using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Products.Backend.Models;
using Products.Domain;
using Products.Backend.Helpers;

namespace Products.Backend.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images";
                if(productView.ImageFile != null)
                {
                    pic = FileHelper.UploadPhoto(productView.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                Product product = ToProduct(productView);
                product.Image = pic;
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", productView.CategoryId);
            return View(productView);
        }

        private Product ToProduct(ProductView productView)
        {
            return new Product()
            {

                Category = productView.Category,
                CategoryId = productView.CategoryId,
                Description = productView.Description,
                Image = productView.Image,
                IsActive = productView.IsActive,
                LastPurchase = productView.LastPurchase,
                Price = productView.Price,
                ProductId = productView.ProductId,
                Remarks = productView.Remarks,
                Stock = productView.Stock

            };
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", product.CategoryId);
            var productView = ToView(product);
            
            return View(productView);
        }

        private ProductView ToView(Product product)
        {
            return new ProductView()
            {
                Category = product.Category,
                CategoryId = product.CategoryId,
                Image = product.Image,
                Description = product.Description,
                IsActive = product.IsActive,
                LastPurchase = product.LastPurchase,
                Price = product.Price,
                ProductId = product.ProductId,
                Remarks = product.Remarks,
                Stock = product.Stock

            };
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var pic = productView.Image;
                var folder = "~/Content/Images";
                if (productView.ImageFile != null)
                {
                    pic = FileHelper.UploadPhoto(productView.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                Product product = ToProduct(productView);
                product.Image = pic;
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", productView.CategoryId);
            return View(productView);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
