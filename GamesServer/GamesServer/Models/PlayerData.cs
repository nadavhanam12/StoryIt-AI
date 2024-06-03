using System.Drawing;

namespace GamesServer.Models
{
    public class PlayerData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public string ColorString {  get; set; }
        public byte[] Avatar { get; set; }
        public byte[] AvateByteArray { get; set; }
    }
}
