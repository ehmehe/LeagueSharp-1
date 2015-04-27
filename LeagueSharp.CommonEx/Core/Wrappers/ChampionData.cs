using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.CommonEx.Properties;
using Newtonsoft.Json.Linq;

namespace LeagueSharp.CommonEx.Core.Wrappers
{
    /// <summary>
    ///     Provides information about champions.
    /// </summary>
    public class ChampionData
    {
        private readonly JToken _championToken;

        /// <summary>
        ///     Creates a new instance, using the Player's Champion Name.
        /// </summary>
        public ChampionData() : this(ObjectManager.Player.ChampionName)
        {
        }

        /// <summary>
        ///     Creates a new instance, getting information about that champion.
        /// </summary>
        /// <param name="championName">Champion Name</param>
        public ChampionData(string championName)
        {
            var damageFile = JObject.Parse(Encoding.UTF8.GetString(Resources.ChampionData));

            _championToken = damageFile["data"][championName];

            if (_championToken == null)
            {
                throw new ArgumentException("Champion does not exist.");
            }
        }

        /// <summary>
        ///     Gets the ID of the champion.
        /// </summary>
        /// <value>ID</value>
        public int Id
        {
            get { return _championToken["id"].ToObject<int>(); }
        }

        /// <summary>
        ///     Gets the title of the champion.
        /// </summary>
        /// <example>"Ezreal" - "the Prodigal Explorer"</example>
        /// <value>Title of champion</value>
        public string Title
        {
            get { return _championToken["title"].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets a <see cref="JsonSpellWrapper" />, which contains hefty information about the spell.
        /// </summary>
        /// <param name="slot">Spell Slot. (Q - R)</param>
        /// <returns>
        ///     <see cref="JsonSpellWrapper" />
        /// </returns>
        public JsonSpellWrapper GetSpell(SpellSlot slot)
        {
            return new JsonSpellWrapper(_championToken["spells"].Children().ToArray()[(int) slot]);
        }
    }

    /// <summary>
    ///     Wraps the "spells" section of the JSON file.
    /// </summary>
    public class JsonSpellWrapper
    {
        private readonly JToken _spellToken;

        internal JsonSpellWrapper(JToken spellToken)
        {
            this._spellToken = spellToken;
        }

        /// <summary>
        ///     Gets the name of the spell.
        /// </summary>
        /// <value>Name of the spell.</value>
        public string Name
        {
            get { return _spellToken["name"].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets the description of the spell.
        /// </summary>
        /// <value>Description</value>
        public string Description
        {
            get { return _spellToken["description"].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets the sanitized description of the spell.
        /// </summary>
        /// <value>Sanitized Description</value>
        public string SanitizedDescription
        {
            get { return _spellToken["sanitizedDescription"].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets the string showed when you hover over the ability.
        /// </summary>
        /// <value>Tool tip</value>
        public string ToolTip
        {
            get { return _spellToken["tooltip"].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets the sanitized string showed when you hover over the ability.
        /// </summary>
        /// <value>Sanitized Tool Tip</value>
        public string SanitizedToolTip
        {
            get { return _spellToken["sanitizedTooltip"].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets the max rank which you can no longed upgrade the spell.
        /// </summary>
        /// <value>Max Rank of the spell</value>
        public int MaxRank
        {
            get { return _spellToken["maxrank"].ToObject<int>(); }
        }

        /// <summary>
        ///     Gets an array, containing the cost of the spell. The array matches the level of the spell.
        /// </summary>
        /// <value>Int[] of spell clost.</value>
        public int[] Cost
        {
            get { return _spellToken["cost"].ToObject<int[]>(); }
        }

        /// <summary>
        ///     Gets the type of cost needed to cast the spell.
        /// </summary>
        /// <example>Mana</example>
        /// <value>Type of cost</value>
        public string CostType
        {
            get { return _spellToken["costType"].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets the cost of the spell, with a '/' between each cost.
        /// </summary>
        /// <example>1/2/3/4/5</example>
        /// <value>Cost of the spell as a string.</value>
        public string CostString
        {
            get { return _spellToken["costBurn"].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets the cooldown of the spell as an array. The array matches the level of the spell.
        /// </summary>
        /// <value>Cooldown of the spell.</value>
        public float[] Cooldown
        {
            get { return _spellToken["cooldown"].ToObject<float[]>(); }
        }

        /// <summary>
        ///     Gets the cooldown of the spell, with a '/' between each cooldown.
        /// </summary>
        /// <example>1/2/3/4/5</example>
        /// <value>Cooldown of the spell as a string.</value>
        public string CooldownString
        {
            get { return _spellToken["cooldownBurn"].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets the damage of the spell as an array. The array matches the level of the spell.
        /// </summary>
        /// <value>Damage of the spell.</value>
        public int[] Damage
        {
            get { return _spellToken["effect"].Children().ToArray()[1].ToObject<int[]>(); }
        }

        /// <summary>
        ///     Gets the damage of the spell, with a '/' between each damage.
        /// </summary>
        /// <example>1/2/3/4/5</example>
        /// <value>Damage of the spell as a string.</value>
        public string DamageString
        {
            get { return _spellToken["effectBurn"].Children().ToArray()[1].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets the range of the spell as an array. The array matches the level of the spell.
        /// </summary>
        /// <value>Range of the spell.</value>
        public int[] RangeArray
        {
            get { return _spellToken["range"].ToObject<int[]>(); }
        }

        /// <summary>
        ///     Gets the range of the spell.
        /// </summary>
        /// <value>Range</value>
        public int Range
        {
            get { return Convert.ToInt32(_spellToken["rangeBurn"].ToObject<string>()); }
        }

        /// <summary>
        ///     Gets the name of the spell.
        /// </summary>
        /// <value>Name of the spel.</value>
        public string Key
        {
            get { return _spellToken["key"].ToObject<string>(); }
        }

        /// <summary>
        ///     Gets the image of the spell asynchronous. (As displayed on the HUD in game)
        /// </summary>
        /// <returns>Image of the spell.</returns>
        public async Task<Image> GetImageAsync()
        {
            // Download the PNG
            var webClient = new HttpClient();
            var data =
                await
                    webClient.GetByteArrayAsync(
                        string.Format(
                            "http://ddragon.leagueoflegends.com/cdn/5.2.1/img/spell/{0}",
                            _spellToken["image"]["full"].ToObject<string>()));

            return new Bitmap(new MemoryStream(data));
        }

        /// <summary>
        ///     Gets the image of the spell synchronous. (As displayed on the HUD in game)
        /// </summary>
        /// <returns>Image of the spell.</returns>
        public Image GetImage()
        {
            // Download the PNG
            var webClient = new HttpClient();
            var data =
                webClient.GetByteArrayAsync(
                    string.Format(
                        "http://ddragon.leagueoflegends.com/cdn/5.2.1/img/spell/{0}",
                        _spellToken["image"]["full"].ToObject<string>())).Result;

            return new Bitmap(new MemoryStream(data));
        }
    }
}