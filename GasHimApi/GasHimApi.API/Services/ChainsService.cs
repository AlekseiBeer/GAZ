
using GasHimApi.Data;
using GasHimApi.Data.Models;

namespace GasHimApi.API.Services
{
    public class ChainsService
    {
        private readonly IRepository<Substance> _substanceRepo;
        private readonly IRepository<Process> _processRepo;
        private readonly ILogger<ChainsService> _logger;

        private const int MaxDepth = 5;
        private const int MinDepth = 1;

        private long _dfsCallCount = 0;
        private long _reverseDfsCallCount = 0;

        public ChainsService(IRepository<Substance> substanceRepo,
                             IRepository<Process> processRepo,
                             ILogger<ChainsService> logger)
        {
            _substanceRepo = substanceRepo;
            _processRepo = processRepo;
            _logger = logger;
        }

        public async Task<List<List<string>>> GetChainsByStartOnly(string startSubstance)
        {
            var substances = await _substanceRepo.GetAllAsync();
            var processes = await _processRepo.GetAllAsync();
            var enriched = EnrichProcesses(processes);

            _dfsCallCount = 0;
            var chains = DFSFromStart(startSubstance, MinDepth, substances.Select(s => s.Name!).ToList(), enriched);
            _logger.LogInformation("[DFSFromStart] Всего вызовов DFS: {Count}", _dfsCallCount);
            return chains;
        }

        public async Task<List<List<string>>> GetChainsByTargetOnly(string targetSubstance)
        {
            var substances = await _substanceRepo.GetAllAsync();
            var processes = await _processRepo.GetAllAsync();
            var enriched = EnrichProcesses(processes);

            _reverseDfsCallCount = 0;
            var chains = ReverseDFSForTarget(targetSubstance, MinDepth, substances.Select(s => s.Name!).ToList(), enriched);
            _logger.LogInformation("[ReverseDFSForTarget] Всего вызовов ReverseDFS: {Count}", _reverseDfsCallCount);
            return chains;
        }

        public async Task<List<List<string>>> GetChainsByStartAndTarget(string start, string target)
        {
            var substances = await _substanceRepo.GetAllAsync();
            var processes = await _processRepo.GetAllAsync();
            var enriched = EnrichProcesses(processes);

            _dfsCallCount = 0;
            var chains = DFS(start, target, substances.Select(s => s.Name!).ToList(), enriched);
            _logger.LogInformation("[DFS] Всего вызовов DFS: {Count}", _dfsCallCount);
            return chains;
        }

        public async Task<List<List<string>>> GetAllChains()
        {
            var substances = await _substanceRepo.GetAllAsync();
            var processes = await _processRepo.GetAllAsync();
            var enriched = EnrichProcesses(processes);

            _dfsCallCount = 0;
            var allChains = new List<List<string>>();
            var substanceNames = substances.Select(s => s.Name).ToList();
            foreach (var subst in substanceNames)
            {
                var chains = DFSFromStart(subst!, 1, substanceNames!, enriched);
                allChains.AddRange(chains);
            }
            _logger.LogInformation("[Комплексный DFS] Всего вызовов DFS: {Count}", _dfsCallCount);
            return allChains;
        }

        private List<List<string>> DFSFromStart(string start, int minDepth, List<string> substances,
            List<EnrichedProcess> processes)
        {
            var result = new List<List<string>>();

            void dfs(string current, List<string> path, HashSet<string> visited, int depth)
            {
                _dfsCallCount++;
                if (depth >= minDepth)
                {
                    result.Add(new List<string>(path));
                }
                if (depth >= MaxDepth)
                    return;
                visited.Add(current);
                foreach (var proc in processes.Where(p => p.Inputs!.Contains(current)))
                {
                    foreach (var output in proc.Outputs!)
                    {
                        if (!substances.Contains(output))
                            continue;
                        if (visited.Contains(output))
                            continue;
                        var newPath = new List<string>(path) { $"[{proc.Name}]", output };
                        dfs(output, newPath, new HashSet<string>(visited), depth + 1);
                    }
                }
            }

            dfs(start, new List<string> { start }, new HashSet<string>(), 0);
            return result;
        }

        private List<List<string>> DFS(string start, string target, List<string> substances,
            List<EnrichedProcess> processes)
        {
            var result = new List<List<string>>();

            void dfsInner(string current, List<string> path, HashSet<string> visited, int depth)
            {
                _dfsCallCount++;
                if (depth > MaxDepth)
                    return;
                if (current == target)
                {
                    result.Add(new List<string>(path));
                    return;
                }
                visited.Add(current);
                foreach (var proc in processes.Where(p => p.Inputs!.Contains(current)))
                {
                    foreach (var output in proc.Outputs!)
                    {
                        if (!substances.Contains(output))
                            continue;
                        if (visited.Contains(output))
                            continue;
                        var newPath = new List<string>(path) { $"[{proc.Name}]", output };
                        dfsInner(output, newPath, new HashSet<string>(visited), depth + 1);
                    }
                }
            }

            dfsInner(start, new List<string> { start }, new HashSet<string>(), 0);
            return result;
        }

        private List<List<string>> ReverseDFSForTarget(string target, int minDepth, List<string> substances,
            List<EnrichedProcess> processes)
        {
            var result = new List<List<string>>();

            void dfsReverse(string current, List<string> path, HashSet<string> visited, int depth)
            {
                _reverseDfsCallCount++;
                if (depth >= minDepth)
                {
                    result.Add(new List<string>(path));
                }
                if (depth >= MaxDepth)
                    return;
                visited.Add(current);
                foreach (var proc in processes.Where(p => p.Outputs!.Contains(current)))
                {
                    foreach (var input in proc.Inputs!)
                    {
                        if (!substances.Contains(input))
                            continue;
                        if (visited.Contains(input))
                            continue;
                        var newPath = new List<string>(path) { $"[{proc.Name}]", input };
                        dfsReverse(input, newPath, new HashSet<string>(visited), depth + 1);
                    }
                }
            }

            dfsReverse(target, new List<string> { target }, new HashSet<string>(), 0);
            // Переворачиваем цепочки, чтобы они шли от исходного вещества к цели
            var corrected = result.Select(chain =>
            {
                var rev = new List<string>(chain);
                rev.Reverse();
                return rev;
            }).ToList();
            return corrected;
        }

        // Вспомогательный метод для разбора строки "A; B; C" в список ["A", "B", "C"]
        private List<string> ParseSubstances(string substancesStr)
        {
            if (string.IsNullOrWhiteSpace(substancesStr))
                return new List<string>();

            return substancesStr.Split(';')
                                  .Select(s => s.Trim())
                                  .Where(s => !string.IsNullOrEmpty(s))
                                  .ToList();
        }

        // Обогащаем процессы, преобразуя строки входов/выходов в списки
        private List<EnrichedProcess> EnrichProcesses(IEnumerable<Process> processes)
        {
            return processes.Select(p => new EnrichedProcess
            {
                Name = p.Name!,
                Inputs = ParseSubstances(p.MainInputs!),
                Outputs = ParseSubstances(p.MainOutputs!)
            }).ToList();
        }

        // Вспомогательный класс для удобства работы с данными процессов
        private class EnrichedProcess
        {
            public string? Name { get; set; }
            public List<string>? Inputs { get; set; }
            public List<string>? Outputs { get; set; }
        }
    }
}
