// game.Server.DTOs.BlockDTO.cs
public class BlockDTO
{
    public int BlockId { get; set; }
    public string BlockType { get; set; } // Send the string representation of the enum
    public int ItemId { get; set; }
    public int MinAmount { get; set; }
    public int MaxAmount { get; set; }
}