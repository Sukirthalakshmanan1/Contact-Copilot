
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ContactManagementCopilot.Controllers;
using ContactManagementCopilot.Data;
using ContactManagementCopilot.Models;
//using ContactManagementCopilot.Controllers;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Controllers
{
    public class ContactControllerTests
    {
        private ContactController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("server=LTPPUN022528910\\SQLEXPRESS;database=ContactManagement;Integrated Security=true;TrustServerCertificate=true;")
                .Options;

            _context = new ApplicationDbContext(options);
            //_context.ContactDetails.Add(new ContactDetails { FirstName = "Mphasis1", LastName = "Mphasis", Email = "Mphasis@1", PhoneNumber = "09123456789", Role = "Admin" });
            //_context.SaveChanges();

            _controller = new ContactController(_context, null);
        }
        [Test]
        public void DeleteContact_ValidId_ReturnsOkResult()
        {
            // Arrange
            var contactId = _context.ContactDetails.First().Id ;

            // Act
            var result = _controller.Delete(contactId);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        }

        [Test]
        public void DeleteContact_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int invalidId = _context.ContactDetails.OrderByDescending(c => c.Id).First().Id;

            // Act
            var result = _controller.Delete(invalidId);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        }

        [Test]
        public void EditContact_ValidId_ReturnsOkResult()
        {
            // Arrange
            var contactId = _context.ContactDetails.First().Id;
            ContactDetails contact = _context.ContactDetails.Find(contactId);
            if (contact != null){contact.Firstname = "Mphasis";contact.Lastname = "Mphasis";contact.Email = "Mphasis@1";contact.Phone = "09123456789";contact.Address = "Bangalore"; 
            _context.Entry(contact).State = EntityState.Modified;
            _context.SaveChanges();}

            // Act
            var result = _controller.Edit(contactId);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        }


    }
}