using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GasHimApi.Data;
using GasHimApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GasHimApi.API.Services
{

    public interface ITestDataService
    {
        Task ClearAllAsync(CancellationToken ct);
        Task SeedSubstancesAsync(int count, CancellationToken ct);

        Task ClearProcessesAsync(CancellationToken ct);
        Task SeedProcessesAsync(int count, CancellationToken ct);
    }


    public class TestDataService : ITestDataService
    {
        private readonly ChemicalDbContext _db;
        private static readonly Random _rnd = new();

        public TestDataService(ChemicalDbContext db)
        {
            _db = db;
        }

        public async Task ClearAllAsync(CancellationToken ct)
        {
            await _db.Database.ExecuteSqlRawAsync(@"TRUNCATE TABLE ""Processes"" RESTART IDENTITY CASCADE;", ct);
            await _db.Database.ExecuteSqlRawAsync(@"TRUNCATE TABLE ""Substances"" RESTART IDENTITY CASCADE;", ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task SeedSubstancesAsync(int count, CancellationToken ct)
        {
            if (count <= 0) return;

            var list = new List<Substance>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(new Substance
                {
                    Name = $"Substance-{i:000000}",
                    Synonyms = i % 5 == 0 ? $"Syn-{i}; Alt-{i}" : null
                });
            }

            await _db.Substances.AddRangeAsync(list, ct);
            await _db.SaveChangesAsync(ct);
        }

        // ---------------- НОВОЕ ----------------

        public async Task ClearProcessesAsync(CancellationToken ct)
        {
            await _db.Database.ExecuteSqlRawAsync(@"TRUNCATE TABLE ""Processes"" RESTART IDENTITY CASCADE;", ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task SeedProcessesAsync(int count, CancellationToken ct)
        {
            if (count <= 0) return;

            // Берём существующие вещества
            var substances = await _db.Substances
                                      .AsNoTracking()
                                      .Select(s => s.Name!)
                                      .ToListAsync(ct);

            if (substances.Count < 3)
                throw new InvalidOperationException("В базе должно быть минимум 3 вещества. Сначала засейдите их.");

            var processes = new List<Process>(count);
            for (int i = 0; i < count; i++)
            {
                var name = $"PROC-{i:000000}-{substances[_rnd.Next(substances.Count)]}";

                var mainInputs = Pick(substances, _rnd.Next(1, 4));
                var additionalInputs = PickDistinct(substances, mainInputs, _rnd.Next(0, 3));

                var mainOutputs = PickDistinct(substances, mainInputs.Concat(additionalInputs), _rnd.Next(1, 3));
                var additionalOutputs = PickDistinct(substances, mainInputs.Concat(additionalInputs).Concat(mainOutputs), _rnd.Next(0, 2));

                processes.Add(new Process
                {
                    Name = name,
                    PrimaryFeedstocks = string.Join("; ", mainInputs),
                    SecondaryFeedstocks = string.Join("; ", additionalInputs),
                    PrimaryProducts = string.Join("; ", mainOutputs),
                    ByProducts = string.Join("; ", additionalOutputs),
                    YieldPercentage = Math.Round(_rnd.NextDouble() * 100, 2)
                });
            }

            await _db.Processes.AddRangeAsync(processes, ct);
            await _db.SaveChangesAsync(ct);
        }

        // ---------- helpers ----------
        private static List<string> Pick(IReadOnlyList<string> src, int count)
        {
            if (count <= 0) return new List<string>();
            return src.OrderBy(_ => _rnd.Next()).Take(count).ToList();
        }

        private static List<string> PickDistinct(IEnumerable<string> src, IEnumerable<string> exclude, int count)
        {
            if (count <= 0) return new List<string>();
            var ex = new HashSet<string>(exclude);
            var candidates = src.Where(s => !ex.Contains(s)).ToList();
            return candidates.OrderBy(_ => _rnd.Next()).Take(count).ToList();
        }
    }
}
