namespace game.Server.Models
{
    public class MovePlayerRequest
    {
        public int NewPositionX { get; set; }
        public int NewPositionY { get; set; }

        public int? NewFloorId { get; set; }
    }
}
