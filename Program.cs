using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace Conspirolog_bot
{
    internal class Program //internal — доступен только в пределах сборки
    {
        // private static string token { get; set; } = "your-telegram-token-here";
        private static readonly string token = "your-telegram-token-here";
        private static TelegramBotClient bot;

        //категории
        static Dictionary<string, List<string>> categoryToTheories = new Dictionary<string, List<string>>()
    {
        { "🛸 НЛО и инопланетяне", new List<string>() { "Зона 51", "Люди-рептилоиды", "Высадка на Луну — подделка", "Земля плоская", "HAARP" } },
        { "🧬 Медицина и технологии", new List<string>() { "COVID-19 создан искусственно", "Чипирование через вакцины", "Химиотрейлы", "Контроль через 5G", "Заговор фармкомпаний" } },
        { "🏛 Политика и элиты", new List<string> () { "Новый Мировой Порядок", "Иллюминаты", "Кеннеди", "Смерть Дианы", "Эпштейн" } },
        { "🎬 Прочее", new List<string>() { "MK-Ultra", "Голливуд", "Тесла и бесплатная энергия", "Подземные города", "Королевская семья" } },
    };
        static Dictionary<string, string> theoryImages = new Dictionary<string, string>()
        {
            { "Зона 51", "https://overclockers.ru/st/legacy/blog/417293/436155_O.jpg" },
    { "Люди-рептилоиды", "https://tse3.mm.bing.net/th/id/OIP.YVfj29mkFIUKC99x7q0RiwHaD7?rs=1&pid=ImgDetMain&cb=idpwebpc2" },
    { "Высадка на Луну — подделка", "https://focus.ua/static/storage/thumbs/920x465/a/5e/a10cd199-b4f0f59eb06afe73dd2d1584831db5ea.jpg?v=3814_1" },
    { "Земля плоская", "https://i.ytimg.com/vi/AFFM3WuRNjg/maxresdefault.jpg" },
    { "HAARP", "https://feedsfree.com/wp-content/uploads/2019/02/haarp1-changed-size.jpg" },
    { "COVID-19 создан искусственно", "https://heraldodemexico.com.mx/u/fotografias/m/2020/3/26/f1280x720-199780_331455_5050.jpg" },
    { "Чипирование через вакцины", "https://ichef.bbci.co.uk/news/640/cpsprodpb/CA70/production/_118242815_tv067010283.jpg" },
    { "Химиотрейлы", "https://cdn.lifehacker.ru/wp-content/uploads/2020/11/1280px-Gehling_PZL-106_AR_Kruk_O_1604912569-e1604912590801.jpg" },
    { "Контроль через 5G", "https://www.gov.pl/photo/format/af6cd2d9-bbe9-4f30-8c4e-9908a196383d/resolution/1920x810" },
    { "Заговор фармкомпаний", "https://tse4.mm.bing.net/th/id/OIP.kGFGHhSUv2ziRfnk1hE_4AHaE8?w=600&h=400&rs=1&pid=ImgDetMain&cb=idpwebpc2" },
    { "Новый Мировой Порядок", "https://ichef.bbci.co.uk/news/640/cpsprodpb/177B9/production/_111758169_conspiracy_theory_976x549_1-nc.png" },
    { "Иллюминаты", "https://tse3.mm.bing.net/th/id/OIP.THAe52nSpMks9gm4W-ziogHaE7?rs=1&pid=ImgDetMain&cb=idpwebpc2" },
    { "Кеннеди", "https://rtvi.com/wp-content/uploads/2025/01/ap18251114136393-768x432.webp" },
    { "Смерть Дианы", "https://cdn.milenio.com/uploads/media/2024/08/28/lady-di-misterio-muerte-expuesto.jpg" },
    { "Эпштейн", "https://tse1.mm.bing.net/th/id/OIP.1NYCxmYYCxmcLh7CMPnDVwHaEK?rs=1&pid=ImgDetMain&o=7&rm=3" },
    { "MK-Ultra", "https://th.bing.com/th/id/R.7aca89549d7f51fa96602ab254c986fe?rik=0NS5i5NlyubS%2bA&pid=ImgRaw&r=0&sres=1&sresct=1" },
    { "Голливуд", "https://th.bing.com/th/id/R.18243320cdfa8c405b3bb66c5e8b814a?rik=qiYKGOn22HFMcg&pid=ImgRaw&r=0" },
    { "Тесла и бесплатная энергия", "https://www.campus-astrologia.es/wp-content/uploads/2017/07/Electro-Tesla-600x398.jpg" },
    { "Подземные города", "https://avatars.dzeninfra.ru/get-zen_doc/9233083/pub_63fe1a69cb11142e3f035177_63fe1a8284d4d649ec93b900/scale_1200" },
    { "Королевская семья", "https://content.api.news/v3/images/bin/c6956c11e0952bef97365dc8c82cbd89" },
        };
        
        //описание
        static Dictionary<string, string> theoryDescriptions = new Dictionary<string, string>()
    {
        { "Зона 51", "Зона 51 — американский Ватикан тайн\r\nТы знаешь, что официально это просто военная база в Неваде. Но почему же её охраняют лучше, чем ядерные объекты? Сотни свидетелей говорят: там хранят обломки НЛО, проводят обратную разработку инопланетных технологий, а некоторые утверждают, что видели настоящих пришельцев! Тебе кажется, что правительство просто так десятилетиями скрывает, что внутри?\r\n\r\n" },
        { "Люди-рептилоиды", "Люди-рептилоиды — правят миром\r\nПолитики, звёзды, королевские особы… А что если они — вовсе не люди? Рептилоиды якобы прибыли с древней планеты и слились с элитой, чтобы управлять человечеством. Зрачки у Обамы? Змееподобная кожа у королевы? Это просто «глюк в матрице», или проскальзывающая истина?" },
        { "Высадка на Луну — подделка", "Высадка на Луну — великая постановка\r\n1969 год, якобы первый человек на Луне. А теперь — подумай: почему ни одна страна больше не повторила этого подвига? Почему тени на кадрах идут в разные стороны? Стэнли Кубрик, по слухам, режиссировал всё это на секретной студии NASA. Ведь в разгар Холодной войны США нуждались в великой победе…" },
        { "Земля плоская", "Земля плоская\r\nЕсли самолёт летит прямо, почему он не «опускается вниз»? Почему горизонт всегда прямой, а не изогнутый? Сторонники теории плоской Земли утверждают: мы живём на гигантском диске, окружённом ледяной стеной (Антарктида), и нас держат в неведении, чтобы скрыть правду о нашем положении во Вселенной." },
        { "HAARP", "HAARP — климатическое оружие\r\nНа Аляске стоит научный комплекс, который якобы исследует ионосферу. А теперь фантазируй: а что, если его настоящая задача — управление погодой, вызов землетрясений и наводнений, создание ураганов? Всё — в целях глобального контроля." },
        { "COVID-19 создан искусственно", "COVID-19 создан искусственно\r\nПандемия унесла миллионы жизней. А что если это — тщательно продуманный план? Одни говорят, вирус разработан в лаборатории Ухани, другие — что он создан элитами, чтобы запустить тотальный контроль, сломать экономику и перестроить мир по новым правилам." },
        { "Чипирование через вакцины", "Чипирование через вакцины\r\nТы идёшь делать укол от COVID-19, а вместе с ним получаешь наночип, способный отслеживать твои передвижения, поведение и даже мысли. Билл Гейтс, Microsoft, ID2020 — в этом всём видят зловещий цифровой контроль над личностью." },
        { "Химиотрейлы", "Химиотрейлы\r\nТы видел белые следы от самолётов в небе? Это не просто конденсат, говорят теоретики. Это химические вещества, распыляемые для контроля над сознанием, климатом, или даже для стерилизации населения." },
        { "Контроль через 5G", "Контроль через 5G\r\nСеть 5G даёт быстрый интернет, но за ним может скрываться нечто большее. Некоторые уверены: это инфраструктура тотального контроля, возможно, влияющая на мозг, иммунитет или даже испускающая «психотронные» волны." },
        { "Заговор фармкомпаний", "Заговор фармкомпаний\r\nПочему лекарства такие дорогие? Почему от рака всё ещё нет массово доступного лечения? Потому что фармацевтические гиганты зарабатывают на болезнях. Исцеление — невыгодно. Они якобы прячут лекарства, чтобы продавать лечение пожизненно." },
        { "Новый Мировой Порядок", "Новый Мировой Порядок\r\nЕдиное правительство, единая валюта, цифровой паспорт и контроль за каждым шагом. Многие считают, что мировая элита — в том числе Ротшильды, Рокфеллеры и МВФ — движется к созданию тоталитарного глобального государства." },
        { "Иллюминаты", "Иллюминаты\r\nСекретное общество, контролирующее политику, банки и шоу-бизнес. Их символ — всевидящее око на долларе. Музыканты в клипах используют их символику. Одни говорят — это культ сатаны, другие — элита, решающая судьбу мира." },
        { "Кеннеди", "Убийство Кеннеди\r\nОфициально — стрелял Ли Харви Освальд. Но если это была операция ЦРУ? Или мафии? Или спецслужб, не согласных с мирной политикой Кеннеди? Убийство президента — слишком чистое, слишком быстрое… будто зачистка." },
        { "Смерть Дианы", "Смерть принцессы Дианы\r\nАвтокатастрофа в парижском туннеле. Случайность? Или королевская семья решила избавиться от неудобной фигуры? Диана угрожала раскрыть тёмные тайны монархии и встречалась с «неподходящим» мужчиной. Заговор — был бы идеальной зачисткой." },
        { "Эпштейн", "Джеффри Эпштейн\r\nОфициально — повесился в тюрьме. Но камеры внезапно не работали, охранники «заснули». Эпштейн знал слишком много о звёздах, политиках и их участии в педофильской сети. Его смерть — спасение для элиты." },
        { "MK-Ultra", "MK-Ultra\r\nПрограмма ЦРУ, которая реально существовала. Эксперименты с ЛСД, психотронными методами и контролем над разумом. А что, если её не свернули, а лишь перенесли в подполье? Люди в Голливуде и политике иногда «сходят с ума» слишком подозрительно…" },
        { "Голливуд", "Голливуд — фабрика управления сознанием\r\nТы смотришь фильмы, а тебе внедряют идеи: культ денег, насилия, разрушение традиционных ценностей. Звёзды — лишь марионетки, а продюсеры и студии действуют как идеологические оружейники. Голливуд — в руках тайных обществ." },
        { "Тесла и бесплатная энергия", "Никола Тесла и бесплатная энергия\r\nТесла создал устройство, способное передавать энергию без проводов — бесплатно. А теперь подумай: кто потеряет прибыль, если у всех будет энергия даром? Его труды исчезли, архивы засекречены, изобретения — похоронены." },
        { "Подземные города", "Подземные города\r\nГлубоко под землёй — не шахты, а целые базы. Говорят, там живут элиты, готовящиеся к глобальной катастрофе, или даже внеземные цивилизации. Об этом шепчут бывшие военные. Что скрывают подземные тоннели США и Европы?" },
        { "Королевская семья", "Королевская семья — не просто символ\r\nСтаринные ритуалы, скрытые богатства, абсолютная неприкосновенность… Что, если британская королевская семья — это не просто монархия, а древний культ, связанный с рептилоидами или иллюминатами? Диана знала слишком много…" },
    };


        static async Task Main(string[] args)
        {
            bot = new TelegramBotClient(token); // инициализация бота с помощью ключа
            var cts = new CancellationTokenSource(); //Источник токена отмены, нужен чтобы корректно остановить получение обновлений, если потребуется.
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // получаем все типы обновлений
            };

            // Запуск бота
            bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: cts.Token);
            var me = await bot.GetMe();//Получаем информацию о боте (например, имя пользователя) и выводим её в консоль
            Console.WriteLine($" Бот запущен: {me.Username}");
            Console.ReadLine();
        }

        private static async Task HandleErrorAsync(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
        {
            Console.WriteLine($"⚠ Ошибка: {exception.Message}");
            await Task.CompletedTask;
            //return Task.CompletedTask;
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            if (update.Type == UpdateType.Message && update.Message?.Text != null) //Проверяем, что это текстовое сообщение
            {
                string msg = update.Message.Text;
                var chatId = update.Message.Chat.Id;

                if (msg == "/start")
                {
                    // Приветствие при запуске
                    string welcomeMessage = "👋 Привет! Я бот, который расскажет тебе о самых известных теориях заговора.\n\n" +
                                            "Вот что я умею:\n" +
                                            "• Предлагаю списки теорий по категориям\n" +
                                            "• Даю краткие описания каждой из них\n\n" +
                                            "Введите /seelist, чтобы начать";

                    await client.SendMessage(
                        chatId: chatId,
                        text: welcomeMessage,
                        cancellationToken: token);
                }
                else if (msg == "/seelist")
                {
                    // Кнопки категорий
                    var keyboard = new InlineKeyboardMarkup(new[]
                    {
                new[] { InlineKeyboardButton.WithCallbackData("🛸 НЛО и инопланетяне") },
                new[] { InlineKeyboardButton.WithCallbackData("🧬 Медицина и технологии") },
                new[] { InlineKeyboardButton.WithCallbackData("🏛 Политика и элиты") },
                new[] { InlineKeyboardButton.WithCallbackData("🎬 Прочее") },
            });

                    await client.SendMessage(
                        chatId: chatId,
                        text: "Выбери категорию:",
                        replyMarkup: keyboard,
                        cancellationToken: token);
                }
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackData = update.CallbackQuery.Data;
                var chatId = update.CallbackQuery.Message.Chat.Id;

                // Если нажата категория — показать список теорий
                if (categoryToTheories.ContainsKey(callbackData))
                {
                    var buttons = new List<InlineKeyboardButton[]>();
                    foreach (var theory in categoryToTheories[callbackData])
                    {
                        buttons.Add(new[] { InlineKeyboardButton.WithCallbackData(theory) });
                    }

                    var keyboard = new InlineKeyboardMarkup(buttons);

                    await client.SendMessage(
                        chatId: chatId,
                        text: $"Теории из категории *{callbackData}*:",
                        parseMode: ParseMode.Markdown,
                        replyMarkup: keyboard,
                        cancellationToken: token);
                }
                else if (theoryDescriptions.ContainsKey(callbackData))
                {
                    var theory = update.CallbackQuery.Data;
                    var chat_Id = update.CallbackQuery.Message.Chat.Id;

                    if (theoryDescriptions.ContainsKey(theory))
                    {
                        string description = theoryDescriptions[theory];
                        string imageUrl = theoryImages.ContainsKey(theory) ? theoryImages[theory] : null;

                        if (imageUrl != null)
                        {
                            await client.SendPhoto(
                                chatId: chat_Id,
                                photo: imageUrl,
                                caption: description,
                                cancellationToken: token);
                        }
                        else
                        {
                            await client.SendMessage(chatId, description, cancellationToken: token);
                        }
                    }
                }

            }
        }
    }
}
