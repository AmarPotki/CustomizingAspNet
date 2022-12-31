using System.Text.Json;

public class MyCustomConfigurationSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    => new CustomConfigurationProvider();

}
public class CustomConfigurationProvider : ConfigurationProvider
{
    public override void Load()
    {
      //  var text = File.ReadAllText(@"D:\SecurityMetadata.json");
        //  var text = File.ReadAllText(@"D:\SecurityMetadata.xml");
        // var text =""
        //var options = new JsonSerializerOptions
        //{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        //var content = JsonSerializer.Deserialize<SecurityMetadata>
        //    (text, options);
        //if (content != null)
        //{
        //    Data = new Dictionary<string, string>
        //    {
        //        {"ApiKey", content.ApiKey},
        //        {"ApiSecret", content.ApiSecret}
        //    }!;
        //}

        Data = new Dictionary<string, string?>
        {
            { "ApiKey", "My Api" },
            { "ApiSecret","My Secret"},
            {"AppSettings:Bar","CustomConfig"}
        };
    }
}

public class SecurityMetadata
{
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}