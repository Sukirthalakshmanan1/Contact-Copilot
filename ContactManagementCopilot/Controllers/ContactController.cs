//using System.Web.Mvc;
using ContactManagementCopilot.Data;
using ContactManagementCopilot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagementCopilot.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ContactController> _logger;

        public ContactController(ApplicationDbContext dbContext, ILogger<ContactController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        

       public ActionResult Index()
{
    var contacts = _dbContext.ContactDetails.ToList();
    
    if (TempData["SuccessMessage"] != null)
    {
        ViewBag.SuccessMessage = TempData["SuccessMessage"];
    }
    
    if (TempData["ErrorMessage"] != null)
    {
        ViewBag.ErrorMessage = TempData["ErrorMessage"];
    }
    
    return View(contacts);
}


        // GET: Contact/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ContactDetails contact)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }
        
            try
            {
                // Check if contact already exists
                var existingContact = _dbContext.ContactDetails.FirstOrDefault(c =>
                    c.Firstname == contact.Firstname &&
                    c.Lastname == contact.Lastname &&
                    c.Phone == contact.Phone);
        
                if (existingContact != null)
                {
                    ModelState.AddModelError("", "A contact with the same first name, last name, and phone number already exists.");
                    return View(contact);
                }
        
                _dbContext.ContactDetails.Add(contact);
                _dbContext.SaveChanges();
        
                // Log the contact creation
                LogContactCreation(contact);
        
                TempData["SuccessMessage"] = "Contact created successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the contact.");
                TempData["ErrorMessage"] = "An error occurred while creating the contact.";
            }
        
            return View(contact);
        }

        
        private void LogContactCreation(ContactDetails contact)
        {
            string logMessage = $"Contact created: {contact.Firstname} {contact.Lastname} (ID: {contact.Id}) at {DateTime.Now}";

            string logFilePath = Path.Combine("C:\\VS code\\ContactManagementCopilot\\ContactManagementCopilot\\wwwroot\\log\\", "ContactCreationLog.txt");
            System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }

        // GET: Contact/Edit/5
        public ActionResult Edit(int id)
        {
            // Retrieve contact from database based on id
            ContactDetails contact = _dbContext.ContactDetails.Find(id);

            return View(contact);
        }

        // POST: Contact/Edit/5
        
        [HttpPost]
        public ActionResult Edit(int id, ContactDetails contact)
        {
            try
            {
                // Check if contact with same first name, last name, and phone number already exists
                var existingContact = _dbContext.ContactDetails
                    .FirstOrDefault(c => c.Id != id &&
                                         c.Firstname == contact.Firstname &&
                                         c.Lastname == contact.Lastname &&
                                         c.Phone == contact.Phone);

                if (existingContact != null)
                {

                     ModelState.AddModelError("", "A contact with the same first name, last name, and phone number already exists.");
                      return View(contact);
                }

                // Update contact details in database
                ContactDetails contactToUpdate = _dbContext.ContactDetails.Find(id);
                if (contactToUpdate != null)
                {
                    contactToUpdate.Firstname = contact.Firstname;
                    contactToUpdate.Lastname = contact.Lastname;
                    contactToUpdate.Email = contact.Email;
                    contactToUpdate.Phone = contact.Phone;
                    contactToUpdate.Address = contact.Address;
                    // Update other properties as needed
                    _dbContext.Entry(contactToUpdate).State = EntityState.Modified;
                    _dbContext.SaveChanges();

                    // Log the edit operation
                    LogEdit(contactToUpdate);

                    TempData["SuccessMessage"] = "Contact updated successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Contact not found.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating contact with ID {id}");
                TempData["ErrorMessage"] = "An error occurred while updating the contact.";
                return RedirectToAction("Index");
            }
        }
        private void LogEdit(ContactDetails contact)
        {
            string logMessage = $"Contact edited: ID={contact.Id}, Name={contact.Firstname} {contact.Lastname}, Email={contact.Email}, Phone={contact.Phone}, Address={contact.Address}";

            string logFilePath = Path.Combine("C:\\VS code\\ContactManagementCopilot\\ContactManagementCopilot\\wwwroot\\log\\", "ContactCreationLog.txt");
            System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }




[HttpGet]
public ActionResult Delete(int id)
{
    var contact = _dbContext.ContactDetails.Find(id);
    if (contact == null)
    {
        return NotFound();
    }
    return View(contact);
}

[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public ActionResult DeleteConfirmed(int id)
{
    try
    {
        var contactToDelete = _dbContext.ContactDetails.Find(id);
        if (contactToDelete == null)
        {
            return NotFound();
        }

        _dbContext.ContactDetails.Remove(contactToDelete);
        _dbContext.SaveChanges();

        LogDelete(contactToDelete);

        TempData["SuccessMessage"] = "Contact deleted successfully.";
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error deleting contact with ID {Id}", id);
        TempData["ErrorMessage"] = "An error occurred while deleting the contact.";
        return RedirectToAction("Index");
    }
}

        private void LogDelete(ContactDetails contact)
        {
            string logMessage = $"Contact deleted: ID={contact.Id}, Name={contact.Firstname} {contact.Lastname}, Email={contact.Email}, Phone={contact.Phone}, Address={contact.Address}";
        // _logger.LogInformation(logMessage);
             string logFilePath = Path.Combine("C:\\VS code\\ContactManagementCopilot\\ContactManagementCopilot\\wwwroot\\log\\", "ContactCreationLog.txt");
             System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
       
   
   
    }
}