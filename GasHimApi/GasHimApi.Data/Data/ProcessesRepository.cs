using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq; // Добавлено для использования LINQ
using GasHimApi.Data.Models;

namespace GasHimApi.Data
{
    public class ProcessesRepository : IRepository<Process>
    {
        private readonly ChemicalDbContext _context;

        public ProcessesRepository(ChemicalDbContext context)
        {
            _context = context;
        }

        public async Task<List<Process>> GetAllAsync()
        {
            return await _context.Processes.ToListAsync();
        }

        public async Task<Process> GetByIdAsync(int id)
        {
            return await _context.Processes.FindAsync(id);
        }

        public async Task AddAsync(Process process)
        {
            await _context.Processes.AddAsync(process);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Process process)
        {
            _context.Entry(process).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var process = await GetByIdAsync(id);
            if (process != null)
            {
                _context.Processes.Remove(process);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Полнотекстовый поиск по процессам.
        /// </summary>
        /// <param name="query">Строка поиска.</param>
        /// <returns>Список процессов, удовлетворяющих поиску.</returns>
        public async Task<List<Process>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Process>();

            return await _context.Processes
                .Where(p =>
                    (p.MainInputs != null && p.MainInputs.Contains(query)) ||
                    (p.AdditionalInputs != null && p.AdditionalInputs.Contains(query)) ||
                    (p.MainOutputs != null && p.MainOutputs.Contains(query)) ||
                    (p.AdditionalOutputs != null && p.AdditionalOutputs.Contains(query))
                )
                .ToListAsync();
        }
    }
}
