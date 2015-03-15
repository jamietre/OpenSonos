using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OpenSonos.LocalMusicServer.Browsing
{
    public class GuidIdentityProvider : IIdentityProvider
    {
        private readonly ConcurrentDictionary<string, SonosIdentifier> _pathToGuid;

        public GuidIdentityProvider():this(null)
        {
            
        }

        public GuidIdentityProvider(IEnumerable<KeyValuePair<string, SonosIdentifier>> backingStore)
        {
            _pathToGuid = new ConcurrentDictionary<string, SonosIdentifier>();

            if (backingStore == null)
            {
                return;
            }

            foreach (var item in backingStore)
            {
                _pathToGuid.TryAdd(item.Key, item.Value);
            }
        }

        public SonosIdentifier IdFor(string path)
        {
            if (_pathToGuid.ContainsKey(path))
            {
                return _pathToGuid[path];
            }

            var identifier = new SonosIdentifier
            {
                Id = Guid.NewGuid().ToString(),
                Path = path
            };

            _pathToGuid.TryAdd(path, identifier);
            return identifier;
        }

        public SonosIdentifier FromRequestId(string requestedId)
        {
            if (_pathToGuid.SingleOrDefault(x => x.Value.Id == requestedId).Value != null)
            {
                return _pathToGuid.SingleOrDefault(x => x.Value.Id == requestedId).Value;
            }
            
            return new SonosIdentifier();
        }
    }
}