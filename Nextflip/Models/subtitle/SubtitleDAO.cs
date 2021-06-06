﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Nextflip.Models;
using MySql.Data.MySqlClient;

namespace Nextflip.Models.subtitle
{
    public class SubtitleDAO : ISubtitleDAO
    {
        public string ConnectionString { get; set; }

        public async Task<SubtitleDTO> GetSubtitleByEpisodeID(string episodeID)
        {
            SubtitleDTO subtitle = null;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                string Sql = $"Select subtitleID, language, status, subtitleURL From Subtitle Where episodeID = {episodeID}";
                using (var command = new MySqlCommand(Sql, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            subtitle = new SubtitleDTO
                            {
                                SubtitleID = reader.GetString(0),
                                EpisodeID = episodeID,
                                Language = reader.GetString(1),
                                Status = reader.GetString(2),
                                SubtitleURL = reader.GetString(3)
                            };
                        }
                    }
                }
                connection.Close();
            }
            return subtitle;
        }
    }
}
