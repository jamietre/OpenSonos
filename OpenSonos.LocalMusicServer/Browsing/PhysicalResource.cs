using System.Linq;

namespace OpenSonos.LocalMusicServer.Browsing
{
    public abstract class PhysicalResource : IRepresentAResource
    {
        public SonosIdentifier Identifier { get; private set; }
        public string DisplayName { get { return Identifier.Path.Split('\\').Last(); } }

        protected PhysicalResource(SonosIdentifier identifier)
        {
            Identifier = identifier;
        }

        public static PhysicalResource FromId(SonosIdentifier identifier)
        {
            return identifier.IsDirectory 
                ? (PhysicalResource) new Container(identifier) 
                : new MusicFile(identifier);
        }
    }
}