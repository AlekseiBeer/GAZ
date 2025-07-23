using GasHimApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasHimApi.Data.Data
{
    internal class DataSeeder
    {
        public static async Task SeedAsync(ChemicalDbContext context)
        {
            // Добавляем вещества
            if (!context.Substances.Any())
            {
                var substances = new List<Substance>
                {
/*                    new Substance { Name = "АБС-пластик" },
                    new Substance { Name = "Акриловая кислота" },                    
                    new Substance { Name = "Ацетон" },
                    new Substance { Name = "Ацетилен" },
                    new Substance { Name = "Бензол" },
                    new Substance { Name = "1,4 Бутандиол" },
                    new Substance { Name = "1,2 Дихлорэтан" },
                    new Substance { Name = "1 Пропанол" },
                    new Substance { Name = "Формальдегид" },
                    new Substance { Name = "Этаноламин" },
                    new Substance { Name = "Этилакрилат" },
                    new Substance { Name = "Этилацетат" },
                    new Substance { Name = "Этиленхлорид" },
                    new Substance { Name = "Кумол" },
                    new Substance { Name = "Бисфенол А" },
                    new Substance { Name = "Азот"},
                    new Substance { Name = "Водород"},
                    new Substance { Name = "Аммиак"},
                    new Substance { Name = "Карбамид"},
                    new Substance { Name = "Метиламин"},
                    new Substance { Name = "Синтез-газ"},
                    new Substance { Name = "Бутилакрилат" }*/


                    new Substance { Name = "Пропан" },
                    new Substance { Name = "Пропилен" },

                    new Substance { Name = "Азот"},
                    new Substance { Name = "Водород"},
                    new Substance { Name = "Аммиак"},
                    new Substance { Name = "Карбамид"},
                    new Substance { Name = "Метиламин"},
                    new Substance { Name = "Синтез-газ"},
                    new Substance { Name = "Формальдегид" },
                    new Substance { Name = "Акриловая кислота" },
                    new Substance { Name = "Бутилакрилат" },
                    new Substance { Name = "Ацетилен" },
                    new Substance { Name = "Воздух"},
                    new Substance { Name = "Метанол"},
                    new Substance { Name = "Метан"},
                    new Substance { Name = "Уксусный альдегид"},
                    new Substance { Name = "Уксусная кислота"},
                    new Substance { Name = "Монохлоруксусная кислота"},
                    new Substance { Name = "Толуол"},
                    new Substance { Name = "Бензиновые фракции"}
                };

                context.Substances.AddRange(substances);
                await context.SaveChangesAsync();
            }

            // Добавляем процессы
            if (!context.Processes.Any())
            {
                var processes = new List<Process>
                {
                    /*new Process { Name = "Непрерывная эмульсионная полимеризация", MainInputs = "Бутадиен; Стирол; Акриламид", MainOutputs = "АБС-пластик" },
                    new Process { Name = "Непрерывная полимеризация в массе", MainInputs = "Полибутадиен; Стирол; Акрилонитрил", MainOutputs = "АБС-пластик" },
                    new Process { Name = "Двухстадийное окисление пропилена", MainInputs = "Пропилен; Кислород; Пар", MainOutputs = "Акриловая кислота" },
                    new Process { Name = "Дегидратация и окисление глицерина", MainInputs = "Глицерин; Кислород", MainOutputs = "Акриловая кислота" },
                    new Process { Name = "Гидролиз акрилоилхлорида", MainInputs = "Этилен; Кислород; CO₂", MainOutputs = "Акриловая кислота" },
                    new Process { Name = "Кумольный метод (окисление кумола)", MainInputs = "Кумол; Воздух", MainOutputs = "Ацетон; Фенол" },
                    new Process { Name = "Окислительный пиролиз метана", MainInputs = "Метан; Кислород", MainOutputs = "Ацетилен" },
                    new Process { Name = "Коксование каменного угля", MainInputs = "Уголь", MainOutputs = "Бензол" },
                    new Process { Name = "Термическое гидродеалкилирование", MainInputs = "Толуол; Водород", MainOutputs = "Бензол" },
                    new Process { Name = "Синтез из ацетилена и формальдегида", MainInputs = "Ацетилен; Формальдегид", MainOutputs = "1,4 Бутандиол" },
                    new Process { Name = "Синтез из пропилена", MainInputs = "Пропилен; Уксусная кислота; Кислород", MainOutputs = "1,4 Бутандиол" },
                    new Process { Name = "Синтез из аллилового спирта и изобутилена", MainInputs = "Аллиловый спирт; Изобутилен", MainOutputs = "1,4 Бутандиол" },
                    new Process { Name = "Синтез из фурфурола", MainInputs = "Фурфурол", MainOutputs = "1,4 Бутандиол" },
                    new Process { Name = "Синтез из бутадиена", MainInputs = "Бутадиен", MainOutputs = "1,4 Бутандиол" },
                    new Process { Name = "Оксосинтез", MainInputs = "Этилен; CO; H₂", MainOutputs = "1 Пропанол" },
                    new Process { Name = "Окислительное дегидрирование метанола", MainInputs = "Метанол; Воздух", MainOutputs = "Формальдегид" },
                    new Process { Name = "Оксиэтилирование безводного аммиака", MainInputs = "Аммиак; Этиленоксид", MainOutputs = "Этаноламин" },
                    new Process { Name = "Прямая этерификация акриловой кислоты", MainInputs = "Акриловая кислота; Этанол", MainOutputs = "Этилакрилат" },
                    new Process { Name = "Прямая этерификация уксусной кислоты", MainInputs = "Уксусная кислота; Этанол", MainOutputs = "Этилацетат" },
                    new Process { Name = "Дегидрирование этанола", MainInputs = "Этанол", MainOutputs = "Этилацетат" },
                    new Process { Name = "Совместное производство дихлорэтана и винилхлорида", MainInputs = "Этилен; Хлор; Кислород", MainOutputs = "Этиленхлорид; 1,2 Дихлорэтан" },
                    new Process { Name = "Газофазное гидрохлорирование ацетилена", MainInputs = "Ацетилен; Хлороводород", MainOutputs = "Этиленхлорид" },
                    new Process { Name = "Комбинированный метод (этилен + ацетилен + HCl)", MainInputs = "Этилен; Ацетилен; Хлороводород", MainOutputs = "Этиленхлорид" },
                    new Process { Name = "Алкилирование бензола пропиленом", MainInputs = "Бензол; Пропилен", MainOutputs = "Кумол" },
                    new Process { Name = "Получение бисфенола А из фенола и ацетона", MainInputs = "Фенол; Ацетон", MainOutputs = "Бисфенол А" },

                    new Process { Name = "Криогенное разложение воздуха", MainInputs = "Воздух", MainOutputs = "Азот" },
                    new Process { Name = "Паровая конверсия метана", MainInputs = "Метан; Пар", MainOutputs = "Водород; Углекислый газ; Угарный газ" },
                    new Process { Name = "Подземная газификация угля", MainInputs = "Уголь; Пар; Воздух", MainOutputs = "Водород; Углекислый газ; Угарный газ" },
                    new Process { Name = "Производство из азота и водорода", MainInputs = "Азот; Водород", MainOutputs = "Аммиак" },
                    new Process { Name = "Реакция Базарова", MainInputs = "Аммиак; Углекислый газ", MainOutputs = "Карбамид" },
                    new Process { Name = "Акилирование аммиака ", MainInputs = "Аммиак; Метанол", MainOutputs = "Метиламин; Диметиламин; Триметиламин" },
                    new Process { Name = "Паровая конверсия углеводородов", MainInputs = "Метан; Пар", MainOutputs = "Синтез-газ" },
                    new Process { Name = "Прямая этерификация акриловой кислоты", MainInputs = "Акриловая кислота; Бутанол", MainOutputs = "Бутилакрилат; Вода" }*/

                    new Process { Name = "Разделение нестабильного углеводородного конденсата", MainInputs = "Нефтяной газ", MainOutputs = "Пропан" },
                    new Process { Name = "Каталитическое дегидрирование пропана", MainInputs = "Пропан", MainOutputs = "Пропилен" },
                    new Process { Name = "Выделение из газов каталитического крекинга", MainInputs = "Газ каталитического крекинга", MainOutputs = "Пропилен" },
                    new Process { Name = "Выделение из газов пиролиза", MainInputs = "Газ пиролиза", MainOutputs = "Пропилен" },

                    new Process { Name = "Криогенное разложение воздуха", MainInputs = "Воздух", MainOutputs = "Азот" },
                    new Process { Name = "Паровая конверсия метана", MainInputs = "Метан; Пар", MainOutputs = "Водород; Углекислый газ; Угарный газ" },
                    new Process { Name = "Подземная газификация угля", MainInputs = "Уголь", MainOutputs = "Водород; Углекислый газ; Угарный газ" },
                    new Process { Name = "Производство из азота и водорода", MainInputs = "Азот; Водород", MainOutputs = "Аммиак" },
                    new Process { Name = "Реакция Базарова", MainInputs = "Аммиак; Углекислый газ", MainOutputs = "Карбамид" },
                    new Process { Name = "Акилирование аммиака", MainInputs = "Аммиак; Метанол", MainOutputs = "Метиламин; Диметиламин; Триметиламин" },
                    new Process { Name = "Паровая конверсия углеводородов", MainInputs = "Метан; Пар", MainOutputs = "Синтез-газ" },
                    new Process { Name = "Окислительное дегидрирование метанола", MainInputs = "Метанол", MainOutputs = "Формальдегид" },
                    new Process { Name = "Двухстадийное окисление пропилена", MainInputs = "Пропилен; Кислород; Пар", MainOutputs = "Акриловая кислота" },
                    new Process { Name = "Дегидратация и окисление глицерина", MainInputs = "Глицерин; Кислород", MainOutputs = "Акриловая кислота" },
                    new Process { Name = "Гидролиз акрилоилхлорида", MainInputs = "Этилен; Кислород; CO₂", MainOutputs = "Акриловая кислота" },
                    new Process { Name = "Прямая этерификация акриловой кислоты", MainInputs = "Акриловая кислота; Бутанол", MainOutputs = "Бутилакрилат; Вода" },
                    new Process { Name = "Окислительный пиролиз метана", MainInputs = "Метан; Кислород", MainOutputs = "Ацетилен" },
                    new Process { Name = "Парофазная гидратация ацетилена", MainInputs = "Ацетилен; Вода", MainOutputs = "Уксусный альдегид; Кротоновый альдегид; Ацетон" },
                    new Process { Name = "Одностадийное окисление этилена", MainInputs = "Этилен", MainOutputs = "Уксусный альдегид; Уксусная кислота; Муравьиная кислота; Метилхлорид; Этилхлорид; Хлорацетальдегид; Кротоновый альдегид; Диоксид углерода" },
                    new Process { Name = "Двухстадийное окисление этилена", MainInputs = "Этилен", MainOutputs = "Уксусный альдегид; Уксусная кислота; Муравьиная кислота; Метилхлорид; Этилхлорид; Хлорацетальдегид; Кротоновый альдегид; Диоксид углерода" },
                    new Process { Name = "Жидкофазное окисление Н-бутана", MainInputs = "Н-бутан; Кислород", MainOutputs = "Уксусная кислота; Муравьиная кислота; Ацетон-метилацетатная фракция; Меилэтилкетон-этилфцетатная фракция" },
                    new Process { Name = "Карбонилирование метанола <BASF>", MainInputs = "Метанол; Окись углерода", MainOutputs = "Уксусная кислота; Деметиловый эфир; Метилацетат" },
                    new Process { Name = "Карбонилирование метанола <MONSANTO>", MainInputs = "Метанол; Окись углерода", MainOutputs = "Уксусная кислота; Деметиловый эфир; Метилацетат" },
                    new Process { Name = "Карбонилирование метанола <BP CHEMICAL LTD>", MainInputs = "Метанол; Окись углерода", MainOutputs = "Уксусная кислота; Деметиловый эфир; Метилацетат" },
                    new Process { Name = "Производство уксусной кислоты из Синтез-газа", MainInputs = "Синтез-газ", MainOutputs = "Уксусная кислота" },
                    new Process { Name = "Хлорирование уксусной кислоты в присутствии уксусного ангидрида с хлористым ацетилом", MainInputs = "Уксусная кислота", MainOutputs = "Монохлоруксусная кислота" },
                    new Process { Name = "Омыление трихлорэтилена водой в присутствии серной кислоты", MainInputs = "Трихлорэтилен", MainOutputs = "Монохлоруксусная кислота" },
                    new Process { Name = "Каталитический риформинг бензинов с последующим разделением продуктов", MainInputs = "Бензиновые фракции", MainOutputs = "Толуол; Бензол; Фракция ксилолов" }, //Фракции - не тоные продукты. как их выводить
                };

                context.Processes.AddRange(processes);
                await context.SaveChangesAsync();
            }
        }
    }
}
