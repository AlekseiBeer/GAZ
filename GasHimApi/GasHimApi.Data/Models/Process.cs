namespace GasHimApi.Data.Models
{
    /// <summary>
    /// Модель химического процесса.
    /// </summary>
    public class Process
    {
        public int Id { get; set; }

        /// <summary>
        /// Название процесса.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Основные входные вещества (id через ; или ,).
        /// </summary>
        public string? MainInputs { get; set; }

        /// <summary>
        /// Дополнительные входные вещества (id через ; или ,).
        /// </summary>
        public string? AdditionalInputs { get; set; }

        /// <summary>
        /// Основные выходные вещества (id через ; или ,).
        /// </summary>
        public string? MainOutputs { get; set; }

        /// <summary>
        /// Дополнительные выходные вещества (id через ; или ,).
        /// </summary>
        public string? AdditionalOutputs { get; set; }

        /// <summary>
        /// Процент выхода продукта.
        /// </summary>
        public double YieldPercent { get; set; }
    }
}