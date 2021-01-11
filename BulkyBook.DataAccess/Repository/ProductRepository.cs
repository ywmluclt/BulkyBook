using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var productFromDb = _db.Products.FirstOrDefault(predicate => predicate.Id == product.Id);
            if (productFromDb != null)
            {
                if (product.ImageUrl != null)
                {
                    productFromDb.ImageUrl = product.ImageUrl;
                }
                productFromDb.ISBN = product.ISBN;
                productFromDb.ListPrice = product.ListPrice;
                productFromDb.Price = product.Price;
                productFromDb.Price100 = product.Price100;
                productFromDb.Price50 = product.Price50;
                productFromDb.Title = product.Title;
                productFromDb.Description = product.Description;
                productFromDb.CoverTypeId = product.CoverTypeId;
                productFromDb.CategoryId = product.CoverTypeId;

            }
        }
    }
}
