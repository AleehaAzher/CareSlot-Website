using InternshipFinalProject_Application.ServiceInterfaces;
using InternshipFinalProject_Core.Models;
using InternshipFinalProject_Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Application.Services
{
    public class InvoiceService:IInvoiceService
    {
        private readonly IInvoiceRepository invoiceRepo;

        public InvoiceService(IInvoiceRepository invoiceRepo)
        {
            this.invoiceRepo = invoiceRepo;
        }
        public async Task<InvoiceModel> GetInvoice(int id)
        {
            var response = await invoiceRepo.GetInvoice(id);
            
            if(response==null)
            {
                return null;
            }
            return response;
        }
    }
}
