namespace SubspeakConfigTool.Shared.Models;

public class Wordlist
{
    public Wordlist()
    {
        Name = string.Empty;
        Words = new List<WordRecord>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public List<WordRecord> Words { get; set; }
}
