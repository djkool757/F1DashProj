namespace Pitwall.Models;

public class Sprint
{
    public string Number { get; set; }
    public int Postion { get; set; }
    public string PositionText { get; set; }
    public string Points { get; set; }
    public Driver Driver { get; set; }
    public Constructor Constructor { get; set; }
    public string Grid { get; set; }
    public string Laps { get; set; }
    public string Status { get; set; }
}
