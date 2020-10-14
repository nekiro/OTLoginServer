using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using TibiaLoginServer.Models;
using static TibiaLoginServer.Models.Character;

namespace TibiaLoginServer.Classes
{
    public class DatabaseManager
    {
        private readonly MySqlConnection _connection;

        private string Host { get; set; }
        private string User { get; set; }
        private string Password { get; set; }
        private string DbName { get; set; }
        private string Port { get; set; }

        private const string connectionString = "host = {0}; userid = {1}; password = {2}; database = {3}; port = {4}";

        public DatabaseManager()
        {
            _connection = new MySqlConnection();
        }

        public async Task<bool> Setup()
        {
            _connection.ConnectionString = String.Format(connectionString, ConfigLoader.GetString("mysqlHost"), ConfigLoader.GetString("mysqlUser"), 
                ConfigLoader.GetString("mysqlPass"), ConfigLoader.GetString("mysqlDatabase"), ConfigLoader.GetInteger("mysqlPort"));
            Console.WriteLine(_connection.ConnectionString);

            try
            {
                await _connection.OpenAsync();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public string Hash(string stringToHash)
        {
            using (var sha1 = new SHA1Managed())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(stringToHash))).Replace("-", "").ToLower();
            }
        }

        public async Task<int> GetAccountId(string name, string password)
        {
            var cmd = new MySqlCommand($"SELECT `id` FROM `accounts` WHERE `name` = '{name}' AND `password` = '{Hash(password)}'", _connection);
            using (var dataReader = await cmd.ExecuteReaderAsync())
            {
                if (dataReader.Read())
                {
                    if (!dataReader.HasRows)
                    {
                        return 0;
                    }

                    return int.Parse(dataReader.GetString(0));
                }
            }
            return 0;
        }

        public ICollection<World> GetWorlds()
        {
            // this is just a dummy method, cause tfs does not support more than one world
            List<World> worlds = new List<World>();
            World world = new World()
            {
                Name = ConfigLoader.GetString("serverName"),
                ExternalAddressProtected = ConfigLoader.GetString("ip"),
                ExternalPortUnprotected = ConfigLoader.GetInteger("loginProtocolPort").ToString(),
                ExternalPortProtected = ConfigLoader.GetInteger("loginProtocolPort").ToString(),
                ExternalAddressUnprotected = ConfigLoader.GetString("ip"),
                PvpType = 0, // todo fix this to get config pvp
            };
            worlds.Add(world);
            return worlds;
        }

        public async Task<ICollection<Character>> GetAccountCharacters(int id)
        {
            var cmd = new MySqlCommand($"SELECT `name`, `level`, `vocation`, `sex`, `looktype`, `lookhead`, `lookbody`, `looklegs`, `lookfeet`, `lookaddons`  FROM `players` WHERE `account_id` = {id}", _connection);
            using (var dataReader = await cmd.ExecuteReaderAsync())
            {
                List<Character> characters = new List<Character>();
                while (await dataReader.ReadAsync())
                {
                    characters.Add(new Character()
                    {
                        WorldId = 0, // not used in tfs
                        Name = dataReader.GetString(dataReader.GetOrdinal("name")),
                        Level = dataReader.GetInt32(dataReader.GetOrdinal("level")),
                        Vocation = (VocationEnum)Enum.Parse(typeof(VocationEnum), dataReader.GetInt32(dataReader.GetOrdinal("vocation")).ToString()),
                        IsMale = dataReader.GetInt32(dataReader.GetOrdinal("sex")) == 0,
                        IsHidden = false, // not used in tfs
                        IsMainCharacter = false, // not used in tfs
                        Tutorial = false, // not used in tfs
                        OutfitId = dataReader.GetInt32(dataReader.GetOrdinal("looktype")),
                        HeadColor = dataReader.GetInt32(dataReader.GetOrdinal("lookhead")),
                        TorsoColor = dataReader.GetInt32(dataReader.GetOrdinal("lookbody")),
                        LegsColor = dataReader.GetInt32(dataReader.GetOrdinal("looklegs")),
                        DetailColor = dataReader.GetInt32(dataReader.GetOrdinal("lookfeet")),
                        AddonsFlags = dataReader.GetInt32(dataReader.GetOrdinal("lookaddons")),
                        IsTournamentParticipant = false // not used in tfs
                    });
                }

                return characters;
            }
        }

        public BoostedCreatureResponse GetBoostedCreature()
        {
            return new BoostedCreatureResponse() { RaceId = 1496 };
        }

        public CacheInfoResponse GetCacheInfo()
        {
            return new CacheInfoResponse()
            {
                PlayersOnline = 666,
                TwitchStreams = 666,
                TwitchViewer = 666,
                GamingYoutubeStreams = 666,
                GamingYoutubeViewer = 666,
            };
        }
    }
}
