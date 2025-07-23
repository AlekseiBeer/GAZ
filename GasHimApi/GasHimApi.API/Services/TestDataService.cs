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
    }

    public class TestDataService : ITestDataService
    {
        private readonly ChemicalDbContext _db;

        public TestDataService(ChemicalDbContext db)
        {
            _db = db;
        }

        public async Task ClearAllAsync(CancellationToken ct)
        {
            // Если есть внешние ключи между таблицами — учитываем порядок удаления.
            // Здесь пример для Substances и Processes.
            await _db.Database.ExecuteSqlRawAsync(@"TRUNCATE TABLE ""Processes"" RESTART IDENTITY CASCADE;", ct);
            await _db.Database.ExecuteSqlRawAsync(@"TRUNCATE TABLE ""Substances"" RESTART IDENTITY CASCADE;", ct);

            // Если будут другие таблицы — добавь сюда. Можно и через RemoveRange, но TRUNCATE быстрее.
        }

        public async Task SeedSubstancesAsync(int count, CancellationToken ct)
        {
            var rnd = new Random();

            // Наборы для генерации «правдоподобных» названий и синонимов
            string[] bases = { "Aceto", "Hydro", "Chloro", "Nitro", "Benzo", "Methyl", "Ethyl", "Propyl", "Butyl", "Phenyl", "Sulfo", "Amino", "Carbo", "Fluoro", "Phospho" };
            string[] endings = { "lene", "l", "ate", "ide", "ine", "one", "ol", "ene", "ane", "yne", "acid", "oxide", "chloride", "sulfate", "phosphate" };
            string[] synonymsParts = { "dimethyl", "monoethyl", "tri", "tetra", "alpha", "beta", "gamma", "delta", "solution", "anhydrous", "hydrate", "salt", "ester", "ketone", "alcohol" };

            var list = new List<Substance>(count);

            for (int i = 0; i < count; i++)
            {
                var name = bases[rnd.Next(bases.Length)] + endings[rnd.Next(endings.Length)];
                // Добавим индекс, чтобы имена точно были уникальны
                name = $"{name}-{i + 1}";

                // Синонимы: 0–3 случайных слов
                int synCount = rnd.Next(0, 4);
                var syns = synCount == 0
                    ? null
                    : string.Join("; ", Enumerable.Range(0, synCount)
                        .Select(_ => synonymsParts[rnd.Next(synonymsParts.Length)]));

                list.Add(new Substance
                {
                    Name = name,
                    Synonyms = syns
                });
            }

            await _db.Substances.AddRangeAsync(list, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}
