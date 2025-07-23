namespace GasHimApi.Data.Models
{
    /// <summary>
    /// Модель вещества.
    /// </summary>
    public class Substance
    {
        public int Id { get; set; }

        /// <summary>
        /// Основное название вещества.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Синонимы названий вещества (через ; или ,).
        /// </summary>
        public string? Synonyms { get; set; }
    }
}