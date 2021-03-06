using System;

namespace OpenSonos.LocalMusicServer.Browsing
{
    public interface IMusicRepository
    {
        DateTime LastUpdate { get; }

		ResourceCollection GetResources(string identifier);
		ResourceCollection Search(string query);
    }
}