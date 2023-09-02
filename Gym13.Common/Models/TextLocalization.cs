using Newtonsoft.Json;

public class TextLocalization
{
    public string? KA { get; set; }
    public string? EN { get; set; }
    [JsonIgnore]
    public string AllLanguageTogather => $"{KA}{EN}".ToLower();
    [JsonIgnore]
    public string? SerializedText { get; set; }
    public string? Text => this[Thread.CurrentThread.CurrentUICulture.Name.ToUpper()];

    private string? this[string? propertyName] => (string?)GetType()?.GetProperty(propertyName!)?.GetValue(this, null);

    public TextLocalization(string? serializedText)
    {
        if (serializedText != null)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<LocalizationHelper>(serializedText);
                KA = obj?.ka;
                EN = obj?.en;
                SerializedText = serializedText;
            }
            catch (Exception)
            {

            }

        }
    }
    public static implicit operator string?(TextLocalization? textLocalization) => textLocalization?.Text;

    public bool AnyOfLanguageContains(string keyword) => AllLanguageTogather.Contains(keyword);
    private bool EqualsAnyOfLanguage(string keyword) => EN == keyword || KA == keyword;

    public bool StartOrEqualsAnyOfLenaguage(string keyword) => StartWithAnyOfLanguage(keyword) || EqualsAnyOfLanguage(keyword);
    private bool StartWithAnyOfLanguage(string keyword) => (EN?.StartsWith(keyword) ?? false) || (KA?.StartsWith(keyword) ?? false);

    public bool Contains(string? keyWord) => Text?.Contains(keyWord) ?? false;


    [JsonConstructor]
    protected TextLocalization(string? ka, string? en)
    {
        KA = ka;
        EN = en;
        string jsonString = JsonConvert.SerializeObject(new { KA = ka, EN = en });
        SerializedText = jsonString;
    }

    public static TextLocalization Create(string? ka, string? en)
    {
        return new TextLocalization(ka, en);
    }
}
public class LocalizationHelper
{
    public string? ka { get; set; }
    public string? en { get; set; }
}

