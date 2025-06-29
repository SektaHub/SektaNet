using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace backend.Models.Discord
{
    public partial class Message
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime? TimeStampEdited { get; set; }
        public DateTime? CallEndedTimeStamp { get; set; }
        public bool IsPinned { get; set; }
        public string Content { get; set; }
        public string? AuthorId { get; set; }
        public DiscordUser? Author { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<Embed> Embeds { get; set; }
        public List<Reaction> Reactions { get; set; }
        public List<DiscordUser> Mentions { get; set; }

        public Guid DiscordServerId { get; set; }  // Add this line
        public DiscordChannelExport DiscordServer { get; set; }

        [GeneratedRegex(@"<@&(?<id>\d+)>", RegexOptions.Compiled)]
        private static partial Regex RoleMentionRegex();

        [GeneratedRegex(@"<@!?(?<id>\d+)>", RegexOptions.Compiled)]
        private static partial Regex UserMentionRegex();

        [GeneratedRegex(@"<a?:(?<name>[A-Za-z0-9_~]+):\d+>", RegexOptions.Compiled)]
        private static partial Regex EmojiRegex();

        [GeneratedRegex(@"<#(?<id>\d+)>", RegexOptions.Compiled)]
        private static partial Regex ChannelMentionRegex();

        private static readonly IReadOnlyDictionary<string, string> _userNames =
            new Dictionary<string, string>
            {
                ["182826498590375937"] = "bokalce",
                ["187286582426861569"] = "borjan",
                ["218413441436745728"] = "kasmirkafe",
                ["223760661891645440"] = "authorized_bot",
                ["223762832779509761"] = "dime1903",
                ["223764867444768768"] = "nubs4dayz",
                ["232474295757045760"] = "_tosho",
                ["303931396941021195"] = "stalin2511",
                ["304590957645529118"] = "_bluenix",
                ["323799845347262475"] = "molikrog",
                ["324880450973859840"] = "lavur0",
                ["371757631649611779"] = "elfen12",
                ["477546082717532171"] = "tallii",
                ["855590724552491038"] = "hustlers_university",
                ["164413567288868864"] = "nookmkd",
                ["323088588084477952"] = "andrej058077",
                ["456226577798135808"] = "Deleted User",
                ["1011666801135992903"] = "Hey, Netflix",
                ["1015297409942822932"] = "stefan5138",
                ["1016662470317842436"] = "Flute",
                ["1032381694826778704"] = "Snowsgiving Bot",
                ["1130157682435821678"] = "Munix",
                ["1258016157798236191"] = "SektaBot",
                ["186577497683525632"] = "teki2228",
                ["226635942339805184"] = "pejo2388",
                ["236547039377358849"] = "dodevski",
                ["267646157537280001"] = "praaroxiel",
                ["268440843692539915"] = "13_dexter_49",
                ["276456315373748236"] = "_lightforge_",
                ["282633398693003264"] = "justdimi",
                ["323243178859954187"] = "toni.k.",
                ["324909477193056266"] = "kiri_l",
                ["366226695419723778"] = "msml13.",
                ["383785881904873485"] = "_nahhh",
                ["387941351930462209"] = "cwmutt",
                ["413779529597845515"] = "teabet.",
                ["478216590966521858"] = "toshkica.",
                ["484614946223685642"] = "mekdonalds5318",
                ["484824459849498647"] = "goatse2694",
                ["516757784100995082"] = "amyloidosis",
                ["520202773296578563"] = "shakletoo",
                ["563434444321587202"] = "Maki",
                ["658802810934001685"] = "jesussaves666",
                ["672822334641537041"] = "FreeStuff",
                ["707330374946128013"] = "elenakeckaroska",
                ["761198382600945664"] = "janushka.",
                ["762955046202900500"] = "tejka001",
                ["766048157560864828"] = "itsmartina.",
                ["781959667190464513"] = "mgjondeva",
                ["789384550865698816"] = "dekstersiveskio",
                ["797154435381329940"] = "lightforge3128",
                ["923263568491741287"] = "klosar7615",
                ["934424637067128862"] = "gejmeriste",
                ["936929561302675456"] = "Midjourney Bot",
                ["939909891450093679"] = "trajce4533",
                ["187198736194076672"] = "_flameo",
                ["204920077773045761"] = "schwizl",
                ["207275341297876992"] = "_pajko",
                ["228981957998936064"] = "mikicalee0763",
                ["232481300068433920"] = "nubs4dayz1693",
                ["232483770245054464"] = "tikituku",
                ["294954253510770689"] = "bajatamusmula7836",
                ["302502433748156439"] = "bulletblast_0386",
                ["189083155683213322"] = "ice6",
                ["204255221017214977"] = "YAGPDB.xyz",
                ["228537642583588864"] = "Vexera",
                ["235088799074484224"] = "Rythm",
                ["260029935287140353"] = "kolekeski",
                ["302503386878574592"] = "edgybou",
                ["312985879306043393"] = "kipro.",
                ["323910433578942464"] = "majka_ti",
                ["357273537188462594"] = "pajkomadafaka2139",
                ["440980790545874945"] = "femaleeddie",
                ["563448491540611083"] = "aleksandar4183",
                ["716377728470745460"] = "tournlquet",
                ["749689583486369803"] = "wulfgirl.",
                ["765584555901714473"] = "dragishhhn",
                ["852509694341283871"] = "SpellCast",
                ["897761580891136061"] = "infernal4550",
                ["952867116741165086"] = "filipskv",
                ["116275390695079945"] = "Nadeko",
                ["119482224713269248"] = "Lindsey",
                ["155149108183695360"] = "Dyno",
                ["159985870458322944"] = "MEE6",
                ["187636089073172481"] = "DuckHunt",
                ["220552109198671874"] = "strilak",
                ["239631525350604801"] = "Pancake",
                ["252128902418268161"] = "Rythm 2",
                ["298673420181438465"] = "Poll Bot",
                ["396417601737326593"] = "Nayami",
                ["416313719891427339"] = "Zoki",
                ["437055663965995018"] = "PokeStory",
                ["710425515248320523"] = "Officer Jenny",
                ["713038874875658250"] = "Archive Bot",
                ["301424917474443264"] = "The Protector",
                ["303904389968560129"] = "Spoti-Search",
                ["367163580400795649"] = "Spotify Bot",
                ["603293828929159210"] = "george7316"
            };
        private static readonly IReadOnlyDictionary<string, string> _roleNames =
            new Dictionary<string, string>()
            {
                //KANAL5
                ["964191020961124352"] = "#TERRARIASKVAD",
                ["1036292844849213520"] = "#BRANITELINADREVNITEDVASKVAD",
                ["981902205290438656"] = "#CIVeriSkvad",
                ["976617909013086258"] = "#CSSKVAD",
                ["989986718927167488"] = "#DONTGLADUVAJZAEDNOSKVAD",
                ["1215023778912800778"] = "#DOZHDOPADENRIZIK2",
                ["1064976425574219786"] = "#KATANSKVAD",
                ["1022443992106938368"] = "#KLESHERISKVAD",
                ["976617504438911046"] = "#LOLESKVAD",
                ["976617957411147826"] = "#OVERWATCHERISKVAD",
                ["976617877421555732"] = "#POESKVAD",
                ["976618095638609940"] = "#POINTANDKLIKERISKVAD",
                ["1251991046129319997"] = "#RLcraft",
                ["1064189355280834591"] = "#STIVGENG",
                ["976618416196710411"] = "#TERRARIASKVAD",
                ["1021430588726575114"] = "#TFTNERDS",
                ["1060576430490132551"] = "#VRVOTNAMASATASKVAD",
                ["995280977179971634"] = "#WOWNIGERI",
                ["985901763947286550"] = "archive",
                ["976632908490080356"] = "blue",
                ["870448996328546354"] = "BOOSTER",
                ["976803928979157002"] = "cyan",
                ["1026328512753311755"] = "doom",
                ["1040003034815680566"] = "Free Stuff",
                ["1023266326468362332"] = "Geymer",
                ["976632854291283978"] = "green",
                ["1036265075650076777"] = "Hey, Netflix",
                ["999336688306364446"] = "iskacanje",
                ["976614556791095316"] = "kurtogeng",
                ["976632963561308160"] = "lavander",
                ["978265017474482177"] = "libtard",
                ["976633151881379871"] = "magenta",
                ["1196241689812152372"] = "Manta",
                ["976613492083785789"] = "Members",
                ["1036262580764815403"] = "Midjourney Bot",
                ["978260978795966485"] = "moron",
                ["976632748347363348"] = "orange",
                ["976632610736472065"] = "red18",
                ["1258021957488738327"] = "SektaBot",
                ["1049416528111611955"] = "Snowsgiving Bot",
                ["976628532358815774"] = "totkash",
                ["976633105899221123"] = "white",
                ["976632823194742795"] = "yellow",
                ["978261221121863700"] = "zajak",
                ["976643724601868318"] = "~~~~~🤖AI~~~~~",
                ["877576630447505408"] = "❓",
                ["994293790569414706"] = "🤮weebs🤮",
                //WW3
                ["873981725372473404"] = ".",
                ["964317006969065472"] = "borjanodebi",
                ["1125193358378270790"] = "idfk",
                ["1258565661941305427"] = "SektaBot",
                ["409405367336828953"] = "#SmiteSKVAD",
                ["403725364137951252"] = "Instrument maker",
                ["432500845993787423"] = "bliznak",
                ["388396266788356100"] = "pretsedatel",
                ["388364448898744322"] = "toshanov",
                ["448865510248153099"] = "#SektaSKVAD",
                ["450052872600551425"] = "#CSSKVAD",
                ["448926054749306899"] = "gay",
                ["439198329704808448"] = "gods u badmington",
                ["423192060326051850"] = "lil pump fan",
                ["456778737833345036"] = "kosharkari",
                ["389171145770795008"] = "gospod",
                ["433275715778117642"] = "kire",
                ["466581989835931658"] = "#LOLSKVAD",
                ["469537703055327263"] = "nepismen",
                ["470283671329308682"] = "WOWSKVAD",
                ["477556901148753960"] = "guitarist",
                ["477992906490249217"] = "#PODOBRIOTCSSkvad",
                ["484852891975155723"] = "gospod",
                ["388363662185988097"] = "policaec",
                ["480423119497199616"] = "nadeko",
                ["487551280294920192"] = "#BrawlhallaSKVAD",
                ["496800093144481795"] = "sex offender",
                ["496800119274995713"] = "child rapist",
                ["502924360886124565"] = "#ouvervvatcherskiskvad",
                ["502924368264036372"] = "#MinecraftSKVAD",
                ["528613594200408076"] = "#VisceraCleanupSKVAD",
                ["528686676021870594"] = "#ROTMGSKVAD",
                ["388364358092062731"] = "Cigance",
                ["531234157460979722"] = "#CSSKVAD",
                ["531517522785206276"] = "#DNDSKVAD",
                ["553958027603083295"] = "Dyno",
                ["564213787184332800"] = "#ElderScrollsOnlineSKVAD",
                ["569297264967942179"] = "#TerrariaSKVAD",
                ["555471621675286558"] = "RoadToPlakari",
                ["578897549365346315"] = "#GTASKVAD",
                ["580077469726670849"] = "matematicari",
                ["388363747145547788"] = "minister",
                ["535920273330012200"] = "#TFTSKVAD",
                ["603977719650451505"] = "satanicari",
                ["653339938506670090"] = "#UnturnedSKVAD",
                ["675309277417832452"] = "biggayday",
                ["704349994634706956"] = "#ValorantSKVAD",
                ["774627426378317875"] = "Finkashi",
                ["419202939597488128"] = "gods u fizika",
                ["493379880285765632"] = "botovi",
                ["834076046008778772"] = "Dota2SKVAD",
            };

            private static readonly IReadOnlyDictionary<string, string> _channelNames = new Dictionary<string, string>
            {
                ["403713486133395457"] = "u park",
                ["388361012832763909"]= "mainhall",

            };

        public string GetFormattedContent(bool useDefaults = true)
        {
            if (string.IsNullOrEmpty(Content))
                return Content ?? string.Empty;

            if (!Content.Contains('<') || !Content.Contains('>'))
                return Content;

            string result = Content;

            // roles
            result = RoleMentionRegex().Replace(result, m =>
            {
                var id = m.Groups["id"].Value;
                if (_roleNames.TryGetValue(id, out var role))
                    return $"<@{role}>";
                else
                    return useDefaults ? "<@unknown-role>" : m.Value;
            });

            // users
            result = UserMentionRegex().Replace(result, m =>
            {
                var id = m.Groups["id"].Value;
                if (_userNames.TryGetValue(id, out var user))
                    return $"<@{user}>";
                else
                    return useDefaults ? "<@unknown-user>" : m.Value;
            });

            // channels
            result = ChannelMentionRegex().Replace(result, m =>
            {
                var id = m.Groups["id"].Value;
                if (_channelNames.TryGetValue(id, out var channel))
                    return $"<#{channel}>";
                else
                    return useDefaults ? "<#unknown-channel>" : m.Value;
            });

            // emojis
            result = EmojiRegex().Replace(result, m => $":{m.Groups["name"].Value}:");

            return result;
        }



    }
}
