namespace UnitPrice.Utils;

public class Emoji
{
    public static string GetRandomEmoji()
    {
        var rnd = new Random();
        var code = rnd.Next(0x1F347, 0x1F6F8);
        return char.ConvertFromUtf32(code);
    }
}