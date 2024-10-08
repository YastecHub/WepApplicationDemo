﻿using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Data;
using WebApiDemo.Filters.ActionFilters;
using WebApiDemo.Models;
using WebAPIDemo.Attributes;
using WebAPIDemo.Filters.ActionFilters.V2;
using WebAPIDemo.Filters.AuthFilters;
using WebAPIDemo.Filters.ExceptionFilters;

namespace WebAPIDemo.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("api/[controller]")]
    [JwtTokenAuthFilter]
    public class ShirtsController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public ShirtsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [RequiredClaim("read", "true")]
        public IActionResult GetShirts()
        {
            return Ok(db.Shirts.ToList());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [RequiredClaim("read", "true")]
        public IActionResult GetShirtById(int id)
        {
            return Ok(HttpContext.Items["shirt"]);
        }

        [HttpPost]
        [TypeFilter(typeof(Shirt_ValidateCreateShirtFilterAttribute))]
        [Shirt_EnsureDescriptionIsPresentFilter]
        [RequiredClaim("write", "true")]
        public IActionResult CreateShirt([FromBody] Shirt shirt)
        {
            this.db.Shirts.Add(shirt);
            this.db.SaveChanges();

            return CreatedAtAction(nameof(GetShirtById),
                new { id = shirt.ShirtId },
                shirt);
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [Shirt_ValidateUpdateShirtFilter]
        [TypeFilter(typeof(Shirt_HandleUpdateExceptionsFilterAttribute))]
        [Shirt_EnsureDescriptionIsPresentFilter]
        [RequiredClaim("write", "true")]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
            var shirtToUpdate = HttpContext.Items["shirt"] as Shirt;
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Price = shirt.Price;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;
            shirtToUpdate.Description = shirt.Description;

            db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [RequiredClaim("delete", "true")]
        public IActionResult DeleteShirt(int id)
        {
            var shirtToDelete = HttpContext.Items["shirt"] as Shirt;

            db.Shirts.Remove(shirtToDelete);
            db.SaveChanges();

            return Ok(shirtToDelete);
        }
    }
}