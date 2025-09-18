using InternshipFinalProject_Core.Models;
using InternshipFinalProject_Core.RepositoryInterfaces;
using InternshipFinalProject_Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Infrastructure.Repositories
{
    public class InvoiceRepository:IInvoiceRepository
    {
        private readonly DataAccessClass dataAccessClassObj;

        public InvoiceRepository(DataAccessClass dataAccessClassObj)
        {
            this.dataAccessClassObj = dataAccessClassObj;
        }
        public async Task<InvoiceModel> GetInvoice(int id)
        {
            try
            {
                var response = await dataAccessClassObj.InvoiceTable
                    .Include(a => a.Appointment)
                    .Include(a => a.Doctor).ThenInclude(d => d.User)
                    .Include(a => a.Patient).ThenInclude(p => p.User)
                    .FirstOrDefaultAsync(a => a.AppointmentId == id);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetInvoice: {ex.Message}");
                return null;
            }
        }
    }
}
