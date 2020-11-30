using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using MySqlConnector;
using OTLoginServer.Models;
using static OTLoginServer.Models.Character;

namespace OTLoginServer.Classes
{
    public class DatabaseManager
    {
        private readonly MySqlConnection _connection;

        private const string connectionString = "host = {0}; userid = {1}; password = {2}; database = {3}; port = {4}";

        public DatabaseManager()
        {
            _connection = new MySqlConnection();
        }

        ~DatabaseManager()
        {
            _connection.Close();
        }

        public async Task<bool> Setup()
        {
            _connection.ConnectionString = string.Format(connectionString, ConfigLoader.GetString("mysqlHost"), ConfigLoader.GetString("mysqlUser"), 
                ConfigLoader.GetString("mysqlPass"), ConfigLoader.GetString("mysqlDatabase"), ConfigLoader.GetInteger("mysqlPort"));

            try
            {
                await _connection.OpenAsync();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            Console.WriteLine($"Connected to {_connection.Database} at {ConfigLoader.GetString("mysqlHost")}:{ConfigLoader.GetInteger("mysqlPort")} via {ConfigLoader.GetString("mysqlUser")}");
            return true;
        }

        public async Task<Account> GetAccount(string email, string password)
        {
            using var cmd = new MySqlCommand($"SELECT `id`, `premdays`, `lastday` FROM `accounts` WHERE `email` = '{email}' AND `password` = '{Const.Sha1Hash(password)}'", _connection);
            using (var dataReader = await cmd.ExecuteReaderAsync())
            {
                if (dataReader.Read())
                {
                    Account account = new Account();

                    if (!dataReader.HasRows)
                    {
                        return null;
                    }

                    account.Id = dataReader.GetInt32(dataReader.GetOrdinal("id"));

                    int premDays = dataReader.GetInt32(dataReader.GetOrdinal("premdays"));
                    account.IsPremium = premDays > 0;
                    account.LastLoginTime = dataReader.GetInt64(dataReader.GetOrdinal("lastday"));
                    account.PremiumUntil = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + premDays * 86400;

                    return account;
                }
            }
            return null;
        }

        public ICollection<World> GetWorlds()
        {
            // this is just a dummy method, cause tfs does not support more than one world
            List<World> worlds = new List<World>();
            World world = new World()
            {
                Name = ConfigLoader.GetString("serverName"),
            };

            world.ExternalAddressProtected = world.ExternalAddressUnprotected = ConfigLoader.GetString("ip");
            world.ExternalPortProtected = world.ExternalPortUnprotected = ConfigLoader.GetInteger("gameProtocolPort");

            string pvpType = ConfigLoader.GetString("worldType");
            if (pvpType == "pvp")
            {
                world.PvpType = 0;
            }
            else if (pvpType == "no-pvp")
            {
                world.PvpType = 1;
            }
            else if (pvpType == "pvp-enforced")
            {
                world.PvpType = 2;
            }

            worlds.Add(world);
            return worlds;
        }

        public async Task<ICollection<Character>> GetAccountCharacters(int id)
        {
            using var cmd = new MySqlCommand($"SELECT `name`, `level`, `vocation`, `sex`, `looktype`, `lookhead`, `lookbody`, `looklegs`, `lookfeet`, `lookaddons`  FROM `players` WHERE `account_id` = {id}", _connection);
            using var dataReader = await cmd.ExecuteReaderAsync();

            List<Character> characters = new List<Character>();
            while (await dataReader.ReadAsync())
            {
                characters.Add(new Character()
                {
                    Name = dataReader.GetString(dataReader.GetOrdinal("name")),
                    Level = dataReader.GetInt32(dataReader.GetOrdinal("level")),
                    Vocation = (VocationEnum)System.Enum.Parse(typeof(VocationEnum), dataReader.GetInt32(dataReader.GetOrdinal("vocation")).ToString()),
                    IsMale = dataReader.GetInt32(dataReader.GetOrdinal("sex")) == 0,
                    OutfitId = dataReader.GetInt32(dataReader.GetOrdinal("looktype")),
                    HeadColor = dataReader.GetInt32(dataReader.GetOrdinal("lookhead")),
                    TorsoColor = dataReader.GetInt32(dataReader.GetOrdinal("lookbody")),
                    LegsColor = dataReader.GetInt32(dataReader.GetOrdinal("looklegs")),
                    DetailColor = dataReader.GetInt32(dataReader.GetOrdinal("lookfeet")),
                    AddonsFlags = dataReader.GetInt32(dataReader.GetOrdinal("lookaddons")),
                });
            }

            return characters;
        }

        public EventsScheduleResponse GetScheduledEvents()
        {
            // you can use this method to parse events from your database

            EventsScheduleResponse eventsResponse = new EventsScheduleResponse();
            eventsResponse.EventList = new List<Event>();
            eventsResponse.EventList.Add(new Event()
            {
                StartDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                EndDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 5 * 86400,
                ColorLight = "#7a1b34",
                ColorDark = "#64162b",
                Name = "Sample Event",
                Description = "Nekiro was here"
            });

            return eventsResponse;
        }

        public BoostedCreatureResponse GetBoostedCreature()
        {
            // you can use this method to parse boosted creature from your database
            return new BoostedCreatureResponse() { RaceId = 1496 };
        }

        public CacheInfoResponse GetCacheInfo()
        {
            // you can use this method to parse info stuff from your database
            return new CacheInfoResponse()
            {
                PlayersOnline = 123,
                TwitchStreams = 456,
                TwitchViewer = 789,
                GamingYoutubeStreams = 1000,
                GamingYoutubeViewer = 24444,
            };
        }
    }
}
