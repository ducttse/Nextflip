﻿namespace Nextflip.Models.subtitle
{
    public interface ISubtitleDAO
    {
        SubtitleDTO GetSubtitleByEpisodeID(string episodeID);
    }
}