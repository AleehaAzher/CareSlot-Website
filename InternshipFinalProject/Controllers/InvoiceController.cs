using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipProject.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace InternshipFinalProject.Controllers
{
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceService invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            this.invoiceService = invoiceService;
        }
        [HttpGet]
        public async Task<IActionResult> GetInvoice(int id) 
        {
            try
            {
                var response = await invoiceService.GetInvoice(id);
                if (response == null)
                {
                    return Json(new { success = false, message = "Invoice not found" });
                }

                return Json(new
                {
                    success = true,
                    patientName = response.Patient.User.FullName,
                    patientAddress = response.Patient.Address,
                    date = response.InvoiceDate,
                    fee = response.Doctor.Fees
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
