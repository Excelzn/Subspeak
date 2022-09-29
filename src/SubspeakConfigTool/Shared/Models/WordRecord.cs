namespace SubspeakConfigTool.Shared.Models;

public class WordRecord
{
    public WordRecord()
    {
        Word = string.Empty;
        Lists = new List<Wordlist>();
    }
    public int Id { get; set; }
    public string Word { get; set; }
    public List<Wordlist> Lists { get; set; }
}
