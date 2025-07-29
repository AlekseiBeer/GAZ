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
                    /*new Process { Name = "Непрерывная эмульсионная полимеризация", PrimaryFeedstocks = "Бутадиен; Стирол; Акриламид", PrimaryProducts = "АБС-пластик" },
                    new Process { Name = "Непрерывная полимеризация в массе", PrimaryFeedstocks = "Полибутадиен; Стирол; Акрилонитрил", PrimaryProducts = "АБС-пластик" },
                    new Process { Name = "Двухстадийное окисление пропилена", PrimaryFeedstocks = "Пропилен; Кислород; Пар", PrimaryProducts = "Акриловая кислота" },
                    new Process { Name = "Дегидратация и окисление глицерина", PrimaryFeedstocks = "Глицерин; Кислород", PrimaryProducts = "Акриловая кислота" },
                    new Process { Name = "Гидролиз акрилоилхлорида", PrimaryFeedstocks = "Этилен; Кислород; CO₂", PrimaryProducts = "Акриловая кислота" },
                    new Process { Name = "Кумольный метод (окисление кумола)", PrimaryFeedstocks = "Кумол; Воздух", PrimaryProducts = "Ацетон; Фенол" },
                    new Process { Name = "Окислительный пиролиз метана", PrimaryFeedstocks = "Метан; Кислород", PrimaryProducts = "Ацетилен" },
                    new Process { Name = "Коксование каменного угля", PrimaryFeedstocks = "Уголь", PrimaryProducts = "Бензол" },
                    new Process { Name = "Термическое гидродеалкилирование", PrimaryFeedstocks = "Толуол; Водород", PrimaryProducts = "Бензол" },
                    new Process { Name = "Синтез из ацетилена и формальдегида", PrimaryFeedstocks = "Ацетилен; Формальдегид", PrimaryProducts = "1,4 Бутандиол" },
                    new Process { Name = "Синтез из пропилена", PrimaryFeedstocks = "Пропилен; Уксусная кислота; Кислород", PrimaryProducts = "1,4 Бутандиол" },
                    new Process { Name = "Синтез из аллилового спирта и изобутилена", PrimaryFeedstocks = "Аллиловый спирт; Изобутилен", PrimaryProducts = "1,4 Бутандиол" },
                    new Process { Name = "Синтез из фурфурола", PrimaryFeedstocks = "Фурфурол", PrimaryProducts = "1,4 Бутандиол" },
                    new Process { Name = "Синтез из бутадиена", PrimaryFeedstocks = "Бутадиен", PrimaryProducts = "1,4 Бутандиол" },
                    new Process { Name = "Оксосинтез", PrimaryFeedstocks = "Этилен; CO; H₂", PrimaryProducts = "1 Пропанол" },
                    new Process { Name = "Окислительное дегидрирование метанола", PrimaryFeedstocks = "Метанол; Воздух", PrimaryProducts = "Формальдегид" },
                    new Process { Name = "Оксиэтилирование безводного аммиака", PrimaryFeedstocks = "Аммиак; Этиленоксид", PrimaryProducts = "Этаноламин" },
                    new Process { Name = "Прямая этерификация акриловой кислоты", PrimaryFeedstocks = "Акриловая кислота; Этанол", PrimaryProducts = "Этилакрилат" },
                    new Process { Name = "Прямая этерификация уксусной кислоты", PrimaryFeedstocks = "Уксусная кислота; Этанол", PrimaryProducts = "Этилацетат" },
                    new Process { Name = "Дегидрирование этанола", PrimaryFeedstocks = "Этанол", PrimaryProducts = "Этилацетат" },
                    new Process { Name = "Совместное производство дихлорэтана и винилхлорида", PrimaryFeedstocks = "Этилен; Хлор; Кислород", PrimaryProducts = "Этиленхлорид; 1,2 Дихлорэтан" },
                    new Process { Name = "Газофазное гидрохлорирование ацетилена", PrimaryFeedstocks = "Ацетилен; Хлороводород", PrimaryProducts = "Этиленхлорид" },
                    new Process { Name = "Комбинированный метод (этилен + ацетилен + HCl)", PrimaryFeedstocks = "Этилен; Ацетилен; Хлороводород", PrimaryProducts = "Этиленхлорид" },
                    new Process { Name = "Алкилирование бензола пропиленом", PrimaryFeedstocks = "Бензол; Пропилен", PrimaryProducts = "Кумол" },
                    new Process { Name = "Получение бисфенола А из фенола и ацетона", PrimaryFeedstocks = "Фенол; Ацетон", PrimaryProducts = "Бисфенол А" },

                    new Process { Name = "Криогенное разложение воздуха", PrimaryFeedstocks = "Воздух", PrimaryProducts = "Азот" },
                    new Process { Name = "Паровая конверсия метана", PrimaryFeedstocks = "Метан; Пар", PrimaryProducts = "Водород; Углекислый газ; Угарный газ" },
                    new Process { Name = "Подземная газификация угля", PrimaryFeedstocks = "Уголь; Пар; Воздух", PrimaryProducts = "Водород; Углекислый газ; Угарный газ" },
                    new Process { Name = "Производство из азота и водорода", PrimaryFeedstocks = "Азот; Водород", PrimaryProducts = "Аммиак" },
                    new Process { Name = "Реакция Базарова", PrimaryFeedstocks = "Аммиак; Углекислый газ", PrimaryProducts = "Карбамид" },
                    new Process { Name = "Акилирование аммиака ", PrimaryFeedstocks = "Аммиак; Метанол", PrimaryProducts = "Метиламин; Диметиламин; Триметиламин" },
                    new Process { Name = "Паровая конверсия углеводородов", PrimaryFeedstocks = "Метан; Пар", PrimaryProducts = "Синтез-газ" },
                    new Process { Name = "Прямая этерификация акриловой кислоты", PrimaryFeedstocks = "Акриловая кислота; Бутанол", PrimaryProducts = "Бутилакрилат; Вода" }*/

                    new Process { Name = "Разделение нестабильного углеводородного конденсата", PrimaryFeedstocks = "Нефтяной газ", PrimaryProducts = "Пропан" },
                    new Process { Name = "Каталитическое дегидрирование пропана", PrimaryFeedstocks = "Пропан", PrimaryProducts = "Пропилен" },
                    new Process { Name = "Выделение из газов каталитического крекинга", PrimaryFeedstocks = "Газ каталитического крекинга", PrimaryProducts = "Пропилен" },
                    new Process { Name = "Выделение из газов пиролиза", PrimaryFeedstocks = "Газ пиролиза", PrimaryProducts = "Пропилен" },

                    new Process { Name = "Криогенное разложение воздуха", PrimaryFeedstocks = "Воздух", PrimaryProducts = "Азот" },
                    new Process { Name = "Паровая конверсия метана", PrimaryFeedstocks = "Метан; Пар", PrimaryProducts = "Водород; Углекислый газ; Угарный газ" },
                    new Process { Name = "Подземная газификация угля", PrimaryFeedstocks = "Уголь", PrimaryProducts = "Водород; Углекислый газ; Угарный газ" },
                    new Process { Name = "Производство из азота и водорода", PrimaryFeedstocks = "Азот; Водород", PrimaryProducts = "Аммиак" },
                    new Process { Name = "Реакция Базарова", PrimaryFeedstocks = "Аммиак; Углекислый газ", PrimaryProducts = "Карбамид" },
                    new Process { Name = "Акилирование аммиака", PrimaryFeedstocks = "Аммиак; Метанол", PrimaryProducts = "Метиламин; Диметиламин; Триметиламин" },
                    new Process { Name = "Паровая конверсия углеводородов", PrimaryFeedstocks = "Метан; Пар", PrimaryProducts = "Синтез-газ" },
                    new Process { Name = "Окислительное дегидрирование метанола", PrimaryFeedstocks = "Метанол", PrimaryProducts = "Формальдегид" },
                    new Process { Name = "Двухстадийное окисление пропилена", PrimaryFeedstocks = "Пропилен; Кислород; Пар", PrimaryProducts = "Акриловая кислота" },
                    new Process { Name = "Дегидратация и окисление глицерина", PrimaryFeedstocks = "Глицерин; Кислород", PrimaryProducts = "Акриловая кислота" },
                    new Process { Name = "Гидролиз акрилоилхлорида", PrimaryFeedstocks = "Этилен; Кислород; CO₂", PrimaryProducts = "Акриловая кислота" },
                    new Process { Name = "Прямая этерификация акриловой кислоты", PrimaryFeedstocks = "Акриловая кислота; Бутанол", PrimaryProducts = "Бутилакрилат; Вода" },
                    new Process { Name = "Окислительный пиролиз метана", PrimaryFeedstocks = "Метан; Кислород", PrimaryProducts = "Ацетилен" },
                    new Process { Name = "Парофазная гидратация ацетилена", PrimaryFeedstocks = "Ацетилен; Вода", PrimaryProducts = "Уксусный альдегид; Кротоновый альдегид; Ацетон" },
                    new Process { Name = "Одностадийное окисление этилена", PrimaryFeedstocks = "Этилен", PrimaryProducts = "Уксусный альдегид; Уксусная кислота; Муравьиная кислота; Метилхлорид; Этилхлорид; Хлорацетальдегид; Кротоновый альдегид; Диоксид углерода" },
                    new Process { Name = "Двухстадийное окисление этилена", PrimaryFeedstocks = "Этилен", PrimaryProducts = "Уксусный альдегид; Уксусная кислота; Муравьиная кислота; Метилхлорид; Этилхлорид; Хлорацетальдегид; Кротоновый альдегид; Диоксид углерода" },
                    new Process { Name = "Жидкофазное окисление Н-бутана", PrimaryFeedstocks = "Н-бутан; Кислород", PrimaryProducts = "Уксусная кислота; Муравьиная кислота; Ацетон-метилацетатная фракция; Меилэтилкетон-этилфцетатная фракция" },
                    new Process { Name = "Карбонилирование метанола <BASF>", PrimaryFeedstocks = "Метанол; Окись углерода", PrimaryProducts = "Уксусная кислота; Деметиловый эфир; Метилацетат" },
                    new Process { Name = "Карбонилирование метанола <MONSANTO>", PrimaryFeedstocks = "Метанол; Окись углерода", PrimaryProducts = "Уксусная кислота; Деметиловый эфир; Метилацетат" },
                    new Process { Name = "Карбонилирование метанола <BP CHEMICAL LTD>", PrimaryFeedstocks = "Метанол; Окись углерода", PrimaryProducts = "Уксусная кислота; Деметиловый эфир; Метилацетат" },
                    new Process { Name = "Производство уксусной кислоты из Синтез-газа", PrimaryFeedstocks = "Синтез-газ", PrimaryProducts = "Уксусная кислота" },
                    new Process { Name = "Хлорирование уксусной кислоты в присутствии уксусного ангидрида с хлористым ацетилом", PrimaryFeedstocks = "Уксусная кислота", PrimaryProducts = "Монохлоруксусная кислота" },
                    new Process { Name = "Омыление трихлорэтилена водой в присутствии серной кислоты", PrimaryFeedstocks = "Трихлорэтилен", PrimaryProducts = "Монохлоруксусная кислота" },
                    new Process { Name = "Каталитический риформинг бензинов с последующим разделением продуктов", PrimaryFeedstocks = "Бензиновые фракции", PrimaryProducts = "Толуол; Бензол; Фракция ксилолов" }, //Фракции - не тоные продукты. как их выводить
                };

                context.Processes.AddRange(processes);
                await context.SaveChangesAsync();
            }
        }
    }
}
